namespace GestionCanchas.Formularios;

partial class FrmHorarios
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
        lblInicio = new Label();
        dtpInicio = new DateTimePicker();
        lblFin = new Label();
        dtpFin = new DateTimePicker();
        btnAgregar = new Button();
        btnEliminar = new Button();
        dgvHorarios = new DataGridView();
        grpNuevo.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHorarios).BeginInit();
        SuspendLayout();

        grpNuevo.Controls.Add(lblInicio);
        grpNuevo.Controls.Add(dtpInicio);
        grpNuevo.Controls.Add(lblFin);
        grpNuevo.Controls.Add(dtpFin);
        grpNuevo.Controls.Add(btnAgregar);
        grpNuevo.Controls.Add(btnEliminar);
        grpNuevo.Location = new Point(12, 12);
        grpNuevo.Name = "grpNuevo";
        grpNuevo.Size = new Size(560, 70);
        grpNuevo.TabIndex = 0;
        grpNuevo.TabStop = false;
        grpNuevo.Text = "Horario";

        lblInicio.AutoSize = true;
        lblInicio.Location = new Point(15, 32);
        lblInicio.Name = "lblInicio";
        lblInicio.Size = new Size(42, 15);
        lblInicio.TabIndex = 0;
        lblInicio.Text = "Desde:";

        dtpInicio.CustomFormat = "HH:mm";
        dtpInicio.Format = DateTimePickerFormat.Custom;
        dtpInicio.Location = new Point(65, 28);
        dtpInicio.Name = "dtpInicio";
        dtpInicio.ShowUpDown = true;
        dtpInicio.Size = new Size(80, 23);
        dtpInicio.TabIndex = 1;

        lblFin.AutoSize = true;
        lblFin.Location = new Point(165, 32);
        lblFin.Name = "lblFin";
        lblFin.Size = new Size(40, 15);
        lblFin.TabIndex = 2;
        lblFin.Text = "Hasta:";

        dtpFin.CustomFormat = "HH:mm";
        dtpFin.Format = DateTimePickerFormat.Custom;
        dtpFin.Location = new Point(215, 28);
        dtpFin.Name = "dtpFin";
        dtpFin.ShowUpDown = true;
        dtpFin.Size = new Size(80, 23);
        dtpFin.TabIndex = 3;

        btnAgregar.Location = new Point(320, 25);
        btnAgregar.Name = "btnAgregar";
        btnAgregar.Size = new Size(100, 28);
        btnAgregar.TabIndex = 4;
        btnAgregar.Text = "Agregar";
        btnAgregar.UseVisualStyleBackColor = true;
        btnAgregar.Click += btnAgregar_Click;

        btnEliminar.Location = new Point(430, 25);
        btnEliminar.Name = "btnEliminar";
        btnEliminar.Size = new Size(110, 28);
        btnEliminar.TabIndex = 5;
        btnEliminar.Text = "Eliminar";
        btnEliminar.UseVisualStyleBackColor = true;
        btnEliminar.Click += btnEliminar_Click;

        dgvHorarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        dgvHorarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvHorarios.Location = new Point(12, 92);
        dgvHorarios.Name = "dgvHorarios";
        dgvHorarios.Size = new Size(560, 456);
        dgvHorarios.TabIndex = 1;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 560);
        Controls.Add(grpNuevo);
        Controls.Add(dgvHorarios);
        Name = "FrmHorarios";
        Text = "Horarios";
        Load += FrmHorarios_Load;
        grpNuevo.ResumeLayout(false);
        grpNuevo.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHorarios).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private GroupBox grpNuevo;
    private Label lblInicio;
    private DateTimePicker dtpInicio;
    private Label lblFin;
    private DateTimePicker dtpFin;
    private Button btnAgregar;
    private Button btnEliminar;
    private DataGridView dgvHorarios;
}
