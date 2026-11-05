using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

/// <summary>Alta, edición y restablecimiento de contraseña de los usuarios. Solo para administradores.</summary>
public class FrmUsuarios : FormularioBase, IRefrescable
{
    private const int AnchoCampos = 316;

    private readonly UsuarioNegocio _negocio = new UsuarioNegocio();
    private readonly DataGridView _grilla = new DataGridView { Dock = DockStyle.Fill };
    private readonly TextBox _txtUsuario = new TextBox();
    private readonly TextBox _txtNombre = new TextBox();
    private readonly ComboBox _cboRol = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly CheckBox _chkActivo = new CheckBox { Text = "Usuario activo", AutoSize = true };
    private readonly TextBox _txtClave = new TextBox { UseSystemPasswordChar = true };
    private readonly TextBox _txtRepetir = new TextBox { UseSystemPasswordChar = true };
    private readonly Label _lblClave = new Label();
    private readonly Label _lblRepetir = new Label();
    private readonly Label _lblModo = new Label();
    private readonly Button _btnNuevo = new Button { Text = "Nuevo usuario" };
    private readonly Button _btnGuardar = new Button { Text = "Guardar" };
    private readonly Button _btnRestablecer = new Button { Text = "Restablecer contraseña" };

    private Usuario _seleccionado;
    private int _yBotonesSinClave;
    private int _yBotonesConClave;

    public FrmUsuarios()
    {
        Text = "Usuarios";
        AutoScaleMode = AutoScaleMode.Font;
        AutoScaleDimensions = new SizeF(7F, 15F);
        ClientSize = new Size(1000, 600);

        var tabla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 364F));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var tarjetaLista = new Tarjeta { Titulo = "Cuentas", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 14, 0) };
        tarjetaLista.Controls.Add(_grilla);
        var tarjetaDatos = new Tarjeta { Titulo = "Datos del usuario", Dock = DockStyle.Fill, Margin = new Padding(0) };
        ConstruirFicha(tarjetaDatos);

        tabla.Controls.Add(tarjetaLista, 0, 0);
        tabla.Controls.Add(tarjetaDatos, 1, 0);
        Controls.Add(tabla);

        Grillas.MensajeVacio(_grilla, "Todavía no hay usuarios.");
        _grilla.SelectionChanged += (sender, e) => MostrarSeleccionado();
        _cboRol.Items.AddRange(Roles.Todos);
        _btnNuevo.Click += (sender, e) => Nuevo();
        _btnGuardar.Click += Guardar;
        _btnRestablecer.Click += Restablecer;
        Load += (sender, e) => Refrescar();
    }

    private void ConstruirFicha(Tarjeta tarjeta)
    {
        int x = 20;
        int y = 52;

        _lblModo.Location = new Point(x, y);
        _lblModo.Size = new Size(AnchoCampos, 20);
        _lblModo.ForeColor = Tema.TextoSecundario;
        tarjeta.Controls.Add(_lblModo);
        y += 30;

        Campo(tarjeta, "Usuario", _txtUsuario, x, ref y);
        _txtUsuario.MaxLength = 30;
        Campo(tarjeta, "Nombre y apellido", _txtNombre, x, ref y);
        _txtNombre.MaxLength = 80;
        Campo(tarjeta, "Rol", _cboRol, x, ref y);

        _chkActivo.Location = new Point(x, y);
        tarjeta.Controls.Add(_chkActivo);
        y += 36;

        _yBotonesSinClave = y + 8;
        _lblClave.Text = "Contraseña";
        _lblRepetir.Text = "Repetir contraseña";
        Campo(tarjeta, _lblClave, _txtClave, x, ref y);
        Campo(tarjeta, _lblRepetir, _txtRepetir, x, ref y);

        _yBotonesConClave = y + 8;
        _btnGuardar.Size = new Size(150, 34);
        _btnGuardar.Location = new Point(x + AnchoCampos - 150, _yBotonesConClave);
        _btnNuevo.Size = new Size(150, 34);
        _btnNuevo.Location = new Point(x, _yBotonesConClave);
        _btnRestablecer.Size = new Size(AnchoCampos, 34);
        _btnRestablecer.Left = x;
        tarjeta.Controls.AddRange(new Control[] { _btnGuardar, _btnNuevo, _btnRestablecer });
    }

    private static void Campo(Control contenedor, string etiqueta, Control campo, int x, ref int y) =>
        Campo(contenedor, new Label { Text = etiqueta }, campo, x, ref y);

    private static void Campo(Control contenedor, Label etiqueta, Control campo, int x, ref int y)
    {
        etiqueta.Font = Tema.TextoNegrita9;
        etiqueta.AutoSize = true;
        etiqueta.Location = new Point(x, y);
        campo.Location = new Point(x, y + 20);
        campo.Width = AnchoCampos;
        campo.Font = Tema.Texto10;
        contenedor.Controls.Add(etiqueta);
        contenedor.Controls.Add(campo);
        y += 20 + campo.Height + 14;
    }

    public void Refrescar() => Cargar(_seleccionado?.IdUsuario);

    private void Cargar(int? seleccionar)
    {
        try
        {
            _grilla.DataSource = _negocio.Listar();
            Grillas.Formato(_grilla, "dd/MM/yyyy", "FechaAlta");
            Grillas.Formato(_grilla, "dd/MM/yyyy HH:mm", "UltimoAcceso");
            if (seleccionar != null)
                Grillas.Seleccionar<Usuario>(_grilla, u => u.IdUsuario == seleccionar);
            MostrarSeleccionado();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void MostrarSeleccionado()
    {
        _seleccionado = Grillas.Seleccionado<Usuario>(_grilla);
        if (_seleccionado == null)
        {
            Nuevo();
            return;
        }

        _lblModo.Text = _seleccionado.IdUsuario == Sesion.IdUsuario ? "Editando tu propio usuario" : "Editando usuario existente";
        _txtUsuario.Text = _seleccionado.NombreUsuario;
        _txtUsuario.ReadOnly = true;
        _txtNombre.Text = _seleccionado.NombreCompleto;
        _cboRol.SelectedItem = _seleccionado.Rol;
        _chkActivo.Checked = _seleccionado.Activo;
        MostrarCamposClave(false);
    }

    private void Nuevo()
    {
        _seleccionado = null;
        _grilla.ClearSelection();
        _lblModo.Text = "Nuevo usuario";
        _txtUsuario.ReadOnly = false;
        _txtUsuario.Clear();
        _txtNombre.Clear();
        _cboRol.SelectedItem = Roles.Recepcionista;
        _chkActivo.Checked = true;
        _txtClave.Clear();
        _txtRepetir.Clear();
        MostrarCamposClave(true);
        _txtUsuario.Focus();
    }

    private void MostrarCamposClave(bool visibles)
    {
        _lblClave.Visible = _txtClave.Visible = _lblRepetir.Visible = _txtRepetir.Visible = visibles;
        _btnRestablecer.Visible = !visibles;

        int y = visibles ? _yBotonesConClave : _yBotonesSinClave;
        _btnNuevo.Top = _btnGuardar.Top = y;
        _btnRestablecer.Top = y + 46;
    }

    private void Guardar(object sender, EventArgs e)
    {
        try
        {
            var usuario = new Usuario
            {
                IdUsuario = _seleccionado?.IdUsuario ?? 0,
                NombreUsuario = _txtUsuario.Text,
                NombreCompleto = _txtNombre.Text,
                Rol = _cboRol.SelectedItem as string,
                Activo = _chkActivo.Checked
            };

            if (_seleccionado == null)
            {
                if (_txtClave.Text != _txtRepetir.Text)
                    throw new ValidacionException("Las contraseñas no coinciden.");
                _negocio.Crear(usuario, _txtClave.Text);
                Mensajes.Info($"Se creó el usuario \"{usuario.NombreUsuario}\".");
            }
            else
            {
                _negocio.Modificar(usuario);
                Mensajes.Info("Los cambios se guardaron.");
            }
            Cargar(usuario.IdUsuario);
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }

    private void Restablecer(object sender, EventArgs e)
    {
        if (_seleccionado == null)
            return;

        using var formulario = new FrmClave(FrmClave.Modo.Restablecer, _seleccionado);
        formulario.ShowDialog(this);
    }
}
