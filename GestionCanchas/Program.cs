using System.Globalization;
using GestionCanchas.Formularios;

namespace GestionCanchas;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        var cultura = new CultureInfo("es-AR");
        CultureInfo.CurrentCulture = cultura;
        CultureInfo.CurrentUICulture = cultura;
        CultureInfo.DefaultThreadCurrentCulture = cultura;
        CultureInfo.DefaultThreadCurrentUICulture = cultura;

        ApplicationConfiguration.Initialize();
        Application.Run(new FrmPrincipal());
    }
}
