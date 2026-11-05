using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

public partial class FrmHorarios : FormularioBase, IRefrescable
{
    private readonly HorarioNegocio _negocio = new HorarioNegocio();

    public FrmHorarios()
    {
        InitializeComponent();
        Grillas.Configurar(dgvHorarios);
        dtpInicio.Value = DateTime.Today.AddHours(8);
        dtpFin.Value = DateTime.Today.AddHours(9);
    }

    private void FrmHorarios_Load(object sender, EventArgs e) => Refrescar();

    public void Refrescar()
    {
        try
        {
            dgvHorarios.DataSource = _negocio.Listar();
            Grillas.Formato(dgvHorarios, @"hh\:mm", "HoraInicio", "HoraFin");
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void btnAgregar_Click(object sender, EventArgs e)
    {
        var horario = new Horario
        {
            HoraInicio = new TimeSpan(dtpInicio.Value.Hour, dtpInicio.Value.Minute, 0),
            HoraFin = new TimeSpan(dtpFin.Value.Hour, dtpFin.Value.Minute, 0)
        };

        try
        {
            _negocio.Agregar(horario);
            FrmPrincipal.ActualizarPantallas();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        Horario horario = Grillas.Seleccionado<Horario>(dgvHorarios);
        if (horario == null)
        {
            Mensajes.Info("Seleccioná un horario de la grilla.");
            return;
        }
        if (!Mensajes.Confirmar($"¿Eliminar el horario de {horario}?"))
            return;

        try
        {
            _negocio.Eliminar(horario.IdHorario);
            FrmPrincipal.ActualizarPantallas();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }
}
