using GestionCanchas.Datos;

namespace GestionCanchas.Formularios;

public partial class FrmPrincipal : Form
{
    public FrmPrincipal()
    {
        InitializeComponent();
        Shown += (sender, e) => AjustarResumen();
        foreach (MdiClient area in Controls.OfType<MdiClient>())
            area.SizeChanged += (sender, e) => AjustarResumen();
    }

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
            MostrarResumen();
        }
        catch (Exception ex)
        {
            lblEstadoConexion.Text = "Sin conexión a la base de datos";
            Mensajes.Error(ex);
        }
    }

    private void MostrarResumen()
    {
        if (MdiChildren.OfType<FrmResumen>().Any())
            return;

        var resumen = new FrmResumen { MdiParent = this, ForeColor = SystemColors.ControlText };
        resumen.Show();
        AjustarResumen();
    }

    private void AjustarResumen()
    {
        FrmResumen resumen = MdiChildren.OfType<FrmResumen>().FirstOrDefault();
        MdiClient area = Controls.OfType<MdiClient>().FirstOrDefault();
        if (resumen == null || area == null || WindowState == FormWindowState.Minimized)
            return;

        resumen.Location = new Point(20, 20);
        int ancho = Math.Max(area.ClientSize.Width - 40, 400);
        int alto = Math.Max(Math.Min(area.ClientSize.Height - 40, (int)(ancho * 0.5)), 250);
        resumen.Size = new Size(ancho, alto);
    }

    private void AbrirFormulario<T>() where T : Form, new()
    {
        foreach (FrmResumen resumen in MdiChildren.OfType<FrmResumen>())
            resumen.Close();

        T abierto = MdiChildren.OfType<T>().FirstOrDefault();
        if (abierto != null)
        {
            abierto.Activate();
            return;
        }

        var formulario = new T { MdiParent = this, WindowState = FormWindowState.Maximized, ForeColor = SystemColors.ControlText };
        formulario.Show();
    }

    private void mnuInicio_Click(object sender, EventArgs e)

    {

        foreach (Form hijo in MdiChildren)
            hijo.Close();
        MostrarResumen();

    }

    private void mnuSalir_Click(object sender, EventArgs e) => Close();

    private void mnuGestionarClientes_Click(object sender, EventArgs e) => AbrirFormulario<FrmClientes>();

    private void mnuGestionarCanchas_Click(object sender, EventArgs e) => AbrirFormulario<FrmCanchas>();

    private void mnuHorarios_Click(object sender, EventArgs e) => AbrirFormulario<FrmHorarios>();

    private void mnuConsultarReservas_Click(object sender, EventArgs e) => AbrirFormulario<FrmConsultaReservas>();

    private void mnuDisponibilidad_Click(object sender, EventArgs e) => AbrirFormulario<FrmDisponibilidad>();

    private void mnuTurnosFijos_Click(object sender, EventArgs e) => AbrirFormulario<FrmTurnosFijos>();

    private void mnuRegistrarPago_Click(object sender, EventArgs e) => AbrirFormulario<FrmPagos>();

    private void mnuConsultarPagos_Click(object sender, EventArgs e) => AbrirFormulario<FrmConsultaPagos>();

    private void mnuNuevaReserva_Click(object sender, EventArgs e)
    {
        using var formulario = new FrmReserva();
        if (formulario.ShowDialog(this) == DialogResult.OK)
            ActualizarPantallas();
    }
}
