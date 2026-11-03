using System.ComponentModel;

namespace GestionCanchas.Entidades;

public class ReservaDetalle
{
    [DisplayName("N°")]
    public int IdReserva { get; set; }

    [Browsable(false)]
    public int IdCliente { get; set; }

    [Browsable(false)]
    public int IdCancha { get; set; }

    [Browsable(false)]
    public int IdHorario { get; set; }

    public DateTime Fecha { get; set; }

    public string Horario { get; set; }

    public string Cancha { get; set; }

    public string Cliente { get; set; }

    [DisplayName("DNI")]
    public string Dni { get; set; }

    public decimal Importe { get; set; }

    [DisplayName("Pagado")]
    public decimal TotalPagado { get; set; }

    public decimal Saldo { get; set; }

    public string Estado { get; set; }
}
