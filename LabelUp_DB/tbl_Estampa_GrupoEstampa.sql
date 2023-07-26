CREATE TABLE [dbo].[tbl_Estampa_GrupoEstampa]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Id_GrupoEstampa] INT NOT NULL,
	[Id_Estampa] INT NOT NULL,
	[Id_Imagen] INT ,
	[Activo] BIT DEFAULT((0)) NOT NULL,
	[Eliminado] BIT DEFAULT((0)) NOT NULL,
	FOREIGN KEY ([Id_GrupoEstampa]) REFERENCES [dbo].[tbl_Grupo_Estampas](Id),
	FOREIGN KEY ([Id_Estampa]) REFERENCES [dbo].[tbl_Estampas](Id),
	FOREIGN KEY ([Id_Imagen]) REFERENCES [dbo].[tbl_Imagenes](Id)
)
