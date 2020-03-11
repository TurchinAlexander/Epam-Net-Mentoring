USE Northwind
GO

--1.Select all customers living in the USA and Canada from the Customers table .
--Make a request using only the IN operator . Return columns with the user name 
--and country name in the query results. 
--To sort the query results on behalf of clients and place of residence.

SELECT
  C.ContactName,
  C.Country
FROM Customers AS C
WHERE C.Country IN ('USA', 'Canada')
ORDER BY C.ContactName,
         C.Address

--2. Select from the Customers table all customers who do not live in the USA and Canada. 
--Make a request using the IN operator . Return columns with the user name 
--and country name in the query results. 
--To organize the results of the query named customers.
SELECT
  C.ContactName,
  C.Country
FROM Customers AS C
WHERE C.Country NOT IN ('USA', 'Canada')
ORDER BY C.ContactName

--3. Select all countries where customers live from the Customers table. 
--The country must be mentioned only once and the list is sorted in descending order. 
--Do not use the GROUP BY clause. Return only one column in query results

SELECT DISTINCT
  C.Country
FROM Customers AS C
ORDER BY C.Country DESC