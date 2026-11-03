using Microsoft.Data.SqlClient;

namespace GestionCanchas.Datos;

public static class Conexion
{
    private const string Cadena =
        @"Server=(localdb)\MSSQLLocalDB;Database=GestionCanchas;Integrated Security=True;TrustServerCertificate=True";

    public static SqlConnection Abrir()
    {
        var conexion = new SqlConnection(Cadena);
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

    public static void Probar()
    {
        using SqlConnection conexion = Abrir();
    }
}
