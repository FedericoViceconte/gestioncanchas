
using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

public partial class FrmResumen : Form, IRefrescable
{
    private readonly ReservaNegocio _reservaNegocio = new ReservaNegocio();
    private readonly PagoNegocio _pagoNegocio = new PagoNegocio();
    private readonly ClienteNegocio _clienteNegocio = new ClienteNegocio();

    public FrmResumen()
    {
        InitializeComponent();
        Grillas.Configurar(dgvTurnos);
        Grillas.Configurar(dgvDeudores);
        Resize += (sender, e) => Acomodar();
    }

    private void FrmResumen_Load(object sender, EventArgs e)
    {
        Acomodar();
        Refrescar();
    }

    private void Acomodar()
    {
        int alto = Math.Max(ClientSize.Height - dgvTurnos.Top - 12, 100);
        int anchoTurnos = (int)((ClientSize.Width - 36) * 0.58);

        lblCobrado.Left = (int)(ClientSize.Width * 0.30);
        lblDeuda.Left = (int)(ClientSize.Width * 0.62);
        dgvTurnos.Size = new Size(anchoTurnos, alto);
        dgvDeudores.Left = dgvTurnos.Right + 12;
        lblDeudores.Left = dgvDeudores.Left;
        dgvDeudores.Size = new Size(Math.Max(ClientSize.Width - dgvDeudores.Left - 12, 100), alto);
    }

    public void Refrescar()
    {
        try
        {
            DateTime hoy = DateTime.Today;
            lblTitulo.Text = $"Resumen de hoy: {TurnoFijoDetalle.NombreDia((int)hoy.DayOfWeek)} {hoy:dd/MM/yyyy}";

            List<ReservaDetalle> turnos = _reservaNegocio.Listar(new ReservaFiltro { Fecha = hoy })
                .Where(r => r.Estado != EstadosReserva.Cancelada)
                .ToList();
            dgvTurnos.DataSource = turnos;
            Grillas.Ocultar(dgvTurnos, "IdReserva", "Fecha", "Dni", "Importe", "TotalPagado");
            Grillas.Formato(dgvTurnos, "C2", "Saldo");

            decimal cobrado = _pagoNegocio.Listar(null, hoy, hoy)
                .Where(p => !p.Anulado)
                .Sum(p => p.Tipo == TiposPago.Devolucion ? -p.Importe : p.Importe);

            List<Cliente> deudores = _clienteNegocio.Listar(true)
                .OrderByDescending(c => c.Deuda)
                .ToList();
            dgvDeudores.DataSource = deudores;
            Grillas.Ocultar(dgvDeudores, "Dni");
            Grillas.Formato(dgvDeudores, "C2", "Deuda");

            lblTurnos.Text = $"Turnos de hoy: {turnos.Count}";
            lblCobrado.Text = $"Cobrado hoy: {cobrado:C2}";
            lblDeuda.Text = $"Deuda total: {deudores.Sum(c => c.Deuda):C2}";
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void dgvDeudores_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (dgvDeudores.Columns[e.ColumnIndex].Name == "Deuda")
            e.CellStyle.ForeColor = Color.Firebrick;
    }
}