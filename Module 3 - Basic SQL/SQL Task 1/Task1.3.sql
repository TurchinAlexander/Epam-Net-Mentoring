USE Northwind
GO

--1. Select all orders (Order ID) from the Order Details table 
--(orders must not be repeated), where there are products with quantities from 3 to 10 
--inclusive – this is the Quantity column in the Order Details table . 
--Use the BETWEEN operator. The query should only return the OrderID column .

SELECT DISTINCT
  OD.OrderID
FROM [Order Details] AS OD
WHERE OD.Quantity BETWEEN 3 AND 10

 --2. Select all customers from the Customers table whose country name begins with letters 
 --from the range and and p. Use the BETWEEN operator. Check that Germany is included 
 --in the query results . The query should return only the CustomerID 
 --and Country columns and is sorted by Country.

SELECT
  C.CustomerID,
  C.Country
FROM Customers AS C
WHERE LEFT(C.Country, 1) BETWEEN 'b' AND 'g'
ORDER BY C.Country

--3. Select all customers from the Customers table whose country name begins with letters 
--from the range b and g, without using the BETWEEN operator.

SELECT
  C.CustomerID,
  C.Country
FROM Customers AS C
WHERE LEFT(C.Country, 1) IN ('b', 'c', 'd', 'e', 'f', 'g')
ORDER BY C.Country

SELECT
  C.CustomerID,
  C.Country
FROM Customers AS C
WHERE LEFT(C.Country, 1) >= 'b'
      AND LEFT(C.Country, 1) <= 'g'
ORDER BY C.Country