create table dbo.tbl_grupoImagen
(
	Id int identity(1,1) primary key,
	codigo varchar(3),
	Nombre varchar(50)
)
GO

insert into tbl_grupoImagen values('1','Estampa')
insert into tbl_grupoImagen values('2','Grupo_Estampa')
insert into tbl_grupoImagen values('3','Plantilla')
insert into tbl_grupoImagen values('4','Plantilla Estampa')
insert into tbl_grupoImagen values('5','Producto')
GO

ALTER TABLE tbl_imagenes
ADD [Id_GrupoImagen] int,
 FOREIGN KEY ([Id_GrupoImagen]) REFERENCES [dbo].[tbl_grupoImagen] ([Id])
GO

update tbl_Imagenes set id_grupoIMagen = 1
GO