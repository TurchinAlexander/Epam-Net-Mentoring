using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebApplication.Tests
{
    [TestFixture]
    public class NorthwindHandlerTests
    {
        [TestCase("http://localhost:62529/")]
        [TestCase("http://localhost:62529?customer=VITSO")]
        [TestCase("http://localhost:62529?customer=MIT")]
        [TestCase("http://localhost:62529?dateFrom=01-01-1997")]
        [TestCase("http://localhost:62529?dateTo=03-04-1998")]
        [TestCase("http://localhost:62529?take=100")]
        [TestCase("http://localhost:62529?customer=VITSO&take=5")]
        [TestCase("http://localhost:62529?customer=VITSO&skip=5")]
        public async Task Query_ResultXml(string queryUrl)
        {
            var httpClient = new HttpClient();

            await httpClient.GetAsync(queryUrl);
        }

        [TestCase("http://localhost:62529/")]
        [TestCase("http://localhost:62529?customer=VITSO")]
        [TestCase("http://localhost:62529?customer=MIT")]
        [TestCase("http://localhost:62529?dateFrom=01-01-1997")]
        [TestCase("http://localhost:62529?dateTo=03-04-1998")]
        [TestCase("http://localhost:62529?take=100")]
        [TestCase("http://localhost:62529?customer=VITSO&take=5")]
        [TestCase("http://localhost:62529?customer=VITSO&skip=5")]
        public async Task Query_ResultExcel(string queryUrl)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));

            await httpClient.GetAsync(queryUrl);
        }
    }
}