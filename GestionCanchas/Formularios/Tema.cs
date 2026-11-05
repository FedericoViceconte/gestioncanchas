using System.Drawing.Text;
using GestionCanchas.Entidades;

namespace GestionCanchas.Formularios;

/// <summary>
/// Paleta, tipografía y estilos comunes de la aplicación. <see cref="Aplicar"/> recorre un formulario
/// y deja botones, grillas y campos con el mismo aspecto.
/// </summary>
public static class Tema
{
    public static readonly Color Primario = Color.FromArgb(21, 128, 61);
    public static readonly Color PrimarioOscuro = Color.FromArgb(22, 101, 52);
    public static readonly Color PrimarioSuave = Color.FromArgb(220, 252, 231);
    public static readonly Color Acento = Color.FromArgb(74, 222, 128);

    public static readonly Color Lateral = Color.FromArgb(14, 39, 27);
    public static readonly Color LateralHover = Color.FromArgb(23, 58, 41);
    public static readonly Color LateralActivo = Color.FromArgb(30, 78, 53);
    public static readonly Color LateralTexto = Color.FromArgb(209, 230, 218);
    public static readonly Color LateralTextoTenue = Color.FromArgb(124, 158, 140);

    public static readonly Color Fondo = Color.FromArgb(243, 245, 244);
    public static readonly Color Superficie = Color.White;
    public static readonly Color SuperficieAlterna = Color.FromArgb(249, 250, 249);
    public static readonly Color Borde = Color.FromArgb(222, 228, 224);
    public static readonly Color Texto = Color.FromArgb(28, 37, 32);
    public static readonly Color TextoSecundario = Color.FromArgb(100, 113, 106);

    public static readonly Color Peligro = Color.FromArgb(220, 38, 38);
    public static readonly Color PeligroSuave = Color.FromArgb(254, 226, 226);
    public static readonly Color Advertencia = Color.FromArgb(180, 83, 9);
    public static readonly Color AdvertenciaSuave = Color.FromArgb(254, 243, 199);
    public static readonly Color Informacion = Color.FromArgb(29, 78, 216);
    public static readonly Color Neutro = Color.FromArgb(82, 82, 91);

    public static readonly Font Texto9 = new Font("Segoe UI", 9F);
    public static readonly Font TextoNegrita9 = new Font("Segoe UI Semibold", 9F);
    public static readonly Font Texto10 = new Font("Segoe UI", 10F);
    public static readonly Font Subtitulo = new Font("Segoe UI Semibold", 11F);
    public static readonly Font Titulo = new Font("Segoe UI Semibold", 16F);
    public static readonly Font Cifra = new Font("Segoe UI Semibold", 20F);

    public static readonly string FuenteIconos = ElegirFuenteIconos();

    private static readonly string[] TextosPrimarios =
        { "Guardar", "Aceptar", "Registrar pago", "Reservar", "Buscar", "Agregar", "Generar", "Confirmar", "Ingresar", "Crear", "Consultar" };

    private static readonly string[] TextosPeligro =
        { "Eliminar", "Anular", "Cancelar reserva", "Cancelar turno", "Dar de baja", "Marcar ausente", "Ausente" };

    public static Font Iconos(float tamano) => new Font(FuenteIconos, tamano);

    public static void Aplicar(Control raiz)
    {
        if (raiz is Form formulario)
        {
            formulario.BackColor = Fondo;
            formulario.ForeColor = Texto;
        }

        foreach (Control control in raiz.Controls)
        {
            Estilizar(control, raiz as Form);
            if (control.HasChildren && control is not DataGridView && control is not NumericUpDown)
                Aplicar(control);
        }
    }

    private static void Estilizar(Control control, Form formulario)
    {
        switch (control)
        {
            case Button boton when boton.Tag as string != "sin-tema":
                EstiloBoton(boton, boton.Tag switch
                {
                    "primario" => EstiloDeBoton.Primario,
                    "secundario" => EstiloDeBoton.Secundario,
                    "peligro" => EstiloDeBoton.Peligro,
                    _ when EmpiezaCon(boton.Text, TextosPeligro) => EstiloDeBoton.Peligro,
                    _ when formulario?.AcceptButton == boton || EmpiezaCon(boton.Text, TextosPrimarios) => EstiloDeBoton.Primario,
                    _ => EstiloDeBoton.Secundario
                });
                break;
            case DataGridView grilla:
                EstiloGrilla(grilla);
                break;
            case GroupBox grupo:
                grupo.BackColor = Superficie;
                grupo.ForeColor = Texto;
                grupo.Font = TextoNegrita9;
                foreach (Control hijo in grupo.Controls)
                    hijo.Font = Texto9;
                break;
            case TextBox texto:
                texto.BorderStyle = BorderStyle.FixedSingle;
                break;
            case NumericUpDown numero:
                numero.BorderStyle = BorderStyle.FixedSingle;
                break;
            case Label etiqueta:
                etiqueta.ForeColor = TraducirColor(etiqueta.ForeColor);
                break;
            case CheckBox or RadioButton:
                control.ForeColor = Texto;
                break;
        }
    }

    public enum EstiloDeBoton { Primario, Secundario, Peligro, Fantasma }

    public static void EstiloBoton(Button boton, EstiloDeBoton estilo)
    {
        boton.FlatStyle = FlatStyle.Flat;
        boton.Cursor = Cursors.Hand;
        boton.UseVisualStyleBackColor = false;
        boton.Font = TextoNegrita9;
        boton.FlatAppearance.BorderSize = 1;

        switch (estilo)
        {
            case EstiloDeBoton.Primario:
                boton.BackColor = Primario;
                boton.ForeColor = Color.White;
                boton.FlatAppearance.BorderColor = Primario;
                boton.FlatAppearance.MouseOverBackColor = PrimarioOscuro;
                boton.FlatAppearance.MouseDownBackColor = PrimarioOscuro;
                break;
            case EstiloDeBoton.Peligro:
                boton.BackColor = Superficie;
                boton.ForeColor = Peligro;
                boton.FlatAppearance.BorderColor = Color.FromArgb(252, 165, 165);
                boton.FlatAppearance.MouseOverBackColor = PeligroSuave;
                boton.FlatAppearance.MouseDownBackColor = PeligroSuave;
                break;
            case EstiloDeBoton.Fantasma:
                boton.BackColor = Color.Transparent;
                boton.ForeColor = TextoSecundario;
                boton.FlatAppearance.BorderSize = 0;
                boton.FlatAppearance.MouseOverBackColor = Fondo;
                boton.FlatAppearance.MouseDownBackColor = Borde;
                break;
            default:
                boton.BackColor = Superficie;
                boton.ForeColor = Texto;
                boton.FlatAppearance.BorderColor = Borde;
                boton.FlatAppearance.MouseOverBackColor = SuperficieAlterna;
                boton.FlatAppearance.MouseDownBackColor = Borde;
                break;
        }
    }

    public static void EstiloGrilla(DataGridView grilla)
    {
        if (grilla.Tag as string == "con-tema")
            return;
        grilla.Tag = "con-tema";

        grilla.BorderStyle = BorderStyle.None;
        grilla.BackgroundColor = Superficie;
        grilla.GridColor = Borde;
        grilla.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grilla.EnableHeadersVisualStyles = false;
        grilla.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grilla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grilla.ColumnHeadersHeight = 36;
        grilla.RowTemplate.Height = 32;

        DataGridViewCellStyle encabezado = grilla.ColumnHeadersDefaultCellStyle;
        encabezado.BackColor = SuperficieAlterna;
        encabezado.ForeColor = TextoSecundario;
        encabezado.SelectionBackColor = SuperficieAlterna;
        encabezado.SelectionForeColor = TextoSecundario;
        encabezado.Font = TextoNegrita9;
        encabezado.Padding = new Padding(6, 0, 6, 0);
        encabezado.WrapMode = DataGridViewTriState.False;

        DataGridViewCellStyle celdas = grilla.DefaultCellStyle;
        celdas.BackColor = Superficie;
        celdas.ForeColor = Texto;
        celdas.SelectionBackColor = PrimarioSuave;
        celdas.SelectionForeColor = Texto;
        celdas.Font = Texto9;
        celdas.Padding = new Padding(6, 0, 6, 0);
        grilla.AlternatingRowsDefaultCellStyle.BackColor = SuperficieAlterna;

        grilla.CellFormatting += ColorearEstado;
    }

    /// <summary>Pinta la columna Estado de cualquier grilla con un color por estado.</summary>
    private static void ColorearEstado(object sender, DataGridViewCellFormattingEventArgs e)
    {
        var grilla = (DataGridView)sender;
        if (e.RowIndex < 0 || grilla.Columns[e.ColumnIndex].DataPropertyName != "Estado" || e.Value is not string estado)
            return;

        Color? color = estado switch
        {
            EstadosReserva.Pagado or EstadosCancha.Disponible => Primario,
            EstadosReserva.Senado => Advertencia,
            EstadosReserva.Pendiente => Informacion,
            EstadosReserva.Cancelada => Peligro,
            EstadosReserva.Ausente or EstadosCancha.Mantenimiento => Neutro,
            _ => null
        };
        if (color == null)
            return;

        e.CellStyle.ForeColor = color.Value;
        e.CellStyle.SelectionForeColor = color.Value;
        e.CellStyle.Font = TextoNegrita9;
    }

    private static Color TraducirColor(Color color)
    {
        if (color == Color.DarkGreen || color == Color.Green || color == Color.ForestGreen)
            return Primario;
        if (color == Color.Firebrick || color == Color.Red || color == Color.DarkRed)
            return Peligro;
        if (color == Color.DarkOrange || color == Color.Orange || color == Color.Chocolate)
            return Advertencia;
        if (color.IsSystemColor || color == Color.Black)
            return Texto;
        return color;
    }

    private static bool EmpiezaCon(string texto, string[] opciones) =>
        !string.IsNullOrEmpty(texto) &&
        opciones.Any(o => texto.Replace("&", string.Empty).StartsWith(o, StringComparison.OrdinalIgnoreCase));

    private static string ElegirFuenteIconos()
    {
        using var fuentes = new InstalledFontCollection();
        return fuentes.Families.Any(f => f.Name == "Segoe Fluent Icons") ? "Segoe Fluent Icons" : "Segoe MDL2 Assets";
    }
}

/// <summary>Códigos de Segoe Fluent Icons (también presentes en Segoe MDL2 Assets).</summary>
public static class Icono
{
    public const string Inicio = "";
    public const string Agregar = "";
    public const string Grilla = "";
    public const string Calendario = "";
    public const string Repetir = "";
    public const string Tarjeta = "";
    public const string Lista = "";
    public const string Reporte = "";
    public const string Personas = "";
    public const string Bandera = "";
    public const string Reloj = "";
    public const string Administrador = "";
    public const string Llave = "";
    public const string Salir = "";
    public const string Descargar = "";
    public const string Actualizar = "";
    public const string Base = "";
}
