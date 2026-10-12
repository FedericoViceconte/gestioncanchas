namespace GestionCanchas.Entidades;

public static class EstadosReserva
{
    public const string Senado = "Señado";
    public const string Pagado = "Pagado";
    public const string Cancelada = "Cancelada";

    public static readonly string[] Todos = { Senado, Pagado, Cancelada };
}

public static class EstadosCancha
{
    public const string Disponible = "Disponible";
    public const string Mantenimiento = "Mantenimiento";

    public static readonly string[] Todos = { Disponible, Mantenimiento };
}

public static class MetodosPago
{
    public const string Efectivo = "Efectivo";
    public const string Transferencia = "Transferencia";
    public const string Tarjeta = "Tarjeta";

    public static readonly string[] Todos = { Efectivo, Transferencia, Tarjeta };
}
