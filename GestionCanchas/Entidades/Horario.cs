using System.ComponentModel;

namespace GestionCanchas.Entidades;

public class Horario
{
    [Browsable(false)]
    public int IdHorario { get; set; }

    [DisplayName("Desde")]
    public TimeSpan HoraInicio { get; set; }

    [DisplayName("Hasta")]
    public TimeSpan HoraFin { get; set; }

    [Browsable(false)]
    public decimal DuracionHoras => (decimal)(HoraFin - HoraInicio).TotalHours;

    public static string Describir(TimeSpan inicio, TimeSpan fin) => $"{inicio:hh\\:mm} a {fin:hh\\:mm}";

    public override string ToString() => Describir(HoraInicio, HoraFin);
}
