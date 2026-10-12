    /$$$$$ /$$$$$$$        /$$      /$$  /$$$$$$  /$$$$$$$   /$$$$$$   /$$$$$$  /$$   /$$
   |__  $$| $$__  $$      | $$$    /$$$ /$$__  $$| $$__  $$ /$$__  $$ /$$__  $$| $$$ | $$
      | $$| $$  \ $$      | $$$$  /$$$$| $$  \ $$| $$  \ $$| $$  \__/| $$  \ $$| $$$$| $$
      | $$| $$$$$$$/      | $$ $$/$$ $$| $$  | $$| $$$$$$$/| $$ /$$$$| $$$$$$$$| $$ $$ $$
 /$$  | $$| $$____/       | $$  $$$| $$| $$  | $$| $$__  $$| $$|_  $$| $$__  $$| $$  $$$$
| $$  | $$| $$            | $$\  $ | $$| $$  | $$| $$  \ $$| $$  \ $$| $$  | $$| $$\  $$$
|  $$$$$$/| $$            | $$ \/  | $$|  $$$$$$/| $$  | $$|  $$$$$$/| $$  | $$| $$ \  $$
 \______/ |__/            |__/     |__/ \______/ |__/  |__/ \______/ |__/  |__/|__/  \__/

# Gestión de Canchas de Fútbol

Proyecto final de Programación Orientada a Objetos. Aplicación Windows Forms en C# (.NET 10) con base de datos SQL Server LocalDB.

## Requisitos

- Visual Studio 2026 con las cargas de trabajo:
  - Desarrollo de escritorio de .NET
  - Almacenamiento y procesamiento de datos (instala SQL Server LocalDB)

## Puesta en marcha

1. Crear la base: en Visual Studio, *Ver > Explorador de objetos de SQL Server*, conectar a `(localdb)\MSSQLLocalDB`, abrir una nueva consulta y ejecutar `GestionCanchas/Scripts/GestionCanchas.sql`.
2. Abrir `GestionCanchas.slnx`.
3. Ejecutar con F5. El paquete NuGet `Microsoft.Data.SqlClient` se restaura solo.

Si la instancia de LocalDB tiene otro nombre, se cambia la cadena en `Datos/Conexion.cs`.

## Estructura

| Carpeta | Responsabilidad |
|---|---|
| `Entidades` | Clases que representan las tablas y los datos que muestran las grillas |
| `Datos` | Acceso a SQL con ADO.NET (una clase por tabla) |
| `Negocio` | Validaciones y reglas antes de ir a la base |
| `Formularios` | Pantallas de Windows Forms |
| `Scripts` | Script de creación de la base con datos de prueba |

## Reglas principales

- No se puede reservar la misma cancha, fecha y horario dos veces (índice único filtrado en la base + validación en `ReservaNegocio`).
- La seña mínima es el 15% del importe de la reserva.
- Estados de reserva: Señado, Pagado, Cancelada. La cancelada libera el turno y no genera deuda.
- La deuda del cliente se calcula con la vista `vw_DeudaClientes`.
- Transacciones: alta de reserva con su seña (`ReservaDatos.InsertarConSena`) y registro de pago con actualización de estado (`PagoDatos.Registrar`).

    ___          ___                  ___           ___           ___           ___           ___           ___     
   /  /\        /  /\                /__/\         /  /\         /  /\         /  /\         /  /\         /__/\    
  /  /:/       /  /::\              |  |::\       /  /::\       /  /::\       /  /:/_       /  /::\        \  \:\   
 /__/::\      /  /:/\:\             |  |:|:\     /  /:/\:\     /  /:/\:\     /  /:/ /\     /  /:/\:\        \  \:\  
 \__\/\:\    /  /:/~/:/           __|__|:|\:\   /  /:/  \:\   /  /:/~/:/    /  /:/_/::\   /  /:/~/::\   _____\__\:\ 
    \  \:\  /__/:/ /:/           /__/::::| \:\ /__/:/ \__\:\ /__/:/ /:/___ /__/:/__\/\:\ /__/:/ /:/\:\ /__/::::::::\
     \__\:\ \  \:\/:/            \  \:\~~\__\/ \  \:\ /  /:/ \  \:\/:::::/ \  \:\ /~~/:/ \  \:\/:/__\/ \  \:\~~\~~\/
     /  /:/  \  \::/              \  \:\        \  \:\  /:/   \  \::/~~~~   \  \:\  /:/   \  \::/       \  \:\  ~~~ 
    /__/:/    \  \:\               \  \:\        \  \:\/:/     \  \:\        \  \:\/:/     \  \:\        \  \:\     
    \__\/      \  \:\               \  \:\        \  \::/       \  \:\        \  \::/       \  \:\        \  \:\    
                \__\/                \__\/         \__\/         \__\/         \__\/         \__\/         \__\/    
-----------------------------------------------------------------------------------------------------------------------
                        TODOS LOS DERECHOS RESERVADOS 2026
-----------------------------------------------------------------------------------------------------------------------