using GestionCanchas.Entidades;
using GestionCanchas.Negocio;

namespace GestionCanchas.Tests;

// Every case stops at a business rule before reaching SQL Server, so no database is needed.

public class ClienteTests
{
    [Theory]
    [InlineData("1234567")]
    [InlineData("12345678")]
    public void DniDeSieteUOchoDigitosEsValido(string dni) =>
        Assert.True(ClienteNegocio.EsDniValido(dni));

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("123456")]
    [InlineData("123456789")]
    [InlineData("12.345.678")]
    [InlineData("1234567a")]
    [InlineData("１２３４５６７")]
    public void DniConOtroFormatoEsInvalido(string dni) =>
        Assert.False(ClienteNegocio.EsDniValido(dni));

    [Theory]
    [InlineData("123", "Ana", "Pérez", null, "El DNI debe tener 7 u 8 dígitos, sin puntos.")]
    [InlineData("30111222", "  ", "Pérez", null, "El nombre es obligatorio.")]
    [InlineData("30111222", "Ana", "", null, "El apellido es obligatorio.")]
    [InlineData("30111222", "Ana", "Pérez", "11 4444-5555 int.", "El teléfono solo puede tener números, espacios, guiones o '+'.")]
    [InlineData("30111222", "Ana", "Pérez", "+54 11 4444-5555 0000", "El teléfono admite hasta 20 caracteres.")]
    public void GuardarRechazaDatosInvalidos(string dni, string nombre, string apellido, string telefono, string mensaje)
    {
        var cliente = new Cliente { Dni = dni, Nombre = nombre, Apellido = apellido, Telefono = telefono };
        var error = Assert.Throws<ValidacionException>(() => new ClienteNegocio().Guardar(cliente));
        Assert.Equal(mensaje, error.Message);
    }

    [Fact]
    public void GuardarRechazaNombresDeMasDeCincuentaCaracteres()
    {
        var cliente = new Cliente { Dni = "30111222", Nombre = new string('a', 51), Apellido = "Pérez" };
        var error = Assert.Throws<ValidacionException>(() => new ClienteNegocio().Guardar(cliente));
        Assert.Equal("El nombre y el apellido admiten hasta 50 caracteres.", error.Message);
    }

    [Fact]
    public void BuscarExigeAlMenosDosCaracteres()
    {
        var error = Assert.Throws<ValidacionException>(() => new ClienteNegocio().Buscar(" a "));
        Assert.Contains("al menos 2", error.Message);
    }
}

public class CanchaTests
{
    [Theory]
    [InlineData(0, "Fútbol 5", 15000, "Disponible", "El número de cancha debe ser mayor a cero.")]
    [InlineData(1, " ", 15000, "Disponible", "El tipo de cancha es obligatorio.")]
    [InlineData(1, "Fútbol 5 con césped sintético de última generación", 15000, "Disponible", "El tipo admite hasta 30 caracteres.")]
    [InlineData(1, "Fútbol 5", 0, "Disponible", "El precio por hora debe ser mayor a cero.")]
    [InlineData(1, "Fútbol 5", 15000, "Cerrada", "Seleccioná un estado válido.")]
    public void GuardarRechazaDatosInvalidos(int numero, string tipo, int precioHora, string estado, string mensaje)
    {
        var cancha = new Cancha { Numero = numero, Tipo = tipo, PrecioHora = precioHora, Estado = estado };
        var error = Assert.Throws<ValidacionException>(() => new CanchaNegocio().Guardar(cancha));
        Assert.Equal(mensaje, error.Message);
    }
}

public class HorarioTests
{
    [Theory]
    [InlineData(19, 18)]
    [InlineData(18, 18)]
    public void LaHoraDeFinDebeSerPosteriorALaDeInicio(int inicio, int fin)
    {
        var horario = new Horario { HoraInicio = TimeSpan.FromHours(inicio), HoraFin = TimeSpan.FromHours(fin) };
        var error = Assert.Throws<ValidacionException>(() => new HorarioNegocio().Agregar(horario));
        Assert.Equal("La hora de fin debe ser posterior a la de inicio.", error.Message);
    }

    [Fact]
    public void LaDuracionSeCalculaEnHoras()
    {
        var horario = new Horario { HoraInicio = new TimeSpan(18, 0, 0), HoraFin = new TimeSpan(19, 30, 0) };
        Assert.Equal(1.5m, horario.DuracionHoras);
    }
}

public class ReservaTests
{
    private readonly ReservaNegocio _negocio = new ReservaNegocio();

    [Fact]
    public void LaSenaMinimaEsElQuincePorCiento()
    {
        Assert.Equal(0.15m, ReservaNegocio.PorcentajeSenaMinima);
        Assert.Equal(3000m, _negocio.CalcularSenaMinima(20000m));
    }

    [Fact]
    public void LaSenaMinimaSeRedondeaACentavos() =>
        Assert.Equal(5000.00m, _negocio.CalcularSenaMinima(33333.33m));

    [Theory]
    [InlineData(12000.0, 18, 0, 19, 30, 18000.0)]
    [InlineData(15500.50, 20, 0, 21, 0, 15500.50)]
    public void ElImporteEsPrecioPorHoraPorDuracion(double precio, int desdeH, int desdeM, int hastaH, int hastaM, double esperado)
    {
        var cancha = new Cancha { PrecioHora = (decimal)precio };
        var horario = new Horario { HoraInicio = new TimeSpan(desdeH, desdeM, 0), HoraFin = new TimeSpan(hastaH, hastaM, 0) };
        Assert.Equal((decimal)esperado, _negocio.CalcularImporte(cancha, horario));
    }
}

public class PagoTests
{
    [Theory]
    [InlineData("Efectivo")]
    [InlineData("Transferencia")]
    [InlineData("Tarjeta")]
    public void LosMetodosDePagoValidosSeAceptan(string metodo) =>
        PagoNegocio.ValidarMetodo(metodo);

    [Theory]
    [InlineData("Cheque")]
    [InlineData("")]
    [InlineData(null)]
    public void UnMetodoDePagoDesconocidoSeRechaza(string metodo)
    {
        var error = Assert.Throws<ValidacionException>(() => PagoNegocio.ValidarMetodo(metodo));
        Assert.Equal("Seleccioná un método de pago válido.", error.Message);
    }
}
