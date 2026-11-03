
using System.ComponentModel;
using System.Globalization;

namespace GestionCanchas.Entidades;

public class TurnoFijoDetalle
{
    [DisplayName("N°")]
    public int IdTurnoFijo { get; set; }

    public string Cliente { get; set; }

    [DisplayName("DNI")]
    public string Dni { get; set; }

    public string Cancha { get; set; }

    [DisplayName("Día")]
    public string Dia { get; set; }

    public string Horario { get; set; }

    [DisplayName("Próximas reservas")]
    public int ReservasFuturas { get; set; }

    [DisplayName("Reservado hasta")]
    public DateTime? Hasta { get; set; }

    public bool Activo { get; set; }

    public static string NombreDia(int diaSemana)
    {
        string nombre = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName((DayOfWeek)diaSemana);
        return char.ToUpper(nombre[0]) + nombre.Substring(1);
    }
}
