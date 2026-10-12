using GestionCanchas.Datos;

namespace GestionCanchas.Formularios;

public partial class FrmPrincipal : Form
{
    public FrmPrincipal()
    {
        InitializeComponent();
    }

    // Recarga todas las pantallas abiertas después de un alta, baja o modificación
    public static void ActualizarPantallas()
    {
        FrmPrincipal principal = Application.OpenForms.OfType<FrmPrincipal>().FirstOrDefault();
        if (principal == null)
            return;

        foreach (Form hijo in principal.MdiChildren)
        {
            if (hijo is IRefrescable refrescable)
                refrescable.Refrescar();
        }
    }

    private void FrmPrincipal_Load(object sender, EventArgs e)
    {
        try
        {
            Conexion.Probar();
            lblEstadoConexion.Text = "Conectado a la base de datos";
        }
        catch (Exception ex)
        {
            lblEstadoConexion.Text = "Sin conexión a la base de datos";
            Mensajes.Error(ex);
        }
    }

    private void AbrirFormulario<T>() where T : Form, new()
    {
        T abierto = MdiChildren.OfType<T>().FirstOrDefault();
        if (abierto != null)
        {
            abierto.Activate();
            return;
        }

        var formulario = new T { MdiParent = this, WindowState = FormWindowState.Maximized };
        formulario.Show();
    }

    private void mnuSalir_Click(object sender, EventArgs e) => Close();

    private void mnuGestionarClientes_Click(object sender, EventArgs e) => AbrirFormulario<FrmClientes>();

    private void mnuGestionarCanchas_Click(object sender, EventArgs e) => AbrirFormulario<FrmCanchas>();

    private void mnuHorarios_Click(object sender, EventArgs e) => AbrirFormulario<FrmHorarios>();

    private void mnuConsultarReservas_Click(object sender, EventArgs e) => AbrirFormulario<FrmConsultaReservas>();

    private void mnuDisponibilidad_Click(object sender, EventArgs e) => AbrirFormulario<FrmDisponibilidad>();

    private void mnuRegistrarPago_Click(object sender, EventArgs e) => AbrirFormulario<FrmPagos>();

    private void mnuConsultarPagos_Click(object sender, EventArgs e) => AbrirFormulario<FrmConsultaPagos>();

    private void mnuNuevaReserva_Click(object sender, EventArgs e)
    {
        using var formulario = new FrmReserva();
        if (formulario.ShowDialog(this) == DialogResult.OK)
            ActualizarPantallas();
    }
}
