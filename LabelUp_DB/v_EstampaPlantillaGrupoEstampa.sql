CREATE VIEW dbo.v_EstampaPlantillaGrupoEstampa
AS
SELECT DISTINCT e.*,ege.Id_GrupoEstampa, i.URI ImagenEstampa, ipl.URI ImagenPlantillaEstampa, pe.Id_Plantilla
FROM tbl_plantillas_estampas pe 
INNER JOIN tbl_Estampa_GrupoEstampa ege ON ege.Id_Estampa = pe.Id_Estampa AND ege.Activo = 1 AND ege.Eliminado = 0
INNER JOIN tbl_Estampas e ON e.id = ege.Id_Estampa and e.Visible = 1 and e.Eliminado = 0
INNER JOIN tbl_Imagenes i on i.Id = e.Id_Imagen
INNER JOIN tbl_Imagenes ipl on ipl.Id = pe.Id_ImagenPlantilla 
where pe.Activo = 1 AND pe.Eliminado = 0