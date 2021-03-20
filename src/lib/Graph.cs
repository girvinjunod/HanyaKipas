using Vertex = HanyaKipas.Lib.Node;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;

namespace HanyaKipas.Lib
{
    static class Globals
    {
        public static bool dfsfound = false;
        public static List<Vertex> nodelist = new();
    }
    public class Graph
    {
        private Dictionary<Vertex, LinkedList<Vertex>> adjList = new();
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

        // Searching!
        /// <summary>
        /// Mencari sudut-sudut yang harus dilalui untuk mencapai suatu sudut
        /// dari sudut lain menggunakan metode BFS
        /// </summary>
        /// <param name="entry">sudut asal</param>
        /// <param name="target">sudut tujuan</param>
        /// <returns>list vertex (sudut) yang harus dilalui</returns>
        public List<Vertex> BFS(Vertex entry, Vertex target)
        {
            Queue<Vertex> toVisit = new(); // vertex yang akan dikunjungi
            // mencatat vertex pendahulu dari suatu vertex
            Dictionary<Vertex, Vertex> memory = new();
            Dictionary<Vertex, bool> visited;
            List<Vertex> res = new(); // menyimpan vertex hasil
            Vertex curVert; // vertex yang sedang dikunjungi
            bool found = target == entry;

            visited = new();
            foreach (KeyValuePair<Vertex, LinkedList<Vertex>> kvp in adjList)
            {
                visited.Add(kvp.Key, false);
            }
            memory.Add(entry, entry);

            // akan divisit node entry
            toVisit.Enqueue(entry);
            while (toVisit.Count > 0 && !found)
            {
                // tentuin sudut yang akan divisit
                curVert = toVisit.Dequeue();
                // apakah sudut sudah ketemu?
                found = target.Equals(curVert);
                // kalau belum divisit dan belum ketemu, visit sudutnya
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
                    visited[curVert] = true; // tandain udah divisit
                }
            }

            // menambahkan sudut dari target sampai entry
            if (found)
            {
                curVert = target;
                do
                {
                    res.Add(curVert);
                    curVert = memory[curVert];
                } while (!curVert.Equals(entry));
                res.Add(curVert);
            }

            return res;
        }
        /// <summary>
        /// Mencari sudut-sudut yang harus dilalui untuk mencapai suatu sudut
        /// dari sudut lain menggunakan metode DFS
        /// </summary>
        /// <param name="entry">sudut asal</param>
        /// <param name="target">sudut tujuan</param>
        /// <returns>list vertex (sudut) yang harus dilalui</returns>
        /*public List<Vertex> DFS(Vertex entry, Vertex target)
        {
            Dictionary<Vertex, bool> visited;
            Stack<Vertex> verStack = new();
            List<Vertex> res = new();
            Dictionary<Vertex, Vertex> memory = new();
            Vertex popped;
            Vertex previous = entry;
            bool found = false;

            visited = new();
            foreach (KeyValuePair<Vertex, LinkedList<Vertex>> kvp in adjList)
            {
                visited.Add(kvp.Key, false);
            }

            verStack.Push(entry);
            while (!found && verStack.Count > 0)
            {
                uint jmlMsk = 0;
                popped = verStack.Pop(); // curVert
                if (popped.Equals(target)) // kalo udah ketemu
                {
                    found = true;
                    memory.Add(popped, previous);
                }
                else
                {
                    // masukin tetangga2nya ke stack
                    for (LinkedListNode<Vertex> vn = adjList[popped].Last;
                         vn != null;
                         vn = vn.Previous)
                    {
                        Vertex vertex = vn.Value;

                        if (!visited[vertex])
                        {
                            verStack.Push(vertex);
                            jmlMsk++;
                        }
                    }
                    visited[popped] = true; // tandain udah divisit
                    memory.Add(popped, previous);
                    previous = jmlMsk > 0 ? popped : memory[previous];
                }
            }

            if (found)
            {
                popped = target;
                do
                {
                    res.Add(popped);
                    popped = memory[popped];
                } while (!popped.Equals(entry));
                res.Add(popped);
            }

            return res;*/
        public void DFS(Vertex entryNode, Vertex target, List<Vertex> visited)
        {
            Vertex curVert = entryNode;
            if (curVert.Equals(target)) //Jika ketemu target
            { //basis
                Globals.nodelist.Add(curVert);
                Globals.dfsfound = true;
                return;
            }
            visited.Add(curVert); //Ditambah ke visited
            foreach (Vertex vertex in adjList[curVert]) //rekursi
            {
                if (!visited.Contains(vertex) && !Globals.dfsfound) //Jika belum divisit dan belum ketemu
                {
                    DFS(vertex, target, visited); //rekursi dengan list visited baru
                }
            }
            if (Globals.dfsfound) Globals.nodelist.Add(curVert); //ditambah dari belakang saat sudah ketemu
            return;
        }
        public Dictionary<Vertex, int> FriendRecommendation(Vertex terpilih)
        {
            Dictionary<Vertex, int> countmutual = new(); //bikin dict buat ngeliat per jumlah mutual friends
            foreach (Vertex tetangga in adjList[terpilih]) //liat tetangga terpilih
            {
                foreach (Vertex tetangganya_tetangga in adjList[tetangga]) //liat tetangganya tetangga
                {
                    if (!tetangganya_tetangga.Equals(terpilih) &&!adjList[terpilih].Contains(tetangganya_tetangga)) //jika bukan terpilih atau teman terpilih
                    {
                        if (!countmutual.ContainsKey(tetangganya_tetangga)) //jika belum ada tambah 1
                        {
                            countmutual.Add(tetangganya_tetangga, 1);
                        }
                        else //jika udah ada tambahin ke yg udah ada
                        {
                            int count = countmutual[tetangganya_tetangga];
                            count += 1;
                            countmutual.Remove(tetangganya_tetangga);
                            countmutual.Add(tetangganya_tetangga, count);
                        }
                    }
                }
            }
            Debug.WriteLine("Debug Friend Rec");
            foreach (KeyValuePair<Vertex, int> kvp in countmutual) //buat debug
            {
                Debug.WriteLine("Friend Recommendation: " + kvp.Key.GetInfo());
                Debug.WriteLine("Mutual Friends: " + kvp.Value);
            }
            return countmutual;
        }
        public List<Vertex> MutualFriends(Vertex terpilih, Vertex Recommended)
        {
            List < Vertex> mutual = new();
            foreach(Vertex tetangga in adjList[terpilih])
            {
                if (adjList[Recommended].Contains(tetangga))
                {
                    mutual.Add(tetangga);
                }
            }
            return mutual;
            
        }

        }
    }
