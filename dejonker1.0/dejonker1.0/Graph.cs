using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dejonker1._0
{
    class Graph
    {
        public int[,] AdjacencyMatrix;
        public int Nodes; // must this really be public? Get...?
        public int Edges;
        public Edge[] AdjacencyList;

        Random random = new Random(); // 6942069

        public Graph(int nodes, int edges)
        {
            Nodes = nodes;
            Edges = edges;
            AdjacencyList = new Edge[edges];
            for (int i = 0; i < edges; ++i)
            {
                AdjacencyList[i] = new Edge();
            }
            AdjacencyMatrix = this.GenerateMatrix();
        }

        public int[,] GenerateMatrix()
        {
            if (Edges < Nodes - 1) throw new Exception("Too few edges");
            if (Edges > Nodes * (Nodes - 1)) throw new Exception("Too many edges");

            int[,] adjacencyMatrix = new int[Nodes, Nodes];

            // Gives every cell a value of zero
            for (int y = 0; y < Nodes; y++)
            {
                for (int x = 0; x < Nodes; x++)
                {
                    adjacencyMatrix[x, y] = 0;
                }
            }

            int placedEdges = 0;

            for (int i = 1; i < Nodes; i++)
            {
                // produce edge between rnd(0, amountofnodes) to new node
                int fromVertex = random.Next(0, i);
                int weight = random.Next(1, 10);

                adjacencyMatrix[i, fromVertex] = weight;
                placedEdges++;
            }

            while (placedEdges < Edges)
            {
                int fromVertex = random.Next(0, Nodes);
                int weight = random.Next(1, 10);

                int targetVertex = random.Next(0, Nodes);
                while (targetVertex == fromVertex || adjacencyMatrix[targetVertex, fromVertex] != 0) //|| adjacencyMatrix[fromVertex, targetVertex] != 0)// tredje condition tar bort parallella kanter
                {
                    fromVertex = random.Next(0, Nodes);
                    targetVertex = random.Next(0, Nodes);
                }

                adjacencyMatrix[targetVertex, fromVertex] = weight;
                placedEdges++;
            }

            return adjacencyMatrix;
        }

        public Graph ConvertToAdjacencyListGraph() // not kosher at all
        {
            Graph graph = new Graph(Nodes, Nodes);

            int iterator = 0; // weird quick fix (arrays are not "soft")

            for (int y = 0; y < Nodes; y++)
            {
                for (int x = 0; x < Nodes; x++)
                {
                    if (AdjacencyMatrix[x, y] != 0)
                    {
                        AdjacencyList[iterator].From = y;
                        AdjacencyList[iterator].Target = x;
                        AdjacencyList[iterator].Weight = AdjacencyMatrix[x, y];
                        iterator++;
                    }
                }
            }

            return graph;
        }

        public int[] Dijkstra(int source)
        {
            int[] distance = new int[Nodes]; // The output array. dist[i] 
                                     // will hold the shortest 
                                     // distance from src to i 

            // sptSet[i] will true if vertex 
            // i is included in shortest path 
            // tree or shortest distance from 
            // src to i is finalized 
            bool[] sptSet = new bool[Nodes];

            // Initialize all distances as 
            // INFINITE and stpSet[] as false 
            for (int i = 0; i < Nodes; i++)
            {
                distance[i] = int.MaxValue;
                sptSet[i] = false;
            }

            // Distance of source vertex 
            // from itself is always 0 
            distance[source] = 0;

            // Find shortest path for all vertices 
            for (int count = 0; count < Nodes - 1; count++)
            {
                // Pick the minimum distance vertex 
                // from the set of vertices not yet 
                // processed. u is always equal to 
                // src in first iteration. 
                int u = minDistance(distance, sptSet);

                // Mark the picked vertex as processed 
                sptSet[u] = true;

                // Update dist value of the adjacent 
                // vertices of the picked vertex. 
                for (int v = 0; v < Nodes; v++)

                    // Update dist[v] only if is not in 
                    // sptSet, there is an edge from u 
                    // to v, and total weight of path 
                    // from src to v through u is smaller 
                    // than current value of dist[v] 

                    // Originally from 
                    if (!sptSet[v] && AdjacencyMatrix[v, u] != 0 && distance[u] != int.MaxValue && distance[u] + AdjacencyMatrix[v, u] < distance[v])
                        distance[v] = distance[u] + AdjacencyMatrix[v, u];
            }

            // print the constructed distance array 
            return distance;
        }

        // Got to refactor this one
        int minDistance(int[] dist, bool[] sptSet)
        {
            // Initialize min value 
            int min = int.MaxValue, min_index = -1;

            for (int v = 0; v < Nodes; v++)
                if (sptSet[v] == false && dist[v] <= min)
                {
                    min = dist[v];
                    min_index = v;
                }

            return min_index;
        }

        public int[] BellmanFord(int source) // s must be source, right?
        {
            int[] distance = new int[Nodes];

            for (int i = 0; i < Nodes; ++i)
            {
                distance[i] = int.MaxValue;
            }

            distance[source] = 0;

            for (int i = 1; i < Nodes; ++i)
            {
                for (int j = 0; j < Edges; ++j)
                {
                    int u = AdjacencyList[j].From;
                    int v = AdjacencyList[j].Target;
                    int w = AdjacencyList[j].Weight;

                    if (distance[u] != int.MaxValue && distance[u] + w < distance[v])
                    {
                        distance[v] = distance[u] + w;
                    }
                }
            }

            return distance;
        }

        public void _temporaryPrintDistances()
        {
            for (int i = 0; i < Nodes; i++)
            {
                Console.WriteLine($"{i}: Dijksta gets {Dijkstra(0)[i]} and Bellman-Ford gets {BellmanFord(0)[i]}");
            }
        }

        public void _temporaryPrintMatrix()
        {
            for (int y = 0; y < AdjacencyMatrix.GetLength(1); y++)
            {
                for (int x = 0; x < AdjacencyMatrix.GetLength(0); x++)
                {
                    Console.Write(AdjacencyMatrix[x, y]);
                }
                Console.WriteLine(); // Adds newline
            }
        }

        public void _temporaryPrintAdjacencyList()
        {
            foreach(Edge edge in AdjacencyList)
            {
                Console.WriteLine($"We have one edge from {edge.From} to {edge.Target} with weight {edge.Weight}.");
            }
        }

        /// <summary>
        /// Used in an adjacency list
        /// </summary>
        public class Edge
        {
            public int From;
            public int Target;
            public int Weight;

            public Edge()
            {
                From = 0;
                Target = 0;
                Weight = 0;
            }
        }
    }
}
