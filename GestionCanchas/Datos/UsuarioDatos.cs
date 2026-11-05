using GestionCanchas.Entidades;
using Microsoft.Data.SqlClient;

namespace GestionCanchas.Datos;

public class UsuarioDatos
{
    private const string SelectBase =
        "SELECT IdUsuario, NombreUsuario, NombreCompleto, ClaveHash, Rol, Activo, FechaAlta, UltimoAcceso FROM USUARIOS";

    public List<Usuario> Listar()
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(SelectBase + " ORDER BY Activo DESC, NombreUsuario", conexion);
        return Leer(comando);
    }

    public Usuario ObtenerPorId(int idUsuario)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(SelectBase + " WHERE IdUsuario = @id", conexion);
        comando.Parameters.AddWithValue("@id", idUsuario);
        return Leer(comando).FirstOrDefault();
    }

    public Usuario ObtenerPorNombre(string nombreUsuario)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(SelectBase + " WHERE NombreUsuario = @nombre", conexion);
        comando.Parameters.AddWithValue("@nombre", nombreUsuario);
        return Leer(comando).FirstOrDefault();
    }

    public int Contar()
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand("SELECT COUNT(*) FROM USUARIOS", conexion);
        return (int)comando.ExecuteScalar();
    }

    public int ContarAdministradoresActivos()
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            "SELECT COUNT(*) FROM USUARIOS WHERE Rol = @rol AND Activo = 1", conexion);
        comando.Parameters.AddWithValue("@rol", Roles.Administrador);
        return (int)comando.ExecuteScalar();
    }

    public bool ExisteNombre(string nombreUsuario, int idUsuarioExcluido)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            "SELECT COUNT(*) FROM USUARIOS WHERE NombreUsuario = @nombre AND IdUsuario <> @id", conexion);
        comando.Parameters.AddWithValue("@nombre", nombreUsuario);
        comando.Parameters.AddWithValue("@id", idUsuarioExcluido);
        return (int)comando.ExecuteScalar() > 0;
    }

    public void Insertar(Usuario usuario)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            @"INSERT INTO USUARIOS (NombreUsuario, NombreCompleto, ClaveHash, Rol, Activo)
              OUTPUT INSERTED.IdUsuario, INSERTED.FechaAlta
              VALUES (@nombre, @completo, @clave, @rol, @activo)", conexion);
        comando.Parameters.AddWithValue("@nombre", usuario.NombreUsuario);
        comando.Parameters.AddWithValue("@completo", usuario.NombreCompleto);
        comando.Parameters.AddWithValue("@clave", usuario.ClaveHash);
        comando.Parameters.AddWithValue("@rol", usuario.Rol);
        comando.Parameters.AddWithValue("@activo", usuario.Activo);
        using SqlDataReader lector = comando.ExecuteReader();
        lector.Read();
        usuario.IdUsuario = (int)lector["IdUsuario"];
        usuario.FechaAlta = (DateTime)lector["FechaAlta"];
    }

    public void Modificar(Usuario usuario)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            @"UPDATE USUARIOS SET NombreCompleto = @completo, Rol = @rol, Activo = @activo
              WHERE IdUsuario = @id", conexion);
        comando.Parameters.AddWithValue("@completo", usuario.NombreCompleto);
        comando.Parameters.AddWithValue("@rol", usuario.Rol);
        comando.Parameters.AddWithValue("@activo", usuario.Activo);
        comando.Parameters.AddWithValue("@id", usuario.IdUsuario);
        comando.ExecuteNonQuery();
    }

    public void CambiarClave(int idUsuario, string claveHash)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand("UPDATE USUARIOS SET ClaveHash = @clave WHERE IdUsuario = @id", conexion);
        comando.Parameters.AddWithValue("@clave", claveHash);
        comando.Parameters.AddWithValue("@id", idUsuario);
        comando.ExecuteNonQuery();
    }

    public void RegistrarAcceso(int idUsuario)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            "UPDATE USUARIOS SET UltimoAcceso = SYSDATETIME() WHERE IdUsuario = @id", conexion);
        comando.Parameters.AddWithValue("@id", idUsuario);
        comando.ExecuteNonQuery();
    }

    private static List<Usuario> Leer(SqlCommand comando)
    {
        var usuarios = new List<Usuario>();
        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            usuarios.Add(new Usuario
            {
                IdUsuario = (int)lector["IdUsuario"],
                NombreUsuario = (string)lector["NombreUsuario"],
                NombreCompleto = (string)lector["NombreCompleto"],
                ClaveHash = (string)lector["ClaveHash"],
                Rol = (string)lector["Rol"],
                Activo = (bool)lector["Activo"],
                FechaAlta = (DateTime)lector["FechaAlta"],
                UltimoAcceso = lector["UltimoAcceso"] as DateTime?
            });
        }
        return usuarios;
    }
}
