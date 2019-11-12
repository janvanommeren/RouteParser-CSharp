using System;
using System.IO;
using Microsoft.CodeAnalysis.CSharp;

namespace RouteParser_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputDirectory = "..\\..\\..\\Generated\\Roslyn";
            var astFiles = Directory.GetFiles(inputDirectory);
            var routeAnalyzer = new RouteAnalyzer();
            foreach (var astFile in astFiles)
            {
                var sourceCode = File.ReadAllText(astFile);
                var tree = CSharpSyntaxTree.ParseText(sourceCode);
                routeAnalyzer.Visit(tree.GetRoot());
                //File.WriteAllText(outputDirectory + "\\" + fileName, result.ToFullString());
            }
            Console.WriteLine("Hello World!");
        }
    }
}