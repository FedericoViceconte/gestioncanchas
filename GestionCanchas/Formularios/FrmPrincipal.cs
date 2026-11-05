using GestionCanchas.Datos;
using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

public partial class FrmPrincipal : FormularioBase
{
    private Form _pantalla;
    private BotonNavegacion _botonActivo;
    private BotonNavegacion _botonInicio;

    public FrmPrincipal()
    {
        InitializeComponent();
        ConstruirMenu();
    }

    /// <summary>Indica si la ventana se cerró para volver al inicio de sesión.</summary>
    public bool CerroSesion { get; private set; }

    public static void ActualizarPantallas()
    {
        FrmPrincipal principal = Application.OpenForms.OfType<FrmPrincipal>().FirstOrDefault();
        (principal?._pantalla as IRefrescable)?.Refrescar();
    }

    private void FrmPrincipal_Load(object sender, EventArgs e)
    {
        Tema.EstiloBoton(btnNuevaReserva, Tema.EstiloDeBoton.Primario);
        lblConexion.Text = $"●  {Conexion.NombreBase} · {Conexion.Servidor}";
        lblConexion.ForeColor = Tema.TextoSecundario;
        Text = $"Gestión de Canchas · {Sesion.Actual?.NombreCompleto}";
        Navegar(_botonInicio);
    }

    private void ConstruirMenu()
    {
        bool administrador = Sesion.EsAdministrador;
        _botonInicio = Opcion("Inicio", Icono.Inicio, "Inicio", FechaLarga(), () => new FrmResumen());

        var opciones = new List<Control>
        {
            _botonInicio,
            new SeccionNavegacion("Reservas"),
            Opcion("Disponibilidad", Icono.Grilla, "Disponibilidad",
                "Grilla de canchas por horario. Hacé doble clic en un turno libre para reservarlo.", () => new FrmDisponibilidad()),
            Opcion("Reservas", Icono.Calendario, "Reservas",
                "Buscá, modificá, cancelá o cobrá reservas.", () => new FrmConsultaReservas()),
            Opcion("Turnos fijos", Icono.Repetir, "Turnos fijos",
                "Reservas semanales para clientes habituales.", () => new FrmTurnosFijos()),
            new SeccionNavegacion("Cobros"),
            Opcion("Registrar pago", Icono.Tarjeta, "Registrar pago",
                "Cobrá señas y saldos de reservas.", () => new FrmPagos()),
            Opcion("Pagos", Icono.Lista, "Pagos",
                "Historial de pagos y devoluciones, con quién registró cada uno.", () => new FrmConsultaPagos()),
            Opcion("Caja", Icono.Reporte, "Caja",
                "Cobros, devoluciones y neto por período, método de pago y usuario.", () => new FrmCaja()),
            new SeccionNavegacion(administrador ? "Administración" : "Clientes"),
            Opcion("Clientes", Icono.Personas, "Clientes",
                "Alta, edición, búsqueda y deuda de cada cliente.", () => new FrmClientes())
        };

        if (administrador)
        {
            opciones.Add(Opcion("Canchas", Icono.Bandera, "Canchas",
                "Número, tipo, precio por hora y estado de cada cancha.", () => new FrmCanchas()));
            opciones.Add(Opcion("Horarios", Icono.Reloj, "Horarios",
                "Franjas horarias en las que se puede reservar.", () => new FrmHorarios()));
            opciones.Add(Opcion("Usuarios", Icono.Administrador, "Usuarios",
                "Cuentas de acceso, roles y contraseñas.", () => new FrmUsuarios()));
        }

        panelNavegacion.SuspendLayout();
        for (int i = opciones.Count - 1; i >= 0; i--)
            panelNavegacion.Controls.Add(opciones[i]);
        panelNavegacion.ResumeLayout();
    }

    private BotonNavegacion Opcion(string texto, string icono, string titulo, string subtitulo, Func<Form> crear)
    {
        var boton = new BotonNavegacion { Text = texto, Icono = icono, Tag = new Destino(titulo, subtitulo, crear) };
        boton.Click += (sender, e) => Navegar(boton);
        return boton;
    }

    private void Navegar(BotonNavegacion boton)
    {
        if (boton.Tag is not Destino destino)
            return;

        Form pantalla;
        try
        {
            Cursor = Cursors.WaitCursor;
            pantalla = destino.Crear();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
            return;
        }
        finally
        {
            Cursor = Cursors.Default;
        }

        pantalla.TopLevel = false;
        pantalla.FormBorderStyle = FormBorderStyle.None;
        pantalla.Dock = DockStyle.Fill;

        panelContenido.SuspendLayout();
        Form anterior = _pantalla;
        _pantalla = pantalla;
        panelContenido.Controls.Add(pantalla);
        pantalla.Show();
        if (anterior != null)
        {
            panelContenido.Controls.Remove(anterior);
            anterior.Dispose();
        }
        panelContenido.ResumeLayout();

        if (_botonActivo != null)
            _botonActivo.Activo = false;
        _botonActivo = boton;
        boton.Activo = true;
        lblTitulo.Text = destino.Titulo;
        lblSubtitulo.Text = destino.Subtitulo;
    }

    private void NuevaReserva()
    {
        using var formulario = new FrmReserva();
        if (formulario.ShowDialog(this) == DialogResult.OK)
            ActualizarPantallas();
    }

    private void btnNuevaReserva_Click(object sender, EventArgs e) => NuevaReserva();

    private void btnCambiarClave_Click(object sender, EventArgs e)
    {
        using var formulario = new FrmClave(FrmClave.Modo.CambiarPropia);
        formulario.ShowDialog(this);
    }

    private void btnCerrarSesion_Click(object sender, EventArgs e)
    {
        if (!Mensajes.Confirmar("¿Cerrar la sesión?"))
            return;

        Sesion.Cerrar();
        CerroSesion = true;
        Close();
    }

    private void FrmPrincipal_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.N)
        {
            e.SuppressKeyPress = true;
            NuevaReserva();
        }
        else if (e.KeyCode == Keys.F5)
        {
            e.SuppressKeyPress = true;
            (_pantalla as IRefrescable)?.Refrescar();
        }
    }

    private void panelMarca_Paint(object sender, PaintEventArgs e)
    {
        Dibujo.Logo(e.Graphics, new RectangleF(22, 22, 40, 40));
        Dibujo.Texto(e.Graphics, "Gestión de Canchas", new Font("Segoe UI Semibold", 11.5F), Color.White,
            new Rectangle(74, 22, panelMarca.Width - 80, 22));
        Dibujo.Texto(e.Graphics, "Complejo de fútbol", Tema.Texto9, Tema.LateralTextoTenue,
            new Rectangle(74, 44, panelMarca.Width - 80, 18));
    }

    private void panelUsuario_Paint(object sender, PaintEventArgs e)
    {
        Usuario usuario = Sesion.Actual;
        if (usuario == null)
            return;

        Graphics g = e.Graphics;
        Dibujo.Preparar(g);
        using (var separador = new Pen(Tema.LateralHover))
            g.DrawLine(separador, 18, 0, panelUsuario.Width - 18, 0);

        var avatar = new Rectangle(20, 18, 38, 38);
        using (var fondo = new SolidBrush(Tema.LateralActivo))
            g.FillEllipse(fondo, avatar);
        Dibujo.Texto(g, Iniciales(usuario.NombreCompleto), Tema.TextoNegrita9, Tema.Acento, avatar,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        Dibujo.Texto(g, usuario.NombreCompleto, Tema.TextoNegrita9, Color.White,
            new Rectangle(68, 18, panelUsuario.Width - 80, 20));
        Dibujo.Texto(g, $"{usuario.Rol} · @{usuario.NombreUsuario}", Tema.Texto9, Tema.LateralTextoTenue,
            new Rectangle(68, 38, panelUsuario.Width - 80, 18));
    }

    private void panelEncabezado_Paint(object sender, PaintEventArgs e)
    {
        using var borde = new Pen(Tema.Borde);
        e.Graphics.DrawLine(borde, 0, panelEncabezado.Height - 1, panelEncabezado.Width, panelEncabezado.Height - 1);
    }

    private static string Iniciales(string nombre)
    {
        string[] partes = (nombre ?? string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return partes.Length switch
        {
            0 => "?",
            1 => partes[0][..1].ToUpperInvariant(),
            _ => (partes[0][..1] + partes[^1][..1]).ToUpperInvariant()
        };
    }

    private static string FechaLarga()
    {
        string fecha = DateTime.Today.ToString("dddd d 'de' MMMM 'de' yyyy");
        return char.ToUpper(fecha[0]) + fecha[1..];
    }

    private sealed record Destino(string Titulo, string Subtitulo, Func<Form> Crear);
}
