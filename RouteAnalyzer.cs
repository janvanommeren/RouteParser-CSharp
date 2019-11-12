using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RouteParser_CSharp
{
    public class RouteAnalyzer : CSharpSyntaxWalker
    {
        private string classRoute = null;
        public List<string> routeList {  get; }

        public RouteAnalyzer()
        {
            routeList = new List<string>();
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (node.BaseList != null && node.BaseList.Types != null)
            {
                foreach (var inherited in node.BaseList.Types)
                {
                    var inheritedTypeName = inherited.Type.ToString();
                    if (inheritedTypeName.Equals("Controller") || inheritedTypeName.Equals("ApiController"))
                    {
                        foreach(var classAttributeList in node.AttributeLists)
                        {
                            foreach (var attribute in classAttributeList.Attributes)
                            {
                                var attributeName = attribute.Name.ToString();
                                if (attributeName.Equals("RoutePrefix"))
                                {
                                    foreach (var attributeArgument in attribute.ArgumentList.Arguments)
                                    {
                                        var attributeValue = attributeArgument.Expression.ToString();
                                        classRoute = attributeValue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            base.VisitClassDeclaration(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            //1. get value of [Route] attribute
            //2. attribute on method can also be declared with: [ActionName] next to normal route attribute
            //    if this is the case, the normal MVC routing scheme will be used.
            //3. custom routing attributes (negate class route prefix) can be declared by starting route with ~
            foreach(var methodAttribute in node.AttributeLists)
            {
                foreach (var attribute in methodAttribute.Attributes)
                {
                    if (attribute.Name.ToString().Equals("Route"))
                    {
                        foreach (var attributeArgument in attribute.ArgumentList.Arguments)
                        {
                            var routeValue = attributeArgument.Expression.ToString();
                            if (routeValue.StartsWith("~"))
                            {
                                routeValue = routeValue.Replace("~", "");
                            }
                            else if (!String.IsNullOrEmpty(classRoute))
                            {
                                routeValue = classRoute + "/" + routeValue;
                            }

                            if (!routeValue.StartsWith("/"))
                            {
                                routeValue = "/" + routeValue;
                            }
                            routeList.Add(routeValue);
                        }
                    }
                    else if (attribute.Name.Equals("ActionName"))
                    {
                        
                    }
                }
            }
            base.VisitMethodDeclaration(node);
        }
    }
}