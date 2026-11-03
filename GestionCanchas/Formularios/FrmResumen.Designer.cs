
namespace GestionCanchas.Formularios;

partial class FrmResumen
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
        lblTitulo = new Label();
        lblTurnos = new Label();
        lblCobrado = new Label();
        lblDeuda = new Label();
        lblTurnosHoy = new Label();
        dgvTurnos = new DataGridView();
        lblDeudores = new Label();
        dgvDeudores = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)dgvTurnos).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvDeudores).BeginInit();
        SuspendLayout();

        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitulo.Location = new Point(12, 10);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(200, 25);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Resumen de hoy";

        lblTurnos.AutoSize = true;
        lblTurnos.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblTurnos.Location = new Point(14, 50);
        lblTurnos.Name = "lblTurnos";
        lblTurnos.Size = new Size(120, 20);
        lblTurnos.TabIndex = 1;

        lblCobrado.AutoSize = true;
        lblCobrado.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblCobrado.ForeColor = Color.DarkGreen;
        lblCobrado.Location = new Point(270, 50);
        lblCobrado.Name = "lblCobrado";
        lblCobrado.Size = new Size(120, 20);
        lblCobrado.TabIndex = 2;

        lblDeuda.AutoSize = true;
        lblDeuda.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblDeuda.ForeColor = Color.Firebrick;
        lblDeuda.Location = new Point(560, 50);
        lblDeuda.Name = "lblDeuda";
        lblDeuda.Size = new Size(120, 20);
        lblDeuda.TabIndex = 3;

        lblTurnosHoy.AutoSize = true;
        lblTurnosHoy.Location = new Point(12, 88);
        lblTurnosHoy.Name = "lblTurnosHoy";
        lblTurnosHoy.Size = new Size(85, 15);
        lblTurnosHoy.TabIndex = 4;
        lblTurnosHoy.Text = "Turnos de hoy:";

        dgvTurnos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvTurnos.Location = new Point(12, 108);
        dgvTurnos.Name = "dgvTurnos";
        dgvTurnos.Size = new Size(530, 330);
        dgvTurnos.TabIndex = 5;

        lblDeudores.AutoSize = true;
        lblDeudores.Location = new Point(556, 88);
        lblDeudores.Name = "lblDeudores";
        lblDeudores.Size = new Size(110, 15);
        lblDeudores.TabIndex = 6;
        lblDeudores.Text = "Clientes con deuda:";

        dgvDeudores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvDeudores.Location = new Point(556, 108);
        dgvDeudores.Name = "dgvDeudores";
        dgvDeudores.Size = new Size(372, 330);
        dgvDeudores.TabIndex = 7;
        dgvDeudores.CellFormatting += dgvDeudores_CellFormatting;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(940, 452);
        ControlBox = false;
        Controls.Add(lblTitulo);
        Controls.Add(lblTurnos);
        Controls.Add(lblCobrado);
        Controls.Add(lblDeuda);
        Controls.Add(lblTurnosHoy);
        Controls.Add(dgvTurnos);
        Controls.Add(lblDeudores);
        Controls.Add(dgvDeudores);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmResumen";
        ShowIcon = false;
        StartPosition = FormStartPosition.Manual;
        Location = new Point(20, 20);
        Text = "Resumen del día";
        Load += FrmResumen_Load;
        ((System.ComponentModel.ISupportInitialize)dgvTurnos).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvDeudores).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblTitulo;
    private Label lblTurnos;
    private Label lblCobrado;
    private Label lblDeuda;
    private Label lblTurnosHoy;
    private DataGridView dgvTurnos;
    private Label lblDeudores;
    private DataGridView dgvDeudores;
}