using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterPlacement.utils
{
    public class Graph
    {
        private int V { get; set; }
        private int E { get; set; }
        private List<Cell>[] adj;

        /**
         * Initializes an empty graph with {@code V} vertices and 0 edges.
         * param V the number of vertices
         *
         * @param  V number of vertices
         * @throws IllegalArgumentException if {@code V < 0}
         */
        public Graph(int V)
        {
            if (V < 0) throw new ArgumentException("Number of vertices must be nonnegative");
            this.V = V;
            this.E = 0;
            adj = new List<Cell>[V];
            for (int v = 0; v < V; v++)
            {
                adj[v] = new List<Cell>();
            }
        }

        // throw an IllegalArgumentException unless {@code 0 <= v < V}
        private void validateVertex(int v)
        {
            if (v < 0 || v >= V)
                throw new ArgumentException("vertex " + v + " is not between 0 and " + (V - 1));
        }

        /**
         * Adds the undirected edge v-w to this graph.
         *
         * @param  v one vertex in the edge
         * @param  w the other vertex in the edge
         * @throws IllegalArgumentException unless both {@code 0 <= v < V} and {@code 0 <= w < V}
         */
        public void addEdge(Cell v, Cell w)
        {
            validateVertex(v);
            validateVertex(w);
            E++;
            adj[v].Add(w);
            adj[w].Add(v);
        }


        /**
         * Returns the vertices adjacent to vertex {@code v}.
         *
         * @param  v the vertex
         * @return the vertices adjacent to vertex {@code v}, as an iterable
         * @throws IllegalArgumentException unless {@code 0 <= v < V}
         */
        public IEnumerable<Cell> GetAdjacentVertices(int v)
        {
            validateVertex(v);
            return adj[v];
        }

        /**
         * Returns the degree of vertex {@code v}.
         *
         * @param  v the vertex
         * @return the degree of vertex {@code v}
         * @throws IllegalArgumentException unless {@code 0 <= v < V}
         */
        public int degree(int v)
        {
            validateVertex(v);
            return adj[v].Count;
        }


        /**
         * Returns a string representation of this graph.
         *
         * @return the number of vertices <em>V</em>, followed by the number of edges <em>E</em>,
         *         followed by the <em>V</em> adjacency lists
         */
        public String toString()
        {
            StringBuilder s = new StringBuilder();
            s.Append(V + " vertices, " + E + " edges " + "\r\n");
            for (int v = 0; v < V; v++)
            {
                s.Append(v + ": ");

                foreach (var cell in adj)
                {
                    s.Append(cell + " ");
                }

                s.Append("\r\n");
            }
            return s.ToString();
        }

    }

}
