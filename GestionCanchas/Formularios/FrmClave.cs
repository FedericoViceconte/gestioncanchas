using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Formularios;

/// <summary>Cambio de la contraseña propia, o restablecimiento de la de otro usuario por un administrador.</summary>
public class FrmClave : FormularioBase
{
    public enum Modo { CambiarPropia, Restablecer }

    private const int Ancho = 340;

    private readonly UsuarioNegocio _negocio = new UsuarioNegocio();
    private readonly Modo _modo;
    private readonly Usuario _usuario;
    private readonly TextBox _txtActual;
    private readonly TextBox _txtNueva;
    private readonly TextBox _txtRepetir;

    public FrmClave(Modo modo, Usuario usuario = null)
    {
        _modo = modo;
        _usuario = usuario;

        Text = modo == Modo.CambiarPropia ? "Cambiar mi contraseña" : $"Restablecer contraseña de {usuario?.NombreUsuario}";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Font;
        AutoScaleDimensions = new SizeF(7F, 15F);

        int y = 20;
        var aviso = new Label
        {
            Text = modo == Modo.CambiarPropia
                ? "Ingresá tu contraseña actual y elegí una nueva."
                : $"Elegí una contraseña nueva para {usuario?.NombreCompleto}. Comunicásela en persona.",
            Location = new Point(24, y),
            Size = new Size(Ancho, 36),
            ForeColor = Tema.TextoSecundario
        };
        Controls.Add(aviso);
        y += 44;

        if (modo == Modo.CambiarPropia)
            _txtActual = Campo("Contraseña actual", ref y);
        _txtNueva = Campo("Contraseña nueva", ref y);
        _txtRepetir = Campo("Repetir contraseña nueva", ref y);

        var pista = new Label
        {
            Text = $"Mínimo {UsuarioNegocio.LongitudMinimaClave} caracteres, con letras y números.",
            Location = new Point(24, y - 6),
            AutoSize = true,
            ForeColor = Tema.TextoSecundario
        };
        Controls.Add(pista);
        y += 26;

        var guardar = new Button { Text = "Guardar", Size = new Size(110, 34), Location = new Point(24 + Ancho - 110, y) };
        var cancelar = new Button
        {
            Text = "Cancelar",
            Size = new Size(100, 34),
            Location = new Point(guardar.Left - 110, y),
            DialogResult = DialogResult.Cancel
        };
        guardar.Click += Guardar;
        Controls.Add(guardar);
        Controls.Add(cancelar);
        AcceptButton = guardar;
        CancelButton = cancelar;

        ClientSize = new Size(Ancho + 48, y + 58);
    }

    private TextBox Campo(string etiqueta, ref int y)
    {
        Controls.Add(new Label { Text = etiqueta, Location = new Point(24, y), AutoSize = true, Font = Tema.TextoNegrita9 });
        var texto = new TextBox
        {
            Location = new Point(24, y + 20),
            Width = Ancho,
            Font = Tema.Texto10,
            UseSystemPasswordChar = true
        };
        Controls.Add(texto);
        y += 20 + texto.Height + 14;
        return texto;
    }

    private void Guardar(object sender, EventArgs e)
    {
        try
        {
            if (_txtNueva.Text != _txtRepetir.Text)
                throw new ValidacionException("Las contraseñas nuevas no coinciden.");

            if (_modo == Modo.CambiarPropia)
                _negocio.CambiarClavePropia(_txtActual.Text, _txtNueva.Text);
            else
                _negocio.RestablecerClave(_usuario.IdUsuario, _txtNueva.Text);

            Mensajes.Info("La contraseña se actualizó.");
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            Mensajes.Error(ex);
        }
    }
}
