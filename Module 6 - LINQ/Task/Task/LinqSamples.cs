// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
    [Title("LINQ Module")]
    [Prefix("Linq")]
    public class LinqSamples : SampleHarness
    {
        private DataSource dataSource = new DataSource();

        [Category("Tasks")]
        [Title("Task 1")]
        [Description("Show all customers which total turnover of their orders is more than the specified value.")]
        public void Linq1()
        {
            var totalTurnover = 1000;

            var customers = dataSource.Customers
                .Where(c => c.Orders.Sum(o => o.Total) > totalTurnover)
                .Select(c => new
                {
                    CustomerId = c.CustomerID,
                    TotalTurnover = c.Orders.Sum(o => o.Total)
                });

            foreach (var customer in customers)
            {
                ObjectDumper.Write($"CustomerId - {customer.CustomerId}, Total Turnover - {customer.TotalTurnover}");
            }

            totalTurnover = 1500;

            foreach (var customer in customers)
            {
                ObjectDumper.Write($"CustomerId - {customer.CustomerId}, Total Turnover - {customer.TotalTurnover}");
            }
        }

        [Category("Tasks")]
        [Title("Task 2")]
        [Description("For every customer show a list of employees which lives in the same city as the customer.")]
        public void Linq2()
        {
            var results = dataSource.Customers
                .Select(c => new
                {
                    CustomerId = c.CustomerID,
                    Suppliers = dataSource.Suppliers
                        .Where(s => s.Country == c.Country
                                    && s.City == c.City)
                });

            foreach (var result in results)
            {
                ObjectDumper.Write(
                    $"CustomerId - {result.CustomerId}, Suppliers - {string.Join(", ", result.Suppliers.Select(s => s.SupplierName))}");
            }
        }

        [Category("Tasks")]
        [Title("Task 2. Using grouping.")]
        [Description("For every customer show a list of employees which lives in the same city as the customer.")]
        public void Linq2Group()
        {
            var groupResults = dataSource.Customers
                .GroupJoin(
                    dataSource.Suppliers,
                    c => new {c.Country, c.City},
                    s => new {s.Country, s.City},
                    (c, s) => new
                    {
                        Customer = c,
                        Suppliers = s
                    });

            foreach (var result in groupResults)
            {
                ObjectDumper.Write(
                    $"CustomerId - {result.Customer.CustomerID}, Suppliers - {string.Join(", ", result.Suppliers.Select(s => s.SupplierName))}");
            }
        }

        [Category("Tasks")]
        [Title("Task 3.")]
        [Description("Show all customers which have at least one order which total is more than the specified value.")]
        public void Linq3()
        {
            var totalLimit = 1000;

            var results = dataSource.Customers
                .Where(c => c.Orders.Any(o => o.Total > totalLimit));

            foreach (var result in results)
            {
                ObjectDumper.Write($"Customer - {result.CompanyName}");
            }
        }

        [Category("Tasks")]
        [Title("Task 4.")]
        [Description("Show customers and their date when they became it.")]
        public void Linq4()
        {
            var results = dataSource.Customers
                .Where(c => c.Orders.Length > 0)
                .Select(c => new
                {
                    Customer = c,
                    Date = c.Orders.Min(o => o.OrderDate)
                });

            foreach (var result in results)
            {
                ObjectDumper.Write(
                    $"Customer - {result.Customer.CustomerID}, Date - {result.Date.Month}/{result.Date.Year}");
            }
        }

        [Category("Tasks")]
        [Title("Task 5.")]
        [Description("Show customers and their date when he became it. Show them in sorted order.")]
        public void Linq5()
        {
            var results = dataSource.Customers
                .Where(c => c.Orders.Length > 0)
                .Select(c => new
                {
                    Customer = c,
                    Date = c.Orders.Min(o => o.OrderDate)
                })
                .OrderBy(c => c.Date.Year)
                .ThenBy(c => c.Date.Month)
                .ThenByDescending(c => c.Customer.Orders.Sum(o => o.Total))
                .ThenBy(c => c.Customer.CompanyName);

            foreach (var result in results)
            {
                ObjectDumper.Write(
                    $"Customer - {result.Customer.CustomerID}, Date - {result.Date.Month}/{result.Date.Year}");
            }
        }

        [Category("Tasks")]
        [Title("Task 6.")]
        [Description(
            "Show customers who has bad Postal Code or Region is null or Phone doesn't have operator code in brackets.'.")]
        public void Linq6()
        {
            var results = dataSource.Customers
                .Where(c => c.PostalCode != null
                            && !Regex.IsMatch(c.PostalCode, "\\d+")
                            || c.Region == null
                            || !Regex.IsMatch(c.Phone, "/(.+/)"));

            foreach (var result in results)
            {
                ObjectDumper.Write(
                    $"Customer - {result.CompanyName}, Postal Code - {result.PostalCode}, Region is Null - {result.Region == null}, Phone - {result.Phone}");
            }
        }

        [Category("Tasks")]
        [Title("Task 7.")]
        [Description("Group all products.")]
        public void Linq7()
        {
            var results = dataSource.Products
                .GroupBy(p => p.Category, (c, p) => new
                {
                    Category = c,
                    Products = p.GroupBy(pr => pr.UnitsInStock, (count, i) => new
                    {
                        Count = count,
                        Items = i.OrderByDescending(it => it.UnitPrice)
                    })
                });

            foreach (var result in results)
            {
                foreach (var product in result.Products)
                {
                    foreach (var item in product.Items)
                    {
                        ObjectDumper.Write(
                            $"Category - {result.Category}, Product Count - {product.Count}, Product - {item.ProductName}, Price - {item.UnitPrice}");
                    }
                }
            }
        }

        [Category("Tasks")]
        [Title("Task 8.")]
        [Description("Group all products in cheap, medium and expensive.")]
        public void Linq8()
        {
            var mediumLimit = 50;
            var expensiveLimit = 200;

            var results = dataSource.Products
                .GroupBy(p => p.UnitPrice > expensiveLimit ? "Expensive"
                    : p.UnitPrice > mediumLimit ? "Medium"
                    : "Cheap", (c, p) => new
                {
                    Category = c,
                    Products = p.OrderBy(i => i.UnitPrice)
                });

            foreach (var result in results)
            {
                ObjectDumper.Write($"Category - {result.Category}");
                foreach (var product in result.Products)
                {
                    ObjectDumper.Write($"  Product - {product.ProductName}, Price - {product.UnitPrice}");
                }
            }
        }

        [Category("Tasks")]
        [Title("Task 9.")]
        [Description("Calculate average profit and intencity of each city.")]
        public void Linq9()
        {
            var results = dataSource.Customers
                .GroupBy(c => c.City, (city, customers) => new
                {
                    City = city,
                    Profit = customers.Average(cust => cust.Orders.Sum(o => o.Total)),
                    Intensity = customers.Sum(cust => cust.Orders.Length) / customers.Count()
                });

            foreach (var result in results)
            {
                ObjectDumper.Write($"City - {result.City}, Profit - {result.Profit}, Intensity - {result.Intensity}");
            }
        }
    }
}