using System;
using System.Collections.Generic;

namespace max_graph
{
    public class Graph
    {
        // Graph node
        public int[,] node;
        // Number of nodes
        public int size;

        // Constructor
        public Graph(int size)
        {
            if (size <= 0)
            {
                // Invalid number of nodes
                Console.WriteLine("invalid number of nodes!");
                return;
            }
            this.node = new int[size, size];

            this.size = size;

            for (int row = 0; row < this.size; row++)
            {
                for (int col = 0; col < this.size; col++)
                {
                    this.node[row, col] = 0;
                }
            }
        }

        // Constructor
        public Graph(Graph G, int size)
        {
            if (size <= 0)
            {
                // Invalid number of nodes
                Console.WriteLine("invalid number of nodes!");
                return;
            }
            this.node = new int[size, size];
            this.size = size;
            this.node = G.node;
        }


        // Get vertices of graph
        public List<int> GetVertices()
        {
            List<int> verices = new();
            for (int i = 0; i < this.size; i++)
            {
                verices.Add(i);
            }
            return verices;
        }

        // Add edge between start and end node
        public void addEdge(int start, int end)
        {
            if (this.size > start && this.size > end)
            {
                // Set the connection
                this.node[start, end] = 1;
                this.node[end, start] = 1;
            }
        }


        // Get neighbours of a node
        public List<int> GetNeighbours(int vertex)
        {
            List<int> neighbours = new();
            for (var col = 0; col < this.size; col++)
            {
                if (this.node[vertex, col] == 1)
                {
                    neighbours.Add(col);
                }
            }
            return neighbours;
        }


        // Get Non neighbours of a node
        public List<int> GetNonNeighbours(int vertex)
        {
            List<int> nonNeighbours = new();
            for (var col = 0; col < this.size; col++)
            {
                if (col == vertex) continue;

                if (this.node[vertex, col] == 0)
                {
                    nonNeighbours.Add(col);
                }

            }
            return nonNeighbours;
        }


        // Remove a node from graph
        public void RemoveVertex(int v)
        {
            for (int col = 0; col < this.size; col++)
            {
                if (this.node[v, col] == 1)
                {
                    this.node[v, col] = 0;
                    this.node[col, v] = 0;
                }
            }
        }


        // Print Graph
        public void PrintNodes()
        {
            if (this.size > 0)
            {
                for (var row = 0; row < this.size; row++)
                {
                    Console.Write(row + " :");
                    for (var col = 0; col < this.size; col++)
                    {
                        if (this.node[row, col] == 1)
                        {
                            // When node is connect
                            Console.Write(" " + col);
                        }
                    }
                    Console.Write("\n");
                }
            }
            else
            {
                Console.WriteLine("Empty Graph!");
            }
        }


        // Get degree of a node
        public int GetDegree(int vertex)
        {

            try
            {

                int degree = 0;
                for (var col = 0; col < this.size; col++)
                {
                    if (node[vertex, col] == 1)
                    {
                        degree++;
                    }
                }
                return degree;
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("This vetex doesnt exist!");
                return -1;
            }


        }


        // Generate random graph given number of nodes
        public static Graph GenerateGraph(int node)
        {
            Graph G = new(node);
            Random rnd = new();
            for (var row = 0; row < node; row++)
            {

                for (var col = 0; col < node; col++)
                {
                    if (row == col) continue;
                    int r = rnd.Next(2);
                    G.node[row, col] = r;
                    G.node[col, row] = r;
                }

            }

            return G;


        }


        // Check if two nodes are neighbours
        public bool IsNeghbour(int node1, int node2)
        {
            if (this.node[node1, node2] == 1) return true;
            else return false;
        }


        // print common induced subgraph from given nodes
        public void MakeGraph(List<int> nodes)
        {

            foreach (var v in nodes)
            {
                Console.Write($"{v}: ");
                foreach (var g in nodes)
                {

                    if (this.IsNeghbour(v, g))
                    {
                        Console.Write($"{g} ");
                    }


                }

                Console.WriteLine();


            }

        }



    }
}