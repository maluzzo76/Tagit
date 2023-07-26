CREATE TABLE [dbo].[tbl_Envios_Parametros]
(
	[Id] INT NOT NULL identity(1,1) PRIMARY KEY,
	[Codigo_Postal] varchar(50),
	[Localidad] varchar(100)
)