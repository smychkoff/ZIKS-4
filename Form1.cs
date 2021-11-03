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

namespace ZIKS4
{
    public partial class InputForm : Form
    {
        static Wocabulary wocabulary = new Wocabulary();
        public InputForm()
        {
            InitializeComponent();
        }
        private void addFileButton_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
            listOfFiles.Items.Add(openFileDialog.FileName);
        }
        private void removeFileButton_Click(object sender, EventArgs e)
        {
            listOfFiles.Items.RemoveAt(listOfFiles.SelectedIndex);
        }
        private void get1GramsButton_Click(object sender, EventArgs e)
        {
            dataGridView.Enabled = true;
            for (int i = 0; i < listOfFiles.Items.Count; i++)
            {
                string file = File.ReadAllText(listOfFiles.Items[i].ToString());
                file = new string(file.Where(c => !char.IsPunctuation(c)).ToArray());
                string str = "";
                for (int j = 0; j < file.Length; j++)
                {
                    if (file[j] == '\\')
                        i += 2;
                    else
                        str += file[j];
                }
                file = str;
                wocabulary.AddGrams(file);
            }
            List<float> possibilities = wocabulary.GetPossibilities();
            DataTable table = new DataTable("1-grams");
            table.Columns.Add("Грама", typeof(string));
            table.Columns.Add("Вірогідність", typeof(float));
            for (int i = 0; i < possibilities.Count; i++)
            {
                DataRow row = table.NewRow();
                row["Грама"] = wocabulary.ch[i].ToString();
                row["Вірогідність"] = (float)possibilities[i];
                table.Rows.Add(row);
            }
            dataGridView.DataSource = table;
            dataGridView.Sort(dataGridView.Columns[1], ListSortDirection.Descending);
            numberOfGramsLabel.Visible = true;
            numberOfGramsLabel.Text = "Кількість 1-грам: " + table.Rows.Count.ToString();
        }
        private void get2GramsButton_Click(object sender, EventArgs e)
        {
            if (wocabulary.ch.Count == 0)
                get1GramsButton_Click(sender, e);
            dataGridView.Enabled = true;
            string file = null;
            for (int i = 0; i < listOfFiles.Items.Count; i++)
            {
                file = File.ReadAllText(listOfFiles.Items[i].ToString());
                file = new string(file.Where(c => !char.IsPunctuation(c)).ToArray());
                string str = "";
                for (int j = 0; j < file.Length; j++)
                {
                    if (file[j] == '\\')
                        i += 2;
                    else
                        str += file[j];
                }
                file = str;
            }
            List<string> bigrams = new List<string>();
            List<int> bigramCount = new List<int>();
            List<float> bigramPossibility = new List<float>();
            for (int i = 0; i < file.Length - 1; i++)
            {
                bool found = false;
                int j = 0;
                string bigram = $"{file[i]}{file[i + 1]}";
                do
                {
                    if (j == bigrams.Count && !found)
                    {
                        found = true;
                        bigrams.Add(bigram);
                        bigramCount.Add(1);
                    }
                    else
                    {
                        if (bigram == bigrams[j])
                        {
                            found = true;
                            bigramCount[j]++;
                        }
                    }
                    j++;
                } while (!found);
            }
            float length = Convert.ToSingle(file.Length);
            foreach (int c in bigramCount)
                bigramPossibility.Add(c / length);
            DataTable table = new DataTable();
            table.Columns.Add("Грама", typeof(string));
            table.Columns.Add("Вірогідність", typeof(float));
            for (int i = 0; i < bigramPossibility.Count; i++)
            {
                DataRow row = table.NewRow();
                row["Грама"] = bigrams[i];
                row["Вірогідність"] = bigramPossibility[i];
                table.Rows.Add(row);
            }
            dataGridView.DataSource = table;
            dataGridView.Sort(dataGridView.Columns[1], ListSortDirection.Descending);
            numberOfGramsLabel.Visible = true;
            numberOfGramsLabel.Text = "Кількість 2-грам: " + table.Rows.Count.ToString();
        }
        private void get3GramsButton_Click(object sender, EventArgs e)
        {
            string file = null;
            for (int i = 0; i < listOfFiles.Items.Count; i++)
            {
                file = File.ReadAllText(listOfFiles.Items[i].ToString());
                file = new string(file.Where(c => !char.IsPunctuation(c)).ToArray());
                string str = "";
                for (int j = 0; j < file.Length; j++)
                {
                    if (file[j] == '\\')
                        i += 2;
                    else
                        str += file[j];
                }
                file = str;
            }
            List<string> threegrams = new List<string>();
            List<int> threegramCount = new List<int>();
            List<float> threegramPossibility = new List<float>();
            for (int i = 0; i < file.Length - 2; i++)
            {
                bool found = false;
                int j = 0;
                string threegram = $"{file[i]}{file[i + 1]}{file[i + 2]}";
                do
                {
                    if (j == threegrams.Count && !found)
                    {
                        found = true;
                        threegrams.Add(threegram);
                        threegramCount.Add(1);
                    }
                    else
                    {
                        if (threegram == threegrams[j])
                        {
                            found = true;
                            threegramCount[j]++;
                        }
                    }
                    j++;
                } while (!found);
            }
            float length = Convert.ToSingle(file.Length);
            foreach (int c in threegramCount)
                threegramPossibility.Add(c / length);
            DataTable table = new DataTable();
            table.Columns.Add("Грама", typeof(string));
            table.Columns.Add("Вірогідність", typeof(float));
            for (int i = 0; i < threegramPossibility.Count; i++)
            {
                DataRow row = table.NewRow();
                row["Грама"] = threegrams[i];
                row["Вірогідність"] = threegramPossibility[i];
                table.Rows.Add(row);
            }
            dataGridView.DataSource = table;
            dataGridView.Sort(dataGridView.Columns[1], ListSortDirection.Descending);
            numberOfGramsLabel.Visible = true;
            numberOfGramsLabel.Text = "Кількість 3-грам: " + table.Rows.Count.ToString();
        }
        private void resetButton_Click(object sender, EventArgs e)
        {
            numberOfGramsLabel.Visible = false;
            dataGridView.DataSource = null;
            wocabulary = new Wocabulary();
            listOfFiles.Items.Clear();
        }
        static int CalcMainColour(float lowerProbability, float higherProbability, float inputProbability, int lowerProbabilityColour, int higherProbabilityClolour)
        {
            return Convert.ToInt32((lowerProbabilityColour * higherProbability + inputProbability * (higherProbabilityClolour - lowerProbabilityColour) - lowerProbability * higherProbabilityClolour) / (higherProbability - lowerProbability));
        }
        static string GetColour(float lowestProbability, float highestProbability, float inputProbability)
        {
            string str = "";
            if (inputProbability >= lowestProbability && inputProbability < (highestProbability - lowestProbability) / 2)
            {
                string r = CalcMainColour(lowestProbability, highestProbability / 2, inputProbability, 10, 120).ToString("X");
                if (r.Length < 2)
                    r = "0" + r;
                str += r + "0A76";
            }
            else
            {
                string b = CalcMainColour(highestProbability / 2, highestProbability, inputProbability, 120, 10).ToString("X");
                if (b.Length < 2)
                    b = "0" + b;
                str = "760A" + b;
            }
            return str;
        }
        private void show2GramsMatrixButton_Click(object sender, EventArgs e)
        {
            float currentPossibility;
            string bigram;
            if (wocabulary.ch.Count == 0)
                get2GramsButton_Click(sender, e);
            DataTable table = dataGridView.DataSource as DataTable;
            DataView view = table.DefaultView;
            view.Sort = "Вірогідність desc";
            table = view.ToTable();
            float lowestPossibility = (float)table.Rows[table.Rows.Count - 1][1];
            float highestPossibility = (float)table.Rows[0][1];
            DataTable matrix = new DataTable();
            matrix.Columns.Add("   ");
            for (int i = 0; i < wocabulary.ch.Count; i++)
                matrix.Columns.Add($"{wocabulary.ch[i]}");
            for (int i = 0; i < wocabulary.ch.Count; i++)
                matrix.Rows.Add($"{wocabulary.ch[i]}");
            dataGridView.DataSource = matrix;
            dataGridView.Columns[0].Frozen = true;
            foreach (DataGridViewColumn c in dataGridView.Columns)
                c.Width = 20;
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                for (int j = 1; j < dataGridView.Columns.Count; j++)
                {
                    currentPossibility = 0;
                    bigram = $"{wocabulary.ch[i]}{wocabulary.ch[j - 1]}";
                    for (int k = 0; k < table.Rows.Count; k++)
                    {
                        if ((string)table.Rows[k][0] == bigram)
                            currentPossibility = (float)table.Rows[k][1];
                    }
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    if (currentPossibility != 0)
                        style.BackColor = Color.FromArgb(int.Parse("FF" + GetColour(lowestPossibility, highestPossibility, currentPossibility), System.Globalization.NumberStyles.HexNumber));
                    else
                        style.BackColor = Color.Black;
                    dataGridView.Rows[i].Cells[j].Style = style;
                }
            }
            dataGridView.Refresh();
        }
        private void Encode(DataTable table)
        {
            foreach (string path in listOfFiles.Items)
            {
                string file = File.ReadAllText(path);
                string wPath = GetEncodedFilePath(path);
                using (StreamWriter writer = new StreamWriter(wPath))
                {
                    for (int i = 0; i < file.Length; i++)
                    {
                        for (int j = 0; j < table.Rows.Count; j++)
                        {
                            if (Convert.ToChar(table.Rows[j][0]) == file[i])
                                writer.Write($"{j} ");
                        }
                    }
                }
            }
        }
        private void encodeButton_Click(object sender, EventArgs e)
        {
            DataTable table = dataGridView.DataSource as DataTable;
            DataView view = table.DefaultView;
            view.Sort = "Вірогідність desc";
            table = view.ToTable();
            if (table.Columns.Count == 2)
            {
                Encode(table);
            }
        }
        private string GetEncodedFilePath(string inputPath)
        {
            string[] temp = inputPath.Split("\\");
            string fileName = temp[temp.Length - 1].Split('.')[0];
            string wPath = null;
            for (int i = 0; i < temp.Length - 1; i++)
            {
                wPath += temp[i] + "\\";
            }
            wPath += fileName + "_encoded.txt";
            return wPath;
        }
        private void Decode(DataTable table)
        {
            foreach (string path in listOfFiles.Items)
            {
                string wPath = getDecodedFilePath(path);
                string[] file = File.ReadAllText(path).Split();
                using (StreamWriter writer = new StreamWriter(wPath))
                {
                    for (int i = 0; i < file.Length - 1; i++)
                    {
                        writer.Write(table.Rows[Convert.ToInt32(file[i])][0]);
                    }
                }
            }
        }
        private string getDecodedFilePath(string inputPath)
        {
            string[] temp = inputPath.Split("\\");
            string fileName = temp[temp.Length - 1].Split('.')[0];
            string wPath = null;
            for (int i = 0; i < temp.Length - 1; i++)
            {
                wPath += temp[i] + "\\";
            }
            wPath += fileName + "_decoded.txt";
            return wPath;
        }
        private void decodeButton_Click(object sender, EventArgs e)
        {
            DataTable table = dataGridView.DataSource as DataTable;
            DataView view = table.DefaultView;
            view.Sort = "Вірогідність desc";
            table = view.ToTable();
            if (table.Columns.Count == 2)
            {
                Decode(table);
            }
        }
    }
}
