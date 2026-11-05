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
        tablaPrincipal = new TableLayoutPanel();
        tarjetaTurnos = new TarjetaIndicador();
        tarjetaOcupacion = new TarjetaIndicador();
        tarjetaCobrado = new TarjetaIndicador();
        tarjetaDeuda = new TarjetaIndicador();
        tablaInferior = new TableLayoutPanel();
        tarjetaAgenda = new Tarjeta();
        dgvTurnos = new DataGridView();
        tablaDerecha = new TableLayoutPanel();
        tarjetaGrafico = new Tarjeta();
        grafico = new GraficoBarras();
        tarjetaDeudores = new Tarjeta();
        dgvDeudores = new DataGridView();
        tablaPrincipal.SuspendLayout();
        tablaInferior.SuspendLayout();
        tarjetaAgenda.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvTurnos).BeginInit();
        tablaDerecha.SuspendLayout();
        tarjetaGrafico.SuspendLayout();
        tarjetaDeudores.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvDeudores).BeginInit();
        SuspendLayout();
        //
        // tablaPrincipal
        //
        tablaPrincipal.ColumnCount = 4;
        tablaPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tablaPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tablaPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tablaPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tablaPrincipal.Controls.Add(tarjetaTurnos, 0, 0);
        tablaPrincipal.Controls.Add(tarjetaOcupacion, 1, 0);
        tablaPrincipal.Controls.Add(tarjetaCobrado, 2, 0);
        tablaPrincipal.Controls.Add(tarjetaDeuda, 3, 0);
        tablaPrincipal.Controls.Add(tablaInferior, 0, 1);
        tablaPrincipal.SetColumnSpan(tablaInferior, 4);
        tablaPrincipal.Dock = DockStyle.Fill;
        tablaPrincipal.Name = "tablaPrincipal";
        tablaPrincipal.RowCount = 2;
        tablaPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 128F));
        tablaPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tablaPrincipal.TabIndex = 0;
        //
        // tarjetas de indicadores
        //
        tarjetaTurnos.Dock = DockStyle.Fill;
        tarjetaTurnos.Icono = Icono.Calendario;
        tarjetaTurnos.Margin = new Padding(0, 0, 14, 14);
        tarjetaTurnos.Name = "tarjetaTurnos";
        tarjetaTurnos.Titulo = "Turnos de hoy";
        tarjetaOcupacion.Acento = Tema.Informacion;
        tarjetaOcupacion.Dock = DockStyle.Fill;
        tarjetaOcupacion.Icono = Icono.Grilla;
        tarjetaOcupacion.Margin = new Padding(0, 0, 14, 14);
        tarjetaOcupacion.Name = "tarjetaOcupacion";
        tarjetaOcupacion.Titulo = "Ocupación de hoy";
        tarjetaCobrado.Dock = DockStyle.Fill;
        tarjetaCobrado.Icono = Icono.Tarjeta;
        tarjetaCobrado.Margin = new Padding(0, 0, 14, 14);
        tarjetaCobrado.Name = "tarjetaCobrado";
        tarjetaCobrado.Titulo = "Cobrado hoy";
        tarjetaDeuda.Acento = Tema.Peligro;
        tarjetaDeuda.Dock = DockStyle.Fill;
        tarjetaDeuda.Icono = Icono.Personas;
        tarjetaDeuda.Margin = new Padding(0, 0, 0, 14);
        tarjetaDeuda.Name = "tarjetaDeuda";
        tarjetaDeuda.Titulo = "Deuda de clientes";
        //
        // tablaInferior
        //
        tablaInferior.ColumnCount = 2;
        tablaInferior.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
        tablaInferior.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
        tablaInferior.Controls.Add(tarjetaAgenda, 0, 0);
        tablaInferior.Controls.Add(tablaDerecha, 1, 0);
        tablaInferior.Dock = DockStyle.Fill;
        tablaInferior.Margin = new Padding(0);
        tablaInferior.Name = "tablaInferior";
        tablaInferior.RowCount = 1;
        tablaInferior.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        //
        // tarjetaAgenda
        //
        tarjetaAgenda.Controls.Add(dgvTurnos);
        tarjetaAgenda.Dock = DockStyle.Fill;
        tarjetaAgenda.Margin = new Padding(0, 0, 14, 0);
        tarjetaAgenda.Name = "tarjetaAgenda";
        tarjetaAgenda.Titulo = "Turnos de hoy";
        //
        // dgvTurnos
        //
        dgvTurnos.Dock = DockStyle.Fill;
        dgvTurnos.Name = "dgvTurnos";
        dgvTurnos.TabIndex = 0;
        //
        // tablaDerecha
        //
        tablaDerecha.ColumnCount = 1;
        tablaDerecha.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tablaDerecha.Controls.Add(tarjetaGrafico, 0, 0);
        tablaDerecha.Controls.Add(tarjetaDeudores, 0, 1);
        tablaDerecha.Dock = DockStyle.Fill;
        tablaDerecha.Margin = new Padding(0);
        tablaDerecha.Name = "tablaDerecha";
        tablaDerecha.RowCount = 2;
        tablaDerecha.RowStyles.Add(new RowStyle(SizeType.Absolute, 250F));
        tablaDerecha.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        //
        // tarjetaGrafico
        //
        tarjetaGrafico.Controls.Add(grafico);
        tarjetaGrafico.Dock = DockStyle.Fill;
        tarjetaGrafico.Margin = new Padding(0, 0, 0, 14);
        tarjetaGrafico.Name = "tarjetaGrafico";
        tarjetaGrafico.Titulo = "Cobrado en los últimos 7 días";
        //
        // grafico
        //
        grafico.Dock = DockStyle.Fill;
        grafico.Name = "grafico";
        //
        // tarjetaDeudores
        //
        tarjetaDeudores.Controls.Add(dgvDeudores);
        tarjetaDeudores.Dock = DockStyle.Fill;
        tarjetaDeudores.Margin = new Padding(0);
        tarjetaDeudores.Name = "tarjetaDeudores";
        tarjetaDeudores.Titulo = "Clientes con deuda";
        //
        // dgvDeudores
        //
        dgvDeudores.Dock = DockStyle.Fill;
        dgvDeudores.Name = "dgvDeudores";
        dgvDeudores.TabIndex = 1;
        dgvDeudores.CellFormatting += dgvDeudores_CellFormatting;
        //
        // FrmResumen
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1040, 640);
        Controls.Add(tablaPrincipal);
        Name = "FrmResumen";
        Text = "Inicio";
        Load += FrmResumen_Load;
        tablaPrincipal.ResumeLayout(false);
        tablaInferior.ResumeLayout(false);
        tarjetaAgenda.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvTurnos).EndInit();
        tablaDerecha.ResumeLayout(false);
        tarjetaGrafico.ResumeLayout(false);
        tarjetaDeudores.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvDeudores).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel tablaPrincipal;
    private TarjetaIndicador tarjetaTurnos;
    private TarjetaIndicador tarjetaOcupacion;
    private TarjetaIndicador tarjetaCobrado;
    private TarjetaIndicador tarjetaDeuda;
    private TableLayoutPanel tablaInferior;
    private Tarjeta tarjetaAgenda;
    private DataGridView dgvTurnos;
    private TableLayoutPanel tablaDerecha;
    private Tarjeta tarjetaGrafico;
    private GraficoBarras grafico;
    private Tarjeta tarjetaDeudores;
    private DataGridView dgvDeudores;
}
