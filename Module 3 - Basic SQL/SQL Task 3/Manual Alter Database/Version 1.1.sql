USE Northwind
GO

IF (NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'EmployeeCard'))
BEGIN
  CREATE TABLE dbo.EmployeeCard 
  (
    EmployeeCardId INT IDENTITY(1, 1) PRIMARY KEY,
    CardNumber VARCHAR(16) NOT NULL,
    EmployeeId INT NOT NULL REFERENCES dbo.Employees(EmployeeId),
    CreatedDate DATETIMEOFFSET NOT NULL,
    ExpiredDate DATETIMEOFFSET NOT NULL
  )
END

