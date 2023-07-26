CREATE TRIGGER dbo.trg_Plantillas
ON dbo.tbl_Plantillas
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
	declare @id int = (SELECT i.id FROM inserted i)

	insert into tbl_Productos_Plantillas
	select id,@id,0, 0,0 from tbl_Productos

	insert into tbl_Plantillas_Estampas 
	select @id,id,1,0,0 from tbl_Estampas
End;
GO