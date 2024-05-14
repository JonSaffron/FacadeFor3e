using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class TestODataResponse
        {
        [Test]
        public void TestStandardError()
            {
            var response = @"{""error"":{""code"":"""",""message"":""Entity 'Proforma' with key '-100' does not exist.""}}";
            var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            httpResponse.Content = new ByteArrayContent(Encoding.UTF8.GetBytes(response));
            httpResponse.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var serviceResult = new ODataServiceResult(new HttpRequestMessage(HttpMethod.Get, "http://localhost/"), httpResponse);
            ClassicAssert.IsTrue(serviceResult.IsError);
            ClassicAssert.AreEqual(1, serviceResult.ErrorMessages.Count());
            ClassicAssert.AreEqual("Entity 'Proforma' with key '-100' does not exist.", serviceResult.ErrorMessages.First());
            }

        [Test]
        public void TestAlternativeError()
            {
            var response = @"{ ""statusCode"": 429, ""message"": ""Rate limit is exceeded. Try again in 38 seconds."" }";
            var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            httpResponse.Content = new ByteArrayContent(Encoding.UTF8.GetBytes(response));
            httpResponse.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var serviceResult = new ODataServiceResult(new HttpRequestMessage(HttpMethod.Get, "http://localhost/"), httpResponse);
            ClassicAssert.IsTrue(serviceResult.IsError);
            ClassicAssert.AreEqual(1, serviceResult.ErrorMessages.Count());
            ClassicAssert.AreEqual("Rate limit is exceeded. Try again in 38 seconds.", serviceResult.ErrorMessages.First());
            }
        }
    }

