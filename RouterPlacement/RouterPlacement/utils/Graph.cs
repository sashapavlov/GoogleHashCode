using System;
using System.Collections.Generic;
using System.Linq;

namespace RouterPlacement.utils
{
    public class Graph<T>
    {
        public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new Dictionary<T, HashSet<T>>();

        public void AddVertex(T vertex)
        {
            AdjacencyList[vertex] = new HashSet<T>();
        }

        public void AddEdge(T firstVertex, T secondVertex)
        {
            if (AdjacencyList.ContainsKey(firstVertex) && AdjacencyList.ContainsKey(secondVertex))
            {
                AdjacencyList[firstVertex].Add(secondVertex);
                AdjacencyList[secondVertex].Add(firstVertex);
            }
        }
    }

    public class BreadthFirstPaths<T> where T : class
    {
        //private bool[] marked;  // marked[v] = is there an s-v path

        private Dictionary<T, bool> marked = new Dictionary<T, bool>();
        private Dictionary<T, T> edgeTo = new Dictionary<T, T>();

        private T s;

        //private T[] edgeTo;      // edgeTo[v] = previous edge on shortest s-v path
        //private T[] distTo;      // distTo[v] = number of edges shortest s-v path

        public BreadthFirstPaths(Graph<T> G, T s)
        {
            //marked = new bool[G.V];
            //distTo = new int[G.V];
            //edgeTo = new int[G.V];
            bfs(G, s);
            this.s = s;
        }
        
        // breadth-first search from a single source
        private void bfs(Graph<T> G, T s)
        {
            Queue<T> q = new Queue<T>();
            marked[s] = true;
            q.Enqueue(s);

            while (q.Any())
            {
                T v = q.Dequeue();
                foreach(T w in G.AdjacencyList[v])
                {
                    if (!marked.ContainsKey(w) || !marked[w])
                    {
                        edgeTo[w] = v;
                        marked[w] = true;
                        q.Enqueue(w);
                    }
                }
            }
        }

        public bool hasPathTo(T v)
        {
            return marked[v];
        }

        public IEnumerable<T> pathTo(T v)
        {
            if (!hasPathTo(v)) return null;
            Stack<T> path = new Stack<T>();
            T x;
            for (x = v; x != s; x = edgeTo[x])
                path.Push(x);
            path.Push(s);
            return path;
        }
    }

}
