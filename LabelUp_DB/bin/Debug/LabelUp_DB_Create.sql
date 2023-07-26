/*
Script de implementación para LabelUp_DB

Una herramienta generó este código.
Los cambios realizados en este archivo podrían generar un comportamiento incorrecto y se perderán si
se vuelve a generar el código.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "LabelUp_DB"
:setvar DefaultFilePrefix "LabelUp_DB"
:setvar DefaultDataPath ""
:setvar DefaultLogPath ""

GO
:on error exit
GO
/*
Detectar el modo SQLCMD y deshabilitar la ejecución del script si no se admite el modo SQLCMD.
Para volver a habilitar el script después de habilitar el modo SQLCMD, ejecute lo siguiente:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'El modo SQLCMD debe estar habilitado para ejecutar correctamente este script.';
        SET NOEXEC ON;
    END


GO
USE [master];


GO

IF (DB_ID(N'$(DatabaseName)') IS NOT NULL) 
BEGIN
    ALTER DATABASE [$(DatabaseName)]
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$(DatabaseName)];
END

GO
PRINT N'Creando la base de datos $(DatabaseName)...'
GO
CREATE DATABASE [$(DatabaseName)] COLLATE SQL_Latin1_General_CP1_CI_AS
GO
USE [$(DatabaseName)];


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                NUMERIC_ROUNDABORT OFF,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL,
                RECOVERY FULL,
                CURSOR_CLOSE_ON_COMMIT OFF,
                AUTO_CREATE_STATISTICS ON,
                AUTO_SHRINK OFF,
                AUTO_UPDATE_STATISTICS ON,
                RECURSIVE_TRIGGERS OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ALLOW_SNAPSHOT_ISOLATION OFF;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET READ_COMMITTED_SNAPSHOT OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_UPDATE_STATISTICS_ASYNC OFF,
                PAGE_VERIFY NONE,
                DATE_CORRELATION_OPTIMIZATION OFF,
                DISABLE_BROKER,
                PARAMETERIZATION SIMPLE,
                SUPPLEMENTAL_LOGGING OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET TRUSTWORTHY OFF,
        DB_CHAINING OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'No se puede modificar la configuración de la base de datos. Debe ser un administrador del sistema para poder aplicar esta configuración.';
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET HONOR_BROKER_PRIORITY OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'No se puede modificar la configuración de la base de datos. Debe ser un administrador del sistema para poder aplicar esta configuración.';
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET TARGET_RECOVERY_TIME = 0 SECONDS 
    WITH ROLLBACK IMMEDIATE;


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET FILESTREAM(NON_TRANSACTED_ACCESS = OFF),
                CONTAINMENT = NONE 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_CREATE_STATISTICS ON(INCREMENTAL = OFF),
                MEMORY_OPTIMIZED_ELEVATE_TO_SNAPSHOT = OFF,
                DELAYED_DURABILITY = DISABLED 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET QUERY_STORE (QUERY_CAPTURE_MODE = ALL, DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_PLANS_PER_QUERY = 200, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 367), MAX_STORAGE_SIZE_MB = 100) 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET QUERY_STORE = OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
        ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
        ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
        ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
        ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
        ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
        ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
        ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
    END


GO
IF fulltextserviceproperty(N'IsFulltextInstalled') = 1
    EXECUTE sp_fulltext_database 'enable';


GO
PRINT N'Creando Tabla [dbo].[tbl_Envios_Parametros]...';


GO
CREATE TABLE [dbo].[tbl_Envios_Parametros] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [Codigo_Postal] VARCHAR (50)    NULL,
    [Localidad]     VARCHAR (100)   NULL,
    [Eliminado]     BIT             NULL,
    [Costo_Envio]   DECIMAL (18, 4) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[tbl_PrametrosPersonalizacion]...';


GO
CREATE TABLE [dbo].[tbl_PrametrosPersonalizacion] (
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [Id_Plantilla] INT NULL,
    [X]            INT NULL,
    [Y]            INT NULL,
    [W]            INT NULL,
    [H]            INT NULL,
    [CuadroTexto]  INT NULL,
    [Zoom]         INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[tbl_Estados_Pedido]...';


GO
CREATE TABLE [dbo].[tbl_Estados_Pedido] (
    [Id]     INT           IDENTITY (1, 1) NOT NULL,
    [Estado] VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[tbl_cart]...';


GO
CREATE TABLE [dbo].[tbl_cart] (
    [Id]             INT              IDENTITY (1, 1) NOT NULL,
    [Fecha]          DATETIME         NULL,
    [Id_Estado]      INT              NULL,
    [Id_Usuario]     INT              NULL,
    [keyUser]        UNIQUEIDENTIFIER NULL,
    [Id_Producto]    INT              NULL,
    [Id_Plantilla]   INT              NULL,
    [Id_Estampa]     INT              NULL,
    [Param1]         VARCHAR (200)    NULL,
    [Param2]         VARCHAR (200)    NULL,
    [Param3]         VARCHAR (200)    NULL,
    [Param4]         VARCHAR (200)    NULL,
    [ParamSize1]     INT              NULL,
    [ParamSize2]     INT              NULL,
    [ParamSize3]     INT              NULL,
    [ParamSize4]     INT              NULL,
    [FontName]       VARCHAR (100)    NULL,
    [FontColor]      VARCHAR (50)     NULL,
    [PrecioUnitario] DECIMAL (18, 2)  NULL,
    [Cantidad]       DECIMAL (18, 2)  NULL,
    [Total]          DECIMAL (18, 2)  NULL,
    [Id_Pedido]      INT              NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[tbl_Usuarios]...';


GO
CREATE TABLE [dbo].[tbl_Usuarios] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Email]    VARCHAR (100) NOT NULL,
    [Activo]   BIT           NULL,
    [Is_Admin] BIT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[tbl_Estampa_GrupoEstampa]...';


GO
CREATE TABLE [dbo].[tbl_Estampa_GrupoEstampa] (
    [Id]              INT IDENTITY (1, 1) NOT NULL,
    [Id_GrupoEstampa] INT NOT NULL,
    [Id_Estampa]      INT NOT NULL,
    [Id_Imagen]       INT NULL,
    [Activo]          BIT NOT NULL,
    [Eliminado]       BIT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[tbl_Productos_Plantillas]...';


GO
CREATE TABLE [dbo].[tbl_Productos_Plantillas] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [Id_Producto]  INT             NOT NULL,
    [Id_Plantilla] INT             NOT NULL,
    [Activo]       BIT             NOT NULL,
    [Eliminado]    BIT             NOT NULL,
    [Precio]       DECIMAL (18, 4) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[tbl_Productos]...';


GO
CREATE TABLE [dbo].[tbl_Productos] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Codigo]      VARCHAR (20)  NOT NULL,
    [Nombre]      VARCHAR (100) NOT NULL,
    [Descripcion] VARCHAR (MAX) NULL,
    [Id_Imagen]   INT           NULL,
    [Visible]     BIT           NULL,
    [Eliminado]   BIT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[tbl_Plantillas_Estampas]...';


GO
CREATE TABLE [dbo].[tbl_Plantillas_Estampas] (
    [Id]                 INT IDENTITY (1, 1) NOT NULL,
    [Id_Plantilla]       INT NOT NULL,
    [Id_Estampa]         INT NOT NULL,
    [Id_ImagenPlantilla] INT NOT NULL,
    [Activo]             BIT NOT NULL,
    [Eliminado]          BIT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[tbl_Grupo_Estampas]...';


GO
CREATE TABLE [dbo].[tbl_Grupo_Estampas] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Codigo]      VARCHAR (20)  NOT NULL,
    [Nombre]      VARCHAR (100) NOT NULL,
    [Descripcion] VARCHAR (MAX) NULL,
    [Id_Imagen]   INT           NULL,
    [Visible]     BIT           NULL,
    [Eliminado]   BIT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[tbl_Estampas]...';


GO
CREATE TABLE [dbo].[tbl_Estampas] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Codigo]      VARCHAR (20)  NOT NULL,
    [Nombre]      VARCHAR (100) NOT NULL,
    [Descripcion] VARCHAR (MAX) NULL,
    [Id_Imagen]   INT           NULL,
    [Visible]     BIT           NULL,
    [Eliminado]   BIT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[tbl_Plantillas]...';


GO
CREATE TABLE [dbo].[tbl_Plantillas] (
    [Id]                 INT           IDENTITY (1, 1) NOT NULL,
    [Codigo]             VARCHAR (20)  NOT NULL,
    [Nombre]             VARCHAR (100) NOT NULL,
    [Descripcion]        VARCHAR (300) NULL,
    [Id_imagen]          INT           NULL,
    [Id_imagenPlantilla] INT           NULL,
    [Visible]            BIT           NULL,
    [Eliminado]          BIT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[tbl_grupoImagen]...';


GO
CREATE TABLE [dbo].[tbl_grupoImagen] (
    [Id]     INT          IDENTITY (1, 1) NOT NULL,
    [codigo] VARCHAR (3)  NULL,
    [Nombre] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[tbl_Imagenes]...';


GO
CREATE TABLE [dbo].[tbl_Imagenes] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]         VARCHAR (50)  NOT NULL,
    [Actio]          BIT           NULL,
    [URI]            VARCHAR (MAX) NOT NULL,
    [Tamanio]        INT           NULL,
    [Extencion]      VARCHAR (20)  NULL,
    [Id_GrupoImagen] INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[tbl_Pedidos]...';


GO
CREATE TABLE [dbo].[tbl_Pedidos] (
    [Id]                          INT              IDENTITY (1, 1) NOT NULL,
    [Fecha]                       DATETIME         NULL,
    [Id_Usuario]                  INT              NULL,
    [Id_Envio]                    INT              NULL,
    [Id_Estado]                   INT              NULL,
    [Total]                       DECIMAL (18, 4)  NULL,
    [keyUser]                     UNIQUEIDENTIFIER NULL,
    [MedioPago]                   VARCHAR (100)    NULL,
    [Direccion_Facturacion]       VARCHAR (MAX)    NULL,
    [Direccion_Entrega]           VARCHAR (MAX)    NULL,
    [Facturacion_Nombre_Apellido] VARCHAR (MAX)    NULL,
    [Facturacion_Piso]            VARCHAR (100)    NULL,
    [Facturacion_Dto]             VARCHAR (100)    NULL,
    [Facturacion_CP]              VARCHAR (100)    NULL,
    [Entrga_Nombre_Apellido]      VARCHAR (MAX)    NULL,
    [Entrega_Piso]                VARCHAR (100)    NULL,
    [Entrega_Dto]                 VARCHAR (100)    NULL,
    [Entrega_CP]                  VARCHAR (100)    NULL,
    [MercadoPagoId]               VARCHAR (100)    NULL,
    [MercadoPagoStatus]           VARCHAR (100)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[tbl_Envios_Parametros]...';


GO
ALTER TABLE [dbo].[tbl_Envios_Parametros]
    ADD DEFAULT ((0)) FOR [Eliminado];


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[tbl_Envios_Parametros]...';


GO
ALTER TABLE [dbo].[tbl_Envios_Parametros]
    ADD DEFAULT ((0)) FOR [Costo_Envio];


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[tbl_Usuarios]...';


GO
ALTER TABLE [dbo].[tbl_Usuarios]
    ADD DEFAULT ((1)) FOR [Activo];


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[tbl_Usuarios]...';


GO
ALTER TABLE [dbo].[tbl_Usuarios]
    ADD DEFAULT ((0)) FOR [Is_Admin];


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[tbl_Estampa_GrupoEstampa]...';


GO
ALTER TABLE [dbo].[tbl_Estampa_GrupoEstampa]
    ADD DEFAULT ((0)) FOR [Activo];


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[tbl_Estampa_GrupoEstampa]...';


GO
ALTER TABLE [dbo].[tbl_Estampa_GrupoEstampa]
    ADD DEFAULT ((0)) FOR [Eliminado];


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[tbl_Productos_Plantillas]...';


GO
ALTER TABLE [dbo].[tbl_Productos_Plantillas]
    ADD DEFAULT ((0)) FOR [Activo];


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[tbl_Productos_Plantillas]...';


GO
ALTER TABLE [dbo].[tbl_Productos_Plantillas]
    ADD DEFAULT ((0)) FOR [Eliminado];


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[tbl_Productos_Plantillas]...';


GO
ALTER TABLE [dbo].[tbl_Productos_Plantillas]
    ADD DEFAULT ((0)) FOR [Precio];


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[tbl_Productos]...';


GO
ALTER TABLE [dbo].[tbl_Productos]
    ADD DEFAULT ((0)) FOR [Eliminado];


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[tbl_Plantillas_Estampas]...';


GO
ALTER TABLE [dbo].[tbl_Plantillas_Estampas]
    ADD DEFAULT ((0)) FOR [Activo];


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[tbl_Plantillas_Estampas]...';


GO
ALTER TABLE [dbo].[tbl_Plantillas_Estampas]
    ADD DEFAULT ((0)) FOR [Eliminado];


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[tbl_Grupo_Estampas]...';


GO
ALTER TABLE [dbo].[tbl_Grupo_Estampas]
    ADD DEFAULT ((0)) FOR [Eliminado];


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[tbl_Estampas]...';


GO
ALTER TABLE [dbo].[tbl_Estampas]
    ADD DEFAULT ((0)) FOR [Eliminado];


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[tbl_Plantillas]...';


GO
ALTER TABLE [dbo].[tbl_Plantillas]
    ADD DEFAULT ((0)) FOR [Eliminado];


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[tbl_Imagenes]...';


GO
ALTER TABLE [dbo].[tbl_Imagenes]
    ADD DEFAULT (0) FOR [Actio];


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_PrametrosPersonalizacion]...';


GO
ALTER TABLE [dbo].[tbl_PrametrosPersonalizacion]
    ADD FOREIGN KEY ([Id_Plantilla]) REFERENCES [dbo].[tbl_Plantillas] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_cart]...';


GO
ALTER TABLE [dbo].[tbl_cart]
    ADD FOREIGN KEY ([Id_Producto]) REFERENCES [dbo].[tbl_Productos] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_cart]...';


GO
ALTER TABLE [dbo].[tbl_cart]
    ADD FOREIGN KEY ([Id_Plantilla]) REFERENCES [dbo].[tbl_Plantillas] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_cart]...';


GO
ALTER TABLE [dbo].[tbl_cart]
    ADD FOREIGN KEY ([Id_Estampa]) REFERENCES [dbo].[tbl_Estampas] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_cart]...';


GO
ALTER TABLE [dbo].[tbl_cart]
    ADD FOREIGN KEY ([Id_Usuario]) REFERENCES [dbo].[tbl_Usuarios] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_cart]...';


GO
ALTER TABLE [dbo].[tbl_cart]
    ADD FOREIGN KEY ([Id_Estado]) REFERENCES [dbo].[tbl_Estados_Pedido] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_cart]...';


GO
ALTER TABLE [dbo].[tbl_cart]
    ADD FOREIGN KEY ([Id_Pedido]) REFERENCES [dbo].[tbl_Pedidos] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_Estampa_GrupoEstampa]...';


GO
ALTER TABLE [dbo].[tbl_Estampa_GrupoEstampa]
    ADD FOREIGN KEY ([Id_GrupoEstampa]) REFERENCES [dbo].[tbl_Grupo_Estampas] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_Estampa_GrupoEstampa]...';


GO
ALTER TABLE [dbo].[tbl_Estampa_GrupoEstampa]
    ADD FOREIGN KEY ([Id_Estampa]) REFERENCES [dbo].[tbl_Estampas] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_Estampa_GrupoEstampa]...';


GO
ALTER TABLE [dbo].[tbl_Estampa_GrupoEstampa]
    ADD FOREIGN KEY ([Id_Imagen]) REFERENCES [dbo].[tbl_Imagenes] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_Productos_Plantillas]...';


GO
ALTER TABLE [dbo].[tbl_Productos_Plantillas]
    ADD FOREIGN KEY ([Id_Producto]) REFERENCES [dbo].[tbl_Productos] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_Productos_Plantillas]...';


GO
ALTER TABLE [dbo].[tbl_Productos_Plantillas]
    ADD FOREIGN KEY ([Id_Plantilla]) REFERENCES [dbo].[tbl_Plantillas] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_Productos]...';


GO
ALTER TABLE [dbo].[tbl_Productos]
    ADD FOREIGN KEY ([Id_Imagen]) REFERENCES [dbo].[tbl_Imagenes] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_Plantillas_Estampas]...';


GO
ALTER TABLE [dbo].[tbl_Plantillas_Estampas]
    ADD FOREIGN KEY ([Id_ImagenPlantilla]) REFERENCES [dbo].[tbl_Imagenes] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_Plantillas_Estampas]...';


GO
ALTER TABLE [dbo].[tbl_Plantillas_Estampas]
    ADD FOREIGN KEY ([Id_Plantilla]) REFERENCES [dbo].[tbl_Plantillas] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_Plantillas_Estampas]...';


GO
ALTER TABLE [dbo].[tbl_Plantillas_Estampas]
    ADD FOREIGN KEY ([Id_Estampa]) REFERENCES [dbo].[tbl_Estampas] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_Grupo_Estampas]...';


GO
ALTER TABLE [dbo].[tbl_Grupo_Estampas]
    ADD FOREIGN KEY ([Id_Imagen]) REFERENCES [dbo].[tbl_Imagenes] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_Estampas]...';


GO
ALTER TABLE [dbo].[tbl_Estampas]
    ADD FOREIGN KEY ([Id_Imagen]) REFERENCES [dbo].[tbl_Imagenes] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_Plantillas]...';


GO
ALTER TABLE [dbo].[tbl_Plantillas]
    ADD FOREIGN KEY ([Id_imagen]) REFERENCES [dbo].[tbl_Imagenes] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_Plantillas]...';


GO
ALTER TABLE [dbo].[tbl_Plantillas]
    ADD FOREIGN KEY ([Id_imagenPlantilla]) REFERENCES [dbo].[tbl_Imagenes] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_Imagenes]...';


GO
ALTER TABLE [dbo].[tbl_Imagenes]
    ADD FOREIGN KEY ([Id_GrupoImagen]) REFERENCES [dbo].[tbl_grupoImagen] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_Pedidos]...';


GO
ALTER TABLE [dbo].[tbl_Pedidos]
    ADD FOREIGN KEY ([Id_Usuario]) REFERENCES [dbo].[tbl_Usuarios] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_Pedidos]...';


GO
ALTER TABLE [dbo].[tbl_Pedidos]
    ADD FOREIGN KEY ([Id_Estado]) REFERENCES [dbo].[tbl_Estados_Pedido] ([Id]);


GO
PRINT N'Creando Clave externa restricción sin nombre en [dbo].[tbl_Pedidos]...';


GO
ALTER TABLE [dbo].[tbl_Pedidos]
    ADD FOREIGN KEY ([Id_Envio]) REFERENCES [dbo].[tbl_Envios_Parametros] ([Id]);


GO
PRINT N'Creando Vista [dbo].[v_EstampaPlantillaGrupoEstampa]...';


GO
CREATE VIEW dbo.v_EstampaPlantillaGrupoEstampa
AS
SELECT DISTINCT e.*,ege.Id_GrupoEstampa, i.URI ImagenEstampa, ipl.URI ImagenPlantillaEstampa, pe.Id_Plantilla
FROM tbl_plantillas_estampas pe 
INNER JOIN tbl_Estampa_GrupoEstampa ege ON ege.Id_Estampa = pe.Id_Estampa AND ege.Activo = 1 AND ege.Eliminado = 0
INNER JOIN tbl_Estampas e ON e.id = ege.Id_Estampa and e.Visible = 1 and e.Eliminado = 0
INNER JOIN tbl_Imagenes i on i.Id = e.Id_Imagen
INNER JOIN tbl_Imagenes ipl on ipl.Id = pe.Id_ImagenPlantilla 
where pe.Activo = 1 AND pe.Eliminado = 0
GO
PRINT N'Creando Vista [dbo].[v_grupoEstampaPlantilla]...';


GO
CREATE VIEW dbo.v_grupoEstampaPlantilla
AS
SELECT DISTINCT ge.id, ge.Codigo,ge.Nombre, ge.Descripcion,ge.Id_Imagen,pp.Activo Visible,pp.Eliminado, pe
.Id_Plantilla Id_Plantilla, i.URI, p.id Id_Producto
FROM tbl_Productos p
INNER JOIN tbl_Productos_Plantillas PP ON PP.Id_Producto = P.ID and pp.Activo = 1 and pp.Eliminado = 0
INNER JOIN tbl_plantillas_estampas pe ON pe.id_plantilla = PP.Id_Plantilla AND pe.Activo = 1 AND pe.Eliminado = 0
INNER JOIN tbl_Estampa_GrupoEstampa ege ON ege.Id_Estampa = pe.Id_Estampa AND ege.Activo = 1 AND ege.Eliminado = 0
INNER JOIN tbl_Grupo_Estampas ge ON ge.id = ege.Id_GrupoEstampa AND ge.Visible = 1 AND ge.Eliminado = 0
INNER JOIN tbl_Imagenes i on i.Id = ge.Id_Imagen
where p.Visible = 1 and p.Eliminado = 0
GO
DECLARE @VarDecimalSupported AS BIT;

SELECT @VarDecimalSupported = 0;

IF ((ServerProperty(N'EngineEdition') = 3)
    AND (((@@microsoftversion / power(2, 24) = 9)
          AND (@@microsoftversion & 0xffff >= 3024))
         OR ((@@microsoftversion / power(2, 24) = 10)
             AND (@@microsoftversion & 0xffff >= 1600))))
    SELECT @VarDecimalSupported = 1;

IF (@VarDecimalSupported > 0)
    BEGIN
        EXECUTE sp_db_vardecimal_storage_format N'$(DatabaseName)', 'ON';
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET MULTI_USER 
    WITH ROLLBACK IMMEDIATE;


GO
PRINT N'Actualización completada.';


GO
