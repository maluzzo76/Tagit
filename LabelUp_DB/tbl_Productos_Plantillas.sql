CREATE TABLE [dbo].[tbl_Productos_Plantillas]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Id_Producto] INT NOT NULL,
	[Id_Plantilla] INT NOT NULL,
	Activo BIT DEFAULT((0)) NOT NULL,
	Eliminado BIT DEFAULT((0)) NOT NULL,
	Precio decimal(18,4) default((0)),
	FOREIGN KEY (Id_Producto) REFERENCES [dbo].[tbl_Productos](Id),
	FOREIGN KEY (Id_Plantilla) REFERENCES [dbo].[tbl_Plantillas](Id)
)
