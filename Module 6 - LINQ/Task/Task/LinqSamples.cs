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

		
    }
}
