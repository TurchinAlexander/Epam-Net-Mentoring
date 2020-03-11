USE Northwind
GO

--1. Identify sellers who serve the 'Western' region (region table)

SELECT DISTINCT
  E.LastName + ' ' + E.FirstName AS 'Employee Name'
FROM Employees AS E
JOIN EmployeeTerritories AS ET
  ON ET.EmployeeID = E.EmployeeID
JOIN Territories AS T
  ON T.TerritoryID = ET.TerritoryID
JOIN Region AS R
  ON R.RegionID = T.RegionID
WHERE R.RegionDescription LIKE '%Western%'

--2.Output the names of all customers from the Customers table and the total number 
--of their orders from the Orders table in the query results . 
--Note that some customers do not have orders, but they must also be displayed 
--in the query results. Order the results of the query by increasing the number 
--of orders.

SELECT
  C.ContactName AS 'Customer',
  COUNT(O.CustomerID) AS 'Total Orders'
FROM Customers AS C
LEFT JOIN Orders AS O
  ON O.CustomerID = C.CustomerID
GROUP BY C.ContactName
ORDER BY COUNT(O.CustomerID)