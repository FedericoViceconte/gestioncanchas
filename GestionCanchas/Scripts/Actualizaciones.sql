-- Actualizaciones del esquema posteriores a GestionCanchas.sql.
-- La aplicación las aplica sola al iniciar. Cada bloque se puede ejecutar más de una vez.

IF OBJECT_ID('dbo.USUARIOS', 'U') IS NULL
BEGIN
    CREATE TABLE USUARIOS (
        IdUsuario      INT IDENTITY(1,1) NOT NULL,
        NombreUsuario  VARCHAR(30)  NOT NULL,
        NombreCompleto VARCHAR(80)  NOT NULL,
        ClaveHash      VARCHAR(200) NOT NULL,
        Rol            VARCHAR(20)  NOT NULL,
        Activo         BIT          NOT NULL CONSTRAINT DF_USUARIOS_Activo DEFAULT 1,
        FechaAlta      DATETIME2(0) NOT NULL CONSTRAINT DF_USUARIOS_FechaAlta DEFAULT SYSDATETIME(),
        UltimoAcceso   DATETIME2(0) NULL,
        CONSTRAINT PK_USUARIOS PRIMARY KEY (IdUsuario),
        CONSTRAINT UQ_USUARIOS_NombreUsuario UNIQUE (NombreUsuario),
        CONSTRAINT CK_USUARIOS_Rol CHECK (Rol IN ('Administrador', 'Recepcionista'))
    );
END
GO

IF COL_LENGTH('dbo.PAGOS', 'IdUsuario') IS NULL
BEGIN
    ALTER TABLE PAGOS ADD IdUsuario INT NULL
        CONSTRAINT FK_PAGOS_USUARIOS FOREIGN KEY REFERENCES USUARIOS (IdUsuario);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PAGOS_FechaPago' AND object_id = OBJECT_ID('dbo.PAGOS'))
BEGIN
    CREATE INDEX IX_PAGOS_FechaPago ON PAGOS (FechaPago) INCLUDE (Importe, MetodoPago, Tipo, Anulado);
END
GO
