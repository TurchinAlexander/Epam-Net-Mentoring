USE Northwind
GO

--1. Select orders in the Orders table that were delivered after may 6, 1998 
--(ShippedDate column), inclusive, and that were delivered with ShipVia >= 2. 
--The request should only return the OrderID, ShippedDate, and ShipVia columns .

SELECT
  O.OrderID AS 'OrderID'
 ,O.ShippedDate AS 'ShippedDate'
 ,O.ShipVia AS 'ShipVia'
FROM Orders AS O
WHERE O.ShippedDate > '06-05-1998'
AND O.ShipVia >= 2

--2. Write a query that outputs only undelivered orders from the Orders table . 
--In the query results, return the string ‘Not Shipped ’ for the ShippedDate column 
--instead of NULL values (use the CAST system function ). 
--The request should only return the OrderID and ShippedDate columns

SELECT
  O.OrderID AS 'OrderID'
 ,CASE WHEN O.ShippedDate IS NULL THEN 'Not Shipped' END AS 'ShippedDate'
FROM Orders AS O
WHERE O.ShippedDate IS NULL

--3. Select orders in the Orders table that were delivered after may 6, 1998 
--(ShippedDate), not including this date, or that have not yet been delivered. 
--Only the OrderID (rename to Order Number) 
--and ShippedDate (rename to Shipped Date) columns should be returned in the request . 
--In the query results, return the string ‘Not Shipped ’ for the ShippedDate column 
--instead of NULL values, and return the date in the default format 
--for the other values

SELECT
  O.OrderID AS 'Order Number'
 ,CASE WHEN O.ShippedDate IS NULL THEN 'Not Shipped' ELSE O.ShippedDate END AS 'Shipped Date'
FROM Orders AS O
WHERE O.ShippedDate > '06-05-1998'
      OR o.ShippedDate IS NULL
