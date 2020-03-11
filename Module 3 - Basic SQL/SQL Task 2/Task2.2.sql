USE Northwind
GO

--1. Use the Orders table to find the number of orders grouped by year. 
--In the query results, you should return two columns with the names Year and Total. 
--Write a verification query that calculates the number of all orders

SELECT
  DATEPART(YEAR, O.OrderDate) AS 'Year',
  COUNT(*) as 'Total'
FROM Orders AS O
GROUP BY DATEPART(YEAR, O.OrderDate)

--2. Use the Orders table to find the number of orders made by each seller. 
--An order for a specified seller is any entry in the Orders table where 
--the EmployeeID column is set to a value for this seller. 
--In the query results, you should return a column with the seller's name 
--(the name obtained by the concatenation LastName & FirstName Should be displayed. 
--This LastName & FirstName string must be obtained by a separate query in the main 
--query column. Also, the main query must use grouping by EmployeeID.) with 
--the column name 'Seller’ and the column with the number of orders to return 
--with the name 'Amount'. The results of the query should be ordered in descending order
--of the number of orders

SELECT
  (SELECT 
    Emp.LastName + ' ' + Emp.FirstName 
   FROM Employees AS Emp 
   WHERE Emp.EmployeeID = O.EmployeeID
   ) AS 'Seller',
   COUNT(*) AS 'Amount'
FROM Orders AS O
JOIN Employees AS E
  ON E.EmployeeID = O.EmployeeID
GROUP BY O.EmployeeID

--3. Use the Orders table to find the number of orders made by each seller 
--and for each buyer. This should only be defined for orders made in 1998.

SELECT
  (SELECT 
    Emp.LastName + ' ' + Emp.FirstName 
   FROM Employees AS Emp 
   WHERE Emp.EmployeeID = O.EmployeeID
   ) AS 'Seller',
   (SELECT
      C.ContactName
    FROM Customers AS C
    WHERE C.CustomerID = O.CustomerID
    ) AS 'Customer',
    COUNT(*) AS 'Orders Count'
FROM Orders AS O
WHERE DATEPART(YEAR, O.OrderDate) = 1998
GROUP BY O.EmployeeID,
         O.CustomerID
ORDER BY O.EmployeeID,
         O.CustomerID

--4. Find buyers and sellers who live in the same city. 
--If only one or more sellers live in a city , or only one or more buyers 
--live in a city, then information about such buyers and sellers should not 
--be included in the result set. Do not use the JOIN construct.

SELECT
  C.ContactName AS 'Customer'
FROM Customers AS C
WHERE C.City IN (
  SELECT E.City FROM Employees AS E WHERE E.City = C.City
)

--5. Find all buyers who live in the same city.

SELECT
  C.ContactName AS 'Customer'
FROM Customers AS C
where C.City IN (
  SELECT AC.City FROM Customers AS AC WHERE AC.City = C.City and AC.CustomerID <> C.CustomerID
)
ORDER BY C.City

--6. Use the Employees table to find the Manager for each salesperson.

SELECT
  E.LastName + ' ' + E.FirstName AS 'Seller',
  B.LastName + ' ' + B.FirstName AS 'Boss'
FROM Employees AS E
LEFT JOIN Employees AS B 
  ON E.ReportsTo = B.EmployeeID