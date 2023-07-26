CREATE TRIGGER dbo.trg_Pedidos
ON dbo.tbl_Pedidos
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
	declare @id int = (SELECT i.Id FROM inserted i), @key uniqueidentifier = (SELECT i.keyUser FROM inserted i)

	update tbl_cart set Id_Pedido = @id where keyUser = @key
End;
GO