
using GestionCanchas.Datos;
using GestionCanchas.Entidades;

namespace GestionCanchas.Negocio;

public class TurnoFijoNegocio
{
    public const int MaximoSemanas = 26;

    private readonly TurnoFijoDatos _datos = new TurnoFijoDatos();
    private readonly ReservaDatos _reservaDatos = new ReservaDatos();
    private readonly ClienteDatos _clienteDatos = new ClienteDatos();
    private readonly CanchaDatos _canchaDatos = new CanchaDatos();
    private readonly HorarioDatos _horarioDatos = new HorarioDatos();

    public List<TurnoFijoDetalle> Listar() => _datos.Listar();

    public (int Creadas, List<DateTime> Omitidas) Crear(TurnoFijo turno, DateTime desde, int semanas)
    {
        if (_clienteDatos.ObtenerPorId(turno.IdCliente) == null)
            throw new ValidacionException("Buscá y seleccioná el cliente por DNI.");
        if (turno.DiaSemana < 0 || turno.DiaSemana > 6)
            throw new ValidacionException("Seleccioná el día de la semana.");
        if (desde.Date < DateTime.Today)
            throw new ValidacionException("La fecha de inicio no puede ser pasada.");
        if (_datos.ExisteActivo(turno.IdCancha, turno.IdHorario, turno.DiaSemana))
            throw new ValidacionException("Ya hay un turno fijo activo en esa cancha, día y horario.");

        turno.IdTurnoFijo = 0;
        return Generar(turno, desde, semanas);
    }

    public (int Creadas, List<DateTime> Omitidas) GenerarSemanas(int idTurnoFijo, int semanas)
    {
        TurnoFijo turno = _datos.ObtenerPorId(idTurnoFijo);
        if (turno == null)
            throw new ValidacionException("Seleccioná un turno fijo.");
        if (!turno.Activo)
            throw new ValidacionException("El turno fijo está dado de baja.");

        DateTime? ultima = _datos.UltimaFecha(idTurnoFijo);
        DateTime desde = ultima.HasValue && ultima.Value.AddDays(1) > DateTime.Today
            ? ultima.Value.AddDays(1)
            : DateTime.Today;
        return Generar(turno, desde, semanas);
    }

    public int DarDeBaja(int idTurnoFijo)
    {
        TurnoFijo turno = _datos.ObtenerPorId(idTurnoFijo);
        if (turno == null)
            throw new ValidacionException("Seleccioná un turno fijo.");
        if (!turno.Activo)
            throw new ValidacionException("El turno fijo ya está dado de baja.");

        return _datos.DarDeBaja(idTurnoFijo);
    }

    private (int Creadas, List<DateTime> Omitidas) Generar(TurnoFijo turno, DateTime desde, int semanas)
    {
        if (semanas < 1 || semanas > MaximoSemanas)
            throw new ValidacionException($"Elegí entre 1 y {MaximoSemanas} semanas.");

        Cancha cancha = _canchaDatos.ObtenerPorId(turno.IdCancha);
        if (cancha == null)
            throw new ValidacionException("Seleccioná una cancha.");
        if (cancha.Estado != EstadosCancha.Disponible)
            throw new ValidacionException($"La cancha {cancha.Numero} está en mantenimiento.");

        Horario horario = _horarioDatos.ObtenerPorId(turno.IdHorario);
        if (horario == null)
            throw new ValidacionException("Seleccioná un horario.");

        DateTime fecha = desde.Date;
        while ((int)fecha.DayOfWeek != turno.DiaSemana)
            fecha = fecha.AddDays(1);

        decimal importe = Math.Round(cancha.PrecioHora * horario.DuracionHoras, 2);
        var reservas = new List<Reserva>();
        var omitidas = new List<DateTime>();

        for (int i = 0; i < semanas; i++, fecha = fecha.AddDays(7))
        {
            bool yaPaso = fecha < DateTime.Today ||
                          (fecha == DateTime.Today && horario.HoraInicio <= DateTime.Now.TimeOfDay);
            if (yaPaso || _reservaDatos.TurnoOcupado(turno.IdCancha, fecha, turno.IdHorario, 0))
            {
                omitidas.Add(fecha);
                continue;
            }

            reservas.Add(new Reserva
            {
                IdCliente = turno.IdCliente,
                IdCancha = turno.IdCancha,
                IdHorario = turno.IdHorario,
                Fecha = fecha,
                Importe = importe,
                Estado = EstadosReserva.Pendiente
            });
        }

        if (reservas.Count == 0)
            throw new ValidacionException("Todas esas fechas ya están ocupadas; no se creó ninguna reserva.");

        _datos.Guardar(turno, reservas);
        return (reservas.Count, omitidas);
    }
}
