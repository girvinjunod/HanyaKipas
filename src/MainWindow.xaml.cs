﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Linq;
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

            try // in case ada error di BFS/DFS, atau di tempat lainnya
            {
                List<Node> nodes;
                if ((bool)RadioBFS.IsChecked)
                {
                    nodes = g1.BFS(new Node(Node1.Text), new Node(Node2.Text));
                }
                else
                {
                    Globals.nodelist = new(); //reset global variabel
                    Globals.dfsfound = false;
                    g1.DFS(new Node(Node1.Text), new Node(Node2.Text), new List<Node>());
                    nodes = Globals.nodelist;
                }

                Dictionary<Node, int> priend = g1.FriendRecommendation(new Node(Node1.Text));
                String result = "";
                //Node a = new Node(Node1.Text);

                result += "Friend Exploration with : "+ Node2.Text  + "\n" ;
                nodes.Reverse();
                foreach (Node apel in nodes)
                {
                    result += apel.GetInfo();
                    if (!nodes[nodes.Count - 1].Equals(apel))
                    {
                        result += "-> ";
                    }
                    //result += "\n";
                }
                result += "\n";
                nodes.Reverse();

                result += "Friend Reccomendation : \n";
                Debug.WriteLine("Debug Mutual friends");
                foreach (KeyValuePair<Node, int> n in priend)
                {
                    List<Node> mutual = g1.MutualFriends(new Node(Node1.Text), n.Key);
                    Debug.WriteLine("Mutual Friend dari " + n.Key.GetInfo());
                    result += "Mutual Friend dari " + n.Key.GetInfo() + " :\n";
                    foreach (Node aw in mutual)
                    {
                        result += aw.GetInfo();
                        if (!mutual[mutual.Count - 1].Equals(aw)){
                            result +=  ", ";
                        }
                        Debug.WriteLine(aw.GetInfo());
                    }
                    result += "\n";
                }
                ResultLabel.Content = result;

                //g1.FriendRecommendation(new Node(Node1.Text));

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
                                    for (int i = 0; i < nodes.Count - 1; i++)
                                    {
                                        if ((nodes[i].GetInfo() == sisi.Target && nodes[i + 1].GetInfo() == sisi.Source)
                                            || (nodes[i].GetInfo() == sisi.Source && nodes[i + 1].GetInfo() == sisi.Target))
                                        {
                                            sisi.Attr.Color = new Microsoft.Msagl.Drawing.Color(163, 191, 69);
                                            sisi.Attr.LineWidth = 2;
                                        }
                                    }
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
                try
                {
                    if (Node1.Text == Node2.Text)
                    {
                        graph.FindNode(Node1.Text).Attr.FillColor = Microsoft.Msagl.Drawing.Color.BlanchedAlmond;
                    }
                    else
                    {
                        graph.FindNode(Node1.Text).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Aqua;
                        graph.FindNode(Node2.Text).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
                    }
                }
                catch { } // ini try-catch buat apa? v':

                SilumanForm.SuspendLayout();
                try // try-catch in case of belom di-set item apa2 di SilumanForm
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

                foreach (Node fill in nodes)
                {
                    if (fill.GetInfo() != Node1.Text && fill.GetInfo() != Node2.Text)
                    {
                        graph.FindNode(fill.GetInfo()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Yellow;
                    }
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Graf belum dipilih.");
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Hapus item-item dari combobox Node1 dan Node2 ketika mau dimasukkan file
            Node1.Items.Clear();
            Node2.Items.Clear();
            // Set text di combo box
            Node1.Text = "Choose A Node...";
            Node2.Text = "Choose A Node...";

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
                Label2.Content = filePath[^1]; // sama aja [-1] di Python
                p = new(dlg.FileName);
            }

            try // in case user batalin milih file
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
        private void Window_Loaded(object sender, RoutedEventArgs e) { }
    }
}
