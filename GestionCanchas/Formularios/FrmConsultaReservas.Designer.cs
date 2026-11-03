namespace GestionCanchas.Formularios;

partial class FrmConsultaReservas
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
        grpFiltros = new GroupBox();
        chkFecha = new CheckBox();
        dtpFecha = new DateTimePicker();
        lblDni = new Label();
        txtDni = new TextBox();
        lblCancha = new Label();
        cboCancha = new ComboBox();
        lblEstado = new Label();
        cboEstado = new ComboBox();
        btnBuscar = new Button();
        btnLimpiar = new Button();
        dgvReservas = new DataGridView();
        btnModificar = new Button();
        btnCancelarReserva = new Button();
        btnRegistrarPago = new Button();
        btnAusente = new Button();
        btnAnularPago = new Button();
        btnDevolucion = new Button();
        lblPagos = new Label();
        dgvPagos = new DataGridView();
        grpFiltros.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvReservas).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvPagos).BeginInit();
        SuspendLayout();

        grpFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpFiltros.Controls.Add(chkFecha);
        grpFiltros.Controls.Add(dtpFecha);
        grpFiltros.Controls.Add(lblDni);
        grpFiltros.Controls.Add(txtDni);
        grpFiltros.Controls.Add(lblCancha);
        grpFiltros.Controls.Add(cboCancha);
        grpFiltros.Controls.Add(lblEstado);
        grpFiltros.Controls.Add(cboEstado);
        grpFiltros.Controls.Add(btnBuscar);
        grpFiltros.Controls.Add(btnLimpiar);
        grpFiltros.Location = new Point(12, 12);
        grpFiltros.Name = "grpFiltros";
        grpFiltros.Size = new Size(976, 65);
        grpFiltros.TabIndex = 0;
        grpFiltros.TabStop = false;
        grpFiltros.Text = "Filtros";

        chkFecha.AutoSize = true;
        chkFecha.Location = new Point(15, 28);
        chkFecha.Name = "chkFecha";
        chkFecha.Size = new Size(60, 19);
        chkFecha.TabIndex = 0;
        chkFecha.Text = "Fecha:";
        chkFecha.UseVisualStyleBackColor = true;
        chkFecha.CheckedChanged += chkFecha_CheckedChanged;

        dtpFecha.Enabled = false;
        dtpFecha.Format = DateTimePickerFormat.Short;
        dtpFecha.Location = new Point(80, 25);
        dtpFecha.Name = "dtpFecha";
        dtpFecha.Size = new Size(120, 23);
        dtpFecha.TabIndex = 1;

        lblDni.AutoSize = true;
        lblDni.Location = new Point(220, 29);
        lblDni.Name = "lblDni";
        lblDni.Size = new Size(30, 15);
        lblDni.TabIndex = 2;
        lblDni.Text = "DNI:";

        txtDni.Location = new Point(255, 25);
        txtDni.MaxLength = 8;
        txtDni.Name = "txtDni";
        txtDni.Size = new Size(100, 23);
        txtDni.TabIndex = 3;

        lblCancha.AutoSize = true;
        lblCancha.Location = new Point(370, 29);
        lblCancha.Name = "lblCancha";
        lblCancha.Size = new Size(48, 15);
        lblCancha.TabIndex = 4;
        lblCancha.Text = "Cancha:";

        cboCancha.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCancha.Location = new Point(425, 25);
        cboCancha.Name = "cboCancha";
        cboCancha.Size = new Size(180, 23);
        cboCancha.TabIndex = 5;

        lblEstado.AutoSize = true;
        lblEstado.Location = new Point(620, 29);
        lblEstado.Name = "lblEstado";
        lblEstado.Size = new Size(45, 15);
        lblEstado.TabIndex = 6;
        lblEstado.Text = "Estado:";

        cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEstado.Location = new Point(670, 25);
        cboEstado.Name = "cboEstado";
        cboEstado.Size = new Size(110, 23);
        cboEstado.TabIndex = 7;

        btnBuscar.Location = new Point(795, 22);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(80, 28);
        btnBuscar.TabIndex = 8;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = true;
        btnBuscar.Click += btnBuscar_Click;

        btnLimpiar.Location = new Point(881, 22);
        btnLimpiar.Name = "btnLimpiar";
        btnLimpiar.Size = new Size(80, 28);
        btnLimpiar.TabIndex = 9;
        btnLimpiar.Text = "Limpiar";
        btnLimpiar.UseVisualStyleBackColor = true;
        btnLimpiar.Click += btnLimpiar_Click;

        dgvReservas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvReservas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvReservas.Location = new Point(12, 85);
        dgvReservas.Name = "dgvReservas";
        dgvReservas.Size = new Size(976, 290);
        dgvReservas.TabIndex = 1;
        dgvReservas.CellFormatting += dgvReservas_CellFormatting;
        dgvReservas.SelectionChanged += dgvReservas_SelectionChanged;

        btnModificar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnModificar.Location = new Point(12, 383);
        btnModificar.Name = "btnModificar";
        btnModificar.Size = new Size(110, 30);
        btnModificar.TabIndex = 2;
        btnModificar.Text = "Modificar";
        btnModificar.UseVisualStyleBackColor = true;
        btnModificar.Click += btnModificar_Click;

        btnCancelarReserva.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnCancelarReserva.Location = new Point(128, 383);
        btnCancelarReserva.Name = "btnCancelarReserva";
        btnCancelarReserva.Size = new Size(130, 30);
        btnCancelarReserva.TabIndex = 3;
        btnCancelarReserva.Text = "Cancelar reserva";
        btnCancelarReserva.UseVisualStyleBackColor = true;
        btnCancelarReserva.Click += btnCancelarReserva_Click;

        btnRegistrarPago.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnRegistrarPago.Location = new Point(264, 383);
        btnRegistrarPago.Name = "btnRegistrarPago";
        btnRegistrarPago.Size = new Size(120, 30);
        btnRegistrarPago.TabIndex = 4;
        btnRegistrarPago.Text = "Registrar pago";
        btnRegistrarPago.UseVisualStyleBackColor = true;
        btnRegistrarPago.Click += btnRegistrarPago_Click;

        btnAusente.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnAusente.Location = new Point(390, 383);
        btnAusente.Name = "btnAusente";
        btnAusente.Size = new Size(130, 30);
        btnAusente.TabIndex = 7;
        btnAusente.Text = "No se presentó";
        btnAusente.UseVisualStyleBackColor = true;
        btnAusente.Click += btnAusente_Click;

        btnAnularPago.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnAnularPago.Location = new Point(526, 383);
        btnAnularPago.Name = "btnAnularPago";
        btnAnularPago.Size = new Size(110, 30);
        btnAnularPago.TabIndex = 8;
        btnAnularPago.Text = "Anular pago";
        btnAnularPago.UseVisualStyleBackColor = true;
        btnAnularPago.Click += btnAnularPago_Click;

        btnDevolucion.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnDevolucion.Location = new Point(642, 383);
        btnDevolucion.Name = "btnDevolucion";
        btnDevolucion.Size = new Size(140, 30);
        btnDevolucion.TabIndex = 9;
        btnDevolucion.Text = "Registrar devolución";
        btnDevolucion.UseVisualStyleBackColor = true;
        btnDevolucion.Click += btnDevolucion_Click;

        lblPagos.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblPagos.AutoSize = true;
        lblPagos.Location = new Point(12, 423);
        lblPagos.Name = "lblPagos";
        lblPagos.Size = new Size(190, 15);
        lblPagos.TabIndex = 5;
        lblPagos.Text = "Pagos de la reserva seleccionada:";

        dgvPagos.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvPagos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvPagos.Location = new Point(12, 443);
        dgvPagos.Name = "dgvPagos";
        dgvPagos.Size = new Size(976, 145);
        dgvPagos.TabIndex = 6;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 600);
        Controls.Add(grpFiltros);
        Controls.Add(dgvReservas);
        Controls.Add(btnModificar);
        Controls.Add(btnCancelarReserva);
        Controls.Add(btnRegistrarPago);
        Controls.Add(btnAusente);
        Controls.Add(btnAnularPago);
        Controls.Add(btnDevolucion);
        Controls.Add(lblPagos);
        Controls.Add(dgvPagos);
        Name = "FrmConsultaReservas";
        Text = "Consultar reservas";
        Load += FrmConsultaReservas_Load;
        grpFiltros.ResumeLayout(false);
        grpFiltros.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvReservas).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvPagos).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private GroupBox grpFiltros;
    private CheckBox chkFecha;
    private DateTimePicker dtpFecha;
    private Label lblDni;
    private TextBox txtDni;
    private Label lblCancha;
    private ComboBox cboCancha;
    private Label lblEstado;
    private ComboBox cboEstado;
    private Button btnBuscar;
    private Button btnLimpiar;
    private DataGridView dgvReservas;
    private Button btnModificar;
    private Button btnCancelarReserva;
    private Button btnRegistrarPago;
    private Button btnAusente;
    private Button btnAnularPago;
    private Button btnDevolucion;
    private Label lblPagos;
    private DataGridView dgvPagos;
}
