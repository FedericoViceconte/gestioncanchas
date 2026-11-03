using GestionCanchas.Entidades;
using Microsoft.Data.SqlClient;

namespace GestionCanchas.Datos;

public class ClienteDatos
{
    private const string SelectBase =
        "SELECT IdCliente, DNI, Nombre, Apellido, Telefono, Deuda FROM vw_DeudaClientes";

    public List<Cliente> Listar(bool soloConDeuda)
    {
        string sql = SelectBase;
        if (soloConDeuda)
            sql += " WHERE Deuda > 0";
        sql += " ORDER BY Apellido, Nombre";

        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(sql, conexion);
        return Leer(comando);
    }

    public Cliente BuscarPorDni(string dni)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(SelectBase + " WHERE DNI = @dni", conexion);
        comando.Parameters.AddWithValue("@dni", dni);
        return Leer(comando).FirstOrDefault();
    }

    public List<Cliente> Buscar(string texto)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(SelectBase +
            @" WHERE DNI LIKE @texto OR Nombre LIKE @texto OR Apellido LIKE @texto OR Telefono LIKE @texto
                  OR (Nombre + ' ' + Apellido) LIKE @texto OR (Apellido + ' ' + Nombre) LIKE @texto
               ORDER BY Apellido, Nombre", conexion);
        comando.Parameters.AddWithValue("@texto", "%" + texto + "%");
        return Leer(comando);
    }

    public Cliente ObtenerPorId(int idCliente)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(SelectBase + " WHERE IdCliente = @id", conexion);
        comando.Parameters.AddWithValue("@id", idCliente);
        return Leer(comando).FirstOrDefault();
    }

    public bool ExisteDni(string dni, int idClienteExcluido)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            "SELECT COUNT(*) FROM CLIENTES WHERE DNI = @dni AND IdCliente <> @id", conexion);
        comando.Parameters.AddWithValue("@dni", dni);
        comando.Parameters.AddWithValue("@id", idClienteExcluido);
        return (int)comando.ExecuteScalar() > 0;
    }

    public bool TieneReservas(int idCliente)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand("SELECT COUNT(*) FROM RESERVAS WHERE IdCliente = @id", conexion);
        comando.Parameters.AddWithValue("@id", idCliente);
        return (int)comando.ExecuteScalar() > 0;
    }

    public void Insertar(Cliente cliente)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            @"INSERT INTO CLIENTES (DNI, Nombre, Apellido, Telefono)
              VALUES (@dni, @nombre, @apellido, @telefono)", conexion);
        CargarParametros(comando, cliente);
        comando.ExecuteNonQuery();
    }

    public void Modificar(Cliente cliente)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            @"UPDATE CLIENTES
              SET DNI = @dni, Nombre = @nombre, Apellido = @apellido, Telefono = @telefono
              WHERE IdCliente = @id", conexion);
        CargarParametros(comando, cliente);
        comando.Parameters.AddWithValue("@id", cliente.IdCliente);
        comando.ExecuteNonQuery();
    }

    public void Eliminar(int idCliente)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand("DELETE FROM CLIENTES WHERE IdCliente = @id", conexion);
        comando.Parameters.AddWithValue("@id", idCliente);
        comando.ExecuteNonQuery();
    }

    private static void CargarParametros(SqlCommand comando, Cliente cliente)
    {
        comando.Parameters.AddWithValue("@dni", cliente.Dni);
        comando.Parameters.AddWithValue("@nombre", cliente.Nombre);
        comando.Parameters.AddWithValue("@apellido", cliente.Apellido);
        comando.Parameters.AddWithValue("@telefono",
            string.IsNullOrEmpty(cliente.Telefono) ? DBNull.Value : cliente.Telefono);
    }

    private static List<Cliente> Leer(SqlCommand comando)
    {
        var clientes = new List<Cliente>();
        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            clientes.Add(new Cliente
            {
                IdCliente = (int)lector["IdCliente"],
                Dni = (string)lector["DNI"],
                Nombre = (string)lector["Nombre"],
                Apellido = (string)lector["Apellido"],
                Telefono = lector["Telefono"] as string ?? "",
                Deuda = Convert.ToDecimal(lector["Deuda"])
            });
        }
        return clientes;
    }
}
