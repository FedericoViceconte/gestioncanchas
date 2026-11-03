namespace GestionCanchas.Entidades;

public class ReservaFiltro
{
    public DateTime? Fecha { get; set; }
    public string Dni { get; set; }
    public int? IdCancha { get; set; }
    public string Estado { get; set; }

    public bool SoloConSaldo { get; set; }
}
