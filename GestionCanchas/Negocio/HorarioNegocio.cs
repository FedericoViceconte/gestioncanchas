using GestionCanchas.Datos;
using GestionCanchas.Entidades;

namespace GestionCanchas.Negocio;

public class HorarioNegocio
{
    private readonly HorarioDatos _datos = new HorarioDatos();

    public List<Horario> Listar() => _datos.Listar();

    public void Agregar(Horario horario)
    {
        if (horario.HoraFin <= horario.HoraInicio)
            throw new ValidacionException("La hora de fin debe ser posterior a la de inicio.");

        if (_datos.SeSuperpone(horario.HoraInicio, horario.HoraFin))
            throw new ValidacionException("El horario se superpone con otro ya cargado.");

        _datos.Insertar(horario);
    }

    public void Eliminar(int idHorario)
    {
        if (_datos.TieneReservas(idHorario))
            throw new ValidacionException("No se puede eliminar el horario porque tiene reservas.");

        _datos.Eliminar(idHorario);
    }
}
