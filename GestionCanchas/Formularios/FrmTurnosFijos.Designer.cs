
namespace GestionCanchas.Formularios;

partial class FrmTurnosFijos
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
        grpNuevo = new GroupBox();
        lblDni = new Label();
        txtDni = new TextBox();
        btnBuscarCliente = new Button();
        lblCliente = new Label();
        btnCrear = new Button();
        lblCancha = new Label();
        cboCancha = new ComboBox();
        lblDia = new Label();
        cboDia = new ComboBox();
        lblHorario = new Label();
        cboHorario = new ComboBox();
        lblDesde = new Label();
        dtpDesde = new DateTimePicker();
        lblSemanas = new Label();
        nudSemanas = new NumericUpDown();
        dgvTurnos = new DataGridView();
        btnGenerar = new Button();
        btnBaja = new Button();
        grpNuevo.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudSemanas).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvTurnos).BeginInit();
        SuspendLayout();

        grpNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpNuevo.Controls.Add(lblDni);
        grpNuevo.Controls.Add(txtDni);
        grpNuevo.Controls.Add(btnBuscarCliente);
        grpNuevo.Controls.Add(lblCliente);
        grpNuevo.Controls.Add(btnCrear);
        grpNuevo.Controls.Add(lblCancha);
        grpNuevo.Controls.Add(cboCancha);
        grpNuevo.Controls.Add(lblDia);
        grpNuevo.Controls.Add(cboDia);
        grpNuevo.Controls.Add(lblHorario);
        grpNuevo.Controls.Add(cboHorario);
        grpNuevo.Controls.Add(lblDesde);
        grpNuevo.Controls.Add(dtpDesde);
        grpNuevo.Controls.Add(lblSemanas);
        grpNuevo.Controls.Add(nudSemanas);
        grpNuevo.Location = new Point(12, 12);
        grpNuevo.Name = "grpNuevo";
        grpNuevo.Size = new Size(976, 105);
        grpNuevo.TabIndex = 0;
        grpNuevo.TabStop = false;
        grpNuevo.Text = "Nuevo turno fijo";

        lblDni.AutoSize = true;
        lblDni.Location = new Point(15, 29);
        lblDni.Name = "lblDni";
        lblDni.Size = new Size(30, 15);
        lblDni.TabIndex = 0;
        lblDni.Text = "DNI:";

        txtDni.Location = new Point(70, 25);
        txtDni.MaxLength = 8;
        txtDni.Name = "txtDni";
        txtDni.Size = new Size(110, 23);
        txtDni.TabIndex = 1;
        txtDni.KeyDown += txtDni_KeyDown;

        btnBuscarCliente.Location = new Point(190, 22);
        btnBuscarCliente.Name = "btnBuscarCliente";
        btnBuscarCliente.Size = new Size(80, 28);
        btnBuscarCliente.TabIndex = 2;
        btnBuscarCliente.Text = "Buscar";
        btnBuscarCliente.UseVisualStyleBackColor = true;
        btnBuscarCliente.Click += btnBuscarCliente_Click;

        lblCliente.AutoSize = true;
        lblCliente.Location = new Point(285, 29);
        lblCliente.Name = "lblCliente";
        lblCliente.Size = new Size(0, 15);
        lblCliente.TabIndex = 3;

        btnCrear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCrear.Location = new Point(846, 62);
        btnCrear.Name = "btnCrear";
        btnCrear.Size = new Size(115, 28);
        btnCrear.TabIndex = 14;
        btnCrear.Text = "Crear turno fijo";
        btnCrear.UseVisualStyleBackColor = true;
        btnCrear.Click += btnCrear_Click;

        lblCancha.AutoSize = true;
        lblCancha.Location = new Point(15, 69);
        lblCancha.Name = "lblCancha";
        lblCancha.Size = new Size(48, 15);
        lblCancha.TabIndex = 4;
        lblCancha.Text = "Cancha:";

        cboCancha.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCancha.Location = new Point(70, 65);
        cboCancha.Name = "cboCancha";
        cboCancha.Size = new Size(170, 23);
        cboCancha.TabIndex = 5;

        lblDia.AutoSize = true;
        lblDia.Location = new Point(255, 69);
        lblDia.Name = "lblDia";
        lblDia.Size = new Size(27, 15);
        lblDia.TabIndex = 6;
        lblDia.Text = "Día:";

        cboDia.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDia.Location = new Point(288, 65);
        cboDia.Name = "cboDia";
        cboDia.Size = new Size(100, 23);
        cboDia.TabIndex = 7;

        lblHorario.AutoSize = true;
        lblHorario.Location = new Point(400, 69);
        lblHorario.Name = "lblHorario";
        lblHorario.Size = new Size(50, 15);
        lblHorario.TabIndex = 8;
        lblHorario.Text = "Horario:";

        cboHorario.DropDownStyle = ComboBoxStyle.DropDownList;
        cboHorario.Location = new Point(455, 65);
        cboHorario.Name = "cboHorario";
        cboHorario.Size = new Size(110, 23);
        cboHorario.TabIndex = 9;

        lblDesde.AutoSize = true;
        lblDesde.Location = new Point(578, 69);
        lblDesde.Name = "lblDesde";
        lblDesde.Size = new Size(42, 15);
        lblDesde.TabIndex = 10;
        lblDesde.Text = "Desde:";

        dtpDesde.Format = DateTimePickerFormat.Short;
        dtpDesde.Location = new Point(625, 65);
        dtpDesde.Name = "dtpDesde";
        dtpDesde.Size = new Size(100, 23);
        dtpDesde.TabIndex = 11;

        lblSemanas.AutoSize = true;
        lblSemanas.Location = new Point(735, 69);
        lblSemanas.Name = "lblSemanas";
        lblSemanas.Size = new Size(55, 15);
        lblSemanas.TabIndex = 12;
        lblSemanas.Text = "Semanas:";

        nudSemanas.Location = new Point(793, 65);
        nudSemanas.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudSemanas.Name = "nudSemanas";
        nudSemanas.Size = new Size(45, 23);
        nudSemanas.TabIndex = 13;
        nudSemanas.Value = new decimal(new int[] { 8, 0, 0, 0 });

        dgvTurnos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvTurnos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvTurnos.Location = new Point(12, 125);
        dgvTurnos.Name = "dgvTurnos";
        dgvTurnos.Size = new Size(976, 425);
        dgvTurnos.TabIndex = 1;

        btnGenerar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnGenerar.Location = new Point(12, 558);
        btnGenerar.Name = "btnGenerar";
        btnGenerar.Size = new Size(170, 30);
        btnGenerar.TabIndex = 2;
        btnGenerar.Text = "Agregar más semanas";
        btnGenerar.UseVisualStyleBackColor = true;
        btnGenerar.Click += btnGenerar_Click;

        btnBaja.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnBaja.Location = new Point(188, 558);
        btnBaja.Name = "btnBaja";
        btnBaja.Size = new Size(120, 30);
        btnBaja.TabIndex = 3;
        btnBaja.Text = "Dar de baja";
        btnBaja.UseVisualStyleBackColor = true;
        btnBaja.Click += btnBaja_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 600);
        Controls.Add(grpNuevo);
        Controls.Add(dgvTurnos);
        Controls.Add(btnGenerar);
        Controls.Add(btnBaja);
        Name = "FrmTurnosFijos";
        Text = "Turnos fijos";
        Load += FrmTurnosFijos_Load;
        grpNuevo.ResumeLayout(false);
        grpNuevo.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudSemanas).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvTurnos).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private GroupBox grpNuevo;
    private Label lblDni;
    private TextBox txtDni;
    private Button btnBuscarCliente;
    private Label lblCliente;
    private Button btnCrear;
    private Label lblCancha;
    private ComboBox cboCancha;
    private Label lblDia;
    private ComboBox cboDia;
    private Label lblHorario;
    private ComboBox cboHorario;
    private Label lblDesde;
    private DateTimePicker dtpDesde;
    private Label lblSemanas;
    private NumericUpDown nudSemanas;
    private DataGridView dgvTurnos;
    private Button btnGenerar;
    private Button btnBaja;
}