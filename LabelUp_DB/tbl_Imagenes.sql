CREATE TABLE [dbo].[tbl_Imagenes]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Nombre] VARCHAR(50) NOT NULL,
	[Actio] BIT DEFAULT(0),
	[URI] VARCHAR(MAX) NOT NULL,
	[Tamanio] INT,
	[Extencion] VARCHAR(20),
	[Id_GrupoImagen] int,
	FOREIGN KEY ([Id_GrupoImagen]) REFERENCES [dbo].[tbl_grupoImagen] ([Id]),
)
