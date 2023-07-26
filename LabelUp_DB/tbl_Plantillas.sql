CREATE TABLE [dbo].[tbl_Plantillas]
(
	[Id]                 INT PRIMARY KEY  IDENTITY (1, 1) NOT NULL,
    [Codigo]             VARCHAR (20)  NOT NULL,
    [Nombre]             VARCHAR (100) NOT NULL,
    [Descripcion]        VARCHAR (300) NULL,
    [Id_imagen]          INT           NULL,
    [Id_imagenPlantilla] INT           NULL,
    [Visible]            BIT           NULL,
    [Eliminado]          BIT           DEFAULT ((0)) NULL,
    FOREIGN KEY ([Id_imagen]) REFERENCES [dbo].[tbl_Imagenes] ([Id]),
    FOREIGN KEY ([Id_imagenPlantilla]) REFERENCES [dbo].[tbl_Imagenes]([Id])
)
