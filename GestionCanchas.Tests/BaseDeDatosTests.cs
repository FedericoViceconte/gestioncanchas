using GestionCanchas.Datos;
using GestionCanchas.Entidades;
using GestionCanchas.Negocio;
using Microsoft.Data.SqlClient;

// La sesión y la cadena de conexión son estáticas: las pruebas no corren en paralelo.
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace GestionCanchas.Tests;

/// <summary>
/// Crea una base GestionCanchasPruebas con el script de la aplicación y la borra al terminar.
/// Usa LocalDB, o el servidor de la variable GESTIONCANCHAS_PRUEBAS_SERVIDOR.
/// </summary>
public sealed class BaseDePruebas : IDisposable
{
    public const string Nombre = "GestionCanchasPruebas";

    private readonly string _cadenaAnterior;

    public BaseDePruebas()
    {
        _cadenaAnterior = Conexion.Cadena;
        string servidor = Environment.GetEnvironmentVariable("GESTIONCANCHAS_PRUEBAS_SERVIDOR");
        if (string.IsNullOrWhiteSpace(servidor))
            servidor = @"(localdb)\MSSQLLocalDB";

        Conexion.Cadena = $"Server={servidor};Database={Nombre};Integrated Security=True;TrustServerCertificate=True";
        BaseDatos.Crear(reemplazarExistente: true);

        var usuarios = new UsuarioNegocio();
        usuarios.CrearPrimerAdministrador(new Usuario { NombreUsuario = "admin", NombreCompleto = "Admin Pruebas" }, "Admin2026");
        Administrador = usuarios.IniciarSesion("admin", "Admin2026");
        usuarios.Crear(new Usuario { NombreUsuario = "recepcion", NombreCompleto = "Recepción Pruebas", Rol = Roles.Recepcionista },
            "Recepcion2026");
        Recepcionista = usuarios.IniciarSesion("recepcion", "Recepcion2026");
        Sesion.Cerrar();
    }

    public Usuario Administrador { get; }

    public Usuario Recepcionista { get; }

    public void Dispose()
    {
        Sesion.Cerrar();
        SqlConnection.ClearAllPools();
        using (SqlConnection master = Conexion.AbrirMaster())
        using (var borrar = new SqlCommand(
                   $"ALTER DATABASE [{Nombre}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{Nombre}];", master))
            borrar.ExecuteNonQuery();
        Conexion.Cadena = _cadenaAnterior;
    }
}

[CollectionDefinition(Nombre)]
public class ColeccionBaseDeDatos : ICollectionFixture<BaseDePruebas>
{
    public const string Nombre = "Base de datos";
}

[Collection(ColeccionBaseDeDatos.Nombre)]
[Trait("Categoria", "BaseDeDatos")]
public class BaseDeDatosTests
{
    private static int _ultimoDni = 30_000_000;

    private readonly BaseDePruebas _base;

    public BaseDeDatosTests(BaseDePruebas baseDePruebas)
    {
        _base = baseDePruebas;
        Sesion.Iniciar(_base.Administrador);
    }

    [Fact]
    public void LaBaseQuedaCreadaYLasActualizacionesSePuedenRepetir()
    {
        Assert.Equal(EstadoBase.Lista, BaseDatos.Verificar());
        BaseDatos.Actualizar();
        BaseDatos.Actualizar();

        List<Usuario> usuarios = new UsuarioNegocio().Listar();
        Assert.Contains(usuarios, u => u.NombreUsuario == "admin" && u.EsAdministrador);
        Assert.Contains(usuarios, u => u.NombreUsuario == "recepcion" && !u.EsAdministrador);
        Assert.Equal(15, new HorarioNegocio().Listar().Count);
        Assert.Equal(3, new CanchaNegocio().ListarDisponibles().Count);
    }

    [Fact]
    public void CrearNoReemplazaUnaBaseExistenteSinPedirlo()
    {
        var error = Assert.Throws<InvalidOperationException>(() => BaseDatos.Crear());
        Assert.Contains("ya existe", error.Message);
        Assert.NotEmpty(new UsuarioNegocio().Listar());
    }

    [Fact]
    public void ElInicioDeSesionRechazaClavesIncorrectasYUsuariosInactivos()
    {
        var negocio = new UsuarioNegocio();
        var usuario = new Usuario { NombreUsuario = "temporal", NombreCompleto = "Usuario Temporal", Rol = Roles.Recepcionista };
        negocio.Crear(usuario, "Temporal2026");

        Assert.Equal("Usuario o contraseña incorrectos.",
            Assert.Throws<ValidacionException>(() => negocio.IniciarSesion("temporal", "Otra2026")).Message);
        Sesion.Iniciar(_base.Administrador);

        usuario.Activo = false;
        negocio.Modificar(usuario);
        Assert.Throws<ValidacionException>(() => negocio.IniciarSesion("temporal", "Temporal2026"));

        Sesion.Iniciar(_base.Administrador);
        usuario.Activo = true;
        negocio.Modificar(usuario);
        Assert.Equal("temporal", negocio.IniciarSesion("TEMPORAL", "Temporal2026").NombreUsuario);
    }

    [Fact]
    public void DespuesDeCincoIntentosFallidosElUsuarioQuedaBloqueadoUnMinuto()
    {
        var negocio = new UsuarioNegocio();
        negocio.Crear(new Usuario { NombreUsuario = "bloqueo", NombreCompleto = "Bloqueo", Rol = Roles.Recepcionista }, "Bloqueo2026");

        for (int i = 0; i < UsuarioNegocio.IntentosAntesDeBloquear; i++)
            Assert.Throws<ValidacionException>(() => negocio.IniciarSesion("bloqueo", "Incorrecta1"));

        var error = Assert.Throws<ValidacionException>(() => negocio.IniciarSesion("bloqueo", "Bloqueo2026"));
        Assert.Contains("demasiados intentos", error.Message);
    }

    [Fact]
    public void UnAdministradorNoPuedeQuitarseElRolNiDesactivarse()
    {
        var negocio = new UsuarioNegocio();
        Usuario yo = negocio.Listar().Single(u => u.NombreUsuario == "admin");

        yo.Rol = Roles.Recepcionista;
        var error = Assert.Throws<ValidacionException>(() => negocio.Modificar(yo));
        Assert.Equal("No podés quitarte el rol de administrador ni desactivar tu propio usuario.", error.Message);
    }

    [Fact]
    public void LaRecepcionNoPuedeAdministrar()
    {
        Sesion.Iniciar(_base.Recepcionista);

        Assert.Throws<AccesoDenegadoException>(() => new CanchaNegocio().Eliminar(1));
        Assert.Throws<AccesoDenegadoException>(() => new UsuarioNegocio().Listar());
        Assert.Throws<AccesoDenegadoException>(() => new PagoNegocio().Anular(1));
        Assert.Throws<AccesoDenegadoException>(() =>
            new CanchaNegocio().Guardar(new Cancha { Numero = 9, Tipo = "Fútbol 5", PrecioHora = 1000, Estado = EstadosCancha.Disponible }));
    }

    [Fact]
    public void CambiarLaClavePropiaExigeLaActual()
    {
        var negocio = new UsuarioNegocio();
        negocio.Crear(new Usuario { NombreUsuario = "cambio", NombreCompleto = "Cambio Clave", Rol = Roles.Recepcionista }, "Inicial2026");
        negocio.IniciarSesion("cambio", "Inicial2026");

        Assert.Throws<ValidacionException>(() => negocio.CambiarClavePropia("Equivocada1", "Nueva2026"));
        negocio.CambiarClavePropia("Inicial2026", "Nueva2026");

        Assert.Throws<ValidacionException>(() => negocio.IniciarSesion("cambio", "Inicial2026"));
        Assert.Equal("cambio", negocio.IniciarSesion("cambio", "Nueva2026").NombreUsuario);
    }

    [Fact]
    public void UnaReservaConSenaQuedaSenadaYElPagoRegistraQuienLoCobro()
    {
        Sesion.Iniciar(_base.Recepcionista);
        (Reserva reserva, int idReserva) = Reservar(DateTime.Today.AddDays(40), 18, sena: 0.5m);

        ReservaDetalle detalle = new ReservaNegocio().ObtenerDetalle(idReserva);
        Assert.Equal(EstadosReserva.Senado, detalle.Estado);
        Assert.Equal(reserva.Importe / 2, detalle.TotalPagado);

        PagoDetalle pago = new PagoNegocio().Listar(null, DateTime.Today, DateTime.Today)
            .Single(p => p.IdReserva == idReserva);
        Assert.Equal("recepcion", pago.Usuario);
        Assert.StartsWith("Cancha ", pago.Cancha);
        Assert.DoesNotContain("Pago", pago.Cancha);
    }

    [Fact]
    public void NoSePuedeReservarDosVecesElMismoTurno()
    {
        DateTime fecha = DateTime.Today.AddDays(41);
        (Reserva reserva, _) = Reservar(fecha, 19, sena: 0.2m);

        var repetida = new Reserva
        {
            IdCliente = NuevoCliente(),
            IdCancha = reserva.IdCancha,
            IdHorario = reserva.IdHorario,
            Fecha = fecha
        };
        var error = Assert.Throws<ValidacionException>(() => new ReservaNegocio().Registrar(repetida, Sena(1_000_000)));
        Assert.Equal("La cancha ya está reservada en esa fecha y horario.", error.Message);
    }

    [Fact]
    public void PagarElSaldoDejaLaReservaPagadaYSumaEnLaCaja()
    {
        var reportes = new ReporteNegocio();
        decimal cobradoAntes = reportes.CajaPorMetodo(DateTime.Today, DateTime.Today)
            .Where(t => t.Grupo == MetodosPago.Tarjeta).Sum(t => t.Cobrado);

        (Reserva reserva, int idReserva) = Reservar(DateTime.Today.AddDays(42), 20, sena: 0.2m);
        decimal saldo = new ReservaNegocio().ObtenerDetalle(idReserva).Saldo;
        decimal restante = new PagoNegocio().Registrar(new Pago { IdReserva = idReserva, Importe = saldo, MetodoPago = MetodosPago.Tarjeta });

        Assert.Equal(0, restante);
        Assert.Equal(EstadosReserva.Pagado, new ReservaNegocio().ObtenerDetalle(idReserva).Estado);

        decimal cobradoDespues = reportes.CajaPorMetodo(DateTime.Today, DateTime.Today)
            .Where(t => t.Grupo == MetodosPago.Tarjeta).Sum(t => t.Cobrado);
        Assert.Equal(cobradoAntes + saldo, cobradoDespues);
        Assert.Contains(reportes.CajaPorUsuario(DateTime.Today, DateTime.Today), t => t.Grupo == "Admin Pruebas");
        Assert.True(reportes.Indicadores(DateTime.Today).CobradoHoy >= reserva.Importe);
    }

    [Fact]
    public void SoloUnAdministradorRegistraDevolucionesDeReservasCanceladas()
    {
        (_, int idReserva) = Reservar(DateTime.Today.AddDays(43), 21, sena: 0.3m);
        new ReservaNegocio().Cancelar(idReserva);
        decimal cobrado = new ReservaNegocio().ObtenerDetalle(idReserva).TotalPagado;

        Sesion.Iniciar(_base.Recepcionista);
        Assert.Throws<AccesoDenegadoException>(() =>
            new PagoNegocio().RegistrarDevolucion(idReserva, cobrado, MetodosPago.Efectivo));

        Sesion.Iniciar(_base.Administrador);
        new PagoNegocio().RegistrarDevolucion(idReserva, cobrado, MetodosPago.Efectivo);

        ReservaDetalle detalle = new ReservaNegocio().ObtenerDetalle(idReserva);
        Assert.Equal(0, detalle.TotalPagado);
        Assert.Contains(new ReporteNegocio().CajaPorMetodo(DateTime.Today, DateTime.Today),
            t => t.Grupo == MetodosPago.Efectivo && t.Devuelto >= cobrado);
    }

    [Fact]
    public void ElGraficoDeIngresosDevuelveUnValorPorDia()
    {
        List<IngresoDiario> dias = new ReporteNegocio().IngresosUltimosDias(DateTime.Today, 7);

        Assert.Equal(7, dias.Count);
        Assert.Equal(DateTime.Today, dias[^1].Dia);
        Assert.Equal(DateTime.Today.AddDays(-6), dias[0].Dia);
    }

    private (Reserva Reserva, int IdReserva) Reservar(DateTime fecha, int hora, decimal sena)
    {
        Cancha cancha = new CanchaNegocio().ListarDisponibles().First();
        Horario horario = new HorarioNegocio().Listar().Single(h => h.HoraInicio == TimeSpan.FromHours(hora));
        var reserva = new Reserva { IdCliente = NuevoCliente(), IdCancha = cancha.IdCancha, IdHorario = horario.IdHorario, Fecha = fecha };
        decimal importe = new ReservaNegocio().CalcularImporte(cancha, horario);

        int id = new ReservaNegocio().Registrar(reserva, Sena(Math.Round(importe * sena, 2)));
        return (reserva, id);
    }

    private static Pago Sena(decimal importe) => new Pago { Importe = importe, MetodoPago = MetodosPago.Efectivo };

    private static int NuevoCliente()
    {
        string dni = Interlocked.Increment(ref _ultimoDni).ToString();
        var negocio = new ClienteNegocio();
        negocio.Guardar(new Cliente { Dni = dni, Nombre = "Cliente", Apellido = "De Prueba", Telefono = "1100000000" });
        return negocio.BuscarPorDni(dni).IdCliente;
    }
}
