using System;

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
            // Array containing the distance to all nodes.
            int[] distance = new int[Nodes];
            bool[] shortestPathTreeSet = new bool[Nodes];
            
            for (int i = 0; i < Nodes; i++)
            {
                distance[i] = int.MaxValue;
                shortestPathTreeSet[i] = false;
            }
            
            distance[source] = 0;

            // Srearching for the shortest path to all other nodes 
            for (int count = 0; count < Nodes - 1; count++)
            {
                int u = minimumDistance(distance, shortestPathTreeSet);

                shortestPathTreeSet[u] = true;

                for (int v = 0; v < Nodes; v++)
                {
                    if (!shortestPathTreeSet[v] && AdjacencyMatrix[v, u] != 0 && distance[u] != int.MaxValue && distance[u] + AdjacencyMatrix[v, u] < distance[v])
                    {
                        distance[v] = distance[u] + AdjacencyMatrix[v, u];
                    }
                }
            }

            return distance;
        }

        // This is the mechanism that separates Dijkstra from Bellman-Ford
        int minimumDistance(int[] distance, bool[] shortestPathTreeSet)
        {
            int minimum = int.MaxValue;
            int minimum_index = -1;

            for (int v = 0; v < Nodes; v++)
            {
                if (shortestPathTreeSet[v] == false && distance[v] <= minimum)
                {
                    minimum = distance[v];
                    minimum_index = v;
                }
            }

            return minimum_index;
        }

        public int[] BellmanFord(int source)
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
