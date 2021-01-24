// Takes a little less than an hour to run
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ExcelLibrary.SpreadSheet; // Used for Excel Integration

namespace dejonker1._0
{
    class Program
    {
        const int MAX_NODES = 100;
        const int TRIES = 5; // Amount of tries for each graph. This is to acquire the median

        static void Main()
        {
            //For Excel integration
            Workbook workbook = new Workbook();
            Worksheet worksheetDijkstra = new Worksheet("Dijkstra");
            Worksheet worksheetBellmanFord = new Worksheet("Bellman-Ford");

            Console.WriteLine("starting...");
            for (int nodes = 10; nodes < MAX_NODES; nodes+=1)
            {
                for (int edges = nodes-1; edges < nodes*(nodes-1); edges+=1)
                {
                    Graph graph = new Graph(nodes, edges);

                    double timeD = MeasureRunTimeDijkstra(graph, TRIES);
                    double timeBF = MeasureRunTimeBellmanFord(graph, TRIES);

                    worksheetDijkstra.Cells[edges, nodes] = new Cell(timeD, CellFormat.General);
                    worksheetBellmanFord.Cells[edges, nodes] = new Cell(timeBF, CellFormat.General);

                }
                Console.WriteLine($"Node {nodes}"); // Shows progress
            }

            workbook.Worksheets.Add(worksheetDijkstra);
            workbook.Worksheets.Add(worksheetBellmanFord);
            workbook.Save(@"C:\Users\gusta\Desktop\cba321\dejonker01.xls"); // This will go to MATLAB

            Console.WriteLine("Worksheet saved");

            Console.Beep(500, 3000); // Plays a sound of 500hz for 3000 ms. This is to signal the program is finished.

            Console.ReadKey();
        }

        static double MeasureRunTimeDijkstra(Graph graph, int tries)
        {
            Stopwatch s = new Stopwatch();
            List<double> timings = new List<double>();

            for (int i = 0; i < tries; i++)
            {
                s.Start();
                graph.Dijkstra(0);
                s.Stop();
                timings.Add(s.Elapsed.TotalMilliseconds);
                s.Reset();
            }

            timings.Sort();

            return timings[2]; // median
        }
        static double MeasureRunTimeBellmanFord(Graph graph, int tries)
        {
            Stopwatch s = new Stopwatch();
            List<double> timings = new List<double>();

            for (int i = 0; i < tries; i++)
            {
                s.Start();
                graph.BellmanFord(0);
                s.Stop();
                timings.Add(s.Elapsed.TotalMilliseconds);
                s.Reset();
            }
            timings.Sort();

            return timings[2]; // median
        }

    }
}
