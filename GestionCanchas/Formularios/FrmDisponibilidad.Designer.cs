namespace GestionCanchas.Formularios;

partial class FrmDisponibilidad
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
        lblFecha = new Label();
        dtpFecha = new DateTimePicker();
        btnAnterior = new Button();
        btnSiguiente = new Button();
        btnHoy = new Button();
        lblAyuda = new Label();
        dgvDisponibilidad = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)dgvDisponibilidad).BeginInit();
        SuspendLayout();

        lblFecha.AutoSize = true;
        lblFecha.Location = new Point(12, 18);
        lblFecha.Name = "lblFecha";
        lblFecha.Size = new Size(41, 15);
        lblFecha.TabIndex = 0;
        lblFecha.Text = "Fecha:";

        dtpFecha.Format = DateTimePickerFormat.Short;
        dtpFecha.Location = new Point(60, 14);
        dtpFecha.Name = "dtpFecha";
        dtpFecha.Size = new Size(130, 23);
        dtpFecha.TabIndex = 1;
        dtpFecha.ValueChanged += dtpFecha_ValueChanged;

        btnAnterior.Location = new Point(200, 11);
        btnAnterior.Name = "btnAnterior";
        btnAnterior.Size = new Size(35, 28);
        btnAnterior.TabIndex = 2;
        btnAnterior.Text = "<";
        btnAnterior.UseVisualStyleBackColor = true;
        btnAnterior.Click += btnAnterior_Click;

        btnSiguiente.Location = new Point(240, 11);
        btnSiguiente.Name = "btnSiguiente";
        btnSiguiente.Size = new Size(35, 28);
        btnSiguiente.TabIndex = 3;
        btnSiguiente.Text = ">";
        btnSiguiente.UseVisualStyleBackColor = true;
        btnSiguiente.Click += btnSiguiente_Click;

        btnHoy.Location = new Point(280, 11);
        btnHoy.Name = "btnHoy";
        btnHoy.Size = new Size(60, 28);
        btnHoy.TabIndex = 4;
        btnHoy.Text = "Hoy";
        btnHoy.UseVisualStyleBackColor = true;
        btnHoy.Click += btnHoy_Click;

        lblAyuda.AutoSize = true;
        lblAyuda.ForeColor = SystemColors.GrayText;
        lblAyuda.Location = new Point(360, 18);
        lblAyuda.Name = "lblAyuda";
        lblAyuda.Size = new Size(260, 15);
        lblAyuda.TabIndex = 5;
        lblAyuda.Text = "Doble clic en un turno libre para reservarlo.";

        dgvDisponibilidad.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvDisponibilidad.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvDisponibilidad.Location = new Point(12, 48);
        dgvDisponibilidad.Name = "dgvDisponibilidad";
        dgvDisponibilidad.Size = new Size(876, 500);
        dgvDisponibilidad.TabIndex = 6;
        dgvDisponibilidad.CellDoubleClick += dgvDisponibilidad_CellDoubleClick;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 560);
        Controls.Add(lblFecha);
        Controls.Add(dtpFecha);
        Controls.Add(btnAnterior);
        Controls.Add(btnSiguiente);
        Controls.Add(btnHoy);
        Controls.Add(lblAyuda);
        Controls.Add(dgvDisponibilidad);
        Name = "FrmDisponibilidad";
        Text = "Disponibilidad de canchas";
        Load += FrmDisponibilidad_Load;
        ((System.ComponentModel.ISupportInitialize)dgvDisponibilidad).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblFecha;
    private DateTimePicker dtpFecha;
    private Button btnAnterior;
    private Button btnSiguiente;
    private Button btnHoy;
    private Label lblAyuda;
    private DataGridView dgvDisponibilidad;
}
