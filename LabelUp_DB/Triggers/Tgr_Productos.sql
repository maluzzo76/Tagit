CREATE TRIGGER [dbo].[trg_Productos]
ON [dbo].[tbl_Productos]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
	declare @id int = (SELECT i.id FROM inserted i)

	insert into tbl_Productos_Plantillas
	select @id,id, 0,0,0 from tbl_Plantillas
End;
