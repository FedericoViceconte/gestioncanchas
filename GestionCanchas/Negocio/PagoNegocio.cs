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

    // Devuelve el saldo que queda después del pago
    public decimal Registrar(Pago pago)
    {
        ReservaDetalle reserva = _reservaDatos.ObtenerDetalle(pago.IdReserva);
        if (reserva == null)
            throw new ValidacionException("Seleccioná una reserva.");
        if (reserva.Estado == EstadosReserva.Cancelada)
            throw new ValidacionException("No se pueden registrar pagos en una reserva cancelada.");
        if (reserva.Saldo <= 0)
            throw new ValidacionException("La reserva ya está paga.");

        ValidarMetodo(pago.MetodoPago);
        if (pago.Importe <= 0)
            throw new ValidacionException("El importe debe ser mayor a cero.");
        if (pago.Importe > reserva.Saldo)
            throw new ValidacionException($"El importe supera el saldo pendiente ({reserva.Saldo:C2}).");

        pago.FechaPago = DateTime.Today;
        decimal saldoRestante = reserva.Saldo - pago.Importe;
        string estado = saldoRestante == 0 ? EstadosReserva.Pagado : EstadosReserva.Senado;

        _pagoDatos.Registrar(pago, estado);
        return saldoRestante;
    }

    public static void ValidarMetodo(string metodo)
    {
        if (!MetodosPago.Todos.Contains(metodo))
            throw new ValidacionException("Seleccioná un método de pago válido.");
    }
}
