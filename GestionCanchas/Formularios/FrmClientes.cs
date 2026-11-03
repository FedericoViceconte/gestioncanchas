using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

public partial class FrmClientes : Form, IRefrescable
{
    private readonly ClienteNegocio _negocio = new ClienteNegocio();
    private int _idSeleccionado;

    public FrmClientes()
    {
        InitializeComponent();
        Grillas.Configurar(dgvClientes);
    }

    private void FrmClientes_Load(object sender, EventArgs e) => Refrescar();

    public void Refrescar()
    {
        try
        {
            MostrarEnGrilla(_negocio.Listar(chkSoloDeuda.Checked));
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void MostrarEnGrilla(List<Cliente> clientes)
    {
        dgvClientes.DataSource = clientes;
        Grillas.Formato(dgvClientes, "C2", "Deuda");
    }

    private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
            return;
        if (dgvClientes.Rows[e.RowIndex].DataBoundItem is not Cliente cliente)
            return;

        CargarEnFormulario(cliente);
    }

    private void dgvClientes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (dgvClientes.Rows[e.RowIndex].DataBoundItem is Cliente cliente && cliente.Deuda > 0)
            e.CellStyle.ForeColor = Color.Firebrick;
    }
    private void CargarEnFormulario(Cliente cliente)
    {
        _idSeleccionado = cliente.IdCliente;
        txtDni.Text = cliente.Dni;
        txtNombre.Text = cliente.Nombre;
        txtApellido.Text = cliente.Apellido;
        txtTelefono.Text = cliente.Telefono;
    }

    private void Limpiar()
    {
        _idSeleccionado = 0;
        txtDni.Clear();
        txtNombre.Clear();
        txtApellido.Clear();
        txtTelefono.Clear();
        dgvClientes.ClearSelection();
        txtDni.Focus();
    }

    private void btnNuevo_Click(object sender, EventArgs e) => Limpiar();

    private void btnGuardar_Click(object sender, EventArgs e)
    {
        var cliente = new Cliente
        {
            IdCliente = _idSeleccionado,
            Dni = txtDni.Text,
            Nombre = txtNombre.Text,
            Apellido = txtApellido.Text,
            Telefono = txtTelefono.Text
        };

        try
        {
            bool esNuevo = cliente.IdCliente == 0;
            _negocio.Guardar(cliente);
            Mensajes.Info(esNuevo ? "Cliente registrado." : "Cliente modificado.");
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
        if (_idSeleccionado == 0)
        {
            Mensajes.Info("Seleccioná un cliente de la grilla.");
            return;
        }
        if (!Mensajes.Confirmar($"¿Eliminar al cliente {txtApellido.Text}, {txtNombre.Text}?"))
            return;

        try
        {
            _negocio.Eliminar(_idSeleccionado);
            Limpiar();
            FrmPrincipal.ActualizarPantallas();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void btnBuscar_Click(object sender, EventArgs e)
    {
        try
        {
            List<Cliente> clientes = _negocio.Buscar(txtBuscarDni.Text);
            if (clientes.Count == 0)
            {
                Mensajes.Info("No se encontró ningún cliente con esos datos.");
                return;
            }

            MostrarEnGrilla(clientes);
            if (clientes.Count == 1)
                CargarEnFormulario(clientes[0]);
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void txtBuscarDni_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;

        e.SuppressKeyPress = true;
        btnBuscar.PerformClick();
    }

    private void btnMostrarTodos_Click(object sender, EventArgs e)
    {
        txtBuscarDni.Clear();
        Refrescar();
    }

    private void chkSoloDeuda_CheckedChanged(object sender, EventArgs e) => Refrescar();
}
