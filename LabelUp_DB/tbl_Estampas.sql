CREATE TABLE [dbo].[tbl_Estampas]
(
	[Id]        INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [Codigo]    VARCHAR (20)  NOT NULL,
    [Nombre]    VARCHAR (100) NOT NULL,
    [Descripcion] VARCHAR(MAX) NULL,
    [Id_Imagen] INT           NULL,
    [Visible]   BIT           NULL,
    [Eliminado] BIT           DEFAULT ((0)) NULL,
    FOREIGN KEY ([Id_Imagen]) REFERENCES [dbo].[tbl_Imagenes] ([Id])
)
