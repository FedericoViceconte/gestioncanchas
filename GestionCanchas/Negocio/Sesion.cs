using GestionCanchas.Entidades;

namespace GestionCanchas.Negocio;

/// <summary>Usuario que inició sesión en la aplicación y permisos según su rol.</summary>
public static class Sesion
{
    public static Usuario Actual { get; private set; }

    public static bool Iniciada => Actual != null;

    public static bool EsAdministrador => Actual?.EsAdministrador == true;

    public static int? IdUsuario => Actual?.IdUsuario;

    internal static void Iniciar(Usuario usuario) => Actual = usuario;

    public static void Cerrar() => Actual = null;

    /// <summary>Corta la operación si el usuario actual no es administrador.</summary>
    /// <param name="accion">Lo que se intentó hacer, en infinitivo: "eliminar canchas".</param>
    public static void ExigirAdministrador(string accion)
    {
        if (!EsAdministrador)
            throw new AccesoDenegadoException($"Solo un administrador puede {accion}.");
    }
}

public class AccesoDenegadoException : ValidacionException
{
    public AccesoDenegadoException(string mensaje) : base(mensaje)
    {
    }
}
