using System;
using System.IO;
using Microsoft.CodeAnalysis.CSharp;
using RouteParser_CSharp;
using Xunit;

namespace RouteParser_CSharp_Test
{
    public class RouteParserTest
    {
        private string sourceCode = @"
        namespace ProductsApp.Controllers
        {
            [RoutePrefix(""test"")]
            public class TestController : Controller
            {
                [Route(""get"")]
                public IHttpActionResult GetTest() {
                    return OK();
                }

                [Route(""~/testget"")]
                public IHttpActionResult GetTest() {
                    return OK();
                }

                [ActionName(""another"")]
                public IHttpActionResult GetTest() {
                    return OK();
                }
            }
        }
        ";
        
        [Fact]
        public void TestRouteParser()
        {
            RouteAnalyzer routeAnalyzer = new RouteAnalyzer();
            sourceCode = sourceCode.Replace("\"", "");
            var tree = CSharpSyntaxTree.ParseText(sourceCode);
            routeAnalyzer.Visit(tree.GetRoot());

            var routeList = routeAnalyzer.routeList;
            Assert.Equal(2, routeList.Count);
        }
        
    }
}