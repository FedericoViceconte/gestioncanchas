using System.Globalization;
using System.Security.Cryptography;

namespace GestionCanchas.Negocio;

/// <summary>
/// Hash de contraseñas con PBKDF2-HMAC-SHA256 y una sal aleatoria por contraseña.
/// Formato guardado: <c>PBKDF2-SHA256$iteraciones$sal$hash</c>, con la sal y el hash en Base64.
/// </summary>
public static class HashClave
{
    private const string Algoritmo = "PBKDF2-SHA256";
    private const int TamanoSal = 16;
    private const int TamanoHash = 32;
    private const int IteracionesMaximas = 10_000_000;

    /// <summary>Iteraciones recomendadas por OWASP para PBKDF2-HMAC-SHA256.</summary>
    public const int Iteraciones = 600_000;

    public static string Calcular(string clave)
    {
        ArgumentNullException.ThrowIfNull(clave);
        byte[] sal = RandomNumberGenerator.GetBytes(TamanoSal);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(clave, sal, Iteraciones, HashAlgorithmName.SHA256, TamanoHash);
        return string.Join('$', Algoritmo, Iteraciones.ToString(CultureInfo.InvariantCulture),
            Convert.ToBase64String(sal), Convert.ToBase64String(hash));
    }

    public static bool Verificar(string clave, string almacenado)
    {
        if (clave == null || !TryLeer(almacenado, out int iteraciones, out byte[] sal, out byte[] esperado))
            return false;

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(clave, sal, iteraciones, HashAlgorithmName.SHA256, esperado.Length);
        return CryptographicOperations.FixedTimeEquals(hash, esperado);
    }

    /// <summary>Indica si el hash se calculó con menos iteraciones que las actuales y conviene recalcularlo.</summary>
    public static bool NecesitaActualizarse(string almacenado) =>
        !TryLeer(almacenado, out int iteraciones, out _, out _) || iteraciones < Iteraciones;

    private static bool TryLeer(string almacenado, out int iteraciones, out byte[] sal, out byte[] hash)
    {
        iteraciones = 0;
        sal = Array.Empty<byte>();
        hash = Array.Empty<byte>();

        string[] partes = almacenado?.Split('$');
        if (partes is not { Length: 4 } || partes[0] != Algoritmo)
            return false;
        if (!int.TryParse(partes[1], NumberStyles.None, CultureInfo.InvariantCulture, out iteraciones)
            || iteraciones <= 0 || iteraciones > IteracionesMaximas)
            return false;

        try
        {
            sal = Convert.FromBase64String(partes[2]);
            hash = Convert.FromBase64String(partes[3]);
        }
        catch (FormatException)
        {
            return false;
        }
        return sal.Length > 0 && hash.Length > 0;
    }
}
