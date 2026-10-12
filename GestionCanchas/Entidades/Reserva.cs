namespace GestionCanchas.Entidades;

public class Reserva
{
    public int IdReserva { get; set; }
    public int IdCliente { get; set; }
    public int IdCancha { get; set; }
    public int IdHorario { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Importe { get; set; }
    public string Estado { get; set; }
}
