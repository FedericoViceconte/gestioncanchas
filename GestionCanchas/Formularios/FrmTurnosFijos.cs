
using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

public partial class FrmTurnosFijos : FormularioBase, IRefrescable
{
    private static readonly DayOfWeek[] Dias =
    {
        DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
        DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
    };

    private readonly TurnoFijoNegocio _negocio = new TurnoFijoNegocio();
    private readonly ClienteNegocio _clienteNegocio = new ClienteNegocio();
    private readonly CanchaNegocio _canchaNegocio = new CanchaNegocio();
    private readonly HorarioNegocio _horarioNegocio = new HorarioNegocio();
    private Cliente _cliente;

    public FrmTurnosFijos()
    {
        InitializeComponent();
        Grillas.MensajeVacio(dgvTurnos, "Todavía no hay turnos fijos.");
        nudSemanas.Maximum = TurnoFijoNegocio.MaximoSemanas;
        foreach (DayOfWeek dia in Dias)
            cboDia.Items.Add(TurnoFijoDetalle.NombreDia((int)dia));
        dtpDesde.MinDate = DateTime.Today;
    }

    private void FrmTurnosFijos_Load(object sender, EventArgs e)
    {
        try
        {
            cboCancha.Items.AddRange(_canchaNegocio.ListarDisponibles().ToArray());
            cboHorario.Items.AddRange(_horarioNegocio.Listar().ToArray());
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
        MostrarCliente();
        Refrescar();
    }

    public void Refrescar()
    {
        try
        {
            dgvTurnos.DataSource = _negocio.Listar();
            Grillas.Formato(dgvTurnos, "dd/MM/yyyy", "Hasta");
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void btnBuscarCliente_Click(object sender, EventArgs e)
    {
        try
        {
            _cliente = _clienteNegocio.BuscarPorDni(txtDni.Text);
            if (_cliente == null)
                Mensajes.Info("No hay ningún cliente con ese DNI. Registralo desde Clientes > Gestionar clientes.");
        }
        catch (Exception ex)
        {
            _cliente = null;
            Mensajes.Error(ex);
        }
        MostrarCliente();
    }

    private void txtDni_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;

        e.SuppressKeyPress = true;
        btnBuscarCliente.PerformClick();
    }

    private void MostrarCliente()
    {
        lblCliente.Text = _cliente == null ? "Buscá el cliente por DNI." : _cliente.NombreCompleto;
        lblCliente.ForeColor = _cliente == null ? SystemColors.GrayText : Color.DarkGreen;
    }

    private void btnCrear_Click(object sender, EventArgs e)
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
        if (cboDia.SelectedIndex < 0)
        {
            Mensajes.Info("Seleccioná el día de la semana.");
            return;
        }
        if (cboHorario.SelectedItem is not Horario horario)
        {
            Mensajes.Info("Seleccioná un horario.");
            return;
        }

        var turno = new TurnoFijo
        {
            IdCliente = _cliente.IdCliente,
            IdCancha = cancha.IdCancha,
            IdHorario = horario.IdHorario,
            DiaSemana = (int)Dias[cboDia.SelectedIndex]
        };

        try
        {
            var resultado = _negocio.Crear(turno, dtpDesde.Value.Date, (int)nudSemanas.Value);
            MostrarResultado("Turno fijo creado.", resultado);
            FrmPrincipal.ActualizarPantallas();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void btnGenerar_Click(object sender, EventArgs e)
    {
        TurnoFijoDetalle turno = Grillas.Seleccionado<TurnoFijoDetalle>(dgvTurnos);
        if (turno == null)
        {
            Mensajes.Info("Seleccioná un turno fijo de la grilla.");
            return;
        }

        try
        {
            var resultado = _negocio.GenerarSemanas(turno.IdTurnoFijo, (int)nudSemanas.Value);
            MostrarResultado("Se agregaron semanas al turno fijo.", resultado);
            FrmPrincipal.ActualizarPantallas();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void btnBaja_Click(object sender, EventArgs e)
    {
        TurnoFijoDetalle turno = Grillas.Seleccionado<TurnoFijoDetalle>(dgvTurnos);
        if (turno == null)
        {
            Mensajes.Info("Seleccioná un turno fijo de la grilla.");
            return;
        }

        string pregunta = $"¿Dar de baja el turno fijo de {turno.Cliente} ({turno.Dia} {turno.Horario}, {turno.Cancha})?\n\n" +
                          "Se cancelan las próximas reservas que no tengan pagos.";
        if (!Mensajes.Confirmar(pregunta))
            return;

        try
        {
            int canceladas = _negocio.DarDeBaja(turno.IdTurnoFijo);
            Mensajes.Info($"Turno fijo dado de baja. Reservas canceladas: {canceladas}.");
            FrmPrincipal.ActualizarPantallas();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private static void MostrarResultado(string titulo, (int Creadas, List<DateTime> Omitidas) resultado)
    {
        string texto = $"{titulo}\nReservas creadas: {resultado.Creadas}.";
        if (resultado.Omitidas.Count > 0)
            texto += "\n\nNo se reservaron estas fechas (ocupadas o ya pasadas): " +
                     string.Join(", ", resultado.Omitidas.Select(f => f.ToString("dd/MM")));
        Mensajes.Info(texto);
    }
}
