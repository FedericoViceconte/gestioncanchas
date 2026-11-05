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
        panelLateral = new Panel();
        panelNavegacion = new Panel();
        panelUsuario = new Panel();
        btnCerrarSesion = new BotonNavegacion();
        btnCambiarClave = new BotonNavegacion();
        panelMarca = new Panel();
        panelEncabezado = new Panel();
        lblConexion = new Label();
        btnNuevaReserva = new Button();
        lblSubtitulo = new Label();
        lblTitulo = new Label();
        panelContenido = new Panel();
        panelLateral.SuspendLayout();
        panelUsuario.SuspendLayout();
        panelEncabezado.SuspendLayout();
        SuspendLayout();
        //
        // panelLateral
        //
        panelLateral.BackColor = Tema.Lateral;
        panelLateral.Controls.Add(panelNavegacion);
        panelLateral.Controls.Add(panelUsuario);
        panelLateral.Controls.Add(panelMarca);
        panelLateral.Dock = DockStyle.Left;
        panelLateral.Name = "panelLateral";
        panelLateral.Size = new Size(240, 760);
        panelLateral.TabIndex = 2;
        //
        // panelNavegacion
        //
        panelNavegacion.AutoScroll = true;
        panelNavegacion.Dock = DockStyle.Fill;
        panelNavegacion.Name = "panelNavegacion";
        panelNavegacion.Padding = new Padding(0, 4, 0, 8);
        panelNavegacion.TabIndex = 1;
        //
        // panelUsuario
        //
        panelUsuario.Controls.Add(btnCambiarClave);
        panelUsuario.Controls.Add(btnCerrarSesion);
        panelUsuario.Dock = DockStyle.Bottom;
        panelUsuario.Name = "panelUsuario";
        panelUsuario.Padding = new Padding(0, 0, 0, 10);
        panelUsuario.Size = new Size(240, 150);
        panelUsuario.TabIndex = 2;
        panelUsuario.Paint += panelUsuario_Paint;
        //
        // btnCambiarClave
        //
        btnCambiarClave.Dock = DockStyle.Bottom;
        btnCambiarClave.Height = 38;
        btnCambiarClave.Icono = Icono.Llave;
        btnCambiarClave.Name = "btnCambiarClave";
        btnCambiarClave.Text = "Cambiar contraseña";
        btnCambiarClave.Click += btnCambiarClave_Click;
        //
        // btnCerrarSesion
        //
        btnCerrarSesion.Dock = DockStyle.Bottom;
        btnCerrarSesion.Height = 38;
        btnCerrarSesion.Icono = Icono.Salir;
        btnCerrarSesion.Name = "btnCerrarSesion";
        btnCerrarSesion.Text = "Cerrar sesión";
        btnCerrarSesion.Click += btnCerrarSesion_Click;
        //
        // panelMarca
        //
        panelMarca.Dock = DockStyle.Top;
        panelMarca.Name = "panelMarca";
        panelMarca.Size = new Size(240, 84);
        panelMarca.TabIndex = 0;
        panelMarca.Paint += panelMarca_Paint;
        //
        // panelEncabezado
        //
        panelEncabezado.BackColor = Tema.Superficie;
        panelEncabezado.Controls.Add(lblConexion);
        panelEncabezado.Controls.Add(btnNuevaReserva);
        panelEncabezado.Controls.Add(lblSubtitulo);
        panelEncabezado.Controls.Add(lblTitulo);
        panelEncabezado.Dock = DockStyle.Top;
        panelEncabezado.Name = "panelEncabezado";
        panelEncabezado.Size = new Size(1040, 76);
        panelEncabezado.TabIndex = 1;
        panelEncabezado.Paint += panelEncabezado_Paint;
        //
        // lblConexion
        //
        lblConexion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblConexion.AutoSize = false;
        lblConexion.Font = Tema.Texto9;
        lblConexion.ForeColor = Tema.TextoSecundario;
        lblConexion.Location = new Point(560, 28);
        lblConexion.Name = "lblConexion";
        lblConexion.Size = new Size(300, 20);
        lblConexion.TextAlign = ContentAlignment.MiddleRight;
        //
        // btnNuevaReserva
        //
        btnNuevaReserva.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnNuevaReserva.Location = new Point(876, 20);
        btnNuevaReserva.Name = "btnNuevaReserva";
        btnNuevaReserva.Size = new Size(144, 36);
        btnNuevaReserva.TabIndex = 0;
        btnNuevaReserva.Text = "+  Nueva reserva";
        btnNuevaReserva.Click += btnNuevaReserva_Click;
        //
        // lblSubtitulo
        //
        lblSubtitulo.AutoEllipsis = true;
        lblSubtitulo.Font = Tema.Texto9;
        lblSubtitulo.ForeColor = Tema.TextoSecundario;
        lblSubtitulo.Location = new Point(28, 44);
        lblSubtitulo.Name = "lblSubtitulo";
        lblSubtitulo.Size = new Size(520, 20);
        //
        // lblTitulo
        //
        lblTitulo.AutoSize = true;
        lblTitulo.Font = Tema.Titulo;
        lblTitulo.ForeColor = Tema.Texto;
        lblTitulo.Location = new Point(24, 12);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Text = "Inicio";
        //
        // panelContenido
        //
        panelContenido.BackColor = Tema.Fondo;
        panelContenido.Dock = DockStyle.Fill;
        panelContenido.Name = "panelContenido";
        panelContenido.Padding = new Padding(20, 16, 20, 20);
        panelContenido.TabIndex = 0;
        //
        // FrmPrincipal
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1280, 760);
        Controls.Add(panelContenido);
        Controls.Add(panelEncabezado);
        Controls.Add(panelLateral);
        KeyPreview = true;
        MinimumSize = new Size(1100, 680);
        Name = "FrmPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Gestión de Canchas";
        WindowState = FormWindowState.Maximized;
        Load += FrmPrincipal_Load;
        KeyDown += FrmPrincipal_KeyDown;
        panelLateral.ResumeLayout(false);
        panelUsuario.ResumeLayout(false);
        panelEncabezado.ResumeLayout(false);
        panelEncabezado.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private Panel panelLateral;
    private Panel panelNavegacion;
    private Panel panelUsuario;
    private BotonNavegacion btnCambiarClave;
    private BotonNavegacion btnCerrarSesion;
    private Panel panelMarca;
    private Panel panelEncabezado;
    private Label lblConexion;
    private Button btnNuevaReserva;
    private Label lblSubtitulo;
    private Label lblTitulo;
    private Panel panelContenido;
}
