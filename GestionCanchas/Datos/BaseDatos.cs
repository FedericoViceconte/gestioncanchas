using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace GestionCanchas.Datos;

public enum EstadoBase
{
    Lista,
    NoExiste,
    SinServidor
}

/// <summary>
/// Crea la base de datos con el script incluido en la aplicación y le aplica las actualizaciones del esquema.
/// </summary>
public static class BaseDatos
{
    private static readonly Regex SeparadorLotes =
        new(@"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    private static readonly Regex NombreValido = new(@"^[A-Za-z_][A-Za-z0-9_]{0,127}$", RegexOptions.CultureInvariant);

    public static EstadoBase Verificar()
    {
        try
        {
            using SqlConnection master = Conexion.AbrirMaster();
            using var comando = new SqlCommand("SELECT DB_ID(@nombre)", master);
            comando.Parameters.AddWithValue("@nombre", Conexion.NombreBase);
            return comando.ExecuteScalar() is null or DBNull ? EstadoBase.NoExiste : EstadoBase.Lista;
        }
        catch (SqlException)
        {
            return EstadoBase.SinServidor;
        }
    }

    /// <summary>
    /// Crea la base configurada con tablas, vistas y datos de ejemplo, y la deja actualizada.
    /// Una base existente solo se reemplaza si se pide de forma explícita.
    /// </summary>
    public static void Crear(bool reemplazarExistente = false)
    {
        string nombre = Conexion.NombreBase;
        if (!NombreValido.IsMatch(nombre))
            throw new InvalidOperationException($"El nombre de base de datos \"{nombre}\" no es válido.");
        if (!reemplazarExistente && Verificar() == EstadoBase.Lista)
            throw new InvalidOperationException($"La base de datos {nombre} ya existe y no se va a reemplazar.");

        string script = Regex.Replace(LeerScript("GestionCanchas.sql"), @"\bGestionCanchas\b", nombre);
        SqlConnection.ClearAllPools();
        using (SqlConnection master = Conexion.AbrirMaster())
            Ejecutar(script, master);
        SqlConnection.ClearAllPools();
        Actualizar();
    }

    /// <summary>Aplica las actualizaciones pendientes del esquema. Se puede llamar en cada inicio.</summary>
    public static void Actualizar()
    {
        using SqlConnection conexion = Conexion.Abrir();
        Ejecutar(LeerScript("Actualizaciones.sql"), conexion);
    }

    private static void Ejecutar(string script, SqlConnection conexion)
    {
        foreach (string lote in SeparadorLotes.Split(script))
        {
            if (string.IsNullOrWhiteSpace(lote))
                continue;

            using var comando = new SqlCommand(lote, conexion) { CommandTimeout = 120 };
            comando.ExecuteNonQuery();
        }
    }

    private static string LeerScript(string archivo)
    {
        string recurso = $"GestionCanchas.Scripts.{archivo}";
        using Stream flujo = typeof(BaseDatos).Assembly.GetManifestResourceStream(recurso)
            ?? throw new InvalidOperationException($"No se encontró el script {archivo} dentro de la aplicación.");
        using var lector = new StreamReader(flujo);
        return lector.ReadToEnd();
    }
}
