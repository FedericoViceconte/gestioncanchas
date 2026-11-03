using GestionCanchas.Entidades;
using Microsoft.Data.SqlClient;

namespace GestionCanchas.Datos;

public class HorarioDatos
{
    public List<Horario> Listar()
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            "SELECT IdHorario, HoraInicio, HoraFin FROM HORARIOS ORDER BY HoraInicio", conexion);
        return Leer(comando);
    }

    public Horario ObtenerPorId(int idHorario)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            "SELECT IdHorario, HoraInicio, HoraFin FROM HORARIOS WHERE IdHorario = @id", conexion);
        comando.Parameters.AddWithValue("@id", idHorario);
        return Leer(comando).FirstOrDefault();
    }

    public List<Horario> ListarLibres(int idCancha, DateTime fecha, int idReservaExcluida)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            @"SELECT h.IdHorario, h.HoraInicio, h.HoraFin
              FROM HORARIOS h
              WHERE NOT EXISTS (SELECT 1
                                FROM RESERVAS r
                                WHERE r.IdHorario = h.IdHorario
                                  AND r.IdCancha = @idCancha
                                  AND r.Fecha = @fecha
                                  AND r.Estado <> @cancelada
                                  AND r.IdReserva <> @idReserva)
              ORDER BY h.HoraInicio", conexion);
        comando.Parameters.AddWithValue("@idCancha", idCancha);
        comando.Parameters.AddWithValue("@fecha", fecha.Date);
        comando.Parameters.AddWithValue("@cancelada", EstadosReserva.Cancelada);
        comando.Parameters.AddWithValue("@idReserva", idReservaExcluida);
        return Leer(comando);
    }

    public bool SeSuperpone(TimeSpan inicio, TimeSpan fin)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            "SELECT COUNT(*) FROM HORARIOS WHERE @inicio < HoraFin AND @fin > HoraInicio", conexion);
        comando.Parameters.AddWithValue("@inicio", inicio);
        comando.Parameters.AddWithValue("@fin", fin);
        return (int)comando.ExecuteScalar() > 0;
    }

    public bool TieneReservas(int idHorario)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand("SELECT COUNT(*) FROM RESERVAS WHERE IdHorario = @id", conexion);
        comando.Parameters.AddWithValue("@id", idHorario);
        return (int)comando.ExecuteScalar() > 0;
    }

    public void Insertar(Horario horario)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            "INSERT INTO HORARIOS (HoraInicio, HoraFin) VALUES (@inicio, @fin)", conexion);
        comando.Parameters.AddWithValue("@inicio", horario.HoraInicio);
        comando.Parameters.AddWithValue("@fin", horario.HoraFin);
        comando.ExecuteNonQuery();
    }

    public void Eliminar(int idHorario)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand("DELETE FROM HORARIOS WHERE IdHorario = @id", conexion);
        comando.Parameters.AddWithValue("@id", idHorario);
        comando.ExecuteNonQuery();
    }

    private static List<Horario> Leer(SqlCommand comando)
    {
        var horarios = new List<Horario>();
        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            horarios.Add(new Horario
            {
                IdHorario = (int)lector["IdHorario"],
                HoraInicio = (TimeSpan)lector["HoraInicio"],
                HoraFin = (TimeSpan)lector["HoraFin"]
            });
        }
        return horarios;
    }
}
