using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

/// <summary>Cierre de caja: cobros, devoluciones y neto de un período, por método de pago y por usuario.</summary>
public class FrmCaja : FormularioBase, IRefrescable
{
    private readonly ReporteNegocio _reportes = new ReporteNegocio();
    private readonly PagoNegocio _pagos = new PagoNegocio();

    private readonly DateTimePicker _dtpDesde = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 120 };
    private readonly DateTimePicker _dtpHasta = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 120 };
    private readonly TarjetaIndicador _tarjetaCobrado = new TarjetaIndicador { Titulo = "Cobrado", Icono = Icono.Tarjeta };
    private readonly TarjetaIndicador _tarjetaDevuelto = new TarjetaIndicador
        { Titulo = "Devuelto", Icono = Icono.Actualizar, Acento = Tema.Advertencia };
    private readonly TarjetaIndicador _tarjetaNeto = new TarjetaIndicador
        { Titulo = "Neto en caja", Icono = Icono.Reporte, Acento = Tema.Informacion };
    private readonly TarjetaIndicador _tarjetaMovimientos = new TarjetaIndicador
        { Titulo = "Movimientos", Icono = Icono.Lista, Acento = Tema.Neutro };
    private readonly DataGridView _dgvMetodos = new DataGridView { Dock = DockStyle.Fill };
    private readonly DataGridView _dgvUsuarios = new DataGridView { Dock = DockStyle.Fill };
    private readonly DataGridView _dgvMovimientos = new DataGridView { Dock = DockStyle.Fill };

    public FrmCaja()
    {
        Text = "Caja";
        AutoScaleMode = AutoScaleMode.Font;
        AutoScaleDimensions = new SizeF(7F, 15F);
        ClientSize = new Size(1040, 640);

        var tabla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 3 };
        for (int i = 0; i < 4; i++)
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
        tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 126F));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        Control filtros = ConstruirFiltros();
        tabla.Controls.Add(filtros, 0, 0);
        tabla.SetColumnSpan(filtros, 4);

        TarjetaIndicador[] tarjetas = { _tarjetaCobrado, _tarjetaDevuelto, _tarjetaNeto, _tarjetaMovimientos };
        for (int i = 0; i < tarjetas.Length; i++)
        {
            tarjetas[i].Dock = DockStyle.Fill;
            tarjetas[i].Margin = new Padding(0, 0, i == tarjetas.Length - 1 ? 0 : 14, 14);
            tabla.Controls.Add(tarjetas[i], i, 1);
        }

        var inferior = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2, Margin = new Padding(0) };
        inferior.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
        inferior.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
        inferior.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        inferior.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        inferior.Controls.Add(EnTarjeta("Por método de pago", _dgvMetodos, new Padding(0, 0, 14, 14)), 0, 0);
        inferior.Controls.Add(EnTarjeta("Por usuario", _dgvUsuarios, new Padding(0, 0, 14, 0)), 0, 1);
        Tarjeta movimientos = EnTarjeta("Movimientos del período", _dgvMovimientos, new Padding(0));
        inferior.Controls.Add(movimientos, 1, 0);
        inferior.SetRowSpan(movimientos, 2);
        tabla.Controls.Add(inferior, 0, 2);
        tabla.SetColumnSpan(inferior, 4);

        Controls.Add(tabla);

        Grillas.MensajeVacio(_dgvMetodos, "Sin cobros en el período.");
        Grillas.MensajeVacio(_dgvUsuarios, "Sin cobros en el período.");
        Grillas.MensajeVacio(_dgvMovimientos, "No hay pagos ni devoluciones en el período elegido.");
        _dtpDesde.Value = _dtpHasta.Value = DateTime.Today;
        Load += (sender, e) => Refrescar();
    }

    private Control ConstruirFiltros()
    {
        var barra = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 0, 0, 14),
            WrapContents = false,
            BackColor = Color.Transparent
        };

        Label Etiqueta(string texto) => new Label
        {
            Text = texto,
            AutoSize = true,
            Margin = new Padding(0, 10, 6, 0),
            Font = Tema.TextoNegrita9
        };

        Button Rapido(string texto, Func<(DateTime Desde, DateTime Hasta)> rango)
        {
            var boton = new Button { Text = texto, Size = new Size(96, 32), Margin = new Padding(0, 2, 8, 0) };
            boton.Click += (sender, e) =>
            {
                (DateTime desde, DateTime hasta) = rango();
                _dtpDesde.Value = desde;
                _dtpHasta.Value = hasta;
                Refrescar();
            };
            return boton;
        }

        DateTime hoy = DateTime.Today;
        int diasDesdeLunes = ((int)hoy.DayOfWeek + 6) % 7;
        _dtpDesde.Margin = new Padding(0, 6, 16, 0);
        _dtpHasta.Margin = new Padding(0, 6, 16, 0);

        var consultar = new Button { Text = "Consultar", Size = new Size(110, 32), Margin = new Padding(0, 2, 24, 0) };
        consultar.Click += (sender, e) => Refrescar();
        var exportar = new Button
        {
            Text = "Exportar movimientos",
            Size = new Size(170, 32),
            Margin = new Padding(0, 2, 0, 0)
        };
        exportar.Click += (sender, e) =>
            Exportar.Csv(_dgvMovimientos, $"caja-{_dtpDesde.Value:yyyy-MM-dd}-a-{_dtpHasta.Value:yyyy-MM-dd}", this);

        barra.Controls.AddRange(new Control[]
        {
            Etiqueta("Desde"), _dtpDesde, Etiqueta("Hasta"), _dtpHasta, consultar,
            Rapido("Hoy", () => (hoy, hoy)),
            Rapido("Esta semana", () => (hoy.AddDays(-diasDesdeLunes), hoy)),
            Rapido("Este mes", () => (new DateTime(hoy.Year, hoy.Month, 1), hoy)),
            exportar
        });
        return barra;
    }

    private static Tarjeta EnTarjeta(string titulo, Control contenido, Padding margen)
    {
        var tarjeta = new Tarjeta { Titulo = titulo, Dock = DockStyle.Fill, Margin = margen };
        tarjeta.Controls.Add(contenido);
        return tarjeta;
    }

    public void Refrescar()
    {
        try
        {
            DateTime desde = _dtpDesde.Value.Date;
            DateTime hasta = _dtpHasta.Value.Date;
            List<TotalCaja> porMetodo = _reportes.CajaPorMetodo(desde, hasta);
            List<TotalCaja> porUsuario = _reportes.CajaPorUsuario(desde, hasta);

            decimal cobrado = porMetodo.Sum(t => t.Cobrado);
            decimal devuelto = porMetodo.Sum(t => t.Devuelto);
            _tarjetaCobrado.Valor = cobrado.ToString("C0");
            _tarjetaCobrado.Detalle = Periodo(desde, hasta);
            _tarjetaDevuelto.Valor = devuelto.ToString("C0");
            _tarjetaDevuelto.Detalle = "Por reservas canceladas";
            _tarjetaNeto.Valor = (cobrado - devuelto).ToString("C0");
            _tarjetaNeto.Detalle = "Cobrado menos devuelto";
            _tarjetaMovimientos.Valor = porMetodo.Sum(t => t.Movimientos).ToString();
            _tarjetaMovimientos.Detalle = "Pagos y devoluciones sin anular";

            MostrarTotales(_dgvMetodos, porMetodo, "Método");
            MostrarTotales(_dgvUsuarios, porUsuario, "Usuario");

            _dgvMovimientos.DataSource = _pagos.Listar(null, desde, hasta).Where(p => !p.Anulado).ToList();
            Grillas.Ocultar(_dgvMovimientos, "IdPago", "IdReserva", "FechaReserva", "Dni", "Anulado");
            Grillas.Formato(_dgvMovimientos, "C2", "Importe");
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private static void MostrarTotales(DataGridView grilla, List<TotalCaja> totales, string encabezado)
    {
        grilla.DataSource = totales;
        Grillas.Formato(grilla, "C2", "Cobrado", "Devuelto", "Neto");
        if (grilla.Columns.Contains("Grupo"))
            grilla.Columns["Grupo"].HeaderText = encabezado;
        if (grilla.Columns.Contains("Movimientos"))
            grilla.Columns["Movimientos"].HeaderText = "Mov.";
    }

    private static string Periodo(DateTime desde, DateTime hasta) =>
        desde == hasta ? desde.ToString("dddd d 'de' MMMM") : $"Del {desde:dd/MM} al {hasta:dd/MM/yyyy}";
}
