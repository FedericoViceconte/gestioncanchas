using GestionCanchas.Datos;
using GestionCanchas.Entidades;

namespace GestionCanchas.Negocio;

public class PagoNegocio
{
    private readonly PagoDatos _pagoDatos = new PagoDatos();
    private readonly ReservaDatos _reservaDatos = new ReservaDatos();

    public List<Pago> ListarPorReserva(int idReserva) => _pagoDatos.ListarPorReserva(idReserva);

    public List<PagoDetalle> Listar(string dni, DateTime desde, DateTime hasta)
    {
        dni = string.IsNullOrWhiteSpace(dni) ? null : dni.Trim();
        if (dni != null && !ClienteNegocio.EsDniValido(dni))
            throw new ValidacionException("El DNI debe tener 7 u 8 dígitos, sin puntos.");
        if (desde.Date > hasta.Date)
            throw new ValidacionException("La fecha 'desde' no puede ser posterior a 'hasta'.");

        return _pagoDatos.Listar(dni, desde, hasta);
    }

    public decimal Registrar(Pago pago)
    {
        ReservaDetalle reserva = _reservaDatos.ObtenerDetalle(pago.IdReserva);
        if (reserva == null)
            throw new ValidacionException("Seleccioná una reserva.");
        if (reserva.Estado == EstadosReserva.Cancelada)
            throw new ValidacionException("No se pueden registrar pagos en una reserva cancelada.");
        if (reserva.Estado == EstadosReserva.Ausente)
            throw new ValidacionException("No se pueden registrar pagos en una reserva marcada como ausente.");
        if (reserva.Saldo <= 0)
            throw new ValidacionException("La reserva ya está paga.");

        ValidarMetodo(pago.MetodoPago);
        if (pago.Importe <= 0)
            throw new ValidacionException("El importe debe ser mayor a cero.");
        if (pago.Importe > reserva.Saldo)
            throw new ValidacionException($"El importe supera el saldo pendiente ({reserva.Saldo:C2}).");

        pago.FechaPago = DateTime.Today;
        pago.IdUsuario = Sesion.IdUsuario;
        decimal saldoRestante = reserva.Saldo - pago.Importe;
        string estado = saldoRestante == 0 ? EstadosReserva.Pagado : EstadosReserva.Senado;

        _pagoDatos.Registrar(pago, estado);
        return saldoRestante;
    }

    public void Anular(int idPago)
    {
        Sesion.ExigirAdministrador("anular pagos");
        Pago pago = _pagoDatos.ObtenerPorId(idPago);
        if (pago == null)
            throw new ValidacionException("Seleccioná un pago.");
        if (pago.Anulado)
            throw new ValidacionException("Ese pago ya está anulado.");

        ReservaDetalle reserva = _reservaDatos.ObtenerDetalle(pago.IdReserva);
        decimal importe = pago.Tipo == TiposPago.Devolucion ? -pago.Importe : pago.Importe;
        decimal nuevoTotalPagado = reserva.TotalPagado - importe;
        if (nuevoTotalPagado < 0)
            throw new ValidacionException("Primero anulá la devolución de esta reserva.");

        string estado = reserva.Estado;
        if (estado == EstadosReserva.Pendiente || estado == EstadosReserva.Senado || estado == EstadosReserva.Pagado)
            estado = nuevoTotalPagado >= reserva.Importe ? EstadosReserva.Pagado
                   : nuevoTotalPagado == 0 ? EstadosReserva.Pendiente
                   : EstadosReserva.Senado;

        _pagoDatos.Anular(idPago, pago.IdReserva, estado);
    }

    public void RegistrarDevolucion(int idReserva, decimal importe, string metodo)
    {
        Sesion.ExigirAdministrador("registrar devoluciones");
        ReservaDetalle reserva = _reservaDatos.ObtenerDetalle(idReserva);
        if (reserva == null)
            throw new ValidacionException("La reserva no existe.");
        if (reserva.Estado != EstadosReserva.Cancelada)
            throw new ValidacionException("Solo se registran devoluciones de reservas canceladas.");
        if (reserva.TotalPagado <= 0)
            throw new ValidacionException("Esa reserva no tiene nada cobrado para devolver.");

        ValidarMetodo(metodo);
        if (importe <= 0)
            throw new ValidacionException("El importe debe ser mayor a cero.");
        if (importe > reserva.TotalPagado)
            throw new ValidacionException($"No se puede devolver más de lo cobrado ({reserva.TotalPagado:C2}).");

        var devolucion = new Pago
        {
            IdReserva = idReserva,
            FechaPago = DateTime.Today,
            Importe = importe,
            MetodoPago = metodo,
            Tipo = TiposPago.Devolucion,
            IdUsuario = Sesion.IdUsuario
        };
        _pagoDatos.Registrar(devolucion, EstadosReserva.Cancelada);
    }
    public static void ValidarMetodo(string metodo)
    {
        if (!MetodosPago.Todos.Contains(metodo))
            throw new ValidacionException("Seleccioná un método de pago válido.");
    }
}
