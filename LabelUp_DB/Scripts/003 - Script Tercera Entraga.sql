
ALTER VIEW [dbo].[v_grupoEstampaPlantilla]
AS
SELECT DISTINCT ge.id, ge.Codigo,ge.Nombre, ge.Descripcion,ge.Id_Imagen,pp.Activo Visible,pp.Eliminado, pe
.Id_Plantilla Id_Plantilla, i.URI, p.id Id_Producto
FROM tbl_Productos p
INNER JOIN tbl_Productos_Plantillas PP ON PP.Id_Producto = P.ID and pp.Activo = 1 and pp.Eliminado = 0
INNER JOIN tbl_plantillas_estampas pe ON pe.id_plantilla = PP.Id_Plantilla AND pe.Activo = 1 AND pe.Eliminado = 0
INNER JOIN tbl_Estampa_GrupoEstampa ege ON ege.Id_Estampa = pe.Id_Estampa AND ege.Activo = 1 AND ege.Eliminado = 0
INNER JOIN tbl_Grupo_Estampas ge ON ge.id = ege.Id_GrupoEstampa AND ge.Visible = 1 AND ge.Eliminado = 0
INNER JOIN tbl_Imagenes i on i.Id = ge.Id_Imagen
where p.Visible = 1 and p.Eliminado = 0
GO

ALTER TABLE DBO.tbl_pedidos
add Facturacion_Nombre_Apellido varchar(max),
	Facturacion_Piso varchar(100),
	Facturacion_Dto varchar(100),
	Facturacion_CP varchar(100),
	Entrga_Nombre_Apellido varchar(max),
	Entrega_Piso varchar(100),
	Entrega_Dto varchar(100),
	Entrega_CP varchar(100)
GO	