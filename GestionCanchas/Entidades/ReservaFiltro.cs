namespace GestionCanchas.Entidades;

// Los criterios en null no filtran
public class ReservaFiltro
{
    public DateTime? Fecha { get; set; }
    public string Dni { get; set; }
    public int? IdCancha { get; set; }
    public string Estado { get; set; }
}
