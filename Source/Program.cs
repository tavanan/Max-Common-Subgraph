using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace MaxCommonSubgraph
{
    class Program
    {
        // maximum partions find so far
        public static List<Tuple<int,int>> incumbent =new();

        // Find the bound
        public static int GetBound(List<Tuple<List<int>,List<int>>> labels)
        {
            int sum = 0;
           foreach(var l in labels)
           {
                sum += Math.Min(l.Item1.Count , l.Item2.Count);
           }

           return sum;
        }


        // Choose a label class with the smallest max(∣G∣, ∣H∣)
        public static Tuple<List<int>, List<int>> SelectLabelClass(List<Tuple<List<int>,List<int>>> labels)
        {

            Tuple<List<int>, List<int>> label = new(new(),new());
            int min = Math.Max(labels[0].Item1.Count , labels[0].Item2.Count);
           foreach(var l in labels)
           {
              if(Math.Max(l.Item1.Count , l.Item2.Count) <=min)
              {
                  min = Math.Max(l.Item1.Count , l.Item2.Count);
                  label = l;
              }
               
           }

           return label;


        }


        // Choose a vertex with max degree from garph g
        public static int SelectVertex(List<int> nodes , Graph g) 
        {
            int vertex = -1;
            int maxdeg = 0;
            foreach(var v in nodes)
            {
                if(g.GetDegree(v) >= maxdeg) maxdeg = g.GetDegree(v);
                vertex = v;
            }
            
            return vertex;

        }


        // Main algorithm
        public static void Search(List<Tuple<List<int>,List<int>>> future, List<Tuple<int,int>> M ,Graph G ,Graph H)
        {
               
               if(M.Count > Program.incumbent.Count) Program.incumbent=M.ToList();
               var bound = M.Count + GetBound(future);

               if(bound <= Program.incumbent.Count)
               {
                   M.Clear();
                   return;
               }

               Tuple<List<int>, List<int>> label = SelectLabelClass(future);
               int vertex = SelectVertex(label.Item1,G);

               foreach(var v in label.Item2)
               {
                   List<Tuple<List<int>,List<int>>> future2 = new();

                   foreach(var l in future)
                   {
                       List<int> g = l.Item1.Intersect(G.GetNeighbours(vertex)).ToList();
                       List<int> h = l.Item2.Intersect(H.GetNeighbours(v)).ToList();

                       if(g.Count != 0 && h.Count !=0)
                       {
                           future2.Add(new Tuple<List<int>, List<int>>(g,h));
                       }

                       List<int> g1 = l.Item1.Intersect(G.GetNonNeighbours(vertex)).ToList();
                       List<int> h1 = l.Item2.Intersect(H.GetNonNeighbours(v)).ToList();

                       if(g1.Count != 0 && h1.Count !=0 )
                       {
                           future2.Add(new Tuple<List<int>, List<int>>(g1,h1));
                       }

                   }
                   M.Add(new Tuple<int, int>(vertex , v));
                   Search(future2,M,G,H);
                   
               }

               label.Item1.Remove(vertex);
               future.Remove(label);

               if(label.Item1.Count > 0)
               {
                   future.Add(new Tuple<List<int>, List<int>>(label.Item1,label.Item2));
               }

               Search(future , M ,G,H);
               
        }


        // Read graph fro file
        public static Graph ReadFromFile(string s)
        {
            
            string[] lines = System.IO.File.ReadAllLines(s);
            Graph g = new(lines.Length);

            for(int i =0; i<g.size;i++)
            {
                //string ss = String.Concat(lines[i].Where(c => !Char.IsWhiteSpace(c)));
                string row = lines[i].Substring(lines[i].LastIndexOf(':') + 1);
                string[] tokens = row.Split(' ');
                for(int j=1; j<tokens.Length;j++)
                {
                    int n = int.Parse(tokens[j].ToString());
                    g.node[i,n] = 1;
                    g.node[n,i] = 1;
                }
            }
            return g;
            
            
        

        }
        

        // Main
        static void Main(string[] args)
        {

            // Add graphs Manually 
            //var G = new Graph(12);
            //var H = new Graph(12);
            
           /* G.addEdge(0,1);
            G.addEdge(1,2);
            G.addEdge(1,3);
            G.addEdge(3,4);
            G.addEdge(4,5);
            G.addEdge(4,6);
            G.addEdge(4,7);
            G.addEdge(4,8);
            G.addEdge(5,6);
            G.addEdge(6,7);
            G.addEdge(6,9);
            G.addEdge(7,8);
            G.addEdge(7,9);
            G.addEdge(8,9);
            G.addEdge(8,10);
            G.addEdge(9,11);
            G.addEdge(10,11);*/


            
           /* H.addEdge(0,1);
            H.addEdge(1,2);
            H.addEdge(1,3);
            H.addEdge(3,4);
            H.addEdge(3,5);
            H.addEdge(3,10);
            H.addEdge(4,5);
            H.addEdge(5,6);
            H.addEdge(5,9);
            H.addEdge(6,7);
            H.addEdge(7,8);
            H.addEdge(8,9);
            H.addEdge(9,10);
            H.addEdge(9,11);*/
            
            

            // Read graph from file
            string curDir = Directory.GetCurrentDirectory();
            string path;
            if(curDir.Contains("Source"))
            {
                int index = curDir.IndexOf("Source");
                path = curDir.Substring(0,index);
            }
            else
            {
                int index = curDir.IndexOf("EXE");
                path = curDir.Substring(0,index);
            }
            
            
            
            Graph G = ReadFromFile($@"{path}\Examples\Graph_G.txt");
            Graph H = ReadFromFile($@"{path}\Examples\Graph_H.txt");
            

            // Generate two random graphs
            //var G = Graph.GenerateGraph(20);
            //var H = Graph.GenerateGraph(20);
            

            // Print graphs
            Console.WriteLine();
            Console.WriteLine("Graph G : ");
            G.PrintNodes();
            Console.WriteLine("------------------------");
            Console.WriteLine("Graph H : ");
            H.PrintNodes();
            

            // Initialize objects
            List<Tuple<List<int>,List<int>>> future = new();
            future.Add(new Tuple<List<int>, List<int>>(G.GetVertices(),H.GetVertices()));
            List<Tuple<int,int>> M = new();
            List<int> v = new();
            List<int> w = new();

            
            // Call for main algorithm
            Search(future,M,G,H);
            Console.WriteLine("---------------------");
            Console.WriteLine("------- Corresponding partions of nodes {v(G),v(H)} ----------");
            foreach(var i in Program.incumbent)
            {
                
                v.Add(i.Item1); w.Add(i.Item2);
                
                Console.Write($"{{{i.Item1},{i.Item2}}}, ");
            }
            
            
            // Print subgraphs
            v=v.Distinct().ToList();
            w=w.Distinct().ToList();

            Console.WriteLine("\n--------------------------");
            
            Console.WriteLine();
            Console.WriteLine("-------- Subgraph of G ---------");
            G.MakeGraph(v);
            Console.WriteLine("-------- Subgraph of H ---------");
            H.MakeGraph(w);
            Console.WriteLine();
            

            
            Console.WriteLine("Press Enter key to exit");
            Console.ReadLine();
            

        }
    }
}
