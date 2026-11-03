using System.ComponentModel;

namespace GestionCanchas.Entidades;

public class Pago
{
    [DisplayName("N° pago")]
    public int IdPago { get; set; }

    [Browsable(false)]
    public int IdReserva { get; set; }

    [DisplayName("Fecha de pago")]
    public DateTime FechaPago { get; set; }

    public decimal Importe { get; set; }

    [DisplayName("Método")]
    public string MetodoPago { get; set; }

    public string Tipo { get; set; } = TiposPago.Pago;

    public bool Anulado { get; set; }
}
