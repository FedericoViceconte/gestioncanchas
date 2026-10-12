namespace GestionCanchas.Formularios;

partial class FrmReserva
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
        btnBuscarCliente = new Button();
        lblCliente = new Label();
        grpTurno = new GroupBox();
        lblFecha = new Label();
        dtpFecha = new DateTimePicker();
        lblCancha = new Label();
        cboCancha = new ComboBox();
        lblHorario = new Label();
        cboHorario = new ComboBox();
        lblImporte = new Label();
        grpSena = new GroupBox();
        lblMonto = new Label();
        nudSena = new NumericUpDown();
        lblSenaMinima = new Label();
        lblMetodo = new Label();
        cboMetodo = new ComboBox();
        btnConfirmar = new Button();
        btnCancelar = new Button();
        grpCliente.SuspendLayout();
        grpTurno.SuspendLayout();
        grpSena.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudSena).BeginInit();
        SuspendLayout();

        grpCliente.Controls.Add(lblDni);
        grpCliente.Controls.Add(txtDni);
        grpCliente.Controls.Add(btnBuscarCliente);
        grpCliente.Controls.Add(lblCliente);
        grpCliente.Location = new Point(12, 12);
        grpCliente.Name = "grpCliente";
        grpCliente.Size = new Size(496, 88);
        grpCliente.TabIndex = 0;
        grpCliente.TabStop = false;
        grpCliente.Text = "Cliente";

        lblDni.AutoSize = true;
        lblDni.Location = new Point(15, 30);
        lblDni.Name = "lblDni";
        lblDni.Size = new Size(30, 15);
        lblDni.TabIndex = 0;
        lblDni.Text = "DNI:";

        txtDni.Location = new Point(90, 27);
        txtDni.MaxLength = 8;
        txtDni.Name = "txtDni";
        txtDni.Size = new Size(110, 23);
        txtDni.TabIndex = 1;
        txtDni.TextChanged += txtDni_TextChanged;
        txtDni.KeyDown += txtDni_KeyDown;

        btnBuscarCliente.Location = new Point(210, 24);
        btnBuscarCliente.Name = "btnBuscarCliente";
        btnBuscarCliente.Size = new Size(80, 28);
        btnBuscarCliente.TabIndex = 2;
        btnBuscarCliente.Text = "Buscar";
        btnBuscarCliente.UseVisualStyleBackColor = true;
        btnBuscarCliente.Click += btnBuscarCliente_Click;

        lblCliente.AutoSize = true;
        lblCliente.Location = new Point(15, 60);
        lblCliente.Name = "lblCliente";
        lblCliente.Size = new Size(0, 15);
        lblCliente.TabIndex = 3;

        grpTurno.Controls.Add(lblFecha);
        grpTurno.Controls.Add(dtpFecha);
        grpTurno.Controls.Add(lblCancha);
        grpTurno.Controls.Add(cboCancha);
        grpTurno.Controls.Add(lblHorario);
        grpTurno.Controls.Add(cboHorario);
        grpTurno.Controls.Add(lblImporte);
        grpTurno.Location = new Point(12, 106);
        grpTurno.Name = "grpTurno";
        grpTurno.Size = new Size(496, 150);
        grpTurno.TabIndex = 1;
        grpTurno.TabStop = false;
        grpTurno.Text = "Turno";

        lblFecha.AutoSize = true;
        lblFecha.Location = new Point(15, 32);
        lblFecha.Name = "lblFecha";
        lblFecha.Size = new Size(41, 15);
        lblFecha.TabIndex = 0;
        lblFecha.Text = "Fecha:";

        dtpFecha.Format = DateTimePickerFormat.Short;
        dtpFecha.Location = new Point(90, 28);
        dtpFecha.Name = "dtpFecha";
        dtpFecha.Size = new Size(140, 23);
        dtpFecha.TabIndex = 1;
        dtpFecha.ValueChanged += dtpFecha_ValueChanged;

        lblCancha.AutoSize = true;
        lblCancha.Location = new Point(15, 67);
        lblCancha.Name = "lblCancha";
        lblCancha.Size = new Size(48, 15);
        lblCancha.TabIndex = 2;
        lblCancha.Text = "Cancha:";

        cboCancha.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCancha.Location = new Point(90, 63);
        cboCancha.Name = "cboCancha";
        cboCancha.Size = new Size(250, 23);
        cboCancha.TabIndex = 3;
        cboCancha.SelectedIndexChanged += cboCancha_SelectedIndexChanged;

        lblHorario.AutoSize = true;
        lblHorario.Location = new Point(15, 102);
        lblHorario.Name = "lblHorario";
        lblHorario.Size = new Size(50, 15);
        lblHorario.TabIndex = 4;
        lblHorario.Text = "Horario:";

        cboHorario.DropDownStyle = ComboBoxStyle.DropDownList;
        cboHorario.Location = new Point(90, 98);
        cboHorario.Name = "cboHorario";
        cboHorario.Size = new Size(140, 23);
        cboHorario.TabIndex = 5;
        cboHorario.SelectedIndexChanged += cboHorario_SelectedIndexChanged;

        lblImporte.AutoSize = true;
        lblImporte.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblImporte.Location = new Point(245, 102);
        lblImporte.Name = "lblImporte";
        lblImporte.Size = new Size(0, 15);
        lblImporte.TabIndex = 6;

        grpSena.Controls.Add(lblMonto);
        grpSena.Controls.Add(nudSena);
        grpSena.Controls.Add(lblSenaMinima);
        grpSena.Controls.Add(lblMetodo);
        grpSena.Controls.Add(cboMetodo);
        grpSena.Location = new Point(12, 262);
        grpSena.Name = "grpSena";
        grpSena.Size = new Size(496, 100);
        grpSena.TabIndex = 2;
        grpSena.TabStop = false;
        grpSena.Text = "Seña";

        lblMonto.AutoSize = true;
        lblMonto.Location = new Point(15, 32);
        lblMonto.Name = "lblMonto";
        lblMonto.Size = new Size(46, 15);
        lblMonto.TabIndex = 0;
        lblMonto.Text = "Monto:";

        nudSena.DecimalPlaces = 2;
        nudSena.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
        nudSena.Location = new Point(120, 28);
        nudSena.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
        nudSena.Name = "nudSena";
        nudSena.Size = new Size(130, 23);
        nudSena.TabIndex = 1;
        nudSena.ThousandsSeparator = true;

        lblSenaMinima.AutoSize = true;
        lblSenaMinima.Location = new Point(265, 32);
        lblSenaMinima.Name = "lblSenaMinima";
        lblSenaMinima.Size = new Size(0, 15);
        lblSenaMinima.TabIndex = 2;

        lblMetodo.AutoSize = true;
        lblMetodo.Location = new Point(15, 67);
        lblMetodo.Name = "lblMetodo";
        lblMetodo.Size = new Size(98, 15);
        lblMetodo.TabIndex = 3;
        lblMetodo.Text = "Método de pago:";

        cboMetodo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboMetodo.Location = new Point(120, 63);
        cboMetodo.Name = "cboMetodo";
        cboMetodo.Size = new Size(130, 23);
        cboMetodo.TabIndex = 4;

        btnConfirmar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnConfirmar.Location = new Point(312, 378);
        btnConfirmar.Name = "btnConfirmar";
        btnConfirmar.Size = new Size(95, 30);
        btnConfirmar.TabIndex = 3;
        btnConfirmar.Text = "Confirmar";
        btnConfirmar.UseVisualStyleBackColor = true;
        btnConfirmar.Click += btnConfirmar_Click;

        btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCancelar.DialogResult = DialogResult.Cancel;
        btnCancelar.Location = new Point(413, 378);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(95, 30);
        btnCancelar.TabIndex = 4;
        btnCancelar.Text = "Cerrar";
        btnCancelar.UseVisualStyleBackColor = true;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancelar;
        ClientSize = new Size(520, 420);
        Controls.Add(grpCliente);
        Controls.Add(grpTurno);
        Controls.Add(grpSena);
        Controls.Add(btnConfirmar);
        Controls.Add(btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmReserva";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Nueva reserva";
        Load += FrmReserva_Load;
        grpCliente.ResumeLayout(false);
        grpCliente.PerformLayout();
        grpTurno.ResumeLayout(false);
        grpTurno.PerformLayout();
        grpSena.ResumeLayout(false);
        grpSena.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudSena).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private GroupBox grpCliente;
    private Label lblDni;
    private TextBox txtDni;
    private Button btnBuscarCliente;
    private Label lblCliente;
    private GroupBox grpTurno;
    private Label lblFecha;
    private DateTimePicker dtpFecha;
    private Label lblCancha;
    private ComboBox cboCancha;
    private Label lblHorario;
    private ComboBox cboHorario;
    private Label lblImporte;
    private GroupBox grpSena;
    private Label lblMonto;
    private NumericUpDown nudSena;
    private Label lblSenaMinima;
    private Label lblMetodo;
    private ComboBox cboMetodo;
    private Button btnConfirmar;
    private Button btnCancelar;
}
