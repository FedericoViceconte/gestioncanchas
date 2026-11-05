using System.Globalization;
using GestionCanchas.Datos;
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

        if (!PrepararBaseDeDatos())
            return;

        while (true)
        {
            using (var login = new FrmLogin())
            {
                if (login.ShowDialog() != DialogResult.OK)
                    return;
            }

            using var principal = new FrmPrincipal();
            Application.Run(principal);
            if (!principal.CerroSesion)
                return;
        }
    }

    /// <summary>
    /// Verifica que la base exista y esté al día. Si no existe, ofrece crearla con datos de ejemplo.
    /// </summary>
    private static bool PrepararBaseDeDatos()
    {
        try
        {
            switch (BaseDatos.Verificar())
            {
                case EstadoBase.SinServidor:
                    MessageBox.Show(
                        $"No se pudo conectar a SQL Server en {Conexion.Servidor}.\n\n" +
                        "Instalá SQL Server Express LocalDB (viene con Visual Studio o se descarga gratis de Microsoft) " +
                        "o indicá otra cadena de conexión en appsettings.json.",
                        "Gestión de Canchas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;

                case EstadoBase.NoExiste:
                    if (!Mensajes.Confirmar(
                            $"No se encontró la base de datos {Conexion.NombreBase} en {Conexion.Servidor}.\n\n" +
                            "¿Querés crearla ahora? Se carga con canchas, horarios y clientes de ejemplo."))
                        return false;
                    Cursor.Current = Cursors.WaitCursor;
                    BaseDatos.Crear();
                    return true;

                default:
                    BaseDatos.Actualizar();
                    return true;
            }
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
            return false;
        }
        finally
        {
            Cursor.Current = Cursors.Default;
        }
    }
}
