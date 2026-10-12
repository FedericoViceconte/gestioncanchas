using System.Data;
using GestionCanchas.Entidades;
using Microsoft.Data.SqlClient;

namespace GestionCanchas.Datos;

public class PagoDatos
{
    public List<Pago> ListarPorReserva(int idReserva)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            @"SELECT IdPago, IdReserva, FechaPago, Importe, MetodoPago
              FROM PAGOS
              WHERE IdReserva = @id
              ORDER BY FechaPago, IdPago", conexion);
        comando.Parameters.AddWithValue("@id", idReserva);

        var pagos = new List<Pago>();
        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            pagos.Add(new Pago
            {
                IdPago = (int)lector["IdPago"],
                IdReserva = (int)lector["IdReserva"],
                FechaPago = (DateTime)lector["FechaPago"],
                Importe = (decimal)lector["Importe"],
                MetodoPago = (string)lector["MetodoPago"]
            });
        }
        return pagos;
    }

    public List<PagoDetalle> Listar(string dni, DateTime desde, DateTime hasta)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            @"SELECT p.IdPago, p.IdReserva, p.FechaPago, p.Importe, p.MetodoPago,
                     r.Fecha AS FechaReserva, c.Apellido, c.Nombre, c.DNI, ca.Numero, ca.Tipo
              FROM PAGOS p
              JOIN RESERVAS r ON r.IdReserva = p.IdReserva
              JOIN CLIENTES c ON c.IdCliente = r.IdCliente
              JOIN CANCHAS ca ON ca.IdCancha = r.IdCancha
              WHERE p.FechaPago BETWEEN @desde AND @hasta
                AND (@dni IS NULL OR c.DNI = @dni)
              ORDER BY p.FechaPago DESC, p.IdPago DESC", conexion);
        comando.Parameters.Add("@desde", SqlDbType.Date).Value = desde.Date;
        comando.Parameters.Add("@hasta", SqlDbType.Date).Value = hasta.Date;
        comando.Parameters.Add("@dni", SqlDbType.VarChar, 8).Value = (object)dni ?? DBNull.Value;

        var pagos = new List<PagoDetalle>();
        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            pagos.Add(new PagoDetalle
            {
                IdPago = (int)lector["IdPago"],
                IdReserva = (int)lector["IdReserva"],
                FechaPago = (DateTime)lector["FechaPago"],
                Importe = (decimal)lector["Importe"],
                MetodoPago = (string)lector["MetodoPago"],
                FechaReserva = (DateTime)lector["FechaReserva"],
                Cliente = $"{lector["Apellido"]}, {lector["Nombre"]}",
                Dni = (string)lector["DNI"],
                Cancha = Cancha.Describir((int)lector["Numero"], (string)lector["Tipo"])
            });
        }
        return pagos;
    }

    // Transacción: el pago y el nuevo estado de la reserva se graban juntos
    public void Registrar(Pago pago, string estadoReserva)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        try
        {
            Insertar(pago, conexion, transaccion);

            using var comando = new SqlCommand(
                "UPDATE RESERVAS SET Estado = @estado WHERE IdReserva = @id", conexion, transaccion);
            comando.Parameters.AddWithValue("@estado", estadoReserva);
            comando.Parameters.AddWithValue("@id", pago.IdReserva);
            comando.ExecuteNonQuery();

            transaccion.Commit();
        }
        catch
        {
            transaccion.Rollback();
            throw;
        }
    }

    // Se usa dentro de transacciones abiertas por esta clase o por ReservaDatos
    internal static void Insertar(Pago pago, SqlConnection conexion, SqlTransaction transaccion)
    {
        using var comando = new SqlCommand(
            @"INSERT INTO PAGOS (IdReserva, FechaPago, Importe, MetodoPago)
              VALUES (@idReserva, @fecha, @importe, @metodo)", conexion, transaccion);
        comando.Parameters.AddWithValue("@idReserva", pago.IdReserva);
        comando.Parameters.Add("@fecha", SqlDbType.Date).Value = pago.FechaPago.Date;
        comando.Parameters.AddWithValue("@importe", pago.Importe);
        comando.Parameters.AddWithValue("@metodo", pago.MetodoPago);
        comando.ExecuteNonQuery();
    }
}
