namespace GestionCanchas.Negocio;

// Error de regla de negocio: se muestra al usuario tal cual, como advertencia
public class ValidacionException : Exception
{
    public ValidacionException(string mensaje) : base(mensaje)
    {
    }
}
