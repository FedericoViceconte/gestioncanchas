using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

public partial class FrmDisponibilidad : Form, IRefrescable
{
    private const string TextoLibre = "Libre";

    private readonly CanchaNegocio _canchaNegocio = new CanchaNegocio();
    private readonly HorarioNegocio _horarioNegocio = new HorarioNegocio();
    private readonly ReservaNegocio _reservaNegocio = new ReservaNegocio();

    public FrmDisponibilidad()
    {
        InitializeComponent();
        Grillas.Configurar(dgvDisponibilidad);
        dgvDisponibilidad.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvDisponibilidad.AllowUserToOrderColumns = false;
    }

    private void FrmDisponibilidad_Load(object sender, EventArgs e) => Refrescar();

    public void Refrescar()
    {
        try
        {
            DateTime fecha = dtpFecha.Value.Date;
            List<Cancha> canchas = _canchaNegocio.ListarDisponibles();
            List<Horario> horarios = _horarioNegocio.Listar();
            Dictionary<(int, int), TurnoOcupado> ocupados = _reservaNegocio.ListarOcupados(fecha)
                .ToDictionary(t => (t.IdCancha, t.IdHorario));

            dgvDisponibilidad.Rows.Clear();
            dgvDisponibilidad.Columns.Clear();
            dgvDisponibilidad.Columns.Add("Horario", "Horario");
            foreach (Cancha cancha in canchas)
            {
                int indice = dgvDisponibilidad.Columns.Add("Cancha" + cancha.IdCancha, cancha.ToString());
                dgvDisponibilidad.Columns[indice].Tag = cancha;
                dgvDisponibilidad.Columns[indice].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            foreach (Horario horario in horarios)
            {
                DataGridViewRow fila = dgvDisponibilidad.Rows[dgvDisponibilidad.Rows.Add()];
                fila.Tag = horario;
                fila.Cells[0].Value = horario.ToString();

                bool yaPaso = fecha < DateTime.Today ||
                              (fecha == DateTime.Today && horario.HoraInicio <= DateTime.Now.TimeOfDay);

                for (int i = 0; i < canchas.Count; i++)
                {
                    ocupados.TryGetValue((canchas[i].IdCancha, horario.IdHorario), out TurnoOcupado turno);
                    PintarCelda(fila.Cells[i + 1], turno, yaPaso);
                }
            }
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private static void PintarCelda(DataGridViewCell celda, TurnoOcupado turno, bool yaPaso)
    {
        if (turno != null)
        {
            celda.Value = turno.Estado switch
            {
                EstadosReserva.Senado => $"{turno.Cliente} (señado)",
                EstadosReserva.Pendiente => $"{turno.Cliente} (fijo)",
                EstadosReserva.Ausente =>$"{turno.Cliente} (no vino)",
                _ => turno.Cliente
            };
            celda.Style.BackColor = Color.MistyRose;
            return;
        }

        if (yaPaso)
        {
            celda.Value = "-";
            celda.Style.BackColor = Color.Gainsboro;
            return;
        }

        celda.Value = TextoLibre;
        celda.Style.BackColor = Color.Honeydew;
    }

    private void dgvDisponibilidad_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 1)
            return;
        if ((dgvDisponibilidad[e.ColumnIndex, e.RowIndex].Value as string) != TextoLibre)
            return;

        var cancha = (Cancha)dgvDisponibilidad.Columns[e.ColumnIndex].Tag;
        var horario = (Horario)dgvDisponibilidad.Rows[e.RowIndex].Tag;

        using var formulario = new FrmReserva(dtpFecha.Value.Date, cancha.IdCancha, horario.IdHorario);
        if (formulario.ShowDialog(this) == DialogResult.OK)
            FrmPrincipal.ActualizarPantallas();
    }

    private void dtpFecha_ValueChanged(object sender, EventArgs e) => Refrescar();

    private void btnAnterior_Click(object sender, EventArgs e) => dtpFecha.Value = dtpFecha.Value.AddDays(-1);

    private void btnSiguiente_Click(object sender, EventArgs e) => dtpFecha.Value = dtpFecha.Value.AddDays(1);

    private void btnHoy_Click(object sender, EventArgs e) => dtpFecha.Value = DateTime.Today;
}
