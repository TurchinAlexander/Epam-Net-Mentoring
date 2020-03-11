USE Northwind
GO

--In the Products table, find all products (ProductName column) where the 'chocolate' 
--substring occurs . It is known that in the substring 'chocolate' one letter 'c' 
--in the middle can be changed - find all products that satisfy this condition

SELECT
  *
FROM Products AS P
WHERE P.ProductName LIKE '%cho_olade%'