using GestionCanchas.Datos;
using GestionCanchas.Entidades;

namespace GestionCanchas.Negocio;

public class ReporteNegocio
{
    private const int DiasMaximosCaja = 366;

    private readonly ReporteDatos _datos = new ReporteDatos();

    public IndicadoresDia Indicadores(DateTime dia) => _datos.Indicadores(dia);

    public List<IngresoDiario> IngresosUltimosDias(DateTime hasta, int dias) =>
        _datos.IngresosPorDia(hasta.Date.AddDays(1 - dias), hasta.Date);

    public List<TotalCaja> CajaPorMetodo(DateTime desde, DateTime hasta)
    {
        ValidarRango(desde, hasta);
        return _datos.CajaPorMetodo(desde, hasta);
    }

    public List<TotalCaja> CajaPorUsuario(DateTime desde, DateTime hasta)
    {
        ValidarRango(desde, hasta);
        return _datos.CajaPorUsuario(desde, hasta);
    }

    public static void ValidarRango(DateTime desde, DateTime hasta)
    {
        if (desde.Date > hasta.Date)
            throw new ValidacionException("La fecha 'desde' no puede ser posterior a 'hasta'.");
        if ((hasta.Date - desde.Date).TotalDays >= DiasMaximosCaja)
            throw new ValidacionException("Elegí un período de hasta un año.");
    }
}
