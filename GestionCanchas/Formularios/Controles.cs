using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using GestionCanchas.Entidades;

namespace GestionCanchas.Formularios;

public static class Dibujo
{
    public static GraphicsPath Redondeado(RectangleF r, float radio)
    {
        float d = radio * 2;
        var camino = new GraphicsPath();
        camino.AddArc(r.X, r.Y, d, d, 180, 90);
        camino.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        camino.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        camino.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        camino.CloseFigure();
        return camino;
    }

    public static void Preparar(Graphics g)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
    }

    public static void Texto(Graphics g, string texto, Font fuente, Color color, Rectangle area,
        TextFormatFlags alineacion = TextFormatFlags.Left | TextFormatFlags.VerticalCenter) =>
        TextRenderer.DrawText(g, texto, fuente, area, color,
            alineacion | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);

    /// <summary>Logo de la aplicación: una cancha vista desde arriba sobre un cuadrado verde.</summary>
    public static void Logo(Graphics g, RectangleF r)
    {
        Preparar(g);
        using (GraphicsPath fondo = Redondeado(r, r.Width * 0.22f))
        using (var relleno = new SolidBrush(Tema.Primario))
            g.FillPath(relleno, fondo);

        float margen = r.Width * 0.19f;
        var cancha = new RectangleF(r.X + margen, r.Y + margen * 1.25f, r.Width - margen * 2, r.Height - margen * 2.5f);
        using var trazo = new Pen(Color.White, Math.Max(1.25f, r.Width * 0.06f)) { LineJoin = LineJoin.Round };
        using (GraphicsPath borde = Redondeado(cancha, r.Width * 0.035f))
            g.DrawPath(trazo, borde);

        float centroX = cancha.X + cancha.Width / 2;
        float centroY = cancha.Y + cancha.Height / 2;
        g.DrawLine(trazo, centroX, cancha.Top, centroX, cancha.Bottom);
        float radio = cancha.Height * 0.24f;
        g.DrawEllipse(trazo, centroX - radio, centroY - radio, radio * 2, radio * 2);

        if (r.Width < 28)
            return;
        float altoArea = cancha.Height * 0.44f;
        float anchoArea = cancha.Width * 0.12f;
        g.DrawRectangle(trazo, cancha.X, centroY - altoArea / 2, anchoArea, altoArea);
        g.DrawRectangle(trazo, cancha.Right - anchoArea, centroY - altoArea / 2, anchoArea, altoArea);
    }

    /// <summary>Importe abreviado para espacios chicos: $ 45 mil, $ 1,2 M.</summary>
    public static string ImporteCorto(decimal importe)
    {
        CultureInfo cultura = CultureInfo.CurrentCulture;
        if (Math.Abs(importe) >= 1_000_000)
            return "$ " + (importe / 1_000_000).ToString("0.#", cultura) + " M";
        if (Math.Abs(importe) >= 1_000)
            return "$ " + (importe / 1_000).ToString("0.#", cultura) + " mil";
        return importe.ToString("C0", cultura);
    }
}

/// <summary>Contenedor blanco con bordes redondeados y un título opcional.</summary>
public class Tarjeta : Panel
{
    private string _titulo = string.Empty;

    public Tarjeta()
    {
        DoubleBuffered = true;
        SetStyle(ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
        BackColor = Color.Transparent;
        Padding = new Padding(16);
    }

    [DefaultValue("")]
    public string Titulo
    {
        get => _titulo;
        set
        {
            _titulo = value ?? string.Empty;
            Padding = new Padding(16, string.IsNullOrEmpty(_titulo) ? 16 : 48, 16, 16);
            Invalidate();
        }
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        base.OnPaintBackground(e);
        Dibujo.Preparar(e.Graphics);
        using GraphicsPath camino = Dibujo.Redondeado(new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f), 10);
        using var relleno = new SolidBrush(Tema.Superficie);
        using var borde = new Pen(Tema.Borde);
        e.Graphics.FillPath(relleno, camino);
        e.Graphics.DrawPath(borde, camino);

        if (!string.IsNullOrEmpty(_titulo))
            Dibujo.Texto(e.Graphics, _titulo, Tema.Subtitulo, Tema.Texto, new Rectangle(16, 12, Width - 32, 26));
    }
}

/// <summary>Tarjeta con un indicador: ícono, título, cifra principal y un detalle.</summary>
public class TarjetaIndicador : Control
{
    private string _titulo = string.Empty;
    private string _valor = "—";
    private string _detalle = string.Empty;
    private string _icono = GestionCanchas.Formularios.Icono.Inicio;
    private Color _acento = Tema.Primario;

    public TarjetaIndicador()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
        BackColor = Color.Transparent;
        Size = new Size(240, 112);
        MinimumSize = new Size(180, 104);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Titulo { get => _titulo; set { _titulo = value; Invalidate(); } }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Valor { get => _valor; set { _valor = value; Invalidate(); } }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Detalle { get => _detalle; set { _detalle = value; Invalidate(); } }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Icono { get => _icono; set { _icono = value; Invalidate(); } }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color Acento { get => _acento; set { _acento = value; Invalidate(); } }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        Dibujo.Preparar(g);

        using (GraphicsPath camino = Dibujo.Redondeado(new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f), 10))
        using (var relleno = new SolidBrush(Tema.Superficie))
        using (var borde = new Pen(Tema.Borde))
        {
            g.FillPath(relleno, camino);
            g.DrawPath(borde, camino);
        }

        var circulo = new Rectangle(Width - 56, 16, 40, 40);
        using (var fondoIcono = new SolidBrush(Color.FromArgb(28, _acento)))
            g.FillEllipse(fondoIcono, circulo);
        using (Font fuenteIcono = Tema.Iconos(15F))
            Dibujo.Texto(g, _icono, fuenteIcono, _acento, circulo,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

        Dibujo.Texto(g, _titulo, Tema.Texto9, Tema.TextoSecundario, new Rectangle(18, 16, Width - 84, 20));
        Dibujo.Texto(g, _valor, Tema.Cifra, Tema.Texto, new Rectangle(16, 38, Width - 32, 38));
        Dibujo.Texto(g, _detalle, Tema.Texto9, Tema.TextoSecundario, new Rectangle(18, Height - 32, Width - 36, 20));
    }
}

/// <summary>Gráfico de barras simple de ingresos netos por día.</summary>
public class GraficoBarras : Control
{
    private IReadOnlyList<IngresoDiario> _datos = Array.Empty<IngresoDiario>();

    public GraficoBarras()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        BackColor = Tema.Superficie;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IReadOnlyList<IngresoDiario> Datos
    {
        get => _datos;
        set { _datos = value ?? Array.Empty<IngresoDiario>(); Invalidate(); }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        Dibujo.Preparar(g);
        g.Clear(BackColor);
        if (_datos.Count == 0)
            return;

        const int altoEtiqueta = 22;
        const int altoValor = 20;
        var area = new Rectangle(0, altoValor, Width, Height - altoEtiqueta - altoValor);
        decimal maximo = Math.Max(_datos.Max(d => d.Neto), 1);
        float ancho = (float)Width / _datos.Count;
        float anchoBarra = Math.Min(ancho * 0.56f, 44);

        using var lineaBase = new Pen(Tema.Borde);
        g.DrawLine(lineaBase, 0, area.Bottom, Width, area.Bottom);

        for (int i = 0; i < _datos.Count; i++)
        {
            IngresoDiario dia = _datos[i];
            float alto = (float)(Math.Max(dia.Neto, 0) / maximo) * (area.Height - 4);
            float x = i * ancho + (ancho - anchoBarra) / 2;
            bool hoy = dia.Dia.Date == DateTime.Today;

            if (alto > 0)
            {
                var barra = new RectangleF(x, area.Bottom - alto, anchoBarra, alto);
                using GraphicsPath camino = Dibujo.Redondeado(barra, Math.Min(6, Math.Min(anchoBarra, alto) / 2));
                using var relleno = new SolidBrush(hoy ? Tema.Primario : Color.FromArgb(134, 199, 157));
                g.FillPath(relleno, camino);
            }

            var columna = new Rectangle((int)(i * ancho), 0, (int)ancho, Height);
            if (dia.Neto > 0)
                Dibujo.Texto(g, Dibujo.ImporteCorto(dia.Neto), Tema.Texto9, Tema.TextoSecundario,
                    new Rectangle(columna.X, (int)(area.Bottom - alto) - altoValor, columna.Width, altoValor),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.Bottom);

            string etiqueta = hoy ? "Hoy" : CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedDayName(dia.Dia.DayOfWeek)
                .TrimEnd('.');
            Dibujo.Texto(g, etiqueta, hoy ? Tema.TextoNegrita9 : Tema.Texto9, hoy ? Tema.Texto : Tema.TextoSecundario,
                new Rectangle(columna.X, area.Bottom + 4, columna.Width, altoEtiqueta - 4),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.Top);
        }
    }
}

/// <summary>Opción del menú lateral, con ícono, texto y estado activo.</summary>
public class BotonNavegacion : Control
{
    private bool _activo;
    private bool _encima;
    private string _icono = GestionCanchas.Formularios.Icono.Inicio;

    public BotonNavegacion()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        Cursor = Cursors.Hand;
        Height = 40;
        Dock = DockStyle.Top;
        BackColor = Tema.Lateral;
        TabStop = true;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Icono { get => _icono; set { _icono = value; Invalidate(); } }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool Activo { get => _activo; set { _activo = value; Invalidate(); } }

    protected override void OnMouseEnter(EventArgs e) { _encima = true; Invalidate(); base.OnMouseEnter(e); }

    protected override void OnMouseLeave(EventArgs e) { _encima = false; Invalidate(); base.OnMouseLeave(e); }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode is Keys.Enter or Keys.Space)
            OnClick(EventArgs.Empty);
        base.OnKeyDown(e);
    }

    protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }

    protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        Dibujo.Preparar(g);
        g.Clear(Tema.Lateral);

        var fondo = new RectangleF(10, 2, Width - 20, Height - 4);
        if (_activo || _encima || Focused)
        {
            using GraphicsPath camino = Dibujo.Redondeado(fondo, 8);
            using var relleno = new SolidBrush(_activo ? Tema.LateralActivo : Tema.LateralHover);
            g.FillPath(relleno, camino);
        }
        if (_activo)
        {
            using var acento = new SolidBrush(Tema.Acento);
            g.FillRectangle(acento, 10, 12, 3, Height - 24);
        }

        Color color = _activo ? Color.White : Tema.LateralTexto;
        using (Font fuenteIcono = Tema.Iconos(12F))
            Dibujo.Texto(g, _icono, fuenteIcono, _activo ? Tema.Acento : Tema.LateralTextoTenue,
                new Rectangle(24, 0, 24, Height), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        Dibujo.Texto(g, Text, _activo ? Tema.TextoNegrita9 : Tema.Texto9, color, new Rectangle(58, 0, Width - 68, Height));
    }

    protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }
}

/// <summary>Título de sección del menú lateral.</summary>
public class SeccionNavegacion : Label
{
    public SeccionNavegacion(string texto)
    {
        Text = texto.ToUpperInvariant();
        Dock = DockStyle.Top;
        Height = 34;
        Padding = new Padding(24, 12, 0, 0);
        Font = new Font("Segoe UI Semibold", 7.5F);
        ForeColor = Tema.LateralTextoTenue;
        BackColor = Tema.Lateral;
    }
}
