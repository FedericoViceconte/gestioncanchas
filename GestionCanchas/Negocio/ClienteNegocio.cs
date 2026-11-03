using GestionCanchas.Datos;
using GestionCanchas.Entidades;

namespace GestionCanchas.Negocio;

public class ClienteNegocio
{
    private readonly ClienteDatos _datos = new ClienteDatos();

    public List<Cliente> Listar(bool soloConDeuda) => _datos.Listar(soloConDeuda);

    public Cliente BuscarPorDni(string dni)
    {
        dni = dni?.Trim();
        if (!EsDniValido(dni))
            throw new ValidacionException("El DNI debe tener 7 u 8 dígitos, sin puntos.");

        return _datos.BuscarPorDni(dni);
    }

    public List<Cliente> Buscar(string texto)
    {
        texto = texto?.Trim();
        if (string.IsNullOrEmpty(texto) || texto.Length < 2)
            throw new ValidacionException("Escribí al menos 2 letras o números del DNI, nombre, apellido o teléfono.");

        return _datos.Buscar(texto);
    }

    public void Guardar(Cliente cliente)
    {
        Validar(cliente);

        if (_datos.ExisteDni(cliente.Dni, cliente.IdCliente))
            throw new ValidacionException($"Ya existe un cliente con el DNI {cliente.Dni}.");

        if (cliente.IdCliente == 0)
        {
            _datos.Insertar(cliente);
            return;
        }
        _datos.Modificar(cliente);
    }

    public void Eliminar(int idCliente)
    {
        if (_datos.TieneReservas(idCliente))
            throw new ValidacionException("No se puede eliminar el cliente porque tiene reservas registradas.");

        _datos.Eliminar(idCliente);
    }

    public static bool EsDniValido(string dni)
    {
        if (string.IsNullOrEmpty(dni))
            return false;
        if (dni.Length < 7 || dni.Length > 8)
            return false;
        return dni.All(char.IsAsciiDigit);
    }

    private static void Validar(Cliente cliente)
    {
        cliente.Dni = cliente.Dni?.Trim();
        cliente.Nombre = cliente.Nombre?.Trim();
        cliente.Apellido = cliente.Apellido?.Trim();
        cliente.Telefono = cliente.Telefono?.Trim();

        if (!EsDniValido(cliente.Dni))
            throw new ValidacionException("El DNI debe tener 7 u 8 dígitos, sin puntos.");
        if (string.IsNullOrEmpty(cliente.Nombre))
            throw new ValidacionException("El nombre es obligatorio.");
        if (string.IsNullOrEmpty(cliente.Apellido))
            throw new ValidacionException("El apellido es obligatorio.");
        if (cliente.Nombre.Length > 50 || cliente.Apellido.Length > 50)
            throw new ValidacionException("El nombre y el apellido admiten hasta 50 caracteres.");

        if (string.IsNullOrEmpty(cliente.Telefono))
            return;
        if (cliente.Telefono.Length > 20)
            throw new ValidacionException("El teléfono admite hasta 20 caracteres.");
        if (!cliente.Telefono.All(c => char.IsAsciiDigit(c) || c == ' ' || c == '-' || c == '+'))
            throw new ValidacionException("El teléfono solo puede tener números, espacios, guiones o '+'.");
    }
}
