﻿CREATE TABLE [dbo].[tbl_Pedidos]
(
	Id INT identity(1,1) primary key,
	Fecha datetime,
	Id_Usuario int,
	Id_Envio int,
	Id_Estado int,
	Total decimal(18,4),
	keyUser uniqueidentifier,
	MedioPago varchar(100),
	Direccion_Facturacion varchar(max),
	Direccion_Entrega	varchar(max),
	Facturacion_Nombre_Apellido varchar(max),
	Facturacion_Piso varchar(100),
	Facturacion_Dto varchar(100),
	Facturacion_CP varchar(100),
	Entrga_Nombre_Apellido varchar(max),
	Entrega_Piso varchar(100),
	Entrega_Dto varchar(100),
	Entrega_CP varchar(100),
	MercadoPagoId varchar(100), 
	MercadoPagoStatus varchar(100),
	FOREIGN KEY ([Id_Usuario]) REFERENCES [dbo].[tbl_Usuarios] ([Id]),
	FOREIGN KEY ([Id_Estado]) REFERENCES [dbo].[tbl_Estados_Pedido] ([Id]),
	FOREIGN KEY ([Id_Envio]) REFERENCES [dbo].[tbl_Envios_Parametros]([Id])
)