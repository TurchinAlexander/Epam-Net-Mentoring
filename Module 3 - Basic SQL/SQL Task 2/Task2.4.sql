USE Northwind
GO

--1. Issue all suppliers ( the CompanyName column in the Suppliers table ) that do not have
--at least one product in stock (UnitsInStock in the Products table is 0).
--Use a nested SELECT for this query using the IN operator

SELECT
  S.CompanyName AS 'Company'
FROM Suppliers AS S
WHERE S.SupplierID IN (
  SELECT 
    P.SupplierID
  FROM Products AS P 
  GROUP BY P.SupplierID
  HAVING MIN(P.UnitsInStock) = 0
)

--2. Issue all sellers who have more than 150 orders. Use nested SELECT

SELECT
  E.LastName + ' ' + E.FirstName AS 'Seller'
FROM Employees AS E
WHERE E.EmployeeID IN (
  SELECT
    O.EmployeeID
  FROM Orders AS O
  GROUP BY O.EmployeeID
  HAVING COUNT(O.OrderID) > 150
)

--3. Issue all customers ( the Customers table) that do not have any orders 
--( for a query in the Orders table ). You use the EXISTS operator.

SELECT
  *
FROM Customers AS C
WHERE NOT EXISTS(
  SELECT
    *
  FROM Orders AS O
  WHERE O.CustomerID = C.CustomerID
)