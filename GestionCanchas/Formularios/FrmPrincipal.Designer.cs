namespace GestionCanchas.Formularios;

partial class FrmPrincipal
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Código generado por el Diseñador de Windows Forms

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
        menuPrincipal = new MenuStrip();
        mnuArchivo = new ToolStripMenuItem();
        mnuSalir = new ToolStripMenuItem();
        mnuClientes = new ToolStripMenuItem();
        mnuGestionarClientes = new ToolStripMenuItem();
        mnuCanchas = new ToolStripMenuItem();
        mnuGestionarCanchas = new ToolStripMenuItem();
        mnuHorarios = new ToolStripMenuItem();
        mnuReservas = new ToolStripMenuItem();
        mnuNuevaReserva = new ToolStripMenuItem();
        mnuConsultarReservas = new ToolStripMenuItem();
        mnuDisponibilidad = new ToolStripMenuItem();
        mnuPagos = new ToolStripMenuItem();
        mnuRegistrarPago = new ToolStripMenuItem();
        mnuConsultarPagos = new ToolStripMenuItem();
        barraEstado = new StatusStrip();
        lblEstadoConexion = new ToolStripStatusLabel();
        menuPrincipal.SuspendLayout();
        barraEstado.SuspendLayout();
        SuspendLayout();
        // 
        // menuPrincipal
        // 
        menuPrincipal.Items.AddRange(new ToolStripItem[] { mnuArchivo, mnuClientes, mnuCanchas, mnuReservas, mnuPagos });
        menuPrincipal.Location = new Point(0, 0);
        menuPrincipal.Name = "menuPrincipal";
        menuPrincipal.Size = new Size(887, 24);
        menuPrincipal.TabIndex = 0;
        // 
        // mnuArchivo
        // 
        mnuArchivo.DropDownItems.AddRange(new ToolStripItem[] { mnuSalir });
        mnuArchivo.Name = "mnuArchivo";
        mnuArchivo.Size = new Size(60, 20);
        mnuArchivo.Text = "&Archivo";
        // 
        // mnuSalir
        // 
        mnuSalir.Name = "mnuSalir";
        mnuSalir.Size = new Size(96, 22);
        mnuSalir.Text = "&Salir";
        mnuSalir.Click += mnuSalir_Click;
        // 
        // mnuClientes
        // 
        mnuClientes.DropDownItems.AddRange(new ToolStripItem[] { mnuGestionarClientes });
        mnuClientes.Name = "mnuClientes";
        mnuClientes.Size = new Size(61, 20);
        mnuClientes.Text = "&Clientes";
        // 
        // mnuGestionarClientes
        // 
        mnuGestionarClientes.Name = "mnuGestionarClientes";
        mnuGestionarClientes.Size = new Size(167, 22);
        mnuGestionarClientes.Text = "Gestionar clientes";
        mnuGestionarClientes.Click += mnuGestionarClientes_Click;
        // 
        // mnuCanchas
        // 
        mnuCanchas.DropDownItems.AddRange(new ToolStripItem[] { mnuGestionarCanchas, mnuHorarios });
        mnuCanchas.Name = "mnuCanchas";
        mnuCanchas.Size = new Size(64, 20);
        mnuCanchas.Text = "C&anchas";
        // 
        // mnuGestionarCanchas
        // 
        mnuGestionarCanchas.Name = "mnuGestionarCanchas";
        mnuGestionarCanchas.Size = new Size(170, 22);
        mnuGestionarCanchas.Text = "Gestionar canchas";
        mnuGestionarCanchas.Click += mnuGestionarCanchas_Click;
        // 
        // mnuHorarios
        // 
        mnuHorarios.Name = "mnuHorarios";
        mnuHorarios.Size = new Size(170, 22);
        mnuHorarios.Text = "Horarios";
        mnuHorarios.Click += mnuHorarios_Click;
        // 
        // mnuReservas
        // 
        mnuReservas.DropDownItems.AddRange(new ToolStripItem[] { mnuNuevaReserva, mnuConsultarReservas, mnuDisponibilidad });
        mnuReservas.Name = "mnuReservas";
        mnuReservas.Size = new Size(64, 20);
        mnuReservas.Text = "&Reservas";
        // 
        // mnuNuevaReserva
        // 
        mnuNuevaReserva.Name = "mnuNuevaReserva";
        mnuNuevaReserva.Size = new Size(170, 22);
        mnuNuevaReserva.Text = "Nueva reserva";
        mnuNuevaReserva.Click += mnuNuevaReserva_Click;
        // 
        // mnuConsultarReservas
        // 
        mnuConsultarReservas.Name = "mnuConsultarReservas";
        mnuConsultarReservas.Size = new Size(170, 22);
        mnuConsultarReservas.Text = "Consultar reservas";
        mnuConsultarReservas.Click += mnuConsultarReservas_Click;
        // 
        // mnuDisponibilidad
        // 
        mnuDisponibilidad.Name = "mnuDisponibilidad";
        mnuDisponibilidad.Size = new Size(170, 22);
        mnuDisponibilidad.Text = "Disponibilidad";
        mnuDisponibilidad.Click += mnuDisponibilidad_Click;
        // 
        // mnuPagos
        // 
        mnuPagos.DropDownItems.AddRange(new ToolStripItem[] { mnuRegistrarPago, mnuConsultarPagos });
        mnuPagos.Name = "mnuPagos";
        mnuPagos.Size = new Size(51, 20);
        mnuPagos.Text = "&Pagos";
        // 
        // mnuRegistrarPago
        // 
        mnuRegistrarPago.Name = "mnuRegistrarPago";
        mnuRegistrarPago.Size = new Size(160, 22);
        mnuRegistrarPago.Text = "Registrar pago";
        mnuRegistrarPago.Click += mnuRegistrarPago_Click;
        // 
        // mnuConsultarPagos
        // 
        mnuConsultarPagos.Name = "mnuConsultarPagos";
        mnuConsultarPagos.Size = new Size(160, 22);
        mnuConsultarPagos.Text = "Consultar pagos";
        mnuConsultarPagos.Click += mnuConsultarPagos_Click;
        // 
        // barraEstado
        // 
        barraEstado.Items.AddRange(new ToolStripItem[] { lblEstadoConexion });
        barraEstado.Location = new Point(0, 519);
        barraEstado.Name = "barraEstado";
        barraEstado.Size = new Size(887, 22);
        barraEstado.TabIndex = 1;
        // 
        // lblEstadoConexion
        // 
        lblEstadoConexion.Name = "lblEstadoConexion";
        lblEstadoConexion.Size = new Size(81, 17);
        lblEstadoConexion.Text = "Conectando...";
        // 
        // FrmPrincipal
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.Green;
        BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
        ClientSize = new Size(887, 541);
        Controls.Add(barraEstado);
        Controls.Add(menuPrincipal);
        ForeColor = SystemColors.ControlLight;
        IsMdiContainer = true;
        MainMenuStrip = menuPrincipal;
        MaximumSize = new Size(903, 580);
        Name = "FrmPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Gestión de Canchas de Fútbol";
        WindowState = FormWindowState.Maximized;
        Load += FrmPrincipal_Load;
        menuPrincipal.ResumeLayout(false);
        menuPrincipal.PerformLayout();
        barraEstado.ResumeLayout(false);
        barraEstado.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip menuPrincipal;
    private ToolStripMenuItem mnuArchivo;
    private ToolStripMenuItem mnuSalir;
    private ToolStripMenuItem mnuClientes;
    private ToolStripMenuItem mnuGestionarClientes;
    private ToolStripMenuItem mnuCanchas;
    private ToolStripMenuItem mnuGestionarCanchas;
    private ToolStripMenuItem mnuHorarios;
    private ToolStripMenuItem mnuReservas;
    private ToolStripMenuItem mnuNuevaReserva;
    private ToolStripMenuItem mnuConsultarReservas;
    private ToolStripMenuItem mnuDisponibilidad;
    private ToolStripMenuItem mnuPagos;
    private ToolStripMenuItem mnuRegistrarPago;
    private ToolStripMenuItem mnuConsultarPagos;
    private StatusStrip barraEstado;
    private ToolStripStatusLabel lblEstadoConexion;
}
