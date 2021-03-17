using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using HanyaKipas.Lib;
using System.IO;

namespace HanyaKipas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //create a form 

            /// InitializeComponent();
        }

        private void btnClickP1(object sender, RoutedEventArgs e)
        {
            //Node a = new("Alex");
            //Node g = new("Girvin");
            //Node j = new("Josep");
            Graph g1 = new Graph();
            bool cek = true;
            string path = Directory.GetCurrentDirectory();
            //Console.Write("Input nama file .txt: ");
            string inputted = "../../../../test/apel.txt";
            path = path + "\\" + inputted;

            string[] lines = System.IO.File.ReadAllLines(@path);
            //System.Console.WriteLine("Banyak sisi = " + lines[0]);
            foreach (string line in lines.Skip(1))
            {
                int temp = line.IndexOf(" ");
                string node1 = line.Substring(0, temp);
                string node2 = line.Substring(temp + 1);
                g1.AddEdge(new Node(node1), new Node(node2));
                //Console.WriteLine("AddEdge(" + node1 + "," + node2 + ")");
                // Ganti AddEdge(node1,node2);
            }



            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            //create the graph content 
            //foreach(Vertex, LinkedList<Vertex> entry in g1.Dictionary)

            foreach(KeyValuePair<Node, LinkedList<Node>> entry in g1.Kamus())
            {
                foreach (Node vert in entry.Value)
                {
                    if (graph.EdgeCount != 0) 
                    {
                        foreach (Microsoft.Msagl.Drawing.Edge sisi in graph.Edges)
                        {
                            if ((sisi.Target == entry.Key.GetInfo() && sisi.Source == vert.GetInfo()) || ((sisi.Source == entry.Key.GetInfo() && sisi.Target == vert.GetInfo())))
                            {
                                cek = false;
                                break;
                            }
                        }
                    }
                    if (cek)
                    {
                        graph.AddEdge(entry.Key.GetInfo(), vert.GetInfo()).Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
                    }
                    cek = true;
                }
            }
            
            graph.FindNode("Apel").Attr.FillColor = Microsoft.Msagl.Drawing.Color.LimeGreen;
            graph.FindNode("Babi").Attr.FillColor = Microsoft.Msagl.Drawing.Color.PaleVioletRed;
            graph.FindNode("Cucurut").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Pink;
            graph.FindNode("Daddy").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Gainsboro;

            //bind the graph to the viewer 
            viewer.Graph = graph;
            //associate the viewer with the form 
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            label1.Content = viewer.Dock;
            //show the form 
            form.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}

