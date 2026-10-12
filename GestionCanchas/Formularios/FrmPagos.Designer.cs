namespace GestionCanchas.Formularios;

partial class FrmPagos
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
        grpCliente = new GroupBox();
        lblDni = new Label();
        txtDni = new TextBox();
        btnBuscar = new Button();
        lblCliente = new Label();
        lblReservas = new Label();
        dgvReservas = new DataGridView();
        grpPago = new GroupBox();
        lblReserva = new Label();
        lblImporte = new Label();
        nudImporte = new NumericUpDown();
        lblMetodo = new Label();
        cboMetodo = new ComboBox();
        btnRegistrar = new Button();
        lblPagos = new Label();
        dgvPagos = new DataGridView();
        grpCliente.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvReservas).BeginInit();
        grpPago.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudImporte).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvPagos).BeginInit();
        SuspendLayout();
        // 
        // grpCliente
        // 
        grpCliente.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpCliente.Controls.Add(lblDni);
        grpCliente.Controls.Add(txtDni);
        grpCliente.Controls.Add(btnBuscar);
        grpCliente.Controls.Add(lblCliente);
        grpCliente.Location = new Point(12, 12);
        grpCliente.Name = "grpCliente";
        grpCliente.Size = new Size(876, 62);
        grpCliente.TabIndex = 0;
        grpCliente.TabStop = false;
        grpCliente.Text = "Cliente";
        // 
        // lblDni
        // 
        lblDni.AutoSize = true;
        lblDni.Location = new Point(15, 28);
        lblDni.Name = "lblDni";
        lblDni.Size = new Size(30, 15);
        lblDni.TabIndex = 0;
        lblDni.Text = "DNI:";
        // 
        // txtDni
        // 
        txtDni.Location = new Point(55, 25);
        txtDni.MaxLength = 8;
        txtDni.Name = "txtDni";
        txtDni.Size = new Size(110, 23);
        txtDni.TabIndex = 1;
        txtDni.TextChanged += txtDni_TextChanged;
        txtDni.KeyDown += txtDni_KeyDown;
        // 
        // btnBuscar
        // 
        btnBuscar.Location = new Point(175, 22);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(80, 28);
        btnBuscar.TabIndex = 2;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = true;
        btnBuscar.Click += btnBuscar_Click;
        // 
        // lblCliente
        // 
        lblCliente.AutoSize = true;
        lblCliente.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblCliente.Location = new Point(275, 28);
        lblCliente.Name = "lblCliente";
        lblCliente.Size = new Size(0, 15);
        lblCliente.TabIndex = 3;
        // 
        // lblReservas
        // 
        lblReservas.AutoSize = true;
        lblReservas.Location = new Point(12, 82);
        lblReservas.Name = "lblReservas";
        lblReservas.Size = new Size(165, 15);
        lblReservas.TabIndex = 1;
        lblReservas.Text = "Reservas con saldo pendiente:";
        // 
        // dgvReservas
        // 
        dgvReservas.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        dgvReservas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvReservas.Location = new Point(12, 102);
        dgvReservas.Name = "dgvReservas";
        dgvReservas.Size = new Size(876, 190);
        dgvReservas.TabIndex = 2;
        dgvReservas.SelectionChanged += dgvReservas_SelectionChanged;
        // 
        // grpPago
        // 
        grpPago.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpPago.Controls.Add(lblReserva);
        grpPago.Controls.Add(lblImporte);
        grpPago.Controls.Add(nudImporte);
        grpPago.Controls.Add(lblMetodo);
        grpPago.Controls.Add(cboMetodo);
        grpPago.Controls.Add(btnRegistrar);
        grpPago.Location = new Point(12, 300);
        grpPago.Name = "grpPago";
        grpPago.Size = new Size(876, 100);
        grpPago.TabIndex = 3;
        grpPago.TabStop = false;
        grpPago.Text = "Registrar pago";
        // 
        // lblReserva
        // 
        lblReserva.AutoSize = true;
        lblReserva.Location = new Point(15, 28);
        lblReserva.Name = "lblReserva";
        lblReserva.Size = new Size(0, 15);
        lblReserva.TabIndex = 0;
        // 
        // lblImporte
        // 
        lblImporte.AutoSize = true;
        lblImporte.Location = new Point(15, 64);
        lblImporte.Name = "lblImporte";
        lblImporte.Size = new Size(46, 15);
        lblImporte.TabIndex = 1;
        lblImporte.Text = "Monto:";
        // 
        // nudImporte
        // 
        nudImporte.DecimalPlaces = 2;
        nudImporte.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
        nudImporte.Location = new Point(70, 60);
        nudImporte.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
        nudImporte.Name = "nudImporte";
        nudImporte.Size = new Size(130, 23);
        nudImporte.TabIndex = 2;
        nudImporte.ThousandsSeparator = true;
        // 
        // lblMetodo
        // 
        lblMetodo.AutoSize = true;
        lblMetodo.Location = new Point(220, 64);
        lblMetodo.Name = "lblMetodo";
        lblMetodo.Size = new Size(52, 15);
        lblMetodo.TabIndex = 3;
        lblMetodo.Text = "Método:";
        // 
        // cboMetodo
        // 
        cboMetodo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboMetodo.Location = new Point(280, 60);
        cboMetodo.Name = "cboMetodo";
        cboMetodo.Size = new Size(140, 23);
        cboMetodo.TabIndex = 4;
        // 
        // btnRegistrar
        // 
        btnRegistrar.Location = new Point(440, 57);
        btnRegistrar.Name = "btnRegistrar";
        btnRegistrar.Size = new Size(130, 30);
        btnRegistrar.TabIndex = 5;
        btnRegistrar.Text = "Registrar pago";
        btnRegistrar.UseVisualStyleBackColor = true;
        btnRegistrar.Click += btnRegistrar_Click;
        // 
        // lblPagos
        // 
        lblPagos.AutoSize = true;
        lblPagos.Location = new Point(12, 410);
        lblPagos.Name = "lblPagos";
        lblPagos.Size = new Size(110, 15);
        lblPagos.TabIndex = 4;
        lblPagos.Text = "Pagos de la reserva:";
        // 
        // dgvPagos
        // 
        dgvPagos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvPagos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvPagos.Location = new Point(12, 430);
        dgvPagos.Name = "dgvPagos";
        dgvPagos.Size = new Size(876, 158);
        dgvPagos.TabIndex = 5;
        // 
        // FrmPagos
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 600);
        Controls.Add(grpCliente);
        Controls.Add(lblReservas);
        Controls.Add(dgvReservas);
        Controls.Add(grpPago);
        Controls.Add(lblPagos);
        Controls.Add(dgvPagos);
        Name = "FrmPagos";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Registrar pago";
        Load += FrmPagos_Load;
        grpCliente.ResumeLayout(false);
        grpCliente.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvReservas).EndInit();
        grpPago.ResumeLayout(false);
        grpPago.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudImporte).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvPagos).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private GroupBox grpCliente;
    private Label lblDni;
    private TextBox txtDni;
    private Button btnBuscar;
    private Label lblCliente;
    private Label lblReservas;
    private DataGridView dgvReservas;
    private GroupBox grpPago;
    private Label lblReserva;
    private Label lblImporte;
    private NumericUpDown nudImporte;
    private Label lblMetodo;
    private ComboBox cboMetodo;
    private Button btnRegistrar;
    private Label lblPagos;
    private DataGridView dgvPagos;
}
