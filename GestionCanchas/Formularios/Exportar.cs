using System.Globalization;
using System.Text;

namespace GestionCanchas.Formularios;

/// <summary>Exporta el contenido visible de una grilla a un CSV que Excel abre con acentos y columnas correctas.</summary>
public static class Exportar
{
    public static void Csv(DataGridView grilla, string nombreSugerido, IWin32Window dueno)
    {
        if (grilla.Rows.Count == 0)
        {
            Mensajes.Info("No hay datos para exportar.");
            return;
        }

        using var dialogo = new SaveFileDialog
        {
            Title = "Exportar a Excel (CSV)",
            Filter = "CSV para Excel (*.csv)|*.csv",
            FileName = $"{nombreSugerido}-{DateTime.Today:yyyy-MM-dd}.csv",
            OverwritePrompt = true
        };
        if (dialogo.ShowDialog(dueno) != DialogResult.OK)
            return;

        try
        {
            File.WriteAllText(dialogo.FileName, GenerarCsv(grilla), new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
            Mensajes.Info($"Se exportaron {grilla.Rows.Count} filas a {Path.GetFileName(dialogo.FileName)}.");
        }
        catch (IOException ex)
        {
            Mensajes.Error(new IOException("No se pudo guardar el archivo. Si está abierto en Excel, cerralo y probá de nuevo.", ex));
        }
    }

    /// <summary>Agrega un botón "Exportar a Excel" a la derecha de <paramref name="referencia"/>.</summary>
    public static Button AgregarBoton(Control referencia, DataGridView grilla, string nombreArchivo)
    {
        var boton = new Button
        {
            Text = "Exportar a Excel",
            Size = new Size(130, referencia is Button ? referencia.Height : 28),
            Location = new Point(referencia.Right + 12, referencia is Button ? referencia.Top : referencia.Top - 4),
            Anchor = referencia.Anchor,
            Tag = "secundario",
            TabIndex = referencia.TabIndex + 1
        };
        boton.Click += (sender, e) => Csv(grilla, nombreArchivo, boton.FindForm());
        referencia.Parent.Controls.Add(boton);
        return boton;
    }

    public static string GenerarCsv(DataGridView grilla)
    {
        List<DataGridViewColumn> columnas = grilla.Columns.Cast<DataGridViewColumn>()
            .Where(c => c.Visible)
            .OrderBy(c => c.DisplayIndex)
            .ToList();

        IEnumerable<IEnumerable<string>> filas = grilla.Rows.Cast<DataGridViewRow>()
            .Where(f => !f.IsNewRow)
            .Select(f => columnas.Select(c => f.Cells[c.Index].FormattedValue?.ToString() ?? string.Empty));

        return GenerarCsv(columnas.Select(c => c.HeaderText), filas, CultureInfo.CurrentCulture.TextInfo.ListSeparator);
    }

    public static string GenerarCsv(IEnumerable<string> encabezados, IEnumerable<IEnumerable<string>> filas, string separador)
    {
        var csv = new StringBuilder();
        csv.AppendLine(string.Join(separador, encabezados.Select(e => Escapar(e, separador))));
        foreach (IEnumerable<string> fila in filas)
            csv.AppendLine(string.Join(separador, fila.Select(v => Escapar(v, separador))));
        return csv.ToString();
    }

    /// <summary>
    /// Pone entre comillas los valores con separadores, comillas o saltos de línea, y neutraliza los que
    /// Excel interpretaría como fórmula (=, +, @ o un guion que no empieza un número).
    /// </summary>
    internal static string Escapar(string valor, string separador)
    {
        valor ??= string.Empty;
        bool pareceFormula = valor.Length > 0 &&
            (valor[0] is '=' or '+' or '@' or '\t' or '\r' ||
             (valor[0] == '-' && (valor.Length == 1 || !(char.IsDigit(valor[1]) || valor[1] is '$' or ' '))));
        if (pareceFormula)
            valor = "'" + valor;

        bool requiereComillas = valor.Contains(separador) || valor.Contains('"') || valor.Contains('\n') || valor.Contains('\r');
        return requiereComillas ? "\"" + valor.Replace("\"", "\"\"") + "\"" : valor;
    }
}
