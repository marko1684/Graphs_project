using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Grafovi_projekat
{
    public partial class Grafovi : Form
    {
        public Grafovi()
        {
            InitializeComponent();
        }

        List<Cvor> nodes = new List<Cvor>();
        List<Ivica> edges = new List<Ivica>();
        StreamWriter sw;

        private void DodajCvor(int tempX,int tempY,string tempIme)
        {
            
            if (tempIme == "")
            {
                MessageBox.Show("Unesi ime čvora!");
                return;
            }    
            Cvor tempC = new Cvor(tempX,tempY,tempIme);
            nodes.Add(tempC);
            updateComboBox(tempIme);
            pictureBox1.Refresh();
        }

        private void updateComboBox(string ime)
        {
            if (ime != "")
            {
                comboBox1.Items.Add(ime);
                comboBox2.Items.Add(ime);
            }
            else
            {
                comboBox1.Items.Clear();
                comboBox2.Items.Clear();
                for(int i = 0; i < nodes.Count; i++)
                {
                    updateComboBox(nodes[i].name);
                }
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (radioButton1.Checked)
            {
                string tempIme = textBox3.Text;
                textBox1.Text = e.X.ToString();
                textBox2.Text = e.Y.ToString();
                DodajCvor(e.X,e.Y,tempIme);
            }
            if (radioButton2.Checked)
            {
                int x = e.X;
                int y = e.Y;
                List<int> k = new List<int>();
                for (int i = 0; i < nodes.Count; i++)
                {
                    if ((Math.Sqrt(Math.Pow((Convert.ToInt32(nodes[i]._x1) - x), 2) + Math.Pow(Convert.ToInt32(nodes[i]._y1) - y, 2))) <= 40)
                    {                      
                        for(int j = 0; j < edges.Count; j++)
                        {
                            if (edges[j].NodeName1 == nodes[i].name || edges[j].NodeName2 == nodes[i].name)
                            {
                                k.Add(j);
                            }
                        }
                        nodes.Remove(nodes[i]);
                        updateComboBox("");
                        int nEdges = edges.Count;
                        for (int c = 0; c < k.Count ; c++)
                        {
                            Ivica Itemp = edges[nEdges - 1];
                            edges[nEdges - 1] = edges[k[c]];
                            edges[k[c]] = Itemp;
                            nEdges--;
                        }
                        for(int j = 0; j < k.Count; j++)
                        {
                            edges.Remove(edges[edges.Count - 1]);
                        }
                            pictureBox1.Refresh();   
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                string tempIme = textBox3.Text;                              
                int tempX = Convert.ToInt32(textBox1.Text);
                int tempY = Convert.ToInt32(textBox2.Text);
                DodajCvor(tempX, tempY, tempIme);
            }
            if (radioButton2.Checked)
            {
                List<int> k = new List<int>();
                int[] names = new int[edges.Count];
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (textBox3.Text==nodes[i].name)
                    {
                        for (int j = 0; j < edges.Count; j++)
                        {
                            if (edges[j].NodeName1 == nodes[i].name || edges[j].NodeName2 == nodes[i].name)
                            {
                                k.Add(j);
                            }
                        }
                        nodes.Remove(nodes[i]);
                        int nEdges = edges.Count;
                        for (int c = 0; c < k.Count; c++)
                        {
                            Ivica Itemp = edges[nEdges - 1];
                            edges[nEdges - 1] = edges[k[c]];
                            edges[k[c]] = Itemp;
                            nEdges--;
                        }
                        for (int j = 0; j < k.Count; j++)
                        {
                            edges.Remove(edges[edges.Count - 1]);
                        }
                        updateComboBox("");
                            pictureBox1.Refresh();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int weight = Convert.ToInt32(textBox6.Text);
            if (comboBox1.SelectedItem != null || comboBox2.SelectedItem != null)
            {
                Ivica edge = new Ivica(comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString(), weight);
                for(int i = 0; i < edges.Count; i++)
                {
                    if(comboBox1.SelectedItem.ToString() == edges[i].NodeName1 && comboBox2.SelectedItem.ToString() == edges[i].NodeName2)
                    {
                        edges.Remove(edges[i]);
                    }
                }
                edges.Add(edge);
                pictureBox1.Refresh();
            }
            else
            {
                MessageBox.Show("Izaberi ivice!");
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Font font = new Font("Arial", 12.0f);
            Pen olovka = new Pen(Color.Red, 3);
            SolidBrush cetka = new SolidBrush(Color.Blue);
            SolidBrush cetka1 = new SolidBrush(Color.Red);
            for (int i = 0; i < edges.Count; i++)
            {
                int x1 = 0;
                int y1 = 0;
                int x2 = 0;
                int y2 = 0;
                for(int j = 0; j < nodes.Count; j++)
                {
                    if (nodes[j].name == edges[i].NodeName1)
                    {
                        x1 = nodes[j]._x1;
                        y1 = nodes[j]._y1;
                    }
                    if (nodes[j].name == edges[i].NodeName2)
                    {
                        x2 = nodes[j]._x1;
                        y2 = nodes[j]._y1;
                    }
                }
                e.Graphics.DrawLine(olovka, x1, y1, x2, y2);
                e.Graphics.DrawString(edges[i].weight.ToString(), font, cetka, (x1 + x2) / 2, (y1 + y2) / 2);
            }
            for (int j = 0; j < nodes.Count; j++)
            {
                Point A = new Point(nodes[j]._x1 - 10, nodes[j]._y1 - 10);           
                e.Graphics.FillEllipse(cetka1, Math.Abs(nodes[j]._x1) - 20, Math.Abs(nodes[j]._y1 - 20), 40, 40);
                e.Graphics.DrawString(nodes[j].name, font, cetka, A);
            }
        }

        private int MinimumDistance(int[] distance, bool[] shortestPathTreeSet, int verticesCount)
        {
            int min = int.MaxValue;
            int minIndex = 0;

            for (int v = 0; v < verticesCount; ++v)
            {
                if (shortestPathTreeSet[v] == false && distance[v] <= min)
                {
                    min = distance[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }

        public void DijkstraAlgo(int[,] graph, int source, int verticesCount)
        {
            int[] distance = new int[verticesCount];
            bool[] shortestPathTreeSet = new bool[verticesCount];

            for (int i = 0; i < verticesCount; ++i)
            {
                distance[i] = int.MaxValue;
                shortestPathTreeSet[i] = false;
            }
            distance[source] = 0;
            for (int count = 0; count < verticesCount - 1; ++count)
            {
                int u = MinimumDistance(distance, shortestPathTreeSet, verticesCount);
                shortestPathTreeSet[u] = true;    
                for (int v = 0; v < verticesCount; ++v)
                    if (!shortestPathTreeSet[v] && Convert.ToBoolean(graph[u, v]) && distance[u] != int.MaxValue && distance[u] + graph[u, v] < distance[v])
                    {
                        distance[v] = distance[u] + graph[u, v];
                    }
            }
            for(int i = 0; i < distance.Length; i++)
            {
                string s = nodes[i].name;
                    s += " : ";
                listBox1.Items.Add(s + distance[i].ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int[,] matrix = new int[nodes.Count, nodes.Count];
            for (int i = 0; i < edges.Count; i++)
            {
                int x = 0;
                int y = 0;
                for (int j = 0; j < nodes.Count; j++)
                {
                    if (edges[i].NodeName1 == nodes[j].name) x = j;
                    if (edges[i].NodeName2 == nodes[j].name) y = j;
                }
                matrix[x, y] = edges[i].weight;
                matrix[y, x] = edges[i].weight;
            }

            DijkstraAlgo(matrix, 0, nodes.Count);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            sw = new StreamWriter("D:/grafovitxt.txt", true);
            sw.WriteLine(nodes.Count);
            for (int i = 0; i < nodes.Count; i++)
            {
                sw.WriteLine(nodes[i]._x1 + " " + nodes[i]._y1 + " " + nodes[i].name);
            }
            sw.WriteLine(edges.Count);
            for (int i = 0; i < edges.Count; i++)
            {
                sw.WriteLine(edges[i].NodeName1 + " " + edges[i].NodeName2 + " " + edges[i].weight.ToString());
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("D:/grafovitxt.txt");
            int i = Convert.ToInt32(sr.ReadLine()) - 1;
            while (i >= 0)
            {
                string[] s = sr.ReadLine().Split(' ');
                DodajCvor(Convert.ToInt32(s[0]), Convert.ToInt32(s[1]), s[2]);
                i--;
            }
            i = Convert.ToInt32(sr.ReadLine()) - 1;

            while (i >= 0)
            {
                string[] s = sr.ReadLine().Split(' ');
                Ivica ivc = new Ivica(s[0], s[1], Convert.ToInt32(s[2]));
                edges.Add(ivc);
                i--;
            }
            sr.Close();
            File.WriteAllText("D:/grafovitxt.txt", String.Empty);
            pictureBox1.Refresh();
        }

        int V = 0;
        int minKey(int[] key, bool[] mstSet)
        {
            int min = int.MaxValue, min_index = -1;

            for (int v = 0; v < V; v++)
                if (mstSet[v] == false && key[v] < min)
                {
                    min = key[v];
                    min_index = v;
                }

            return min_index;
        }

        void printMST(int[] parent, int n, int[,] graph)
        {
            for (int i = 1; i < V; i++)
            {
                listBox2.Items.Add(nodes[parent[i]].name + " - " + nodes[i].name + "\t" + graph[i, parent[i]]);

            }
        }
        void primMST(int[,] graph)
        {
            V = nodes.Count;
            int[] parent = new int[V];
            int[] key = new int[V];
            bool[] mstSet = new bool[V];
            for (int i = 0; i < V; i++)
            {
                key[i] = int.MaxValue;
                mstSet[i] = false;
            }
            key[0] = 0;
            parent[0] = -1;

            for (int count = 0; count < V - 1; count++)
            {
                int u = minKey(key, mstSet); 
                mstSet[u] = true;
                for (int v = 0; v < V; v++)
                    if (graph[u, v] != 0 && mstSet[v] == false &&
                                            graph[u, v] < key[v])
                    {
                        parent[v] = u;
                        key[v] = graph[u, v];
                    }
            } 
            printMST(parent, V, graph);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int[,] graph = new int[nodes.Count, nodes.Count];
            for (int i = 0; i < edges.Count; i++)
            {
                int x = 0;
                int y = 0;
                for (int j = 0; j < nodes.Count; j++)
                {
                    if (edges[i].NodeName1 == nodes[j].name) x = j;
                    if (edges[i].NodeName2 == nodes[j].name) y = j;
                }
                graph[x, y] = edges[i].weight;
                graph[y, x] = edges[i].weight;
            }
            primMST(graph);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Unos u textBox koji je oznacen sa X koordinata predstavlja x koordinatu na kojoj ce se prikazati novi cvor \n " +
                "            Unos u textBox koji je oznacen sa Y koordinata predstavlja y koordinatu na kojoj ce se prikazati novi cvor \n" +
                "            Unos u textBox koji je oznacen sa Ime Cvora unosi se ime cvora koji se unosi i on ce biti upisan unutar njega \n" +
                "            Klikom na dugme primeni ako je selektovan prvi radioButton sa nazivom Dodaj bice dodat nov cvor \n" +
                "            Kada je selektovan drugi radioButton sa nazivom obrisi pri cemu je uneto ime cvora klikom na dugme Primeni taj cvor ce biti obrisan \n" +
                "            Drugi nacin jeste selektovanje radioButton-a sa nazivom obrisi i levi klik na bilo koji od cvorova pri cemu ce se obrisati kliknuti cvor i sve ivice vezane za njega \n" +
                "            Drugi nacin dodavnja cvora jeste levim klikom misa na formu gde se nalaze cvorovi pri cemu textBox sa imenom cvora mora biti popunjen \n" +
                "            Selektovanjem cvorova u ComboBox-ovima i unosom broja(tezine) u textBox koji je oznaces sa tezina i pritiskom na dugme Dodaj, dodace se nova ivica izmedju ta dva cvora. \n" +
                "            Klikom na dugme Minimum Spanning Tree ili Najkrace rastojanje, bice primenjeni algoritmi koji za dati graf bas to i rade i prikazace rezultat u listBox-ovima ispod \n");
        }
    }
}

