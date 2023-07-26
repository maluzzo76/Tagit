CREATE TRIGGER dbo.trg_Estampas
ON dbo.tbl_Estampas
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
	declare @id int = (SELECT i.id FROM inserted i)

	insert into tbl_Estampa_GrupoEstampa
	select id,@id, null, 0,0 from tbl_Grupo_Estampas

	insert into tbl_Plantillas_Estampas 
	select id,@id,1,0,0 from tbl_Plantillas
End;
GO