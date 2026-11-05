using GestionCanchas.Datos;
using GestionCanchas.Entidades;

namespace GestionCanchas.Negocio;

public class ReservaNegocio
{
    public const decimal PorcentajeSenaMinima = 0.15m;

    private readonly ReservaDatos _reservaDatos = new ReservaDatos();
    private readonly ClienteDatos _clienteDatos = new ClienteDatos();
    private readonly CanchaDatos _canchaDatos = new CanchaDatos();
    private readonly HorarioDatos _horarioDatos = new HorarioDatos();

    public List<ReservaDetalle> Listar(ReservaFiltro filtro)
    {
        if (!string.IsNullOrEmpty(filtro.Dni) && !ClienteNegocio.EsDniValido(filtro.Dni))
            throw new ValidacionException("El DNI debe tener 7 u 8 dígitos, sin puntos.");

        return _reservaDatos.Listar(filtro);
    }

    public ReservaDetalle ObtenerDetalle(int idReserva) => _reservaDatos.ObtenerDetalle(idReserva);

    public List<TurnoOcupado> ListarOcupados(DateTime fecha) => _reservaDatos.ListarOcupados(fecha);

    public List<Horario> ListarHorariosLibres(int idCancha, DateTime fecha, int idReservaExcluida)
    {
        List<Horario> libres = _horarioDatos.ListarLibres(idCancha, fecha, idReservaExcluida);
        if (fecha.Date != DateTime.Today)
            return libres;

        TimeSpan ahora = DateTime.Now.TimeOfDay;
        return libres.Where(h => h.HoraInicio > ahora).ToList();
    }

    public decimal CalcularImporte(Cancha cancha, Horario horario) =>
        Math.Round(cancha.PrecioHora * horario.DuracionHoras, 2);

    public decimal CalcularSenaMinima(decimal importe) =>
        Math.Round(importe * PorcentajeSenaMinima, 2);

    public int Registrar(Reserva reserva, Pago sena)
    {
        reserva.Importe = ValidarTurno(reserva);
        PagoNegocio.ValidarMetodo(sena.MetodoPago);

        decimal senaMinima = CalcularSenaMinima(reserva.Importe);
        if (sena.Importe < senaMinima)
            throw new ValidacionException(
                $"La seña mínima es {senaMinima:C2} (15% de {reserva.Importe:C2}).");
        if (sena.Importe > reserva.Importe)
            throw new ValidacionException("La seña no puede superar el importe de la reserva.");

        reserva.Fecha = reserva.Fecha.Date;
        reserva.Estado = sena.Importe == reserva.Importe ? EstadosReserva.Pagado : EstadosReserva.Senado;
        sena.FechaPago = DateTime.Today;
        sena.IdUsuario = Sesion.IdUsuario;

        return _reservaDatos.InsertarConSena(reserva, sena);
    }

    public void Modificar(Reserva reserva)
    {
        ReservaDetalle actual = _reservaDatos.ObtenerDetalle(reserva.IdReserva);
        if (actual == null)
            throw new ValidacionException("La reserva no existe.");
        if (actual.Estado == EstadosReserva.Cancelada)
            throw new ValidacionException("No se puede modificar una reserva cancelada.");
        if (actual.Estado == EstadosReserva.Ausente)
            throw new ValidacionException("No se puede modificar una reserva en la que el cliente no se presentó.");

        reserva.Importe = ValidarTurno(reserva);

        if (actual.TotalPagado > reserva.Importe)
            throw new ValidacionException(
                $"Ya se pagaron {actual.TotalPagado:C2}, más que el nuevo importe ({reserva.Importe:C2}).");

        reserva.Fecha = reserva.Fecha.Date;
        reserva.Estado = actual.TotalPagado == reserva.Importe ? EstadosReserva.Pagado
                       : actual.TotalPagado == 0 ? EstadosReserva.Pendiente
                       : EstadosReserva.Senado;
        _reservaDatos.Modificar(reserva);
    }

    public void Cancelar(int idReserva)
    {
        ReservaDetalle reserva = _reservaDatos.ObtenerDetalle(idReserva);
        if (reserva == null)
            throw new ValidacionException("La reserva no existe.");
        if (reserva.Estado == EstadosReserva.Cancelada)
            throw new ValidacionException("La reserva ya está cancelada.");
        if (reserva.Estado == EstadosReserva.Ausente)
            throw new ValidacionException("La reserva ya está marcada como ausente.");
        if (reserva.Fecha.Date < DateTime.Today)
            throw new ValidacionException("No se puede cancelar una reserva de una fecha pasada.");

        _reservaDatos.Cancelar(idReserva);
    }

    public void MarcarAusente(int idReserva)
    {
        ReservaDetalle reserva = _reservaDatos.ObtenerDetalle(idReserva);
        if (reserva == null)
            throw new ValidacionException("La reserva no existe.");
        if (reserva.Estado == EstadosReserva.Cancelada)
            throw new ValidacionException("La reserva está cancelada.");
        if (reserva.Estado == EstadosReserva.Ausente)
            throw new ValidacionException("La reserva ya está marcada como ausente.");

        Horario horario = _horarioDatos.ObtenerPorId(reserva.IdHorario);
        bool yaEmpezo = reserva.Fecha.Date < DateTime.Today ||
                        (reserva.Fecha.Date == DateTime.Today && horario.HoraInicio <= DateTime.Now.TimeOfDay);
        if (!yaEmpezo)
            throw new ValidacionException("Solo se puede marcar como ausente un turno que ya empezó.");

        _reservaDatos.MarcarAusente(idReserva);
    }

    private decimal ValidarTurno(Reserva reserva)
    {
        if (_clienteDatos.ObtenerPorId(reserva.IdCliente) == null)
            throw new ValidacionException("Seleccioná un cliente existente.");

        Cancha cancha = _canchaDatos.ObtenerPorId(reserva.IdCancha);
        if (cancha == null)
            throw new ValidacionException("Seleccioná una cancha.");
        if (cancha.Estado != EstadosCancha.Disponible)
            throw new ValidacionException($"La cancha {cancha.Numero} está en mantenimiento.");

        Horario horario = _horarioDatos.ObtenerPorId(reserva.IdHorario);
        if (horario == null)
            throw new ValidacionException("Seleccioná un horario.");

        if (reserva.Fecha.Date < DateTime.Today)
            throw new ValidacionException("No se puede reservar en una fecha pasada.");
        if (reserva.Fecha.Date == DateTime.Today && horario.HoraInicio <= DateTime.Now.TimeOfDay)
            throw new ValidacionException("Ese horario de hoy ya empezó.");

        if (_reservaDatos.TurnoOcupado(reserva.IdCancha, reserva.Fecha, reserva.IdHorario, reserva.IdReserva))
            throw new ValidacionException("La cancha ya está reservada en esa fecha y horario.");

        return CalcularImporte(cancha, horario);
    }
}
