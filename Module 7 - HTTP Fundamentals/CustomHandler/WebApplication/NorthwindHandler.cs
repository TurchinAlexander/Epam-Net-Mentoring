using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Routing;
using System.Xml;
using ClosedXML.Excel;
using DataAccessLayer;

namespace WebApplication
{
    public class NorthwindHandler : IHttpHandler
    {
        private NorthwindContext northwindContext = new NorthwindContext();

        public RouteData RouteData { get; set; }

        public bool IsReusable
        {
             get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var data = TakeData(context);

            var acceptHeader = context.Request.Headers["Accept"];

            if (acceptHeader.Contains("text/xml")
                || acceptHeader.Contains("application/xml"))
            {
                string xml = CreateXml(data);

                context.Response.ContentType = "text/xml";
                context.Response.Write(xml);
            }
            else
            {
                var excelData = CreateXls(data);

                context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                context.Response.AppendHeader("content-disposition", "inline; filename=MyExcelFile.xls");
                context.Response.BinaryWrite(excelData);
            }
        }

        private IEnumerable<Order> TakeData(HttpContext context)
        {
            var httpQuery = context.Request.QueryString;

            var query = northwindContext.Orders.Where(o => o.CustomerID != "");

            if (httpQuery["customer"] != null)
            {
                string customerId = httpQuery["customer"];

                query = query.Where(o => o.CustomerID == customerId);
            }

            if (httpQuery["dateFrom"] != null)
            {
                DateTime dateFrom = Convert.ToDateTime(httpQuery["dateFrom"]);

                query = query.Where(o => o.OrderDate > dateFrom);
            }
            else if (httpQuery["dateTo"] != null)
            {
                DateTime dateTo = Convert.ToDateTime(httpQuery["dateTo"]);

                query = query.Where(o => o.OrderDate < dateTo);
            }

            if (httpQuery["skip"] != null)
            {
                int skip = Convert.ToInt32(httpQuery["skip"]);

                query = query.Skip(skip);
            }

            if (httpQuery["take"] != null)
            {
                int take = Convert.ToInt32(httpQuery["take"]);

                query = query.Take(take);
            }

            return query
                .OrderBy(o => o.OrderID)
                .ToArray();
        }

        private string CreateXml(IEnumerable<Order> orders)
        {
            MemoryStream stream = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(stream);

            writer.WriteStartDocument();
            writer.WriteStartElement("Orders");

            foreach (Order order in orders)
            {
                writer.WriteStartElement("Order");
                writer.WriteAttributeString("OrderId", order.OrderID.ToString());
                writer.WriteAttributeString("CustomerId", order.CustomerID);
                writer.WriteAttributeString("OrderDate", order.OrderDate.ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();

            stream.Position = 0;

            StreamReader reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }

        public byte[] CreateXls(IEnumerable<Order> orders)
        {
            var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Sample Sheet");

            int row = 1;
            const int orderColumn = 1;
            const int customerColumn = 2;
            const int orderDateColumn = 3;

            worksheet.Cell(row, orderColumn).Value = "OrderId";
            worksheet.Cell(row, customerColumn).Value = "CustomerId";
            worksheet.Cell(row, orderDateColumn).Value = "OrderDate";

            row++;

            foreach (Order order in orders)
            {
                worksheet.Cell(row, orderColumn).Value = order.OrderID.ToString();
                worksheet.Cell(row, customerColumn).Value = order.CustomerID;
                worksheet.Cell(row, orderDateColumn).Value = order.OrderDate.ToString();

                row++;
            }

            MemoryStream stream = new MemoryStream();

            workbook.SaveAs(stream);

            stream.Position = 0;

            return stream.ToArray();
        }
    }
}
