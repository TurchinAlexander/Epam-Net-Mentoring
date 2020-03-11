USE Northwind
GO

--1. Find the total amount of all orders from the Order Details table, 
--taking into account the number of purchased items and discounts on them. 
--The result of the query must be one record with one column with 
--the column name ' Totals '

SELECT
  SUM((1 - OD.Discount) * OD.Quantity * OD.UnitPrice) AS 'Totals'
FROM [Order Details] AS OD

--2. Use the Orders table to find the number of orders that have not yet been delivered 
--(i.e. there is no delivery date value in the ShippedDate column). 
--Use only the COUNT operator for this query . 
--Do not use the WHERE clause and the GROUP.

SELECT 
  COUNT(CASE WHEN O.ShippedDate IS NULL THEN 1 ELSE NULL END) as 'Not Shipped Count'
FROM Orders AS O

--3. Use the Orders table to find the number of different customers (CustomerID) 
--who made orders. 
--Use the COUNT function and do not use the WHERE and GROUP clauses

SELECT
  COUNT(DISTINCT O.CustomerID) AS 'Customer Count'
FROM Orders AS O

