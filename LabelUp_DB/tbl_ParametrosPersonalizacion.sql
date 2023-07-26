CREATE TABLE dbo.tbl_PrametrosPersonalizacion
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Id_Plantilla INT,
	X INT,
	Y INT,
	W INT,
	H INT,
	CuadroTexto INT,
	Zoom INT,
	FOREIGN KEY (Id_Plantilla) REFERENCES dbo.tbl_Plantillas(Id)
)
