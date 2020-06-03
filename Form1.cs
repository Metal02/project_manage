using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Minakova1
{
    public partial class Form1 : Form
    {
       
       
        public Form1()
        {
            InitializeComponent();
        }
        string[] row = new string[18];
        int M = 0;
        int[] MassVremeni = new int[14];
        int[,] MainMass = new int[0, 0];
        private void button1_Click(object sender, EventArgs e)
        {
            row[0] = "А";
            row[1] = "Б";
            row[2] = "В";
            row[3] = "Г";
            row[4] = "Д";
            row[5] = "Е";
            row[6] = "Ж";
            row[7] = "З";
            row[8] = "И";
            row[9] = "К";
            row[10] = "Л";
            row[11] = "М";
            row[12] = "Н";
            row[13] = "О";
            row[14] = "П";
            row[15] = "Р";
            row[16] = "С";
            row[17] = "Т";
            string[] str1;                        
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {        
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {                                                          
                    var fileStream = openFileDialog.OpenFile();
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                    {
                        str1 = reader.ReadToEnd().Split(new Char[] { ' ', '\r' });
                        dataGridView2.ColumnCount = 1;
                        dataGridView2.RowCount = M;
                        int pos1 = 0;
                               for (int j = 0; j < dataGridView2.RowCount; j++)
                               {                     
                                   dataGridView2[0, j].Value = str1[pos1];
                                   MassVremeni[j] = Convert.ToInt32(str1[pos1]);
                                  pos1++;
                               }
                    }
                }
            }
            for (int i = 0; i < M; i++)
            {
                dataGridView2.Rows[i].HeaderCell.Value = row[i];
            }
            dataGridView2.RowHeadersWidth = 50;
        }
        // ресайз массива
        public static void ResizeArray<T>(ref T[,] original, int cols, int rows)
        {
            T[,] newArray = new T[rows, cols];
            Array.Copy(original, newArray, original.Length);
            original = newArray;
        }
        //  работы предшествия
        private void button2_Click(object sender, EventArgs e)
        {          
            string[] str;           
            dataGridView1.RowHeadersWidth = 50;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var fileStream = openFileDialog.OpenFile();

                    using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                    {
                        str = reader.ReadToEnd().Split(new Char[] { ' ', '\r' });
                       // double N = Math.Sqrt(str.Length);
                        M = Convert.ToInt32(textBox3.Text);
                        int pos = 0;
                        dataGridView1.ColumnCount = M;
                        dataGridView1.RowCount = M;
                        ResizeArray(ref MainMass, M, M);
                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            for (int j = 0; j < dataGridView1.ColumnCount; j++)
                            {
                                if (str[pos] != "")
                                    dataGridView1[j, i].Value = str[pos];
                                    MainMass[i, j] = Convert.ToInt32(str[pos]);
                                pos++;
                            }
                            dataGridView1.Rows[i].HeaderCell.Value = row[i];
                        }
                    }
                }
            }            
        }
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView3.RowCount = M;
            dataGridView3.ColumnCount = 5;
            int[] MassNachalnyxRabot = new int[3];
            int[] MassEarlyStart = new int[M];
            int k = 0;
            int[] MassEarlyFinish = new int[M];

            // поиск начальных работ           
            for (int i = 0; i < M; i++)
            {
                int summ = 0;
                
                for (int j = 0; j < M; j++)
                {

                    summ = summ + MainMass[i, j];
                }
                if (summ < 1)
                {
                    MassNachalnyxRabot[k] = i;
                    k = k + 1;
                }
            }
            // подсчет ранних финишей и старотв начальных работ
            for (int i = 0; i < MassNachalnyxRabot.Length ; i++)
            {
                MassEarlyStart[MassNachalnyxRabot[i]] = 1;
                MassEarlyFinish[MassNachalnyxRabot[i]] = MassEarlyStart[MassNachalnyxRabot[i]] + MassVremeni[MassNachalnyxRabot[i]] - 1;
            }
            
            // заполнение ранних стартов и финишей остальных работ
            int[] massprom = new int[4];
            int max;
            int u = 0;
            for (int i = 0; i < M; i++)
            {
                for (int j =0; j < M; j++)
                {
                    if (MainMass[i,j] > 0)
                    {
                        massprom[u] = MassEarlyFinish[j];
                        u = u + 1;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (massprom[0] > 0)
                {
                    max = massprom.Max();
                    MassEarlyStart[i] = max +1;
                    MassEarlyFinish[i] = MassEarlyStart[i] + MassVremeni[i] - 1;
                    Array.Clear(massprom, 0, massprom.Length);
                    u = 0;
                    max = 0;
                }
            }
            // поиск финальных работ
            int[] MassFinRabot = new int[3];
            int[] MassLateStart = new int[M];
            int t = 0;
            int[] MassLateFinish = new int[M];                 
            for (int j = 0; j < M; j++)
            {
                int summ = 0;

                for (int i = 0; i < M; i++)
                {
                    summ = summ + MainMass[i, j];
                }
                if (summ < 1)
                {
                    MassFinRabot[t] = j;
                    t = t + 1;
                }
            }
            // подсчет поздних финишей и старотв финальных работ
            int[] massprom1 = new int[4];
            for (int i =0; i <4;i++)
            {
                massprom1[i] = 1000;
            }
            int min;
            int u1 = 0;
            for (int i =0; i< MassFinRabot.Length; i++)
            {
                MassLateFinish[MassFinRabot[i]] = MassEarlyFinish[MassFinRabot[i]];
                
            }
            int maxLateFinish = MassLateFinish.Max();
            for ( int i = 0; i< MassLateFinish.Length; i++)
            {
                if (MassLateFinish[i] > 0)
                {
                    MassLateFinish[i] = maxLateFinish;
                }
            }
            for (int i = 0; i < MassFinRabot.Length; i++)
            {
                MassLateStart[MassFinRabot[i]] = MassLateFinish[MassFinRabot[i]] - MassVremeni[MassFinRabot[i]] + 1;
            }
            // заполнение поздних стартов и финишей остальных работ
            for (int j = M-1; j > -1; j--)
            {
                for (int i = M-1; i> -1; i--)
                {
                    if (MainMass[i, j] > 0)
                    {
                        massprom1[u1] =  MassLateStart[i];
                        u1 = u1 + 1;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (massprom1[0] < 500)
                {
                    min = massprom1.Min();
                    MassLateFinish[j] = min - 1;
                    MassLateStart[j] = MassLateFinish[j] - MassVremeni[j] + 1;
                    for (int i = 0; i < 4; i++)
                    {
                        massprom1[i] = 1000;
                    }
                    u1 = 0;
                    min = 0;
                }
            }
            // резервы
            int[] MassRezerv = new int[M];
            for (int i =0; i < M; i++)
            {
                MassRezerv[i] = MassLateFinish[i] - MassEarlyFinish[i];
                dataGridView3.Rows[i].Cells[4].Value = MassRezerv[i];
            }
            // вывод всех массивов в гриды 2 и 3      
            for (int j = 0; j < M; j++)
            {
                dataGridView3.Rows[j].Cells[2].Value = MassLateStart[j];
            }
            for (int j = 0; j < M; j++)
            {
                dataGridView3.Rows[j].Cells[3].Value = MassLateFinish[j];
            }           
            for (int j = 0; j < M; j++)
            {
                dataGridView3.Rows[j].Cells[0].Value = MassEarlyStart[j];               
            }
            for (int j = 0; j < M; j++)
            {                
                  dataGridView3.Rows[j].Cells[1].Value = MassEarlyFinish[j];
            }
            // заполнение букв
            for (int i = 0; i < M; i++)
            {
                dataGridView3.Rows[i].HeaderCell.Value = row[i];
            }
            dataGridView3.RowHeadersWidth = 50;

            // критические работы
            for (int i = 0; i < M; i++)
            {
                if (MassRezerv[i] == 0)
                {
                    textBox1.Text = textBox1.Text + " ->" + Convert.ToString(row[i]);
                }
                
            }
            
        }
    }
}
