CREATE TABLE [dbo].[tbl_Plantillas_Estampas]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Id_Plantilla] INT NOT NULL,
	[Id_Estampa] INT NOT NULL,
	[Id_ImagenPlantilla] INT NOT NULL,
	[Activo] BIT DEFAULT((0)) NOT NULL,
	[Eliminado] BIT DEFAULT((0)) NOT NULL,
	FOREIGN KEY ([Id_ImagenPlantilla]) REFERENCES [dbo].[tbl_Imagenes](Id),
	FOREIGN KEY ([Id_Plantilla]) REFERENCES [dbo].[tbl_Plantillas](Id),
	FOREIGN KEY ([Id_Estampa]) REFERENCES [dbo].[tbl_Estampas](Id)
)
