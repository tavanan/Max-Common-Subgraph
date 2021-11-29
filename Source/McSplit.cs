using System;
using System.Collections.Generic;
using System.Linq;

namespace MaxCommonSubgraph
{
    public class McSplit {

        public static List<Tuple<int,int>> incumbent;
        public int GetBound(List<Tuple<List<int>,List<int>>> labels)
        {
            int sum = 0;
           foreach(var l in labels)
           {
                sum += Math.Min(l.Item1.Count , l.Item2.Count);
           }

           return sum;
        }

        public Tuple<List<int>, List<int>> SelectLabelClass(List<Tuple<List<int>,List<int>>> labels)
        {

            Tuple<List<int>, List<int>> label = new(new(),new());
            int min = 0;
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

        public int SelectVertex(List<int> nodes , Graph g) 
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

        public void Search(List<Tuple<List<int>,List<int>>> future, List<Tuple<int,int>> M ,Graph G ,Graph H)
        {
               
               if(M.Count > incumbent.Count) incumbent = M;
               var bound = M.Count + GetBound(future);

               if(bound <= incumbent.Count) return;

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

        
    }
}