using Vertex = HanyaKipas.Lib.Node;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace HanyaKipas.Lib
{
    public class Graph
    {
        private Dictionary<Vertex, LinkedList<Vertex>> adjList = new();
        /// <summary>
        /// Konstuktor graf tanpa argumen (Hanya menginisialisasi dictionary)
        /// </summary>
        public Graph() {}

        /// <summary>
        /// Konstuktor graf dengan otomatis memasukkan sudut serta sisi,
        /// </summary>
        /// <param name="edges">array of tuples berisi 2 nodes yangn menandakan
        /// dua sudut yang saling bertetanggaan</param>
        public Graph(Tuple<Vertex, Vertex>[] edges)
        {
            foreach (Tuple<Vertex, Vertex> e in edges)
                AddEdge(e.Item1, e.Item2);
        }

        /// <summary>
        /// Konstuktor graf dengan otomatis memasukkan sudut serta sisi,
        /// </summary>
        /// <param name="edges">array of array[2] of nodes, menandakan dua sudut
        /// yang saling bertetanggaan</param>
        public Graph(Vertex[][] edges)
        {
            foreach (Vertex[] e in edges)
                AddEdge(e[0], e[1]);
        }

        /// <summary>
        /// Menambahkan sebuah sudut (tanpa tetangga) ke dalam graf
        /// </summary>
        /// <param name="vertex">sudut yang ingin ditambahkan</param>
        public void AddVertex(Vertex vertex)
        {
            if (!adjList.ContainsKey(vertex))
                adjList.Add(vertex, new LinkedList<Vertex>());
        }

        /// <summary>
        /// Menambahkan informasi ketetanggan dua buah sudut (dan sudutnya
        /// jika belum ada di graf)
        /// </summary>
        /// <param name="v1">sudut 1 yang bertetanggaan dengan sudut 2</param>
        /// <param name="v2">sudut 2 yang bertetanggaan dengan sudut 1</param>
        public void AddEdge(Vertex v1, Vertex v2)
        {
            if (!adjList.ContainsKey(v1))
                AddVertex(v1);
            if (!adjList.ContainsKey(v2))
                AddVertex(v2);

            // algoritma masukin sudut emang agak ribet
            // soalnya biar ngejamin urutan nama di list
            // udah terurut abjad
            if (adjList[v1].Count == 0)
                adjList[v1].AddFirst(v2);
            else
            {
                bool inserted = false;
                for (LinkedListNode<Vertex> v = adjList[v1].First;
                     v != null && !inserted;
                     v = v.Next)
                {
                    int cmp = Vertex.Compare(v.Value, v2);
                    inserted = cmp >= 0;

                    if (cmp > 0)
                        adjList[v1].AddBefore(v, v2);
                }

                if (!inserted)
                    adjList[v1].AddLast(v2);
            }

            if (adjList[v2].Count == 0)
                adjList[v2].AddFirst(v1);
            else
            {
                bool inserted = false;
                for (LinkedListNode<Vertex> v = adjList[v2].First;
                     v != null && !inserted;
                     v = v.Next)
                {
                    int cmp = Vertex.Compare(v.Value, v1);
                    inserted = cmp >= 0;

                    if (cmp > 0)
                        adjList[v2].AddBefore(v, v1);
                }

                if (!inserted)
                    adjList[v2].AddLast(v1);
            }
        }

        /// <summary>
        /// Menuliskan isi graf
        /// </summary>
        public void Print()
        {
            foreach (KeyValuePair<Vertex, LinkedList<Vertex>> entry in adjList)
            {
                Debug.Write(entry.Key.GetInfo() + " <-> ");
                foreach (Vertex vert in entry.Value)
                    Debug.Write(vert.GetInfo() + ", ");
                Debug.WriteLine("");
            }
        }
    }
}
