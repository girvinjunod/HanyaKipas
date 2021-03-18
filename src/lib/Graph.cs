using Vertex = HanyaKipas.Lib.Node;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;

namespace HanyaKipas.Lib
{
    public class Graph
    {
        private Dictionary<Vertex, LinkedList<Vertex>> adjList = new();
        private Dictionary<Vertex, bool> visited;
        /// <summary>
        /// Konstuktor graf tanpa argumen (Hanya menginisialisasi dictionary)
        /// </summary>
        public Graph() { }

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

        /// <getter>
        /// Mengambil Nilai Dictionary
        /// </getter>
        public Dictionary<Vertex, LinkedList<Vertex>> GetAdjList()
        {
            return adjList;
        }

        /// <summary>
        /// Menghapus semua isi (sudut dan sisi) graf
        /// </summary>
        public void Clear()
        {
            foreach (KeyValuePair<Vertex, LinkedList<Vertex>> kvp in adjList)
            {
                adjList.Remove(kvp.Key);
            }
        }

        public List<Vertex> BFS(Vertex entry, Vertex target)
        {
            Queue<Vertex> toVisit = new(); // vertex yang akan dikunjungi
            Dictionary<Vertex, Vertex> memory = new(); // vertex pendahulu dari suatu vertex
            List<Vertex> res = new(); // menyimpan vertex hasil
            Vertex curVert; // vertex yang sedang dikunjungi
            int visitedCount = 0; // banyak vertex yang sudah dikunjngi
            int totalVerts = adjList.Count; // banyak vertex pada graf
            bool found;

            visited = new();
            foreach (KeyValuePair<Vertex, LinkedList<Vertex>> kvp in adjList)
            {
                visited.Add(kvp.Key, false);
            }

            // akan divisit node entry
            toVisit.Enqueue(entry);
            do
            {
                // tentuin sudut yang lagi divisit lalu catat sudutnya
                curVert = toVisit.Dequeue();
                // apakah sudut sudah ketemu?
                found = target.Equals(curVert);
                // kalau belum divisit dan belum ketemu
                if (!visited[curVert] && !found)
                {
                    // iterasi semua vertex tetangga
                    foreach (Vertex vertex in adjList[curVert])
                    {
                        // tambahin sudut tetangganya yang belum divisit
                        if (!memory.ContainsKey(vertex))
                        {
                            toVisit.Enqueue(vertex);
                            memory.Add(vertex, curVert);
                        }
                    }
                    visitedCount++;
                    visited[curVert] = true; // tandain udah divisit
                }
            } while (visitedCount < totalVerts && !found);

            // menambahkan sudut dari target sampai entry
            if (found)
            {
                curVert = target;
                do
                {
                    res.Add(curVert);
                    curVert = memory[curVert];
                } while (curVert != entry);
                res.Add(curVert);
            }

            return res;
        }
        public List<Vertex> DFS(Vertex entryNode, Vertex target, List<Vertex> nodelist)
        {
            //Stack<Vertex> vertices;
            //List<Vertex> res = new();

            Vertex curVert = entryNode;
            List<Vertex> result = new();
            Debug.WriteLine("Curvert :");
            Debug.WriteLine(curVert.GetInfo());
            Debug.WriteLine("target : ");
            Debug.WriteLine(target.GetInfo());
            if (curVert.GetInfo() == target.GetInfo())
            { //basis
                nodelist.Add(curVert);
                //nodelist = nodelist.Distinct().ToList();
                return nodelist;
            }
            nodelist.Add(curVert);
            //Debug.WriteLine(curVert.GetInfo());
            foreach (Vertex vertex in adjList[curVert]) //rekursi
            {
                if (!nodelist.Contains(vertex))
                {
                    //nodelist.Clear();
                    //nodelist.Add(curVert);
                    nodelist.AddRange(DFS(vertex, target, nodelist));
                    //nodelist = nodelist.Distinct().ToList();
                    result = nodelist;
                    //return nodelist;
                }
            }
            ;
            //Debug.WriteLine("HAIIIIIIIIIIIIIIIII");
            //nodelist = nodelist.Distinct().ToList();
            nodelist.Remove(curVert);
            result = result.Distinct().ToList();
            return result;
            //nodelist.Remove(curVert);
            


        }



        //return res;

    } /*
        public List<Vertex> IDS(Vertex entryNode, Vertex target, uint depth)
        {
            List<Vertex> res = new();
            return res;
        }
        public List<Vertex> DLS(Vertex entryNode, Vertex target)
        {
            List<Vertex> res = new();
            return res;
        }
    */
}
