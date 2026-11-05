using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace GestionCanchas.Datos;

/// <summary>
/// Cadena de conexión a SQL Server. Se toma, en este orden, de la variable de entorno
/// GESTIONCANCHAS_CONEXION, de appsettings.json junto al ejecutable o de LocalDB por defecto.
/// </summary>
public static class Conexion
{
    public const string VariableEntorno = "GESTIONCANCHAS_CONEXION";
    public const string ArchivoConfiguracion = "appsettings.json";

    private const string CadenaPredeterminada =
        @"Server=(localdb)\MSSQLLocalDB;Database=GestionCanchas;Integrated Security=True;TrustServerCertificate=True";

    private static string _cadena;

    public static string Cadena
    {
        get => _cadena ??= LeerCadena();
        internal set => _cadena = value;
    }

    public static string NombreBase => new SqlConnectionStringBuilder(Cadena).InitialCatalog;

    public static string Servidor => new SqlConnectionStringBuilder(Cadena).DataSource;

    public static SqlConnection Abrir() => Abrir(Cadena);

    /// <summary>Abre una conexión al mismo servidor, pero sobre la base master.</summary>
    public static SqlConnection AbrirMaster()
    {
        var builder = new SqlConnectionStringBuilder(Cadena) { InitialCatalog = "master" };
        return Abrir(builder.ConnectionString);
    }

    public static void Probar()
    {
        using SqlConnection conexion = Abrir();
    }

    private static SqlConnection Abrir(string cadena)
    {
        var conexion = new SqlConnection(cadena);
        try
        {
            conexion.Open();
            return conexion;
        }
        catch
        {
            conexion.Dispose();
            throw;
        }
    }

    private static string LeerCadena()
    {
        string variable = Environment.GetEnvironmentVariable(VariableEntorno);
        if (!string.IsNullOrWhiteSpace(variable))
            return variable;

        string archivo = Path.Combine(AppContext.BaseDirectory, ArchivoConfiguracion);
        if (!File.Exists(archivo))
            return CadenaPredeterminada;

        try
        {
            using JsonDocument json = JsonDocument.Parse(File.ReadAllText(archivo));
            if (json.RootElement.TryGetProperty("ConnectionStrings", out JsonElement cadenas)
                && cadenas.TryGetProperty("GestionCanchas", out JsonElement cadena)
                && !string.IsNullOrWhiteSpace(cadena.GetString()))
                return cadena.GetString();
            return CadenaPredeterminada;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException(
                $"El archivo {ArchivoConfiguracion} tiene un error de formato (línea {ex.LineNumber + 1}). " +
                @"Recordá escribir las barras invertidas dobles, por ejemplo (localdb)\\MSSQLLocalDB.", ex);
        }
    }
}
