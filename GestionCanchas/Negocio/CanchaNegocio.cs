using GestionCanchas.Datos;
using GestionCanchas.Entidades;

namespace GestionCanchas.Negocio;

public class CanchaNegocio
{
    private readonly CanchaDatos _datos = new CanchaDatos();

    public List<Cancha> Listar() => _datos.Listar(false);

    public List<Cancha> ListarDisponibles() => _datos.Listar(true);

    public void Guardar(Cancha cancha)
    {
        Validar(cancha);

        if (_datos.ExisteNumero(cancha.Numero, cancha.IdCancha))
            throw new ValidacionException($"Ya existe la cancha número {cancha.Numero}.");

        if (cancha.IdCancha == 0)
        {
            _datos.Insertar(cancha);
            return;
        }
        _datos.Modificar(cancha);
    }

    public void Eliminar(int idCancha)
    {
        if (_datos.TieneReservas(idCancha))
            throw new ValidacionException(
                "No se puede eliminar la cancha porque tiene reservas. Si no se va a usar, pasala a Mantenimiento.");

        _datos.Eliminar(idCancha);
    }

    private static void Validar(Cancha cancha)
    {
        cancha.Tipo = cancha.Tipo?.Trim();

        if (cancha.Numero <= 0)
            throw new ValidacionException("El número de cancha debe ser mayor a cero.");
        if (string.IsNullOrEmpty(cancha.Tipo))
            throw new ValidacionException("El tipo de cancha es obligatorio.");
        if (cancha.Tipo.Length > 30)
            throw new ValidacionException("El tipo admite hasta 30 caracteres.");
        if (cancha.PrecioHora <= 0)
            throw new ValidacionException("El precio por hora debe ser mayor a cero.");
        if (!EstadosCancha.Todos.Contains(cancha.Estado))
            throw new ValidacionException("Seleccioná un estado válido.");
    }
}
