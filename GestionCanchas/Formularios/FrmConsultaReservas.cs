using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

public partial class FrmConsultaReservas : Form, IRefrescable
{
    private readonly ReservaNegocio _reservaNegocio = new ReservaNegocio();
    private readonly PagoNegocio _pagoNegocio = new PagoNegocio();
    private readonly CanchaNegocio _canchaNegocio = new CanchaNegocio();

    public FrmConsultaReservas()
    {
        InitializeComponent();
        Grillas.Configurar(dgvReservas);
        Grillas.Configurar(dgvPagos);
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
        if (reserva.Estado != EstadosReserva.Senado)
        {
            Mensajes.Info("Solo se registran pagos en reservas señadas con saldo pendiente.");
            return;
        }

        using var formulario = new FrmPagos(reserva.IdReserva);
        formulario.ShowDialog(this);
        FrmPrincipal.ActualizarPantallas();
    }
}
