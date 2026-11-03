using System.ComponentModel;

namespace GestionCanchas.Entidades;

public class Cancha
{
    [Browsable(false)]
    public int IdCancha { get; set; }

    [DisplayName("Número")]
    public int Numero { get; set; }

    public string Tipo { get; set; }

    [DisplayName("Precio por hora")]
    public decimal PrecioHora { get; set; }

    public string Estado { get; set; }

    public static string Describir(int numero, string tipo) => $"Cancha {numero} - {tipo}";

    public override string ToString() => Describir(Numero, Tipo);
}
