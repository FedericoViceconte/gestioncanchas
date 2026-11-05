
using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

public partial class FrmDevolucion : FormularioBase
{
    private readonly PagoNegocio _pagoNegocio = new PagoNegocio();
    private readonly ReservaDetalle _reserva;

    public FrmDevolucion(ReservaDetalle reserva)
    {
        InitializeComponent();
        _reserva = reserva;
        cboMetodo.Items.AddRange(MetodosPago.Todos);
        cboMetodo.SelectedIndex = 0;
    }

    private void FrmDevolucion_Load(object sender, EventArgs e)
    {
        lblReserva.Text = $"Reserva N° {_reserva.IdReserva}: {_reserva.Cliente}, {_reserva.Fecha:dd/MM/yyyy} {_reserva.Horario}." +
                          $"\nCobrado: {_reserva.TotalPagado:C2}";
        nudImporte.Maximum = Math.Max(_reserva.TotalPagado, 0);
        nudImporte.Value = nudImporte.Maximum;
    }

    private void btnConfirmar_Click(object sender, EventArgs e)
    {
        try
        {
            _pagoNegocio.RegistrarDevolucion(_reserva.IdReserva, nudImporte.Value, cboMetodo.SelectedItem as string);
            Mensajes.Info("Devolución registrada.");
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }
}