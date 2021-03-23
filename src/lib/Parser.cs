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
            HasilParse.Clear();
            string[] lines = System.IO.File.ReadAllLines(@path);
            try
            {
                foreach (string line in lines)
                {
                    int temp = line.IndexOf(" ");
                    string node1 = line.Substring(0, temp);
                    string node2 = line.Substring(temp + 1);
                    HasilParse.AddEdge(new Node(node1), new Node(node2));
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                //HadehMoment.ShowError("Gagal parsing berkas graf.");
                throw new System.ArgumentOutOfRangeException();
            }
        }
    }
}