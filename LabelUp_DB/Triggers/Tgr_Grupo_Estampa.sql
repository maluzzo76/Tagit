CREATE TRIGGER dbo.trg_Grupo_Estampas
ON dbo.tbl_Grupo_Estampas
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
	declare @id int = (SELECT i.id FROM inserted i)

	insert into tbl_Estampa_GrupoEstampa
	select @id,id, null, 0,0 from tbl_Estampas
End;
GO