
namespace GestionCanchas.Formularios;

partial class FrmDevolucion
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
        lblReserva = new Label();
        lblImporte = new Label();
        nudImporte = new NumericUpDown();
        lblMetodo = new Label();
        cboMetodo = new ComboBox();
        btnConfirmar = new Button();
        btnCancelar = new Button();
        ((System.ComponentModel.ISupportInitialize)nudImporte).BeginInit();
        SuspendLayout();

        lblReserva.Location = new Point(12, 12);
        lblReserva.Name = "lblReserva";
        lblReserva.Size = new Size(396, 40);
        lblReserva.TabIndex = 0;

        lblImporte.AutoSize = true;
        lblImporte.Location = new Point(12, 68);
        lblImporte.Name = "lblImporte";
        lblImporte.Size = new Size(112, 15);
        lblImporte.TabIndex = 1;
        lblImporte.Text = "Importe a devolver:";

        nudImporte.DecimalPlaces = 2;
        nudImporte.Location = new Point(140, 65);
        nudImporte.Name = "nudImporte";
        nudImporte.Size = new Size(130, 23);
        nudImporte.TabIndex = 2;
        nudImporte.ThousandsSeparator = true;

        lblMetodo.AutoSize = true;
        lblMetodo.Location = new Point(12, 102);
        lblMetodo.Name = "lblMetodo";
        lblMetodo.Size = new Size(52, 15);
        lblMetodo.TabIndex = 3;
        lblMetodo.Text = "Método:";

        cboMetodo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboMetodo.Location = new Point(140, 98);
        cboMetodo.Name = "cboMetodo";
        cboMetodo.Size = new Size(130, 23);
        cboMetodo.TabIndex = 4;

        btnConfirmar.Location = new Point(212, 145);
        btnConfirmar.Name = "btnConfirmar";
        btnConfirmar.Size = new Size(95, 30);
        btnConfirmar.TabIndex = 5;
        btnConfirmar.Text = "Confirmar";
        btnConfirmar.UseVisualStyleBackColor = true;
        btnConfirmar.Click += btnConfirmar_Click;

        btnCancelar.DialogResult = DialogResult.Cancel;
        btnCancelar.Location = new Point(313, 145);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(95, 30);
        btnCancelar.TabIndex = 6;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = true;

        AcceptButton = btnConfirmar;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancelar;
        ClientSize = new Size(420, 190);
        Controls.Add(lblReserva);
        Controls.Add(lblImporte);
        Controls.Add(nudImporte);
        Controls.Add(lblMetodo);
        Controls.Add(cboMetodo);
        Controls.Add(btnConfirmar);
        Controls.Add(btnCancelar);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmDevolucion";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Registrar devolución";
        Load += FrmDevolucion_Load;
        ((System.ComponentModel.ISupportInitialize)nudImporte).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblReserva;
    private Label lblImporte;
    private NumericUpDown nudImporte;
    private Label lblMetodo;
    private ComboBox cboMetodo;
    private Button btnConfirmar;
    private Button btnCancelar;
}