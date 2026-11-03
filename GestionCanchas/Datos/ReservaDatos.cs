using System.Data;
using GestionCanchas.Entidades;
using Microsoft.Data.SqlClient;

namespace GestionCanchas.Datos;

public class ReservaDatos
{
    private const string SelectDetalle =
        @"SELECT s.IdReserva, s.IdCliente, s.IdCancha, s.IdHorario, s.Fecha,
                 s.Importe, s.TotalPagado, s.Saldo, s.Estado,
                 c.Apellido, c.Nombre, c.DNI,
                 ca.Numero, ca.Tipo,
                 h.HoraInicio, h.HoraFin
          FROM vw_SaldoReservas s
          JOIN CLIENTES c  ON c.IdCliente = s.IdCliente
          JOIN CANCHAS ca  ON ca.IdCancha = s.IdCancha
          JOIN HORARIOS h  ON h.IdHorario = s.IdHorario";

    public List<ReservaDetalle> Listar(ReservaFiltro filtro)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(SelectDetalle +
            @" WHERE (@fecha IS NULL OR s.Fecha = @fecha)
                 AND (@dni IS NULL OR c.DNI = @dni)
                 AND (@idCancha IS NULL OR s.IdCancha = @idCancha)
                 AND (@estado IS NULL OR s.Estado = @estado)
                 AND (@conSaldo = 0 OR (s.Saldo > 0 AND s.Estado IN ('Pendiente', 'Señado')))
               ORDER BY s.Fecha DESC, h.HoraInicio", conexion);

        comando.Parameters.Add("@fecha", SqlDbType.Date).Value = (object)filtro.Fecha ?? DBNull.Value;
        comando.Parameters.Add("@dni", SqlDbType.VarChar, 8).Value = (object)filtro.Dni ?? DBNull.Value;
        comando.Parameters.Add("@idCancha", SqlDbType.Int).Value = (object)filtro.IdCancha ?? DBNull.Value;
        comando.Parameters.Add("@estado", SqlDbType.VarChar, 20).Value = (object)filtro.Estado ?? DBNull.Value;
        comando.Parameters.Add("@conSaldo", SqlDbType.Bit).Value = filtro.SoloConSaldo;
        return LeerDetalle(comando);
    }

    public ReservaDetalle ObtenerDetalle(int idReserva)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(SelectDetalle + " WHERE s.IdReserva = @id", conexion);
        comando.Parameters.AddWithValue("@id", idReserva);
        return LeerDetalle(comando).FirstOrDefault();
    }

    public List<TurnoOcupado> ListarOcupados(DateTime fecha)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            @"SELECT r.IdCancha, r.IdHorario, r.Estado, c.Apellido, c.Nombre
              FROM RESERVAS r
              JOIN CLIENTES c ON c.IdCliente = r.IdCliente
              WHERE r.Fecha = @fecha AND r.Estado <> @cancelada", conexion);
        comando.Parameters.AddWithValue("@fecha", fecha.Date);
        comando.Parameters.AddWithValue("@cancelada", EstadosReserva.Cancelada);

        var turnos = new List<TurnoOcupado>();
        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            turnos.Add(new TurnoOcupado
            {
                IdCancha = (int)lector["IdCancha"],
                IdHorario = (int)lector["IdHorario"],
                Estado = (string)lector["Estado"],
                Cliente = $"{lector["Apellido"]}, {lector["Nombre"]}"
            });
        }
        return turnos;
    }

    public bool TurnoOcupado(int idCancha, DateTime fecha, int idHorario, int idReservaExcluida)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            @"SELECT COUNT(*) FROM RESERVAS
              WHERE IdCancha = @idCancha AND Fecha = @fecha AND IdHorario = @idHorario
                AND Estado <> @cancelada AND IdReserva <> @idReserva", conexion);
        comando.Parameters.AddWithValue("@idCancha", idCancha);
        comando.Parameters.AddWithValue("@fecha", fecha.Date);
        comando.Parameters.AddWithValue("@idHorario", idHorario);
        comando.Parameters.AddWithValue("@cancelada", EstadosReserva.Cancelada);
        comando.Parameters.AddWithValue("@idReserva", idReservaExcluida);
        return (int)comando.ExecuteScalar() > 0;
    }

    public int InsertarConSena(Reserva reserva, Pago sena)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        try
        {
            using var comando = new SqlCommand(
                @"INSERT INTO RESERVAS (IdCliente, IdCancha, IdHorario, Fecha, Importe, Estado)
                  OUTPUT INSERTED.IdReserva
                  VALUES (@idCliente, @idCancha, @idHorario, @fecha, @importe, @estado)",
                conexion, transaccion);
            CargarParametros(comando, reserva);
            int idReserva = (int)comando.ExecuteScalar();

            sena.IdReserva = idReserva;
            PagoDatos.Insertar(sena, conexion, transaccion);

            transaccion.Commit();
            return idReserva;
        }
        catch
        {
            transaccion.Rollback();
            throw;
        }
    }

    public void Modificar(Reserva reserva)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            @"UPDATE RESERVAS
              SET IdCliente = @idCliente, IdCancha = @idCancha, IdHorario = @idHorario,
                  Fecha = @fecha, Importe = @importe, Estado = @estado
              WHERE IdReserva = @id", conexion);
        CargarParametros(comando, reserva);
        comando.Parameters.AddWithValue("@id", reserva.IdReserva);
        comando.ExecuteNonQuery();
    }

    public void Cancelar(int idReserva)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            "UPDATE RESERVAS SET Estado = @cancelada WHERE IdReserva = @id", conexion);
        comando.Parameters.AddWithValue("@cancelada", EstadosReserva.Cancelada);
        comando.Parameters.AddWithValue("@id", idReserva);
        comando.ExecuteNonQuery();
    }

    public void MarcarAusente(int idReserva)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            "UPDATE RESERVAS SET Estado = @ausente WHERE IdReserva = @id", conexion);
        comando.Parameters.AddWithValue("@ausente", EstadosReserva.Ausente);
        comando.Parameters.AddWithValue("@id", idReserva);
        comando.ExecuteNonQuery();
    }

    private static void CargarParametros(SqlCommand comando, Reserva reserva)
    {
        comando.Parameters.AddWithValue("@idCliente", reserva.IdCliente);
        comando.Parameters.AddWithValue("@idCancha", reserva.IdCancha);
        comando.Parameters.AddWithValue("@idHorario", reserva.IdHorario);
        comando.Parameters.Add("@fecha", SqlDbType.Date).Value = reserva.Fecha.Date;
        comando.Parameters.AddWithValue("@importe", reserva.Importe);
        comando.Parameters.AddWithValue("@estado", reserva.Estado);
    }

    private static List<ReservaDetalle> LeerDetalle(SqlCommand comando)
    {
        var reservas = new List<ReservaDetalle>();
        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            reservas.Add(new ReservaDetalle
            {
                IdReserva = (int)lector["IdReserva"],
                IdCliente = (int)lector["IdCliente"],
                IdCancha = (int)lector["IdCancha"],
                IdHorario = (int)lector["IdHorario"],
                Fecha = (DateTime)lector["Fecha"],
                Horario = Horario.Describir((TimeSpan)lector["HoraInicio"], (TimeSpan)lector["HoraFin"]),
                Cancha = Cancha.Describir((int)lector["Numero"], (string)lector["Tipo"]),
                Cliente = $"{lector["Apellido"]}, {lector["Nombre"]}",
                Dni = (string)lector["DNI"],
                Importe = Convert.ToDecimal(lector["Importe"]),
                TotalPagado = Convert.ToDecimal(lector["TotalPagado"]),
                Saldo = Convert.ToDecimal(lector["Saldo"]),
                Estado = (string)lector["Estado"]
            });
        }
        return reservas;
    }
}
