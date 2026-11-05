using System.ComponentModel;

namespace GestionCanchas.Entidades;

/// <summary>Totales de caja agrupados por método de pago o por usuario.</summary>
public class TotalCaja
{
    [DisplayName("Detalle")]
    public string Grupo { get; set; }

    public decimal Cobrado { get; set; }

    public decimal Devuelto { get; set; }

    public decimal Neto => Cobrado - Devuelto;

    public int Movimientos { get; set; }
}

public class IngresoDiario
{
    public DateTime Dia { get; set; }

    public decimal Neto { get; set; }
}

public class IndicadoresDia
{
    public int TurnosHoy { get; set; }

    public int TurnosOcupables { get; set; }

    public decimal CobradoHoy { get; set; }

    public decimal CobradoMes { get; set; }

    public decimal DeudaTotal { get; set; }

    public int ClientesConDeuda { get; set; }

    public decimal Ocupacion => TurnosOcupables == 0 ? 0 : (decimal)TurnosHoy / TurnosOcupables;
}
