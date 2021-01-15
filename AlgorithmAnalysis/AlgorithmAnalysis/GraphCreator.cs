using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelLibrary.SpreadSheet;
using ExcelLibrary;

namespace AlgorithmAnalysis
{
    class GraphCreator
    {
        static Random random = new Random(6942069);
        public static int[,] GenerateMatrix(int vertices, int edges)
        {
            if (edges < vertices - 1) throw new Exception("Too few edges");
            if (edges > vertices * (vertices - 1)) throw new Exception("Too many edges");

            int[,] adjacencyMatrix = new int[vertices,vertices];

            // Gives every cell a value of zero
            for (int y = 0; y < vertices; y++)
            {
                for (int x = 0; x < vertices; x++)
                {
                    adjacencyMatrix[x, y] = 0;
                }
            }




            int placedEdges = 0;

            for (int i = 1; i < vertices; i++) 
            {
                // produce edge between rnd(0, amountofnodes) to new node
                int fromVertex = random.Next(0, i);
                int weight = random.Next(1, 10); // random value between 1 and 9

                adjacencyMatrix[i, fromVertex] = weight;
                placedEdges++;
             }

            while (placedEdges < edges)
            {
                int fromVertex = random.Next(0, vertices);
                int weight = random.Next(1, 10);

                int targetVertex = random.Next(0, vertices);
                while(targetVertex == fromVertex || adjacencyMatrix[targetVertex, fromVertex] != 0 || adjacencyMatrix[fromVertex, targetVertex] != 0) // Tredje condition tar bort parallella kanter
                {
                    fromVertex = random.Next(0, vertices);
                    targetVertex = random.Next(0, vertices);
                }

                adjacencyMatrix[targetVertex, fromVertex] = weight;
                placedEdges++;


            }

            return adjacencyMatrix;
        }

        public static void PrintMatrix(int[,] matrix) // Fucked up, maybe graph should be an object (oop)
        {
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                for (int x = 0; x < matrix.GetLength(0); x++)
                {
                    Console.Write(matrix[x,y]);
                }
                Console.WriteLine(); // Adds newline
            }
        }

        public static void WriteToExcel(string file, int[,] matrix)
        {
            Workbook workbook = new Workbook();
            Worksheet worksheet = new Worksheet("Graph from GraphCreator");

            for (int y= 0; y < matrix.GetLength(1); y++)
            {
                for (int x = 0; x<matrix.GetLength(0); x++)
                {
                    worksheet.Cells[x, y] = new Cell(matrix[x,y]);
                }
            }

            workbook.Worksheets.Add(worksheet);
            workbook.Save(file);
        }

    }
}
