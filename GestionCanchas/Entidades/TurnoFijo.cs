
namespace GestionCanchas.Entidades;

public class TurnoFijo
{
    public int IdTurnoFijo { get; set; }
    public int IdCliente { get; set; }
    public int IdCancha { get; set; }
    public int IdHorario { get; set; }

    public int DiaSemana { get; set; }

    public bool Activo { get; set; } = true;
}
