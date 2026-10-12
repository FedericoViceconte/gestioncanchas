using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

// Alta y modificación de reservas. En modificación no se toca la seña: los pagos van por FrmPagos
public partial class FrmReserva : Form
{
    private readonly ReservaNegocio _reservaNegocio = new ReservaNegocio();
    private readonly ClienteNegocio _clienteNegocio = new ClienteNegocio();
    private readonly CanchaNegocio _canchaNegocio = new CanchaNegocio();

    private readonly int _idReserva;
    private DateTime? _fechaInicial;
    private int _idCanchaInicial;
    private int _idHorarioInicial;
    private Cliente _cliente;
    private bool _cargando;

    public FrmReserva()
    {
        InitializeComponent();
    }

    public FrmReserva(DateTime fecha, int idCancha, int idHorario) : this()
    {
        _fechaInicial = fecha;
        _idCanchaInicial = idCancha;
        _idHorarioInicial = idHorario;
    }

    public FrmReserva(int idReserva) : this()
    {
        _idReserva = idReserva;
    }

    private bool EsModificacion => _idReserva != 0;

    private void FrmReserva_Load(object sender, EventArgs e)
    {
        _cargando = true;
        try
        {
            cboMetodo.Items.AddRange(MetodosPago.Todos);
            cboMetodo.SelectedIndex = 0;
            cboCancha.Items.AddRange(_canchaNegocio.ListarDisponibles().ToArray());

            if (EsModificacion && !CargarReserva())
            {
                Close();
                return;
            }
            if (!EsModificacion)
                dtpFecha.MinDate = DateTime.Today;

            if (_fechaInicial.HasValue)
                dtpFecha.Value = _fechaInicial.Value;
            cboCancha.SelectedItem = cboCancha.Items.OfType<Cancha>().FirstOrDefault(c => c.IdCancha == _idCanchaInicial);
            MostrarCliente();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
        finally
        {
            _cargando = false;
        }

        CargarHorarios(_idHorarioInicial);
    }

    private bool CargarReserva()
    {
        ReservaDetalle reserva = _reservaNegocio.ObtenerDetalle(_idReserva);
        if (reserva == null)
        {
            Mensajes.Info("La reserva ya no existe.");
            return false;
        }
        if (reserva.Estado == EstadosReserva.Cancelada)
        {
            Mensajes.Info("No se puede modificar una reserva cancelada.");
            return false;
        }

        Text = $"Modificar reserva N° {reserva.IdReserva}";
        grpSena.Visible = false;
        ClientSize = new Size(ClientSize.Width, 330);

        _cliente = _clienteNegocio.BuscarPorDni(reserva.Dni);
        txtDni.Text = reserva.Dni;
        _fechaInicial = reserva.Fecha;
        _idCanchaInicial = reserva.IdCancha;
        _idHorarioInicial = reserva.IdHorario;
        return true;
    }

    private void btnBuscarCliente_Click(object sender, EventArgs e)
    {
        try
        {
            _cliente = _clienteNegocio.BuscarPorDni(txtDni.Text);
            MostrarCliente();
            if (_cliente == null)
                Mensajes.Info("No hay ningún cliente con ese DNI. Registralo desde Clientes > Gestionar clientes.");
        }
        catch (Exception ex)
        {
            _cliente = null;
            MostrarCliente();
            Mensajes.Error(ex);
        }
    }

    private void txtDni_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;

        e.SuppressKeyPress = true;
        btnBuscarCliente.PerformClick();
    }

    // Si cambian el DNI después de buscar, el cliente encontrado deja de valer
    private void txtDni_TextChanged(object sender, EventArgs e)
    {
        if (_cliente == null || txtDni.Text.Trim() == _cliente.Dni)
            return;

        _cliente = null;
        MostrarCliente();
    }

    private void MostrarCliente()
    {
        if (_cliente == null)
        {
            lblCliente.Text = "Buscá el cliente por DNI.";
            lblCliente.ForeColor = SystemColors.GrayText;
            return;
        }

        if (_cliente.Deuda > 0)
        {
            lblCliente.Text = $"{_cliente.NombreCompleto}  —  Tiene deuda: {_cliente.Deuda:C2}";
            lblCliente.ForeColor = Color.Firebrick;
            return;
        }

        lblCliente.Text = _cliente.NombreCompleto;
        lblCliente.ForeColor = Color.DarkGreen;
    }

    private void dtpFecha_ValueChanged(object sender, EventArgs e) => RecargarHorariosManteniendoSeleccion();

    private void cboCancha_SelectedIndexChanged(object sender, EventArgs e) => RecargarHorariosManteniendoSeleccion();

    private void cboHorario_SelectedIndexChanged(object sender, EventArgs e) => ActualizarImporte();

    private void RecargarHorariosManteniendoSeleccion()
    {
        if (_cargando)
            return;

        int idHorarioActual = (cboHorario.SelectedItem as Horario)?.IdHorario ?? 0;
        CargarHorarios(idHorarioActual);
    }

    private void CargarHorarios(int idHorarioASeleccionar)
    {
        cboHorario.Items.Clear();
        ActualizarImporte();

        if (cboCancha.SelectedItem is not Cancha cancha)
            return;

        try
        {
            List<Horario> libres = _reservaNegocio.ListarHorariosLibres(cancha.IdCancha, dtpFecha.Value.Date, _idReserva);
            if (libres.Count == 0)
            {
                lblImporte.Text = "No hay horarios libres para esa cancha y fecha.";
                return;
            }

            cboHorario.Items.AddRange(libres.ToArray());
            cboHorario.SelectedItem = libres.FirstOrDefault(h => h.IdHorario == idHorarioASeleccionar);
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void ActualizarImporte()
    {
        if (cboCancha.SelectedItem is not Cancha cancha || cboHorario.SelectedItem is not Horario horario)
        {
            lblImporte.Text = "";
            lblSenaMinima.Text = "";
            return;
        }

        decimal importe = _reservaNegocio.CalcularImporte(cancha, horario);
        decimal senaMinima = _reservaNegocio.CalcularSenaMinima(importe);
        lblImporte.Text = $"Importe: {importe:C2}";
        lblSenaMinima.Text = $"Mínimo (15%): {senaMinima:C2}";
        nudSena.Value = Math.Min(senaMinima, nudSena.Maximum);
    }

    private void btnConfirmar_Click(object sender, EventArgs e)
    {
        if (_cliente == null)
        {
            Mensajes.Info("Buscá y seleccioná el cliente por DNI.");
            return;
        }
        if (cboCancha.SelectedItem is not Cancha cancha)
        {
            Mensajes.Info("Seleccioná una cancha.");
            return;
        }
        if (cboHorario.SelectedItem is not Horario horario)
        {
            Mensajes.Info("Seleccioná un horario.");
            return;
        }

        var reserva = new Reserva
        {
            IdReserva = _idReserva,
            IdCliente = _cliente.IdCliente,
            IdCancha = cancha.IdCancha,
            IdHorario = horario.IdHorario,
            Fecha = dtpFecha.Value.Date
        };

        try
        {
            if (EsModificacion)
            {
                _reservaNegocio.Modificar(reserva);
                Mensajes.Info("Reserva modificada.");
            }
            else
            {
                var sena = new Pago { Importe = nudSena.Value, MetodoPago = cboMetodo.SelectedItem as string };
                int idReserva = _reservaNegocio.Registrar(reserva, sena);
                Mensajes.Info($"Reserva N° {idReserva} registrada. Estado: {reserva.Estado}.");
            }
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }
}
