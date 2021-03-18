using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Graph g1;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnClickP1(object sender, RoutedEventArgs e)
        {
            //create a viewer object
            Microsoft.Msagl.GraphViewerGdi.GViewer GraphViewer = new();
            //create a graph object
            Microsoft.Msagl.Drawing.Graph graph = new("graph");

            Boolean cek = true;

            try
            {
                g1.Print();

                //create the graph content
                foreach (KeyValuePair<Node, LinkedList<Node>> entry in g1.GetAdjList())
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
                graph.FindNode(Node1.Text).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Aqua;
                graph.FindNode(Node2.Text).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;

                SilumanForm.SuspendLayout();
                try
                {
                    SilumanForm.Controls.RemoveAt(0);
                }
                catch { }
                //bind the graph to the viewer
                GraphViewer.Graph = graph;
                GraphViewer.Dock = DockStyle.Fill;
                SilumanForm.Controls.Add(GraphViewer);
                SilumanForm.ResumeLayout();
                SilumanForm.Show();
            }
            catch { }
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

            try
            {
                p.Parse();
                g1 = p.HasilParse;
                foreach (KeyValuePair<Node, LinkedList<Node>> entry in g1.GetAdjList())
                {
                    Node1.Items.Add(entry.Key.GetInfo());
                    Node2.Items.Add(entry.Key.GetInfo());
                }
            }
            catch { }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e) {}

    }
}

