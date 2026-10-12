using GestionCanchas.Entidades;
using Microsoft.Data.SqlClient;

namespace GestionCanchas.Datos;

public class CanchaDatos
{
    private const string SelectBase = "SELECT IdCancha, Numero, Tipo, PrecioHora, Estado FROM CANCHAS";

    public List<Cancha> Listar(bool soloDisponibles)
    {
        string sql = SelectBase;
        if (soloDisponibles)
            sql += " WHERE Estado = @estado";
        sql += " ORDER BY Numero";

        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@estado", EstadosCancha.Disponible);
        return Leer(comando);
    }

    public Cancha ObtenerPorId(int idCancha)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(SelectBase + " WHERE IdCancha = @id", conexion);
        comando.Parameters.AddWithValue("@id", idCancha);
        return Leer(comando).FirstOrDefault();
    }

    public bool ExisteNumero(int numero, int idCanchaExcluida)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            "SELECT COUNT(*) FROM CANCHAS WHERE Numero = @numero AND IdCancha <> @id", conexion);
        comando.Parameters.AddWithValue("@numero", numero);
        comando.Parameters.AddWithValue("@id", idCanchaExcluida);
        return (int)comando.ExecuteScalar() > 0;
    }

    public bool TieneReservas(int idCancha)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand("SELECT COUNT(*) FROM RESERVAS WHERE IdCancha = @id", conexion);
        comando.Parameters.AddWithValue("@id", idCancha);
        return (int)comando.ExecuteScalar() > 0;
    }

    public void Insertar(Cancha cancha)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            @"INSERT INTO CANCHAS (Numero, Tipo, PrecioHora, Estado)
              VALUES (@numero, @tipo, @precio, @estado)", conexion);
        CargarParametros(comando, cancha);
        comando.ExecuteNonQuery();
    }

    public void Modificar(Cancha cancha)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            @"UPDATE CANCHAS
              SET Numero = @numero, Tipo = @tipo, PrecioHora = @precio, Estado = @estado
              WHERE IdCancha = @id", conexion);
        CargarParametros(comando, cancha);
        comando.Parameters.AddWithValue("@id", cancha.IdCancha);
        comando.ExecuteNonQuery();
    }

    public void Eliminar(int idCancha)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand("DELETE FROM CANCHAS WHERE IdCancha = @id", conexion);
        comando.Parameters.AddWithValue("@id", idCancha);
        comando.ExecuteNonQuery();
    }

    private static void CargarParametros(SqlCommand comando, Cancha cancha)
    {
        comando.Parameters.AddWithValue("@numero", cancha.Numero);
        comando.Parameters.AddWithValue("@tipo", cancha.Tipo);
        comando.Parameters.AddWithValue("@precio", cancha.PrecioHora);
        comando.Parameters.AddWithValue("@estado", cancha.Estado);
    }

    private static List<Cancha> Leer(SqlCommand comando)
    {
        var canchas = new List<Cancha>();
        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            canchas.Add(new Cancha
            {
                IdCancha = (int)lector["IdCancha"],
                Numero = (int)lector["Numero"],
                Tipo = (string)lector["Tipo"],
                PrecioHora = (decimal)lector["PrecioHora"],
                Estado = (string)lector["Estado"]
            });
        }
        return canchas;
    }
}
