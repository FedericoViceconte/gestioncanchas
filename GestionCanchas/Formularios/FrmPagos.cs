using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

public partial class FrmPagos : Form, IRefrescable
{
    private readonly ClienteNegocio _clienteNegocio = new ClienteNegocio();
    private readonly ReservaNegocio _reservaNegocio = new ReservaNegocio();
    private readonly PagoNegocio _pagoNegocio = new PagoNegocio();

    private readonly int _idReservaInicial;
    private Cliente _cliente;

    public FrmPagos()
    {
        InitializeComponent();
        Grillas.Configurar(dgvReservas);
        Grillas.Configurar(dgvPagos);
        cboMetodo.Items.AddRange(MetodosPago.Todos);
        cboMetodo.SelectedIndex = 0;
    }

    public FrmPagos(int idReserva) : this()
    {
        _idReservaInicial = idReserva;
    }

    private void FrmPagos_Load(object sender, EventArgs e)
    {
        MostrarReservaSeleccionada();
        if (_idReservaInicial == 0)
            return;

        try
        {
            ReservaDetalle reserva = _reservaNegocio.ObtenerDetalle(_idReservaInicial);
            if (reserva == null)
                return;

            txtDni.Text = reserva.Dni;
            BuscarCliente();
            Grillas.Seleccionar<ReservaDetalle>(dgvReservas, r => r.IdReserva == _idReservaInicial);
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    public void Refrescar()
    {
        if (_cliente == null)
            return;

        int idSeleccionada = Grillas.Seleccionado<ReservaDetalle>(dgvReservas)?.IdReserva ?? 0;
        CargarReservas();
        Grillas.Seleccionar<ReservaDetalle>(dgvReservas, r => r.IdReserva == idSeleccionada);
    }

    private void btnBuscar_Click(object sender, EventArgs e)
    {
        try
        {
            BuscarCliente();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void txtDni_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;

        e.SuppressKeyPress = true;
        btnBuscar.PerformClick();
    }

    private void BuscarCliente()
    {
        _cliente = _clienteNegocio.BuscarPorDni(txtDni.Text);
        if (_cliente == null)
        {
            lblCliente.Text = "";
            dgvReservas.DataSource = null;
            Mensajes.Info("No hay ningún cliente con ese DNI.");
            return;
        }

        CargarReservas();
    }

    private void CargarReservas()
    {
        try
        {
            var filtro = new ReservaFiltro { Dni = _cliente.Dni, SoloConSaldo = true };
            dgvReservas.DataSource = _reservaNegocio.Listar(filtro);
            Grillas.Formato(dgvReservas, "C2", "Importe", "TotalPagado", "Saldo");
            Grillas.Formato(dgvReservas, "dd/MM/yyyy", "Fecha");

            _cliente = _clienteNegocio.BuscarPorDni(_cliente.Dni);
            if (_cliente == null)
                return;
            lblCliente.Text = $"{_cliente.NombreCompleto}  —  Deuda total: {_cliente.Deuda:C2}";
            MostrarReservaSeleccionada();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void dgvReservas_SelectionChanged(object sender, EventArgs e) => MostrarReservaSeleccionada();

    private void MostrarReservaSeleccionada()
    {
        ReservaDetalle reserva = Grillas.Seleccionado<ReservaDetalle>(dgvReservas);
        if (reserva == null)
        {
            lblReserva.Text = "Seleccioná una reserva de la grilla.";
            dgvPagos.DataSource = null;
            btnRegistrar.Enabled = false;
            return;
        }

        lblReserva.Text = $"Reserva N° {reserva.IdReserva}: {reserva.Fecha:dd/MM/yyyy} {reserva.Horario}, " +
                          $"{reserva.Cancha}. Saldo pendiente: {reserva.Saldo:C2}";
        nudImporte.Value = Math.Min(reserva.Saldo, nudImporte.Maximum);
        btnRegistrar.Enabled = true;

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

    private void btnRegistrar_Click(object sender, EventArgs e)
    {
        ReservaDetalle reserva = Grillas.Seleccionado<ReservaDetalle>(dgvReservas);
        if (reserva == null)
        {
            Mensajes.Info("Seleccioná una reserva de la grilla.");
            return;
        }

        var pago = new Pago
        {
            IdReserva = reserva.IdReserva,
            Importe = nudImporte.Value,
            MetodoPago = cboMetodo.SelectedItem as string
        };
        if (!Mensajes.Confirmar($"¿Registrar un pago de {pago.Importe:C2} ({pago.MetodoPago}) para la reserva N° {reserva.IdReserva}?"))
            return;

        try
        {
            decimal saldoRestante = _pagoNegocio.Registrar(pago);
            Mensajes.Info(saldoRestante == 0
                ? "Pago registrado. La reserva quedó pagada."
                : $"Pago registrado. Saldo pendiente: {saldoRestante:C2}.");
            FrmPrincipal.ActualizarPantallas();

            if (MdiParent == null)
                Refrescar();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }
}
