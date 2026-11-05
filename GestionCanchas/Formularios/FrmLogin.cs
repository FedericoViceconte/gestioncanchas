using GestionCanchas.Datos;
using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

/// <summary>
/// Inicio de sesión. Si todavía no hay usuarios, pide crear la cuenta de administrador.
/// </summary>
public class FrmLogin : FormularioBase
{
    private const int AnchoMarca = 380;
    private const int MargenCampos = 56;
    private const int AnchoCampos = 368;

    private readonly UsuarioNegocio _negocio = new UsuarioNegocio();
    private readonly bool _primerUso;

    private readonly Panel _panelMarca = new Panel();
    private readonly Label _lblTitulo = new Label();
    private readonly Label _lblSubtitulo = new Label();
    private readonly Label _lblError = new Label();
    private readonly Label _lblBase = new Label();
    private readonly Button _btnIngresar = new Button();
    private readonly CheckBox _chkMostrarClave = new CheckBox();
    private TextBox _txtNombreCompleto;
    private TextBox _txtUsuario;
    private TextBox _txtClave;
    private TextBox _txtRepetir;

    public FrmLogin()
    {
        _primerUso = !_negocio.HayUsuarios();

        Text = _primerUso ? "Gestión de Canchas · Primer inicio" : "Gestión de Canchas · Iniciar sesión";
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(AnchoMarca + MargenCampos * 2 + AnchoCampos, _primerUso ? 624 : 540);
        AutoScaleMode = AutoScaleMode.Font;
        AutoScaleDimensions = new SizeF(7F, 15F);

        ConstruirMarca();
        ConstruirFormulario();
    }

    private void ConstruirMarca()
    {
        _panelMarca.Dock = DockStyle.Left;
        _panelMarca.Width = AnchoMarca;
        _panelMarca.BackColor = Tema.Lateral;
        _panelMarca.Paint += PintarMarca;
        Controls.Add(_panelMarca);
    }

    private void ConstruirFormulario()
    {
        int x = AnchoMarca + MargenCampos;
        int y = 56;

        _lblTitulo.Text = _primerUso ? "Creá la cuenta de administrador" : "Iniciar sesión";
        _lblTitulo.Font = Tema.Titulo;
        _lblTitulo.ForeColor = Tema.Texto;
        _lblTitulo.AutoSize = true;
        _lblTitulo.Location = new Point(x - 2, y);
        Controls.Add(_lblTitulo);
        y += 40;

        _lblSubtitulo.Text = _primerUso
            ? "Es el primer inicio de la aplicación. Esta cuenta va a poder crear los usuarios de recepción."
            : "Ingresá con tu usuario para continuar.";
        _lblSubtitulo.Font = Tema.Texto9;
        _lblSubtitulo.ForeColor = Tema.TextoSecundario;
        _lblSubtitulo.Location = new Point(x, y);
        _lblSubtitulo.Size = new Size(AnchoCampos, 40);
        Controls.Add(_lblSubtitulo);
        y += _primerUso ? 56 : 44;

        if (_primerUso)
        {
            _txtNombreCompleto = AgregarCampo("Nombre y apellido", x, ref y);
            _txtNombreCompleto.MaxLength = 80;
        }

        _txtUsuario = AgregarCampo("Usuario", x, ref y);
        _txtUsuario.MaxLength = 30;
        _txtClave = AgregarCampo("Contraseña", x, ref y, clave: true);

        if (_primerUso)
        {
            _txtRepetir = AgregarCampo("Repetir contraseña", x, ref y, clave: true);
            var pista = new Label
            {
                Text = $"Mínimo {UsuarioNegocio.LongitudMinimaClave} caracteres, con letras y números.",
                Font = Tema.Texto9,
                ForeColor = Tema.TextoSecundario,
                Location = new Point(x, y - 10),
                Size = new Size(AnchoCampos, 18)
            };
            Controls.Add(pista);
            y += 22;
        }

        _chkMostrarClave.Text = "Mostrar contraseña";
        _chkMostrarClave.AutoSize = true;
        _chkMostrarClave.Location = new Point(x, y - 10);
        _chkMostrarClave.TabStop = false;
        _chkMostrarClave.CheckedChanged += (sender, e) =>
        {
            _txtClave.UseSystemPasswordChar = !_chkMostrarClave.Checked;
            if (_txtRepetir != null)
                _txtRepetir.UseSystemPasswordChar = !_chkMostrarClave.Checked;
        };
        Controls.Add(_chkMostrarClave);
        y += 16;

        _lblError.Font = Tema.Texto9;
        _lblError.ForeColor = Tema.Peligro;
        _lblError.Location = new Point(x, y - 4);
        _lblError.Size = new Size(AnchoCampos, 36);
        Controls.Add(_lblError);
        y += 34;

        _btnIngresar.Text = _primerUso ? "Crear cuenta y entrar" : "Ingresar";
        _btnIngresar.Location = new Point(x, y);
        _btnIngresar.Size = new Size(AnchoCampos, 42);
        _btnIngresar.Click += Ingresar;
        Controls.Add(_btnIngresar);
        AcceptButton = _btnIngresar;

        _lblBase.Text = $"Base de datos: {Conexion.NombreBase} · {Conexion.Servidor}";
        _lblBase.Font = Tema.Texto9;
        _lblBase.ForeColor = Tema.TextoSecundario;
        _lblBase.AutoEllipsis = true;
        _lblBase.Location = new Point(x, ClientSize.Height - 40);
        _lblBase.Size = new Size(AnchoCampos, 20);
        Controls.Add(_lblBase);
    }

    private TextBox AgregarCampo(string etiqueta, int x, ref int y, bool clave = false)
    {
        var lbl = new Label
        {
            Text = etiqueta,
            Font = Tema.TextoNegrita9,
            ForeColor = Tema.Texto,
            AutoSize = true,
            Location = new Point(x, y)
        };
        var txt = new TextBox
        {
            Font = new Font("Segoe UI", 11F),
            Location = new Point(x, y + 22),
            Width = AnchoCampos,
            UseSystemPasswordChar = clave
        };
        txt.TextChanged += (sender, e) => _lblError.Text = string.Empty;
        Controls.Add(lbl);
        Controls.Add(txt);
        y += 22 + txt.Height + 18;
        return txt;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        BackColor = Tema.Superficie;
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        (_primerUso ? _txtNombreCompleto : _txtUsuario).Focus();
    }

    private void Ingresar(object sender, EventArgs e)
    {
        _lblError.Text = string.Empty;
        Cursor = Cursors.WaitCursor;
        _btnIngresar.Enabled = false;
        try
        {
            if (_primerUso)
            {
                if (_txtClave.Text != _txtRepetir.Text)
                    throw new ValidacionException("Las contraseñas no coinciden.");

                _negocio.CrearPrimerAdministrador(new Usuario
                {
                    NombreUsuario = _txtUsuario.Text,
                    NombreCompleto = _txtNombreCompleto.Text
                }, _txtClave.Text);
            }

            _negocio.IniciarSesion(_txtUsuario.Text, _txtClave.Text);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (ValidacionException ex)
        {
            _lblError.Text = ex.Message;
            _txtClave.SelectAll();
            _txtClave.Focus();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
        finally
        {
            Cursor = Cursors.Default;
            _btnIngresar.Enabled = true;
        }
    }

    private void PintarMarca(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        Dibujo.Preparar(g);

        using (var lineas = new Pen(Color.FromArgb(22, 255, 255, 255), 2))
        {
            int alto = _panelMarca.Height;
            g.DrawEllipse(lineas, AnchoMarca - 150, alto - 190, 300, 300);
            g.DrawLine(lineas, AnchoMarca - 150, alto - 40, AnchoMarca + 10, alto - 40);
            g.DrawRectangle(lineas, AnchoMarca - 90, alto - 120, 120, 160);
        }

        Dibujo.Logo(g, new RectangleF(44, 56, 56, 56));
        Dibujo.Texto(g, "Gestión de Canchas", new Font("Segoe UI Semibold", 20F), Color.White,
            new Rectangle(42, 132, AnchoMarca - 70, 40));
        TextRenderer.DrawText(g, "Reservas, turnos fijos y cobros de tu complejo de fútbol en un solo lugar.",
            Tema.Texto10, new Rectangle(44, 178, AnchoMarca - 90, 48), Tema.LateralTexto, TextFormatFlags.WordBreak);

        string[] funciones =
        {
            "Disponibilidad y reservas con seña",
            "Turnos fijos semanales",
            "Pagos, devoluciones y caja diaria",
            "Usuarios con roles y contraseñas seguras"
        };
        using Font fuenteIcono = Tema.Iconos(10F);
        for (int i = 0; i < funciones.Length; i++)
        {
            int y = 262 + i * 32;
            Dibujo.Texto(g, "", fuenteIcono, Tema.Acento, new Rectangle(44, y, 20, 22));
            Dibujo.Texto(g, funciones[i], Tema.Texto9, Tema.LateralTexto, new Rectangle(72, y, AnchoMarca - 100, 22));
        }
    }
}
