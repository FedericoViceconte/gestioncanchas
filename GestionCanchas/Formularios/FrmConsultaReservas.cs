using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

public partial class FrmConsultaReservas : FormularioBase, IRefrescable
{
    private readonly ReservaNegocio _reservaNegocio = new ReservaNegocio();
    private readonly PagoNegocio _pagoNegocio = new PagoNegocio();
    private readonly CanchaNegocio _canchaNegocio = new CanchaNegocio();

    public FrmConsultaReservas()
    {
        InitializeComponent();
        Exportar.AgregarBoton(btnLimpiar, dgvReservas, "reservas");
        btnAnularPago.Visible = btnDevolucion.Visible = Sesion.EsAdministrador;
        Grillas.MensajeVacio(dgvReservas, "No hay reservas con esos filtros.");
        Grillas.MensajeVacio(dgvPagos, "La reserva seleccionada no tiene pagos.");
    }

    private void FrmConsultaReservas_Load(object sender, EventArgs e)
    {
        try
        {
            cboCancha.Items.Add("Todas");
            cboCancha.Items.AddRange(_canchaNegocio.Listar().ToArray());
            cboCancha.SelectedIndex = 0;

            cboEstado.Items.Add("Todos");
            cboEstado.Items.AddRange(EstadosReserva.Todos);
            cboEstado.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }

        Buscar();
    }

    public void Refrescar() => Buscar();

    private void Buscar()
    {
        var filtro = new ReservaFiltro
        {
            Fecha = chkFecha.Checked ? dtpFecha.Value.Date : null,
            Dni = string.IsNullOrWhiteSpace(txtDni.Text) ? null : txtDni.Text.Trim(),
            IdCancha = (cboCancha.SelectedItem as Cancha)?.IdCancha,
            Estado = cboEstado.SelectedIndex > 0 ? cboEstado.SelectedItem as string : null
        };

        try
        {
            dgvReservas.DataSource = _reservaNegocio.Listar(filtro);
            Grillas.Formato(dgvReservas, "C2", "Importe", "TotalPagado", "Saldo");
            Grillas.Formato(dgvReservas, "dd/MM/yyyy", "Fecha");
            CargarPagos();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void CargarPagos()
    {
        ReservaDetalle reserva = Grillas.Seleccionado<ReservaDetalle>(dgvReservas);
        if (reserva == null)
        {
            dgvPagos.DataSource = null;
            return;
        }

        try
        {
            dgvPagos.DataSource = _pagoNegocio.ListarPorReserva(reserva.IdReserva);
            Grillas.Formato(dgvPagos, "C2", "Importe");
            Grillas.Formato(dgvPagos, "dd/MM/yyyy", "FechaPago");
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void dgvReservas_SelectionChanged(object sender, EventArgs e) => CargarPagos();

    private void dgvReservas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (dgvReservas.Columns[e.ColumnIndex].Name != "Estado")
            return;

        switch (e.Value as string)
        {
            case EstadosReserva.Cancelada:
                e.CellStyle.ForeColor = Color.Gray;
                break;
            case EstadosReserva.Pendiente:
                e.CellStyle.ForeColor = Color.SteelBlue;
                break;
            case EstadosReserva.Ausente:
                e.CellStyle.ForeColor = Color.Firebrick;
                break;
            case EstadosReserva.Senado:
                e.CellStyle.ForeColor = Color.DarkOrange;
                break;
            default:
                e.CellStyle.ForeColor = Color.DarkGreen;
                break;
        }
    }

    private void chkFecha_CheckedChanged(object sender, EventArgs e) => dtpFecha.Enabled = chkFecha.Checked;

    private void btnBuscar_Click(object sender, EventArgs e) => Buscar();

    private void btnLimpiar_Click(object sender, EventArgs e)
    {
        chkFecha.Checked = false;
        txtDni.Clear();
        cboCancha.SelectedIndex = 0;
        cboEstado.SelectedIndex = 0;
        Buscar();
    }

    private ReservaDetalle ObtenerSeleccionada()
    {
        ReservaDetalle reserva = Grillas.Seleccionado<ReservaDetalle>(dgvReservas);
        if (reserva == null)
            Mensajes.Info("Seleccioná una reserva de la grilla.");
        return reserva;
    }

    private void btnModificar_Click(object sender, EventArgs e)
    {
        ReservaDetalle reserva = ObtenerSeleccionada();
        if (reserva == null)
            return;

        using var formulario = new FrmReserva(reserva.IdReserva);
        if (formulario.ShowDialog(this) == DialogResult.OK)
            FrmPrincipal.ActualizarPantallas();
    }

    private void btnCancelarReserva_Click(object sender, EventArgs e)
    {
        ReservaDetalle reserva = ObtenerSeleccionada();
        if (reserva == null)
            return;

        string pregunta = $"¿Cancelar la reserva N° {reserva.IdReserva} de {reserva.Cliente} " +
                          $"({reserva.Fecha:dd/MM/yyyy}, {reserva.Horario})?\n\nLos pagos registrados se conservan.";
        if (!Mensajes.Confirmar(pregunta))
            return;

        try
        {
            _reservaNegocio.Cancelar(reserva.IdReserva);
            if (reserva.TotalPagado > 0 &&
                Mensajes.Confirmar($"La reserva tenía {reserva.TotalPagado:C2} cobrados. ¿Registrar la devolución ahora?"))
                AbrirDevolucion(reserva.IdReserva);
            FrmPrincipal.ActualizarPantallas();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void btnAusente_Click(object sender, EventArgs e)
    {
        ReservaDetalle reserva = ObtenerSeleccionada();
        if (reserva == null)
            return;

        string pregunta = $"¿Marcar que {reserva.Cliente} no se presentó a la reserva N° {reserva.IdReserva} " +
                          $"({reserva.Fecha:dd/MM/yyyy}, {reserva.Horario})?\n\nLo pagado no se devuelve y el saldo deja de figurar como deuda.";
        if (!Mensajes.Confirmar(pregunta))
            return;

        try
        {
            _reservaNegocio.MarcarAusente(reserva.IdReserva);
            FrmPrincipal.ActualizarPantallas();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }
    private void btnAnularPago_Click(object sender, EventArgs e)
    {
        Pago pago = Grillas.Seleccionado<Pago>(dgvPagos);
        if (pago == null)
        {
            Mensajes.Info("Seleccioná un pago en la grilla de pagos de abajo.");
            return;
        }

        string pregunta = $"¿Anular el movimiento N° {pago.IdPago} ({pago.Tipo}) de {pago.Importe:C2}?\n\n" +
                          "No se borra: queda registrado como anulado y deja de sumar.";
        if (!Mensajes.Confirmar(pregunta))
            return;

        try
        {
            _pagoNegocio.Anular(pago.IdPago);
            FrmPrincipal.ActualizarPantallas();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void btnDevolucion_Click(object sender, EventArgs e)
    {
        ReservaDetalle reserva = ObtenerSeleccionada();
        if (reserva == null)
            return;
        if (reserva.Estado != EstadosReserva.Cancelada)
        {
            Mensajes.Info("Las devoluciones se registran sobre reservas canceladas.");
            return;
        }

        AbrirDevolucion(reserva.IdReserva);
    }

    private void AbrirDevolucion(int idReserva)
    {
        try
        {
            ReservaDetalle reserva = _reservaNegocio.ObtenerDetalle(idReserva);
            using var formulario = new FrmDevolucion(reserva);
            formulario.ShowDialog(this);
            FrmPrincipal.ActualizarPantallas();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }
    private void btnRegistrarPago_Click(object sender, EventArgs e)
    {
        ReservaDetalle reserva = ObtenerSeleccionada();
        if (reserva == null)
            return;
        if (reserva.Estado != EstadosReserva.Senado && reserva.Estado != EstadosReserva.Pendiente)
        {
            Mensajes.Info("Solo se registran pagos en reservas pendientes o señadas con saldo.");
            return;
        }

        using var formulario = new FrmPagos(reserva.IdReserva);
        formulario.ShowDialog(this);
        FrmPrincipal.ActualizarPantallas();
    }
}
