using System.ComponentModel;

namespace GestionCanchas.Entidades;

public class Cliente
{
    [Browsable(false)]
    public int IdCliente { get; set; }

    [DisplayName("DNI")]
    public string Dni { get; set; }

    public string Nombre { get; set; }

    public string Apellido { get; set; }

    [DisplayName("Teléfono")]
    public string Telefono { get; set; }

    public decimal Deuda { get; set; }

    [Browsable(false)]
    public string NombreCompleto => $"{Apellido}, {Nombre}";
}
