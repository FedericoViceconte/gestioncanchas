using System.ComponentModel;

namespace GestionCanchas.Formularios;

/// <summary>Formulario con el tema de la aplicación y su ícono.</summary>
public class FormularioBase : Form
{
    private static Icon _icono;

    public static Icon IconoAplicacion => _icono ??= CargarIcono();

    protected override void OnLoad(EventArgs e)
    {
        if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
        {
            Tema.Aplicar(this);
            if (IconoAplicacion != null)
                Icon = IconoAplicacion;
        }
        base.OnLoad(e);
    }

    private static Icon CargarIcono()
    {
        using Stream flujo = typeof(FormularioBase).Assembly.GetManifestResourceStream("GestionCanchas.Recursos.GestionCanchas.ico");
        return flujo == null ? null : new Icon(flujo);
    }
}
