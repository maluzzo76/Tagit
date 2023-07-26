CREATE TABLE [dbo].[tbl_Envios_Parametros](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Codigo_Postal] [varchar](50) NULL,
	[Localidad] [varchar](100) NULL,
	[Eliminado] bit default((0)),
	[Costo_Envio] decimal(18,4) default((0))
	)