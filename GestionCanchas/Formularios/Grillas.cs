namespace GestionCanchas.Formularios;

public static class Grillas
{
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
        grilla.BackgroundColor = SystemColors.Window;
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
