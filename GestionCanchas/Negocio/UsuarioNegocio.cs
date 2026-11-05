using System.Text.RegularExpressions;
using GestionCanchas.Datos;
using GestionCanchas.Entidades;

namespace GestionCanchas.Negocio;

public class UsuarioNegocio
{
    public const int LongitudMinimaClave = 8;
    public const int IntentosAntesDeBloquear = 5;
    public static readonly TimeSpan DuracionBloqueo = TimeSpan.FromMinutes(1);

    private static readonly Regex FormatoNombreUsuario = new("^[a-z0-9._-]{3,30}$", RegexOptions.CultureInvariant);
    private static readonly Dictionary<string, (int Fallos, DateTime BloqueadoHasta)> Intentos =
        new(StringComparer.OrdinalIgnoreCase);

    private readonly UsuarioDatos _datos = new UsuarioDatos();

    public bool HayUsuarios() => _datos.Contar() > 0;

    /// <summary>Crea la primera cuenta, que siempre es de administrador. Solo funciona si no hay usuarios.</summary>
    public void CrearPrimerAdministrador(Usuario usuario, string clave)
    {
        if (HayUsuarios())
            throw new ValidacionException("Ya hay usuarios creados. Iniciá sesión con uno de ellos.");

        usuario.Rol = Roles.Administrador;
        usuario.Activo = true;
        Validar(usuario);
        ValidarClave(usuario.NombreUsuario, clave);
        usuario.ClaveHash = HashClave.Calcular(clave);
        _datos.Insertar(usuario);
    }

    public Usuario IniciarSesion(string nombreUsuario, string clave)
    {
        string nombre = Normalizar(nombreUsuario);
        if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(clave))
            throw new ValidacionException("Ingresá tu usuario y tu contraseña.");

        lock (Intentos)
        {
            if (Intentos.TryGetValue(nombre, out var estado) && estado.BloqueadoHasta > DateTime.Now)
                throw new ValidacionException(
                    "Hubo demasiados intentos fallidos. Esperá un minuto y volvé a intentar.");
        }

        Usuario usuario = _datos.ObtenerPorNombre(nombre);
        if (usuario == null || !usuario.Activo || !HashClave.Verificar(clave, usuario.ClaveHash))
        {
            RegistrarFallo(nombre);
            throw new ValidacionException("Usuario o contraseña incorrectos.");
        }

        lock (Intentos)
            Intentos.Remove(nombre);

        if (HashClave.NecesitaActualizarse(usuario.ClaveHash))
        {
            usuario.ClaveHash = HashClave.Calcular(clave);
            _datos.CambiarClave(usuario.IdUsuario, usuario.ClaveHash);
        }
        _datos.RegistrarAcceso(usuario.IdUsuario);
        usuario.UltimoAcceso = DateTime.Now;
        Sesion.Iniciar(usuario);
        return usuario;
    }

    public List<Usuario> Listar()
    {
        Sesion.ExigirAdministrador("ver los usuarios");
        return _datos.Listar();
    }

    public void Crear(Usuario usuario, string clave)
    {
        Validar(usuario);
        ValidarClave(usuario.NombreUsuario, clave);
        Sesion.ExigirAdministrador("crear usuarios");

        if (_datos.ExisteNombre(usuario.NombreUsuario, 0))
            throw new ValidacionException($"Ya existe el usuario \"{usuario.NombreUsuario}\".");

        usuario.ClaveHash = HashClave.Calcular(clave);
        _datos.Insertar(usuario);
    }

    public void Modificar(Usuario usuario)
    {
        Validar(usuario);
        Sesion.ExigirAdministrador("modificar usuarios");

        Usuario actual = _datos.ObtenerPorId(usuario.IdUsuario)
            ?? throw new ValidacionException("Seleccioná un usuario.");

        bool dejaDeSerAdministrador = actual.EsAdministrador && actual.Activo
                                      && (!usuario.EsAdministrador || !usuario.Activo);
        if (dejaDeSerAdministrador && usuario.IdUsuario == Sesion.IdUsuario)
            throw new ValidacionException("No podés quitarte el rol de administrador ni desactivar tu propio usuario.");
        if (dejaDeSerAdministrador && _datos.ContarAdministradoresActivos() <= 1)
            throw new ValidacionException("Tiene que quedar al menos un administrador activo.");

        _datos.Modificar(usuario);
    }

    public void RestablecerClave(int idUsuario, string claveNueva)
    {
        Sesion.ExigirAdministrador("restablecer contraseñas");
        Usuario usuario = _datos.ObtenerPorId(idUsuario)
            ?? throw new ValidacionException("Seleccioná un usuario.");

        ValidarClave(usuario.NombreUsuario, claveNueva);
        _datos.CambiarClave(idUsuario, HashClave.Calcular(claveNueva));
    }

    public void CambiarClavePropia(string claveActual, string claveNueva)
    {
        Usuario usuario = Sesion.Actual == null ? null : _datos.ObtenerPorId(Sesion.Actual.IdUsuario);
        if (usuario == null)
            throw new ValidacionException("Iniciá sesión para cambiar la contraseña.");
        if (!HashClave.Verificar(claveActual ?? string.Empty, usuario.ClaveHash))
            throw new ValidacionException("La contraseña actual no es correcta.");

        ValidarClave(usuario.NombreUsuario, claveNueva);
        if (claveNueva == claveActual)
            throw new ValidacionException("La contraseña nueva tiene que ser distinta de la actual.");

        _datos.CambiarClave(usuario.IdUsuario, HashClave.Calcular(claveNueva));
    }

    public static void ValidarClave(string nombreUsuario, string clave)
    {
        if (string.IsNullOrEmpty(clave) || clave.Length < LongitudMinimaClave)
            throw new ValidacionException($"La contraseña debe tener al menos {LongitudMinimaClave} caracteres.");
        if (!clave.Any(char.IsLetter) || !clave.Any(char.IsDigit))
            throw new ValidacionException("La contraseña debe combinar letras y números.");
        if (string.Equals(clave, nombreUsuario?.Trim(), StringComparison.OrdinalIgnoreCase))
            throw new ValidacionException("La contraseña no puede ser igual al usuario.");
    }

    private static void Validar(Usuario usuario)
    {
        usuario.NombreUsuario = Normalizar(usuario.NombreUsuario);
        usuario.NombreCompleto = usuario.NombreCompleto?.Trim();

        if (!FormatoNombreUsuario.IsMatch(usuario.NombreUsuario ?? string.Empty))
            throw new ValidacionException(
                "El usuario debe tener entre 3 y 30 caracteres: letras, números, punto, guion o guion bajo, sin espacios.");
        if (string.IsNullOrEmpty(usuario.NombreCompleto))
            throw new ValidacionException("El nombre es obligatorio.");
        if (usuario.NombreCompleto.Length > 80)
            throw new ValidacionException("El nombre admite hasta 80 caracteres.");
        if (!Roles.Todos.Contains(usuario.Rol))
            throw new ValidacionException("Seleccioná un rol válido.");
    }

    private static string Normalizar(string nombreUsuario) => nombreUsuario?.Trim().ToLowerInvariant();

    private static void RegistrarFallo(string nombre)
    {
        lock (Intentos)
        {
            int fallos = Intentos.TryGetValue(nombre, out var estado) ? estado.Fallos + 1 : 1;
            DateTime bloqueo = fallos >= IntentosAntesDeBloquear ? DateTime.Now + DuracionBloqueo : DateTime.MinValue;
            Intentos[nombre] = (fallos >= IntentosAntesDeBloquear ? 0 : fallos, bloqueo);
        }
    }
}
