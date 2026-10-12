using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

public partial class FrmConsultaPagos : Form, IRefrescable
{
    private readonly PagoNegocio _negocio = new PagoNegocio();

    public FrmConsultaPagos()
    {
        InitializeComponent();
        Grillas.Configurar(dgvPagos);
        dtpDesde.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        dtpHasta.Value = DateTime.Today;
    }

    private void FrmConsultaPagos_Load(object sender, EventArgs e) => Refrescar();

    public void Refrescar()
    {
        try
        {
            List<PagoDetalle> pagos = _negocio.Listar(txtDni.Text, dtpDesde.Value, dtpHasta.Value);
            dgvPagos.DataSource = pagos;
            Grillas.Formato(dgvPagos, "C2", "Importe");
            Grillas.Formato(dgvPagos, "dd/MM/yyyy", "FechaPago", "FechaReserva");
            lblTotal.Text = $"Total cobrado: {pagos.Sum(p => p.Importe):C2}  ({pagos.Count} pagos)";
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void btnBuscar_Click(object sender, EventArgs e) => Refrescar();
}
