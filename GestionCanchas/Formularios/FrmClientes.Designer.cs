namespace GestionCanchas.Formularios;

partial class FrmClientes
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
        grpDatos = new GroupBox();
        lblDni = new Label();
        txtDni = new TextBox();
        lblNombre = new Label();
        txtNombre = new TextBox();
        lblApellido = new Label();
        txtApellido = new TextBox();
        lblTelefono = new Label();
        txtTelefono = new TextBox();
        btnNuevo = new Button();
        btnGuardar = new Button();
        btnEliminar = new Button();
        grpConsulta = new GroupBox();
        lblBuscarDni = new Label();
        txtBuscarDni = new TextBox();
        btnBuscar = new Button();
        btnMostrarTodos = new Button();
        chkSoloDeuda = new CheckBox();
        dgvClientes = new DataGridView();
        grpDatos.SuspendLayout();
        grpConsulta.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
        SuspendLayout();

        grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpDatos.Controls.Add(lblDni);
        grpDatos.Controls.Add(txtDni);
        grpDatos.Controls.Add(lblNombre);
        grpDatos.Controls.Add(txtNombre);
        grpDatos.Controls.Add(lblApellido);
        grpDatos.Controls.Add(txtApellido);
        grpDatos.Controls.Add(lblTelefono);
        grpDatos.Controls.Add(txtTelefono);
        grpDatos.Controls.Add(btnNuevo);
        grpDatos.Controls.Add(btnGuardar);
        grpDatos.Controls.Add(btnEliminar);
        grpDatos.Location = new Point(12, 12);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(876, 110);
        grpDatos.TabIndex = 0;
        grpDatos.TabStop = false;
        grpDatos.Text = "Datos del cliente";

        lblDni.AutoSize = true;
        lblDni.Location = new Point(15, 30);
        lblDni.Name = "lblDni";
        lblDni.Size = new Size(30, 15);
        lblDni.TabIndex = 0;
        lblDni.Text = "DNI:";

        txtDni.Location = new Point(90, 27);
        txtDni.MaxLength = 8;
        txtDni.Name = "txtDni";
        txtDni.Size = new Size(120, 23);
        txtDni.TabIndex = 1;

        lblNombre.AutoSize = true;
        lblNombre.Location = new Point(230, 30);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(54, 15);
        lblNombre.TabIndex = 2;
        lblNombre.Text = "Nombre:";

        txtNombre.Location = new Point(300, 27);
        txtNombre.MaxLength = 50;
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(200, 23);
        txtNombre.TabIndex = 3;

        lblApellido.AutoSize = true;
        lblApellido.Location = new Point(520, 30);
        lblApellido.Name = "lblApellido";
        lblApellido.Size = new Size(54, 15);
        lblApellido.TabIndex = 4;
        lblApellido.Text = "Apellido:";

        txtApellido.Location = new Point(590, 27);
        txtApellido.MaxLength = 50;
        txtApellido.Name = "txtApellido";
        txtApellido.Size = new Size(200, 23);
        txtApellido.TabIndex = 5;

        lblTelefono.AutoSize = true;
        lblTelefono.Location = new Point(15, 68);
        lblTelefono.Name = "lblTelefono";
        lblTelefono.Size = new Size(56, 15);
        lblTelefono.TabIndex = 6;
        lblTelefono.Text = "Teléfono:";

        txtTelefono.Location = new Point(90, 65);
        txtTelefono.MaxLength = 20;
        txtTelefono.Name = "txtTelefono";
        txtTelefono.Size = new Size(160, 23);
        txtTelefono.TabIndex = 7;

        btnNuevo.Location = new Point(300, 62);
        btnNuevo.Name = "btnNuevo";
        btnNuevo.Size = new Size(90, 28);
        btnNuevo.TabIndex = 8;
        btnNuevo.Text = "Nuevo";
        btnNuevo.UseVisualStyleBackColor = true;
        btnNuevo.Click += btnNuevo_Click;

        btnGuardar.Location = new Point(400, 62);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(90, 28);
        btnGuardar.TabIndex = 9;
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = true;
        btnGuardar.Click += btnGuardar_Click;

        btnEliminar.Location = new Point(500, 62);
        btnEliminar.Name = "btnEliminar";
        btnEliminar.Size = new Size(90, 28);
        btnEliminar.TabIndex = 10;
        btnEliminar.Text = "Eliminar";
        btnEliminar.UseVisualStyleBackColor = true;
        btnEliminar.Click += btnEliminar_Click;

        grpConsulta.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpConsulta.Controls.Add(lblBuscarDni);
        grpConsulta.Controls.Add(txtBuscarDni);
        grpConsulta.Controls.Add(btnBuscar);
        grpConsulta.Controls.Add(btnMostrarTodos);
        grpConsulta.Controls.Add(chkSoloDeuda);
        grpConsulta.Location = new Point(12, 130);
        grpConsulta.Name = "grpConsulta";
        grpConsulta.Size = new Size(876, 62);
        grpConsulta.TabIndex = 1;
        grpConsulta.TabStop = false;
        grpConsulta.Text = "Buscar cliente";

        lblBuscarDni.AutoSize = true;
        lblBuscarDni.Location = new Point(15, 28);
        lblBuscarDni.Name = "lblBuscarDni";
        lblBuscarDni.Size = new Size(45, 15);
        lblBuscarDni.TabIndex = 0;
        lblBuscarDni.Text = "Buscar:";

        txtBuscarDni.Location = new Point(70, 25);
        txtBuscarDni.MaxLength = 50;
        txtBuscarDni.Name = "txtBuscarDni";
        txtBuscarDni.PlaceholderText = "DNI, nombre, apellido o teléfono";
        txtBuscarDni.Size = new Size(220, 23);
        txtBuscarDni.TabIndex = 1;
        txtBuscarDni.KeyDown += txtBuscarDni_KeyDown;

        btnBuscar.Location = new Point(300, 22);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(90, 28);
        btnBuscar.TabIndex = 2;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = true;
        btnBuscar.Click += btnBuscar_Click;

        btnMostrarTodos.Location = new Point(400, 22);
        btnMostrarTodos.Name = "btnMostrarTodos";
        btnMostrarTodos.Size = new Size(110, 28);
        btnMostrarTodos.TabIndex = 3;
        btnMostrarTodos.Text = "Mostrar todos";
        btnMostrarTodos.UseVisualStyleBackColor = true;
        btnMostrarTodos.Click += btnMostrarTodos_Click;

        chkSoloDeuda.AutoSize = true;
        chkSoloDeuda.Location = new Point(530, 27);
        chkSoloDeuda.Name = "chkSoloDeuda";
        chkSoloDeuda.Size = new Size(155, 19);
        chkSoloDeuda.TabIndex = 4;
        chkSoloDeuda.Text = "Solo clientes con deuda";
        chkSoloDeuda.UseVisualStyleBackColor = true;
        chkSoloDeuda.CheckedChanged += chkSoloDeuda_CheckedChanged;

        dgvClientes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvClientes.Location = new Point(12, 200);
        dgvClientes.Name = "dgvClientes";
        dgvClientes.Size = new Size(876, 348);
        dgvClientes.TabIndex = 2;
        dgvClientes.CellClick += dgvClientes_CellClick;
        dgvClientes.CellFormatting += dgvClientes_CellFormatting;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 560);
        Controls.Add(grpDatos);
        Controls.Add(grpConsulta);
        Controls.Add(dgvClientes);
        Name = "FrmClientes";
        Text = "Clientes";
        Load += FrmClientes_Load;
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        grpConsulta.ResumeLayout(false);
        grpConsulta.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private GroupBox grpDatos;
    private Label lblDni;
    private TextBox txtDni;
    private Label lblNombre;
    private TextBox txtNombre;
    private Label lblApellido;
    private TextBox txtApellido;
    private Label lblTelefono;
    private TextBox txtTelefono;
    private Button btnNuevo;
    private Button btnGuardar;
    private Button btnEliminar;
    private GroupBox grpConsulta;
    private Label lblBuscarDni;
    private TextBox txtBuscarDni;
    private Button btnBuscar;
    private Button btnMostrarTodos;
    private CheckBox chkSoloDeuda;
    private DataGridView dgvClientes;
}
