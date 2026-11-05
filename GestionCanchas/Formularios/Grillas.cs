using System.Runtime.CompilerServices;

namespace GestionCanchas.Formularios;

public static class Grillas
{
    private const string MensajeVacioPredeterminado = "No hay datos para mostrar.";
    private static readonly ConditionalWeakTable<DataGridView, string> MensajesVacio = new();

    public static void Configurar(DataGridView grilla)
    {
        grilla.ReadOnly = true;
        grilla.AllowUserToAddRows = false;
        grilla.AllowUserToDeleteRows = false;
        grilla.AllowUserToResizeRows = false;
        grilla.MultiSelect = false;
        grilla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grilla.RowHeadersVisible = false;
        grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        Tema.EstiloGrilla(grilla);

        if (!MensajesVacio.TryGetValue(grilla, out _))
        {
            MensajesVacio.AddOrUpdate(grilla, MensajeVacioPredeterminado);
            grilla.Paint += PintarVacio;
            grilla.DataBindingComplete += AjustarColumnas;
        }
    }

    /// <summary>Reparte el ancho entre las columnas según su contenido, en vez de en partes iguales.</summary>
    private static void AjustarColumnas(object sender, DataGridViewBindingCompleteEventArgs e)
    {
        var grilla = (DataGridView)sender;
        if (grilla.AutoSizeColumnsMode != DataGridViewAutoSizeColumnsMode.Fill)
            return;

        foreach (DataGridViewColumn columna in grilla.Columns)
        {
            int preferido = columna.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
            columna.FillWeight = Math.Clamp(preferido, 40, 600);
            columna.MinimumWidth = Math.Min(preferido, 60);
        }
    }

    /// <summary>Texto que se muestra en el centro de la grilla cuando no tiene filas.</summary>
    public static void MensajeVacio(DataGridView grilla, string mensaje)
    {
        Configurar(grilla);
        MensajesVacio.AddOrUpdate(grilla, mensaje);
        grilla.Invalidate();
    }

    private static void PintarVacio(object sender, PaintEventArgs e)
    {
        var grilla = (DataGridView)sender;
        if (grilla.Rows.Count > 0 || !MensajesVacio.TryGetValue(grilla, out string mensaje))
            return;

        int arriba = grilla.ColumnHeadersVisible ? grilla.ColumnHeadersHeight : 0;
        var area = new Rectangle(0, arriba, grilla.ClientSize.Width, Math.Max(grilla.ClientSize.Height - arriba, 40));
        TextRenderer.DrawText(e.Graphics, mensaje, Tema.Texto9, area, Tema.TextoSecundario,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
    }

    public static void Formato(DataGridView grilla, string formato, params string[] columnas)
    {
        foreach (string nombre in columnas)
        {
            if (!grilla.Columns.Contains(nombre))
                continue;

            DataGridViewColumn columna = grilla.Columns[nombre];
            columna.DefaultCellStyle.Format = formato;
            if (formato == "C2")
                columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
    }

    public static void Ocultar(DataGridView grilla, params string[] columnas)
    {
        foreach (string nombre in columnas)
        {
            if (grilla.Columns.Contains(nombre))
                grilla.Columns[nombre].Visible = false;
        }
    }
    public static T Seleccionado<T>(DataGridView grilla) where T : class =>
        grilla.CurrentRow?.DataBoundItem as T;

    public static void Seleccionar<T>(DataGridView grilla, Func<T, bool> condicion) where T : class
    {
        DataGridViewColumn primeraVisible = grilla.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
        if (primeraVisible == null)
            return;

        foreach (DataGridViewRow fila in grilla.Rows)
        {
            if (fila.DataBoundItem is not T item || !condicion(item))
                continue;

            grilla.CurrentCell = fila.Cells[primeraVisible.Index];
            return;
        }
    }
}
