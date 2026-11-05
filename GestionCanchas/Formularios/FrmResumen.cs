using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

public partial class FrmResumen : FormularioBase, IRefrescable
{
    private readonly ReservaNegocio _reservaNegocio = new ReservaNegocio();
    private readonly ClienteNegocio _clienteNegocio = new ClienteNegocio();
    private readonly ReporteNegocio _reporteNegocio = new ReporteNegocio();

    public FrmResumen()
    {
        InitializeComponent();
        Grillas.MensajeVacio(dgvTurnos, "No hay turnos reservados para hoy.");
        Grillas.MensajeVacio(dgvDeudores, "Ningún cliente tiene deuda.");
        dgvTurnos.DataBindingComplete += (sender, e) => dgvTurnos.ClearSelection();
        dgvDeudores.DataBindingComplete += (sender, e) => dgvDeudores.ClearSelection();
    }

    private void FrmResumen_Load(object sender, EventArgs e) => Refrescar();

    public void Refrescar()
    {
        try
        {
            DateTime hoy = DateTime.Today;
            IndicadoresDia indicadores = _reporteNegocio.Indicadores(hoy);

            tarjetaTurnos.Valor = indicadores.TurnosHoy.ToString();
            tarjetaTurnos.Detalle = "Sin contar las cancelaciones";
            tarjetaOcupacion.Valor = indicadores.Ocupacion.ToString("P0");
            tarjetaOcupacion.Detalle = $"{indicadores.TurnosHoy} de {indicadores.TurnosOcupables} turnos posibles";
            tarjetaCobrado.Valor = indicadores.CobradoHoy.ToString("C0");
            tarjetaCobrado.Detalle = $"En el mes: {indicadores.CobradoMes:C0}";
            tarjetaDeuda.Valor = indicadores.DeudaTotal.ToString("C0");
            tarjetaDeuda.Detalle = indicadores.ClientesConDeuda == 1
                ? "1 cliente con saldo pendiente"
                : $"{indicadores.ClientesConDeuda} clientes con saldo pendiente";

            List<ReservaDetalle> turnos = _reservaNegocio.Listar(new ReservaFiltro { Fecha = hoy })
                .Where(r => r.Estado != EstadosReserva.Cancelada)
                .OrderBy(r => r.Horario)
                .ThenBy(r => r.Cancha)
                .ToList();
            dgvTurnos.DataSource = turnos;
            Grillas.Ocultar(dgvTurnos, "IdReserva", "Fecha", "Dni", "Importe", "TotalPagado");
            Grillas.Formato(dgvTurnos, "C2", "Saldo");

            grafico.Datos = _reporteNegocio.IngresosUltimosDias(hoy, 7);

            dgvDeudores.DataSource = _clienteNegocio.Listar(true)
                .OrderByDescending(c => c.Deuda)
                .ToList();
            Grillas.Ocultar(dgvDeudores, "Dni");
            Grillas.Formato(dgvDeudores, "C2", "Deuda");
            dgvTurnos.ClearSelection();
            dgvDeudores.ClearSelection();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void dgvDeudores_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (dgvDeudores.Columns[e.ColumnIndex].Name != "Deuda")
            return;

        e.CellStyle.ForeColor = Tema.Peligro;
        e.CellStyle.SelectionForeColor = Tema.Peligro;
        e.CellStyle.Font = Tema.TextoNegrita9;
    }
}
