using GestionCanchas.Negocio;
using Microsoft.Data.SqlClient;

namespace GestionCanchas.Formularios;

public static class Mensajes
{
    private const string Titulo = "Gestión de Canchas";

    public static void Info(string texto) =>
        MessageBox.Show(texto, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);

    public static bool Confirmar(string texto) =>
        MessageBox.Show(texto, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2) == DialogResult.Yes;

    public static void Error(Exception ex)
    {
        if (ex is ValidacionException)
        {
            MessageBox.Show(ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (ex is SqlException errorSql)
        {
            MessageBox.Show(TraducirErrorSql(errorSql), "Error de base de datos",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        MessageBox.Show("Ocurrió un error inesperado:\n" + ex.Message, "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private static string TraducirErrorSql(SqlException ex)
    {
        switch (ex.Number)
        {
            case 2601:
            case 2627:
                return "Ya existe un registro con esos datos.";
            case 547:
                return "La operación no se puede realizar porque el registro está relacionado con otros datos.";
            case -2:
                return "La base de datos tardó demasiado en responder. Intentá de nuevo.";
            case -1:
            case 2:
            case 53:
            case 4060:
                return "No se pudo conectar a la base de datos.\n\n" +
                       "Verificá que SQL Server LocalDB esté instalado y que se haya ejecutado el script GestionCanchas.sql.";
            default:
                return "Error de base de datos: " + ex.Message;
        }
    }
}
