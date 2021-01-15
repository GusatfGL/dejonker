using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ExcelLibrary;
using ExcelLibrary.SpreadSheet;
using System.Diagnostics;

namespace AlgorithmAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int[,] adjMatrix = GraphCreator.GenerateMatrix(200,400);

            GraphCreator.WriteToExcel(@"C:\Users\gusta\source\repos\AlgorithmAnalysis\AlgorithmAnalysis\data\hmm.xls", adjMatrix);

            stopwatch.Stop();
            GraphCreator.PrintMatrix(adjMatrix);
            Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds + " ms");

            Console.ReadKey();
        }
    }
}
