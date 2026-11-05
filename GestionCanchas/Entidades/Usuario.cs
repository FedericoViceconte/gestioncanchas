using System.ComponentModel;

namespace GestionCanchas.Entidades;

public class Usuario
{
    [Browsable(false)]
    public int IdUsuario { get; set; }

    [DisplayName("Usuario")]
    public string NombreUsuario { get; set; }

    [DisplayName("Nombre")]
    public string NombreCompleto { get; set; }

    public string Rol { get; set; } = Roles.Recepcionista;

    public bool Activo { get; set; } = true;

    [DisplayName("Alta")]
    public DateTime FechaAlta { get; set; }

    [DisplayName("Último acceso")]
    public DateTime? UltimoAcceso { get; set; }

    [Browsable(false)]
    public string ClaveHash { get; set; }

    [Browsable(false)]
    public bool EsAdministrador => Rol == Roles.Administrador;
}
