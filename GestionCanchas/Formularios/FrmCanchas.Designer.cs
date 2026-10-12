namespace GestionCanchas.Formularios;

partial class FrmCanchas
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
        lblNumero = new Label();
        nudNumero = new NumericUpDown();
        lblTipo = new Label();
        cboTipo = new ComboBox();
        lblPrecio = new Label();
        nudPrecio = new NumericUpDown();
        lblEstado = new Label();
        cboEstado = new ComboBox();
        btnNuevo = new Button();
        btnGuardar = new Button();
        btnEliminar = new Button();
        dgvCanchas = new DataGridView();
        grpDatos.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudNumero).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudPrecio).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvCanchas).BeginInit();
        SuspendLayout();
        // 
        // grpDatos
        // 
        grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpDatos.Controls.Add(lblNumero);
        grpDatos.Controls.Add(nudNumero);
        grpDatos.Controls.Add(lblTipo);
        grpDatos.Controls.Add(cboTipo);
        grpDatos.Controls.Add(lblPrecio);
        grpDatos.Controls.Add(nudPrecio);
        grpDatos.Controls.Add(lblEstado);
        grpDatos.Controls.Add(cboEstado);
        grpDatos.Controls.Add(btnNuevo);
        grpDatos.Controls.Add(btnGuardar);
        grpDatos.Controls.Add(btnEliminar);
        grpDatos.Location = new Point(12, 12);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(876, 110);
        grpDatos.TabIndex = 0;
        grpDatos.TabStop = false;
        grpDatos.Text = "Datos de la cancha";
        // 
        // lblNumero
        // 
        lblNumero.AutoSize = true;
        lblNumero.Location = new Point(15, 30);
        lblNumero.Name = "lblNumero";
        lblNumero.Size = new Size(54, 15);
        lblNumero.TabIndex = 0;
        lblNumero.Text = "Número:";
        // 
        // nudNumero
        // 
        nudNumero.Location = new Point(100, 27);
        nudNumero.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
        nudNumero.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudNumero.Name = "nudNumero";
        nudNumero.Size = new Size(80, 23);
        nudNumero.TabIndex = 1;
        nudNumero.Value = new decimal(new int[] { 1, 0, 0, 0 });
        nudNumero.ValueChanged += nudNumero_ValueChanged;
        // 
        // lblTipo
        // 
        lblTipo.AutoSize = true;
        lblTipo.Location = new Point(200, 30);
        lblTipo.Name = "lblTipo";
        lblTipo.Size = new Size(34, 15);
        lblTipo.TabIndex = 2;
        lblTipo.Text = "Tipo:";
        // 
        // cboTipo
        // 
        cboTipo.Items.AddRange(new object[] { "Fútbol 5", "Fútbol 7", "Fútbol 8", "Fútbol 11" });
        cboTipo.Location = new Point(245, 27);
        cboTipo.MaxLength = 30;
        cboTipo.Name = "cboTipo";
        cboTipo.Size = new Size(150, 23);
        cboTipo.TabIndex = 3;
        // 
        // lblPrecio
        // 
        lblPrecio.AutoSize = true;
        lblPrecio.Location = new Point(415, 30);
        lblPrecio.Name = "lblPrecio";
        lblPrecio.Size = new Size(91, 15);
        lblPrecio.TabIndex = 4;
        lblPrecio.Text = "Precio por hora:";
        // 
        // nudPrecio
        // 
        nudPrecio.DecimalPlaces = 2;
        nudPrecio.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
        nudPrecio.Location = new Point(520, 27);
        nudPrecio.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
        nudPrecio.Name = "nudPrecio";
        nudPrecio.Size = new Size(120, 23);
        nudPrecio.TabIndex = 5;
        nudPrecio.ThousandsSeparator = true;
        // 
        // lblEstado
        // 
        lblEstado.AutoSize = true;
        lblEstado.Location = new Point(660, 30);
        lblEstado.Name = "lblEstado";
        lblEstado.Size = new Size(45, 15);
        lblEstado.TabIndex = 6;
        lblEstado.Text = "Estado:";
        // 
        // cboEstado
        // 
        cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEstado.Location = new Point(715, 27);
        cboEstado.Name = "cboEstado";
        cboEstado.Size = new Size(140, 23);
        cboEstado.TabIndex = 7;
        // 
        // btnNuevo
        // 
        btnNuevo.Location = new Point(100, 64);
        btnNuevo.Name = "btnNuevo";
        btnNuevo.Size = new Size(90, 28);
        btnNuevo.TabIndex = 8;
        btnNuevo.Text = "Nuevo";
        btnNuevo.UseVisualStyleBackColor = true;
        btnNuevo.Click += btnNuevo_Click;
        // 
        // btnGuardar
        // 
        btnGuardar.Location = new Point(200, 64);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(90, 28);
        btnGuardar.TabIndex = 9;
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = true;
        btnGuardar.Click += btnGuardar_Click;
        // 
        // btnEliminar
        // 
        btnEliminar.Location = new Point(300, 64);
        btnEliminar.Name = "btnEliminar";
        btnEliminar.Size = new Size(90, 28);
        btnEliminar.TabIndex = 10;
        btnEliminar.Text = "Eliminar";
        btnEliminar.UseVisualStyleBackColor = true;
        btnEliminar.Click += btnEliminar_Click;
        // 
        // dgvCanchas
        // 
        dgvCanchas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvCanchas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvCanchas.Location = new Point(12, 132);
        dgvCanchas.Name = "dgvCanchas";
        dgvCanchas.Size = new Size(876, 416);
        dgvCanchas.TabIndex = 1;
        dgvCanchas.CellClick += dgvCanchas_CellClick;
        // 
        // FrmCanchas
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 560);
        Controls.Add(grpDatos);
        Controls.Add(dgvCanchas);
        Name = "FrmCanchas";
        Text = "Canchas";
        Load += FrmCanchas_Load;
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudNumero).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudPrecio).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvCanchas).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private GroupBox grpDatos;
    private Label lblNumero;
    private NumericUpDown nudNumero;
    private Label lblTipo;
    private ComboBox cboTipo;
    private Label lblPrecio;
    private NumericUpDown nudPrecio;
    private Label lblEstado;
    private ComboBox cboEstado;
    private Button btnNuevo;
    private Button btnGuardar;
    private Button btnEliminar;
    private DataGridView dgvCanchas;
}
