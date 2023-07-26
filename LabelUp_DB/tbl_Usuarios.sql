CREATE TABLE [dbo].[tbl_Usuarios]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Email] VARCHAR(100) NOT NULL,
	[Activo] BIT default ((1)),
	[Is_Admin] BIT default ((0))
)
