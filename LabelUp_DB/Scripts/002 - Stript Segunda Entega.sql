CREATE TABLE [dbo].[tbl_Envios_Parametros](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Codigo_Postal] [varchar](50) NULL,
	[Localidad] [varchar](100) NULL,
	[Eliminado] bit default((0)),
	[Costo_Envio] decimal(18,4) default((0))
	)
GO

alter table dbo.tbl_Pedidos
add FOREIGN KEY ([Id_Envio]) REFERENCES [dbo].[tbl_Envios_Parametros]([Id])
GO

INSERT INTO tbl_Estados_Pedido (estado) values('Pendiente de pago')
GO

ALTER TABLE [dbo].[tbl_Pedidos]
	add
	MedioPago varchar(100),
	Direccion_Facturacion varchar(max),
	Direccion_Entrega	varchar(max)
GO

ALTER TABLE dbo.tbl_Productos
	ADD Is_Destacado BIT
GO