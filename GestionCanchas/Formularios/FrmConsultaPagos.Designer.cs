namespace GestionCanchas.Formularios;

partial class FrmConsultaPagos
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
        lblDni = new Label();
        txtDni = new TextBox();
        lblDesde = new Label();
        dtpDesde = new DateTimePicker();
        lblHasta = new Label();
        dtpHasta = new DateTimePicker();
        btnBuscar = new Button();
        dgvPagos = new DataGridView();
        lblTotal = new Label();
        grpFiltros.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPagos).BeginInit();
        SuspendLayout();

        grpFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpFiltros.Controls.Add(lblDni);
        grpFiltros.Controls.Add(txtDni);
        grpFiltros.Controls.Add(lblDesde);
        grpFiltros.Controls.Add(dtpDesde);
        grpFiltros.Controls.Add(lblHasta);
        grpFiltros.Controls.Add(dtpHasta);
        grpFiltros.Controls.Add(btnBuscar);
        grpFiltros.Location = new Point(12, 12);
        grpFiltros.Name = "grpFiltros";
        grpFiltros.Size = new Size(876, 62);
        grpFiltros.TabIndex = 0;
        grpFiltros.TabStop = false;
        grpFiltros.Text = "Filtros";

        lblDni.AutoSize = true;
        lblDni.Location = new Point(15, 28);
        lblDni.Name = "lblDni";
        lblDni.Size = new Size(30, 15);
        lblDni.TabIndex = 0;
        lblDni.Text = "DNI:";

        txtDni.Location = new Point(50, 25);
        txtDni.MaxLength = 8;
        txtDni.Name = "txtDni";
        txtDni.Size = new Size(100, 23);
        txtDni.TabIndex = 1;

        lblDesde.AutoSize = true;
        lblDesde.Location = new Point(170, 28);
        lblDesde.Name = "lblDesde";
        lblDesde.Size = new Size(42, 15);
        lblDesde.TabIndex = 2;
        lblDesde.Text = "Desde:";

        dtpDesde.Format = DateTimePickerFormat.Short;
        dtpDesde.Location = new Point(220, 25);
        dtpDesde.Name = "dtpDesde";
        dtpDesde.Size = new Size(110, 23);
        dtpDesde.TabIndex = 3;

        lblHasta.AutoSize = true;
        lblHasta.Location = new Point(345, 28);
        lblHasta.Name = "lblHasta";
        lblHasta.Size = new Size(40, 15);
        lblHasta.TabIndex = 4;
        lblHasta.Text = "Hasta:";

        dtpHasta.Format = DateTimePickerFormat.Short;
        dtpHasta.Location = new Point(392, 25);
        dtpHasta.Name = "dtpHasta";
        dtpHasta.Size = new Size(110, 23);
        dtpHasta.TabIndex = 5;

        btnBuscar.Location = new Point(520, 22);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(80, 28);
        btnBuscar.TabIndex = 6;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = true;
        btnBuscar.Click += btnBuscar_Click;

        dgvPagos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvPagos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvPagos.Location = new Point(12, 82);
        dgvPagos.Name = "dgvPagos";
        dgvPagos.Size = new Size(876, 440);
        dgvPagos.TabIndex = 1;

        lblTotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblTotal.AutoSize = true;
        lblTotal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblTotal.Location = new Point(12, 532);
        lblTotal.Name = "lblTotal";
        lblTotal.Size = new Size(0, 15);
        lblTotal.TabIndex = 2;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 560);
        Controls.Add(grpFiltros);
        Controls.Add(dgvPagos);
        Controls.Add(lblTotal);
        Name = "FrmConsultaPagos";
        Text = "Consultar pagos";
        Load += FrmConsultaPagos_Load;
        grpFiltros.ResumeLayout(false);
        grpFiltros.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPagos).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private GroupBox grpFiltros;
    private Label lblDni;
    private TextBox txtDni;
    private Label lblDesde;
    private DateTimePicker dtpDesde;
    private Label lblHasta;
    private DateTimePicker dtpHasta;
    private Button btnBuscar;
    private DataGridView dgvPagos;
    private Label lblTotal;
}
