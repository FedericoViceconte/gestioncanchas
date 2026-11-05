using GestionCanchas.Entidades;
using GestionCanchas.Formularios;
using GestionCanchas.Negocio;

namespace GestionCanchas.Tests;

// Pruebas que no necesitan SQL Server.

public class HashClaveTests
{
    [Fact]
    public void ElHashNoContieneLaClaveYSeVerifica()
    {
        string hash = HashClave.Calcular("Canchas2026");

        Assert.DoesNotContain("Canchas2026", hash);
        Assert.StartsWith("PBKDF2-SHA256$600000$", hash);
        Assert.True(HashClave.Verificar("Canchas2026", hash));
        Assert.False(HashClave.Verificar("canchas2026", hash));
    }

    [Fact]
    public void CadaHashUsaUnaSalDistinta()
    {
        string primero = HashClave.Calcular("MismaClave1");
        string segundo = HashClave.Calcular("MismaClave1");

        Assert.NotEqual(primero, segundo);
        Assert.True(HashClave.Verificar("MismaClave1", primero));
        Assert.True(HashClave.Verificar("MismaClave1", segundo));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("texto plano")]
    [InlineData("PBKDF2-SHA256$abc$c2Fs$aGFzaA==")]
    [InlineData("PBKDF2-SHA256$1000$no-es-base64$aGFzaA==")]
    public void UnHashInvalidoNuncaSeVerifica(string almacenado)
    {
        Assert.False(HashClave.Verificar("cualquiera1", almacenado));
        Assert.True(HashClave.NecesitaActualizarse(almacenado));
    }
}

public class ReglasDeUsuarioTests
{
    [Theory]
    [InlineData("corta1", "La contraseña debe tener al menos 8 caracteres.")]
    [InlineData("soloLetras", "La contraseña debe combinar letras y números.")]
    [InlineData("12345678", "La contraseña debe combinar letras y números.")]
    [InlineData("recepcion1", "La contraseña no puede ser igual al usuario.")]
    public void LaContrasenaDebeSerRazonable(string clave, string mensaje)
    {
        var error = Assert.Throws<ValidacionException>(() => UsuarioNegocio.ValidarClave("recepcion1", clave));
        Assert.Equal(mensaje, error.Message);
    }

    [Fact]
    public void UnaContrasenaValidaSeAcepta() =>
        UsuarioNegocio.ValidarClave("recepcion", "Turnos2026");

    [Fact]
    public void SinSesionDeAdministradorSeNieganLasAccionesDeAdministracion()
    {
        Sesion.Cerrar();
        var error = Assert.Throws<AccesoDenegadoException>(() => Sesion.ExigirAdministrador("eliminar canchas"));
        Assert.Equal("Solo un administrador puede eliminar canchas.", error.Message);
    }

    [Fact]
    public void LaCajaAceptaPeriodosDeHastaUnAnio()
    {
        DateTime hoy = DateTime.Today;
        ReporteNegocio.ValidarRango(hoy.AddDays(-365), hoy);
        Assert.Throws<ValidacionException>(() => ReporteNegocio.ValidarRango(hoy, hoy.AddDays(-1)));
        Assert.Throws<ValidacionException>(() => ReporteNegocio.ValidarRango(hoy.AddDays(-366), hoy));
    }
}

public class ExportacionCsvTests
{
    [Theory]
    [InlineData("Pérez, Juan", "Pérez, Juan")]
    [InlineData("Pérez; Juan", "\"Pérez; Juan\"")]
    [InlineData("Dijo \"hola\"", "\"Dijo \"\"hola\"\"\"")]
    [InlineData("=HYPERLINK(\"x\")", "\"'=HYPERLINK(\"\"x\"\")\"")]
    [InlineData("+54 11", "'+54 11")]
    [InlineData("@SUMA", "'@SUMA")]
    [InlineData("-$ 4.500,00", "-$ 4.500,00")]
    [InlineData("-10", "-10")]
    [InlineData("-cmd", "'-cmd")]
    public void LosValoresSeEscapanParaExcel(string valor, string esperado) =>
        Assert.Equal(esperado, Exportar.Escapar(valor, ";"));

    [Fact]
    public void ElCsvTieneEncabezadoYUnaFilaPorRegistro()
    {
        string csv = Exportar.GenerarCsv(
            new[] { "Cliente", "Saldo" },
            new[] { new[] { "Pérez, Juan", "$ 15.000,00" }, new[] { "Gómez; Pedro", "$ 0,00" } },
            ";");

        string[] lineas = csv.TrimEnd().Split(Environment.NewLine);
        Assert.Equal(new[] { "Cliente;Saldo", "Pérez, Juan;$ 15.000,00", "\"Gómez; Pedro\";$ 0,00" }, lineas);
    }

    [Fact]
    public void LosRolesSonAdministradorYRecepcionista() =>
        Assert.Equal(new[] { Roles.Administrador, Roles.Recepcionista }, Roles.Todos);
}

public class ConfiguracionTests
{
    [Fact]
    public void ElAppsettingsIncluidoEsJsonValidoYApuntaALocalDb()
    {
        string archivo = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        using var json = System.Text.Json.JsonDocument.Parse(File.ReadAllText(archivo));
        string cadena = json.RootElement.GetProperty("ConnectionStrings").GetProperty("GestionCanchas").GetString();

        var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(cadena);
        Assert.Equal(@"(localdb)\MSSQLLocalDB", builder.DataSource);
        Assert.Equal("GestionCanchas", builder.InitialCatalog);
    }
}
