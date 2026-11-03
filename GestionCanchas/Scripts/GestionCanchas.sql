SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

USE master;
GO

IF DB_ID('GestionCanchas') IS NOT NULL
BEGIN
    ALTER DATABASE GestionCanchas SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE GestionCanchas;
END
GO

CREATE DATABASE GestionCanchas;
GO

USE GestionCanchas;
GO

CREATE TABLE CLIENTES (
    IdCliente INT IDENTITY(1,1) NOT NULL,
    DNI       VARCHAR(8)  NOT NULL,
    Nombre    VARCHAR(50) NOT NULL,
    Apellido  VARCHAR(50) NOT NULL,
    Telefono  VARCHAR(20) NULL,
    CONSTRAINT PK_CLIENTES PRIMARY KEY (IdCliente),
    CONSTRAINT UQ_CLIENTES_DNI UNIQUE (DNI),
    CONSTRAINT CK_CLIENTES_DNI CHECK (DNI NOT LIKE '%[^0-9]%' AND LEN(DNI) BETWEEN 7 AND 8)
);
GO

CREATE TABLE CANCHAS (
    IdCancha   INT IDENTITY(1,1) NOT NULL,
    Numero     INT           NOT NULL,
    Tipo       VARCHAR(30)   NOT NULL,
    PrecioHora DECIMAL(10,2) NOT NULL,
    Estado     VARCHAR(20)   NOT NULL CONSTRAINT DF_CANCHAS_Estado DEFAULT 'Disponible',
    CONSTRAINT PK_CANCHAS PRIMARY KEY (IdCancha),
    CONSTRAINT UQ_CANCHAS_Numero UNIQUE (Numero),
    CONSTRAINT CK_CANCHAS_Numero CHECK (Numero > 0),
    CONSTRAINT CK_CANCHAS_PrecioHora CHECK (PrecioHora > 0),
    CONSTRAINT CK_CANCHAS_Estado CHECK (Estado IN ('Disponible', 'Mantenimiento'))
);
GO

CREATE TABLE HORARIOS (
    IdHorario  INT IDENTITY(1,1) NOT NULL,
    HoraInicio TIME(0) NOT NULL,
    HoraFin    TIME(0) NOT NULL,
    CONSTRAINT PK_HORARIOS PRIMARY KEY (IdHorario),
    CONSTRAINT UQ_HORARIOS UNIQUE (HoraInicio, HoraFin),
    CONSTRAINT CK_HORARIOS_Rango CHECK (HoraFin > HoraInicio)
);
GO

CREATE TABLE RESERVAS (
    IdReserva INT IDENTITY(1,1) NOT NULL,
    IdCliente INT           NOT NULL,
    IdCancha  INT           NOT NULL,
    IdHorario INT           NOT NULL,
    Fecha     DATE          NOT NULL,
    Importe   DECIMAL(10,2) NOT NULL,
    Estado    VARCHAR(20)   NOT NULL,
    CONSTRAINT PK_RESERVAS PRIMARY KEY (IdReserva),
    CONSTRAINT FK_RESERVAS_CLIENTES FOREIGN KEY (IdCliente) REFERENCES CLIENTES (IdCliente),
    CONSTRAINT FK_RESERVAS_CANCHAS  FOREIGN KEY (IdCancha)  REFERENCES CANCHAS (IdCancha),
    CONSTRAINT FK_RESERVAS_HORARIOS FOREIGN KEY (IdHorario) REFERENCES HORARIOS (IdHorario),
    CONSTRAINT CK_RESERVAS_Importe CHECK (Importe > 0),
    CONSTRAINT CK_RESERVAS_Estado CHECK (Estado IN ('Pendiente', 'Señado', 'Pagado', 'Cancelada', 'Ausente'))
);
GO

CREATE TABLE TURNOS_FIJOS (
    IdTurnoFijo INT IDENTITY(1,1) NOT NULL,
    IdCliente   INT     NOT NULL,
    IdCancha    INT     NOT NULL,
    IdHorario   INT     NOT NULL,
    DiaSemana   TINYINT NOT NULL,
    Activo      BIT     NOT NULL CONSTRAINT DF_TURNOS_FIJOS_Activo DEFAULT 1,
    CONSTRAINT PK_TURNOS_FIJOS PRIMARY KEY (IdTurnoFijo),
    CONSTRAINT FK_TURNOS_FIJOS_CLIENTES FOREIGN KEY (IdCliente) REFERENCES CLIENTES (IdCliente),
    CONSTRAINT FK_TURNOS_FIJOS_CANCHAS  FOREIGN KEY (IdCancha)  REFERENCES CANCHAS (IdCancha),
    CONSTRAINT FK_TURNOS_FIJOS_HORARIOS FOREIGN KEY (IdHorario) REFERENCES HORARIOS (IdHorario),
    CONSTRAINT CK_TURNOS_FIJOS_Dia CHECK (DiaSemana BETWEEN 0 AND 6)
);
GO

ALTER TABLE RESERVAS ADD IdTurnoFijo INT NULL
    CONSTRAINT FK_RESERVAS_TURNOS_FIJOS FOREIGN KEY REFERENCES TURNOS_FIJOS (IdTurnoFijo);
GO

CREATE UNIQUE INDEX UX_RESERVAS_Turno
    ON RESERVAS (IdCancha, Fecha, IdHorario)
    WHERE Estado <> 'Cancelada';
GO

CREATE TABLE PAGOS (
    IdPago     INT IDENTITY(1,1) NOT NULL,
    IdReserva  INT           NOT NULL,
    FechaPago  DATE          NOT NULL CONSTRAINT DF_PAGOS_FechaPago DEFAULT CAST(GETDATE() AS DATE),
    Importe    DECIMAL(10,2) NOT NULL,
    MetodoPago VARCHAR(20)   NOT NULL,
    Tipo       VARCHAR(15)   NOT NULL CONSTRAINT DF_PAGOS_Tipo DEFAULT 'Pago',
    Anulado    BIT           NOT NULL CONSTRAINT DF_PAGOS_Anulado DEFAULT 0,
    CONSTRAINT PK_PAGOS PRIMARY KEY (IdPago),
    CONSTRAINT FK_PAGOS_RESERVAS FOREIGN KEY (IdReserva) REFERENCES RESERVAS (IdReserva),
    CONSTRAINT CK_PAGOS_Importe CHECK (Importe > 0),
    CONSTRAINT CK_PAGOS_MetodoPago CHECK (MetodoPago IN ('Efectivo', 'Transferencia', 'Tarjeta')),
    CONSTRAINT CK_PAGOS_Tipo CHECK (Tipo IN ('Pago', 'Devolución'))
);
GO

CREATE VIEW vw_SaldoReservas AS
SELECT r.IdReserva,
       r.IdCliente,
       r.IdCancha,
       r.IdHorario,
       r.Fecha,
       r.Estado,
       r.Importe,
       ISNULL(p.TotalPagado, 0)             AS TotalPagado,
       r.Importe - ISNULL(p.TotalPagado, 0) AS Saldo
FROM RESERVAS r
LEFT JOIN (SELECT IdReserva,
                  SUM(CASE WHEN Tipo = 'Devolución' THEN -Importe ELSE Importe END) AS TotalPagado
           FROM PAGOS
           WHERE Anulado = 0
           GROUP BY IdReserva) p ON p.IdReserva = r.IdReserva;
GO

CREATE VIEW vw_DeudaClientes AS
SELECT c.IdCliente,
       c.DNI,
       c.Nombre,
       c.Apellido,
       c.Telefono,
       ISNULL(SUM(s.Saldo), 0) AS Deuda
FROM CLIENTES c
LEFT JOIN vw_SaldoReservas s ON s.IdCliente = c.IdCliente
                            AND s.Estado NOT IN ('Cancelada', 'Ausente')
                            AND NOT (s.Estado = 'Pendiente' AND s.Fecha > CAST(GETDATE() AS DATE))
GROUP BY c.IdCliente, c.DNI, c.Nombre, c.Apellido, c.Telefono;
GO

INSERT INTO CLIENTES (DNI, Nombre, Apellido, Telefono) VALUES
('45123456', 'Juan',  'Pérez',     '1122334455'),
('40987654', 'Pedro', 'Gómez',     '1144556677'),
('43876543', 'Lucas', 'Rodríguez', '1166778899');

INSERT INTO CANCHAS (Numero, Tipo, PrecioHora, Estado) VALUES
(1, 'Fútbol 5',  30000.00, 'Disponible'),
(2, 'Fútbol 5',  30000.00, 'Disponible'),
(3, 'Fútbol 7',  45000.00, 'Disponible'),
(4, 'Fútbol 11', 70000.00, 'Mantenimiento');

DECLARE @hora INT = 8;
WHILE @hora < 23
BEGIN
    INSERT INTO HORARIOS (HoraInicio, HoraFin)
    VALUES (TIMEFROMPARTS(@hora, 0, 0, 0, 0), TIMEFROMPARTS(@hora + 1, 0, 0, 0, 0));
    SET @hora += 1;
END

DECLARE @hoy DATE = CAST(GETDATE() AS DATE);
DECLARE @h18 INT = (SELECT IdHorario FROM HORARIOS WHERE HoraInicio = '18:00');
DECLARE @h19 INT = (SELECT IdHorario FROM HORARIOS WHERE HoraInicio = '19:00');
DECLARE @h20 INT = (SELECT IdHorario FROM HORARIOS WHERE HoraInicio = '20:00');
DECLARE @h21 INT = (SELECT IdHorario FROM HORARIOS WHERE HoraInicio = '21:00');

INSERT INTO RESERVAS (IdCliente, IdCancha, IdHorario, Fecha, Importe, Estado) VALUES
(1, 1, @h18, DATEADD(DAY,  1, @hoy), 30000.00, 'Señado'),
(2, 3, @h20, DATEADD(DAY,  1, @hoy), 45000.00, 'Pagado'),
(3, 2, @h19, DATEADD(DAY,  2, @hoy), 30000.00, 'Cancelada'),
(1, 1, @h21, DATEADD(DAY, -3, @hoy), 30000.00, 'Pagado');

INSERT INTO PAGOS (IdReserva, FechaPago, Importe, MetodoPago) VALUES
(1, @hoy,                    4500.00, 'Efectivo'),
(2, DATEADD(DAY, -2, @hoy), 10000.00, 'Transferencia'),
(2, @hoy,                   35000.00, 'Efectivo'),
(3, DATEADD(DAY, -1, @hoy),  4500.00, 'Tarjeta'),
(4, DATEADD(DAY, -3, @hoy), 30000.00, 'Efectivo');
GO
