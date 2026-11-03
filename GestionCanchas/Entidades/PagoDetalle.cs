using System.ComponentModel;

namespace GestionCanchas.Entidades;

public class PagoDetalle
{
    [DisplayName("N° pago")]
    public int IdPago { get; set; }

    [DisplayName("Fecha de pago")]
    public DateTime FechaPago { get; set; }

    public string Cliente { get; set; }

    [DisplayName("DNI")]
    public string Dni { get; set; }

    [DisplayName("N° reserva")]
    public int IdReserva { get; set; }

    [DisplayName("Fecha reserva")]
    public DateTime FechaReserva { get; set; }

    public string Cancha { get; set; }

    public decimal Importe { get; set; }

    [DisplayName("Método")]
    public string MetodoPago { get; set; }

    public string Tipo { get; set; }

    public bool Anulado { get; set; }
}
