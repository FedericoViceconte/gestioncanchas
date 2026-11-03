
using System.Data;
using GestionCanchas.Entidades;
using Microsoft.Data.SqlClient;

namespace GestionCanchas.Datos;

public class TurnoFijoDatos
{
    public List<TurnoFijoDetalle> Listar()
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            @"SELECT t.IdTurnoFijo, t.DiaSemana, t.Activo,
                     c.Apellido, c.Nombre, c.DNI, ca.Numero, ca.Tipo, h.HoraInicio, h.HoraFin,
                     (SELECT COUNT(*) FROM RESERVAS r
                      WHERE r.IdTurnoFijo = t.IdTurnoFijo AND r.Fecha >= @hoy AND r.Estado <> @cancelada) AS Futuras,
                     (SELECT MAX(r.Fecha) FROM RESERVAS r
                      WHERE r.IdTurnoFijo = t.IdTurnoFijo AND r.Estado <> @cancelada) AS Hasta
              FROM TURNOS_FIJOS t
              JOIN CLIENTES c  ON c.IdCliente = t.IdCliente
              JOIN CANCHAS ca  ON ca.IdCancha = t.IdCancha
              JOIN HORARIOS h  ON h.IdHorario = t.IdHorario
              ORDER BY t.Activo DESC, t.DiaSemana, h.HoraInicio", conexion);
        comando.Parameters.Add("@hoy", SqlDbType.Date).Value = DateTime.Today;
        comando.Parameters.AddWithValue("@cancelada", EstadosReserva.Cancelada);

        var turnos = new List<TurnoFijoDetalle>();
        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            turnos.Add(new TurnoFijoDetalle
            {
                IdTurnoFijo = (int)lector["IdTurnoFijo"],
                Cliente = $"{lector["Apellido"]}, {lector["Nombre"]}",
                Dni = (string)lector["DNI"],
                Cancha = Cancha.Describir((int)lector["Numero"], (string)lector["Tipo"]),
                Dia = TurnoFijoDetalle.NombreDia(Convert.ToInt32(lector["DiaSemana"])),
                Horario = Horario.Describir((TimeSpan)lector["HoraInicio"], (TimeSpan)lector["HoraFin"]),
                ReservasFuturas = (int)lector["Futuras"],
                Hasta = lector["Hasta"] as DateTime?,
                Activo = (bool)lector["Activo"]
            });
        }
        return turnos;
    }

    public TurnoFijo ObtenerPorId(int idTurnoFijo)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            "SELECT IdTurnoFijo, IdCliente, IdCancha, IdHorario, DiaSemana, Activo FROM TURNOS_FIJOS WHERE IdTurnoFijo = @id",
            conexion);
        comando.Parameters.AddWithValue("@id", idTurnoFijo);
        using SqlDataReader lector = comando.ExecuteReader();
        if (!lector.Read())
            return null;

        return new TurnoFijo
        {
            IdTurnoFijo = (int)lector["IdTurnoFijo"],
            IdCliente = (int)lector["IdCliente"],
            IdCancha = (int)lector["IdCancha"],
            IdHorario = (int)lector["IdHorario"],
            DiaSemana = Convert.ToInt32(lector["DiaSemana"]),
            Activo = (bool)lector["Activo"]
        };
    }

    public bool ExisteActivo(int idCancha, int idHorario, int diaSemana)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            @"SELECT COUNT(*) FROM TURNOS_FIJOS
              WHERE Activo = 1 AND IdCancha = @idCancha AND IdHorario = @idHorario AND DiaSemana = @dia", conexion);
        comando.Parameters.AddWithValue("@idCancha", idCancha);
        comando.Parameters.AddWithValue("@idHorario", idHorario);
        comando.Parameters.AddWithValue("@dia", diaSemana);
        return (int)comando.ExecuteScalar() > 0;
    }

    public DateTime? UltimaFecha(int idTurnoFijo)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            "SELECT MAX(Fecha) FROM RESERVAS WHERE IdTurnoFijo = @id AND Estado <> @cancelada", conexion);
        comando.Parameters.AddWithValue("@id", idTurnoFijo);
        comando.Parameters.AddWithValue("@cancelada", EstadosReserva.Cancelada);
        return comando.ExecuteScalar() as DateTime?;
    }

    public void Guardar(TurnoFijo turno, List<Reserva> reservas)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        try
        {
            if (turno.IdTurnoFijo == 0)
            {
                using var alta = new SqlCommand(
                    @"INSERT INTO TURNOS_FIJOS (IdCliente, IdCancha, IdHorario, DiaSemana)
                      OUTPUT INSERTED.IdTurnoFijo
                      VALUES (@idCliente, @idCancha, @idHorario, @dia)", conexion, transaccion);
                alta.Parameters.AddWithValue("@idCliente", turno.IdCliente);
                alta.Parameters.AddWithValue("@idCancha", turno.IdCancha);
                alta.Parameters.AddWithValue("@idHorario", turno.IdHorario);
                alta.Parameters.AddWithValue("@dia", turno.DiaSemana);
                turno.IdTurnoFijo = (int)alta.ExecuteScalar();
            }

            foreach (Reserva reserva in reservas)
            {
                using var comando = new SqlCommand(
                    @"INSERT INTO RESERVAS (IdCliente, IdCancha, IdHorario, Fecha, Importe, Estado, IdTurnoFijo)
                      VALUES (@idCliente, @idCancha, @idHorario, @fecha, @importe, @estado, @idTurno)",
                    conexion, transaccion);
                comando.Parameters.AddWithValue("@idCliente", reserva.IdCliente);
                comando.Parameters.AddWithValue("@idCancha", reserva.IdCancha);
                comando.Parameters.AddWithValue("@idHorario", reserva.IdHorario);
                comando.Parameters.Add("@fecha", SqlDbType.Date).Value = reserva.Fecha.Date;
                comando.Parameters.AddWithValue("@importe", reserva.Importe);
                comando.Parameters.AddWithValue("@estado", reserva.Estado);
                comando.Parameters.AddWithValue("@idTurno", turno.IdTurnoFijo);
                comando.ExecuteNonQuery();
            }

            transaccion.Commit();
        }
        catch
        {
            transaccion.Rollback();
            throw;
        }
    }

    public int DarDeBaja(int idTurnoFijo)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        try
        {
            using var cancelar = new SqlCommand(
                @"UPDATE r SET Estado = @cancelada
                  FROM RESERVAS r
                  JOIN vw_SaldoReservas s ON s.IdReserva = r.IdReserva
                  WHERE r.IdTurnoFijo = @id AND r.Fecha >= @hoy
                    AND r.Estado = @pendiente AND s.TotalPagado = 0", conexion, transaccion);
            cancelar.Parameters.AddWithValue("@cancelada", EstadosReserva.Cancelada);
            cancelar.Parameters.AddWithValue("@pendiente", EstadosReserva.Pendiente);
            cancelar.Parameters.AddWithValue("@id", idTurnoFijo);
            cancelar.Parameters.Add("@hoy", SqlDbType.Date).Value = DateTime.Today;
            int canceladas = cancelar.ExecuteNonQuery();

            using var baja = new SqlCommand("UPDATE TURNOS_FIJOS SET Activo = 0 WHERE IdTurnoFijo = @id", conexion, transaccion);
            baja.Parameters.AddWithValue("@id", idTurnoFijo);
            baja.ExecuteNonQuery();

            transaccion.Commit();
            return canceladas;
        }
        catch
        {
            transaccion.Rollback();
            throw;
        }
    }
}
