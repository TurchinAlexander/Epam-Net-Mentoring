# Basic SQL

## To Start
To run the scripts we use Northwind Database which you can freely download from Microsoft site.

## Task 1. Create SQL scripts for seach and filter

### Task 1.1 Simple data filtration
1. Select orders in the Orders table that were delivered after may 6, 1998 (ShippedDate column), inclusive, and that were delivered with ShipVia >= 2.  
The request should only return the OrderID, ShippedDate, and ShipVia columns.

2. Write a query that outputs only undelivered orders from the Orders table.  
In the query results, return the string 'Not Shipped' for the ShippedDate column instead of NULL alies (use the CAST system function).  
The request should only return the OrderID and ShippedDate columns.

3. Select orders in the Orders table that were delivered after may 6, 1998 (ShippedDate), not including this date, or that have not yet been delivered.  
Only the OrderID (rename to Order Number) and ShippedDate (rename to Shipped Date) columns should be returned in the request.  
In the query results, return the string 'Not Shipped' for the ShippedDate column instead of NULL values,  
and return the date in the default format for the other values

### Task 1.2. Using IN, DISTICT, ORDER BY, NOT operators

1. Select all customers living in the USA and Canada from the Customers table.   
Make a request using only the IN operator.  
Return columns with the user name and country name in the query results.  
To sort the query results on behalf of clients and place of residence.

2. Select from the Customers table all customers who do not live in the USA and Canada.  
Make a request using the IN operator.  
Return columns with the user name and country name in the query results.  
To organize the results of the query named customers.

3. Select all countries where customers live from the Customers table.  
The country must be mentioned only once and the list is sorted in descending order.  
Do not use the GROUP BY clause. Return only one column in query results.

### Task 1.3 Using BETWEEN, DISTINCT operators

1. Select all orders (Order ID) from the Order Details table (orders must not be repeated),  
where there are products with quantities from 3 to 10 inclusive – this is the Quantity column in the Order Details table. 
Use the BETWEEN operator. The query should only return the OrderID column.

2. Select all customers from the Customers table whose country name begins with letters from the range 'b' and 'g'.   
Use the BETWEEN operator.  
Check that Germany is included in the query results.  
The query should return only the CustomerID and Country columns and is sorted by Country.

3. Select all customers from the Customers table whose country name begins with letters from the range 'b' and 'g',  
without using the BETWEEN operator.

### Task 1.4 Using LIKE operator

1. In the Products table, find all products (ProductName column) where the 'chocolate' substring occurs.  
It is known that in the substring 'chocolate' one letter 'c' in the middle can be changed.  
Find all products that satisfy this condition.

## Task 2. Create SQL scripts for join table and aggregation

### Task 2.1 Using aggregate functions (SUM, COUNT)

## Task 2.2 Join tables, using aggregate functions and GROUP BY, HAVING operators

1. Use the Orders table to find the number of orders grouped by year. 
In the query results, you should return two columns with the names Year and Total. 
Write a verification query that calculates the number of all orders.

2. Use the Orders table to find the number of orders made by each seller. 
An order for a specified seller is any entry in the Orders table where the EmployeeID column is set to a value for this seller.  
In the query results, you should return a column with the seller's name  
(the name obtained by the concatenation LastName & FirstName Should be displayed.  
This LastName & FirstName string must be obtained by a separate query in the main query column.  
Also, the main query must use grouping by EmployeeID.)  
with the column name 'Seller’ and the column with the number of orders to return with the name 'Amount'.  
The results of the query should be ordered in descending order of the number of orders

3. Use the Orders table to find the number of orders made by each seller  
and for each buyer. This should only be defined for orders made in 1998.

4. Find buyers and sellers who live in the same city.  
If only one or more sellers live in a city, or only one or more buyers live in a city,  
then information about such buyers and sellers should not be included in the result set.  
Do not use the JOIN construct.

5. Find all buyers who live in the same city.

6. Use the Employees table to find the Manager for each salesperson.

## Task 2.3 Using Join operator

1. Identify sellers who serve the 'Western' region (region table)

2. Output the names of all customers from the Customers table  
and the total number of their orders from the Orders table in the query results.  
Note that some customers do not have orders, but they must also be displayed in the query results.  
Order the results of the query by increasing the number of orders.

### Task 2.4 Using nested queries

1. Output all suppliers (the CompanyName column in the Suppliers table) that do not have  
at least one product in stock (UnitsInStock in the Products table is 0.  
Use a nested SELECT for this query using the IN operator

2. Output all sellers who have more than 150 orders.  
Use nested SELECT.

3. Output all customers ( the Customers table) that do not have any orders  
(for a query in the Orders table).  
You use the EXISTS operator.