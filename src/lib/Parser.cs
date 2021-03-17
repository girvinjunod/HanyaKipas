using HanyaKipas.Lib;
using System.Linq;

namespace HanyaKipas.Lib
{
    class Parser
    {
        private readonly string path;
        public Graph HasilParse { get; set; }

        public Parser(string _path)
        {
            path = _path;
            HasilParse = new Graph();
        }

        public void Parse()
        {
            string[] lines = System.IO.File.ReadAllLines(@path);
            foreach (string line in lines.Skip(1))
            {
                int temp = line.IndexOf(" ");
                string node1 = line.Substring(0, temp);
                string node2 = line.Substring(temp + 1);
                HasilParse.AddEdge(new Node(node1), new Node(node2));
            }
        }
    }
}