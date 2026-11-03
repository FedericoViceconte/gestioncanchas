using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

public partial class FrmCanchas : Form, IRefrescable
{
    private readonly CanchaNegocio _negocio = new CanchaNegocio();
    private int _idSeleccionada;

    public FrmCanchas()
    {
        InitializeComponent();
        Grillas.Configurar(dgvCanchas);
        cboEstado.Items.AddRange(EstadosCancha.Todos);
        cboEstado.SelectedItem = EstadosCancha.Disponible;
    }

    private void FrmCanchas_Load(object sender, EventArgs e) => Refrescar();

    public void Refrescar()
    {
        try
        {
            dgvCanchas.DataSource = _negocio.Listar();
            Grillas.Formato(dgvCanchas, "C2", "PrecioHora");
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void dgvCanchas_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
            return;
        if (dgvCanchas.Rows[e.RowIndex].DataBoundItem is not Cancha cancha)
            return;

        _idSeleccionada = cancha.IdCancha;
        nudNumero.Value = cancha.Numero;
        cboTipo.Text = cancha.Tipo;
        nudPrecio.Value = cancha.PrecioHora;
        cboEstado.SelectedItem = cancha.Estado;
    }

    private void Limpiar()
    {
        _idSeleccionada = 0;
        nudNumero.Value = nudNumero.Minimum;
        cboTipo.Text = "";
        nudPrecio.Value = 0;
        cboEstado.SelectedItem = EstadosCancha.Disponible;
        dgvCanchas.ClearSelection();
        nudNumero.Focus();
    }

    private void btnNuevo_Click(object sender, EventArgs e) => Limpiar();

    private void btnGuardar_Click(object sender, EventArgs e)
    {
        var cancha = new Cancha
        {
            IdCancha = _idSeleccionada,
            Numero = (int)nudNumero.Value,
            Tipo = cboTipo.Text,
            PrecioHora = nudPrecio.Value,
            Estado = cboEstado.SelectedItem as string
        };

        try
        {
            bool esNueva = cancha.IdCancha == 0;
            _negocio.Guardar(cancha);
            Mensajes.Info(esNueva ? "Cancha registrada." : "Cancha modificada.");
            Limpiar();
            FrmPrincipal.ActualizarPantallas();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        if (_idSeleccionada == 0)
        {
            Mensajes.Info("Seleccioná una cancha de la grilla.");
            return;
        }
        if (!Mensajes.Confirmar($"¿Eliminar la cancha número {nudNumero.Value}?"))
            return;

        try
        {
            _negocio.Eliminar(_idSeleccionada);
            Limpiar();
            FrmPrincipal.ActualizarPantallas();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }
}
