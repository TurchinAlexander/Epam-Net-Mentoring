USE Northwind
GO

IF (EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Region')
    AND NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Regions'))
BEGIN
  EXEC sp_rename 'Region', 'Regions';
END

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'FoundationDate' AND OBJECT_ID = OBJECT_ID(N'dbo.Customers'))
BEGIN
  ALTER TABLE dbo.Customers
    ADD FoundationDate datetimeoffset null 
END