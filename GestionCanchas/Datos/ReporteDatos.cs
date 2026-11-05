using System.Data;
using GestionCanchas.Entidades;
using Microsoft.Data.SqlClient;

namespace GestionCanchas.Datos;

public class ReporteDatos
{
    private const string Totales =
        @"SUM(CASE WHEN p.Tipo = 'Pago' THEN p.Importe ELSE 0 END) AS Cobrado,
          SUM(CASE WHEN p.Tipo = 'Devolución' THEN p.Importe ELSE 0 END) AS Devuelto,
          COUNT(*) AS Movimientos";

    public List<TotalCaja> CajaPorMetodo(DateTime desde, DateTime hasta) =>
        LeerTotales($@"SELECT p.MetodoPago AS Grupo, {Totales}
                       FROM PAGOS p
                       WHERE p.Anulado = 0 AND p.FechaPago BETWEEN @desde AND @hasta
                       GROUP BY p.MetodoPago
                       ORDER BY p.MetodoPago", desde, hasta);

    public List<TotalCaja> CajaPorUsuario(DateTime desde, DateTime hasta) =>
        LeerTotales($@"SELECT ISNULL(u.NombreCompleto, 'Sin usuario') AS Grupo, {Totales}
                       FROM PAGOS p
                       LEFT JOIN USUARIOS u ON u.IdUsuario = p.IdUsuario
                       WHERE p.Anulado = 0 AND p.FechaPago BETWEEN @desde AND @hasta
                       GROUP BY ISNULL(u.NombreCompleto, 'Sin usuario')
                       ORDER BY Grupo", desde, hasta);

    public List<IngresoDiario> IngresosPorDia(DateTime desde, DateTime hasta)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            @"SELECT FechaPago,
                     SUM(CASE WHEN Tipo = 'Devolución' THEN -Importe ELSE Importe END) AS Neto
              FROM PAGOS
              WHERE Anulado = 0 AND FechaPago BETWEEN @desde AND @hasta
              GROUP BY FechaPago", conexion);
        AgregarRango(comando, desde, hasta);

        var porDia = new Dictionary<DateTime, decimal>();
        using (SqlDataReader lector = comando.ExecuteReader())
        {
            while (lector.Read())
                porDia[(DateTime)lector["FechaPago"]] = (decimal)lector["Neto"];
        }

        var dias = new List<IngresoDiario>();
        for (DateTime dia = desde.Date; dia <= hasta.Date; dia = dia.AddDays(1))
            dias.Add(new IngresoDiario { Dia = dia, Neto = porDia.GetValueOrDefault(dia) });
        return dias;
    }

    public IndicadoresDia Indicadores(DateTime dia)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(
            @"SELECT
                (SELECT COUNT(*) FROM RESERVAS WHERE Fecha = @dia AND Estado <> 'Cancelada') AS TurnosHoy,
                (SELECT COUNT(*) FROM CANCHAS WHERE Estado = 'Disponible')
                  * (SELECT COUNT(*) FROM HORARIOS) AS TurnosOcupables,
                (SELECT ISNULL(SUM(CASE WHEN Tipo = 'Devolución' THEN -Importe ELSE Importe END), 0)
                   FROM PAGOS WHERE Anulado = 0 AND FechaPago = @dia) AS CobradoHoy,
                (SELECT ISNULL(SUM(CASE WHEN Tipo = 'Devolución' THEN -Importe ELSE Importe END), 0)
                   FROM PAGOS WHERE Anulado = 0 AND FechaPago BETWEEN @inicioMes AND @dia) AS CobradoMes,
                (SELECT ISNULL(SUM(Deuda), 0) FROM vw_DeudaClientes WHERE Deuda > 0) AS DeudaTotal,
                (SELECT COUNT(*) FROM vw_DeudaClientes WHERE Deuda > 0) AS ClientesConDeuda", conexion);
        comando.Parameters.Add("@dia", SqlDbType.Date).Value = dia.Date;
        comando.Parameters.Add("@inicioMes", SqlDbType.Date).Value = new DateTime(dia.Year, dia.Month, 1);

        using SqlDataReader lector = comando.ExecuteReader();
        lector.Read();
        return new IndicadoresDia
        {
            TurnosHoy = (int)lector["TurnosHoy"],
            TurnosOcupables = (int)lector["TurnosOcupables"],
            CobradoHoy = (decimal)lector["CobradoHoy"],
            CobradoMes = (decimal)lector["CobradoMes"],
            DeudaTotal = (decimal)lector["DeudaTotal"],
            ClientesConDeuda = (int)lector["ClientesConDeuda"]
        };
    }

    private static List<TotalCaja> LeerTotales(string sql, DateTime desde, DateTime hasta)
    {
        using SqlConnection conexion = Conexion.Abrir();
        using var comando = new SqlCommand(sql, conexion);
        AgregarRango(comando, desde, hasta);

        var totales = new List<TotalCaja>();
        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            totales.Add(new TotalCaja
            {
                Grupo = (string)lector["Grupo"],
                Cobrado = (decimal)lector["Cobrado"],
                Devuelto = (decimal)lector["Devuelto"],
                Movimientos = (int)lector["Movimientos"]
            });
        }
        return totales;
    }

    private static void AgregarRango(SqlCommand comando, DateTime desde, DateTime hasta)
    {
        comando.Parameters.Add("@desde", SqlDbType.Date).Value = desde.Date;
        comando.Parameters.Add("@hasta", SqlDbType.Date).Value = hasta.Date;
    }
}
