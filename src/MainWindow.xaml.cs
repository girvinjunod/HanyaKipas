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
using swf = System.Windows.Forms;

namespace HanyaKipas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Parser p;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnClickP1(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            //create a viewer object
            Microsoft.Msagl.WpfGraphControl.GraphViewer viewer = new Microsoft.Msagl.WpfGraphControl.GraphViewer();
            //create a graph object
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            //create the graph content
            //foreach(Vertex, LinkedList<Vertex> entry in g1.Dictionary)

            Boolean cek = true;

            p.Parse();
            Graph g1 = p.HasilParse;

            foreach(KeyValuePair<Node, LinkedList<Node>> entry in g1.GetAdjList())
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

            // graph.FindNode("Apel").Attr.FillColor = Microsoft.Msagl.Drawing.Color.LimeGreen;
            // graph.FindNode("Babi").Attr.FillColor = Microsoft.Msagl.Drawing.Color.PaleVioletRed;
            // graph.FindNode("Cucurut").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Pink;
            // graph.FindNode("Daddy").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Gainsboro;

            //bind the graph to the viewer

            viewer.Graph = graph;
            viewer.BindToPanel(dockPanel);
            //associate the viewer with the form
            //Demon. SuspendLayout();
            //viewer.Dock = System.Windows.Forms.DockStyle.Fill;

            viewer.BindToPanel(Grid1);
            //form.ResumeLayout();
            //label1.Content =
            //show the form
            //form.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".txt";
            dlg.Filter = "TXT Files (*.txt)|*.txt";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string[] filePath = dlg.FileName.Split("\\");
                Label2.Content = filePath[filePath.Length-1];
                p = new(dlg.FileName);
            }
        }

    }

}

