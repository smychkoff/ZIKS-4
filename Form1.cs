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
        string inputText = "";
        static List<DataTable> gramTables;
        GramForm gramForm = new GramForm();
        BigramMatrixForm bigramMatrixForm = new BigramMatrixForm();
        List<char> vocabulary = new List<char> () {'А', 'а', 'Б', 'б', 'В', 'в', 'Г', 'г', 'Ґ', 'ґ', 'Д', 'д', 'Е', 'е', 'Є', 'є', 'Ж', 'ж', 'З', 'з', 'И', 'и', 'І', 'і', 'Ї', 'ї', 'Й', 'й', 'К', 'к', 'Л', 'л', 'М', 'м', 'Н', 'н', 'О', 'о', 'П', 'п', 'Р', 'р', 'С', 'с', 'Т', 'т', 'У', 'у', 'Ф', 'ф', 'Х', 'х', 'Ц', 'ц', 'Ч', 'ч', 'Ш', 'ш', 'Щ', 'щ', 'Ь', 'ь', 'Ю', 'ю', 'Я', 'я', ' ', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '+', '~', '@', '#', '$', '%', '\t', '\n'};
        
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
            if(listOfFiles.SelectedIndex != -1)
                listOfFiles.Items.RemoveAt(listOfFiles.SelectedIndex);
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            listOfFiles.Items.Clear();
            gramForm.Hide();
            bigramMatrixForm.Hide();
            gramTables = new List<DataTable>();
            inputText = "";
        }

        private void GetInputText()
        {
            for (int i = 0; i < listOfFiles.Items.Count; i++)
            {
                inputText = File.ReadAllText(listOfFiles.Items[i].ToString());
                inputText = new string(inputText.Where(c => !char.IsPunctuation(c)).ToArray());
                string tempText = "";
                for (int j = 0; j < inputText.Length; j++)
                {
                    if (inputText[j] == '\\')
                        i += 2;
                    else
                        tempText += inputText[j];
                }
                inputText = tempText;
            }
            gramTables = new List<DataTable>();
        }

        private (List<string>, List<float>) GetGrams(int flag)
        {
            int j;
            bool found;
            string gram;
            List<string> grams = new List<string>();
            List<int> gramCount = new List<int>();
            List<float> gramPossibility = new List<float>();
            for (int i = 0; i < inputText.Length - flag + 1; i++)
            {
                found = false;
                j = 0;
                switch(flag)
                {
                    case 1:
                        gram = $"{inputText[i]}";
                        break;
                    case 2:
                        gram = $"{inputText[i]}{inputText[i + 1]}";
                        break;
                    case 3:
                        gram = $"{inputText[i]}{inputText[i + 1]}{inputText[i + 2]}";
                        break;
                    default:
                        gram = $"{inputText[i]}";
                        break;
                }
                do
                {
                    if (j == grams.Count && !found)
                    {
                        found = true;
                        grams.Add(gram);
                        gramCount.Add(1);
                    }
                    else
                    {
                        if (gram == grams[j])
                        {
                            found = true;
                            gramCount[j]++;
                        }
                    }
                    j++;
                } while (!found);
            }
            float length = Convert.ToSingle(inputText.Length);
            foreach (int c in gramCount)
                gramPossibility.Add(c / length);
            return (grams, gramPossibility);
        }

        private void getGramsButton_Click(object sender, EventArgs e)
        {
            GetInputText();
            
            if (inputText.Length > 2)
            {
                string tableName;
                for (int i = 1; i < 4; i++)
                {
                    (List<string> grams, List<float> gramPossibility) = GetGrams(i);
                    tableName = i + "-grams";
                    gramTables.Add(new DataTable(tableName));
                    gramTables[i - 1].Columns.Add("Грама", typeof(string));
                    gramTables[i - 1].Columns.Add("Вірогідність", typeof(float));
                    for (int j = 0; j < gramPossibility.Count; j++)
                    {
                        DataRow row = gramTables[i - 1].NewRow();
                        row["Грама"] = grams[j];
                        row["Вірогідність"] = gramPossibility[j];
                        gramTables[i - 1].Rows.Add(row);
                    }
                }
                gramForm.Hide();
                gramForm.gramTables = gramTables;
                gramForm.Show();
                bigramMatrixForm.Hide();
                bigramMatrixForm.gramTables = gramTables;
                bigramMatrixForm.Show();
            }
        }

        private int GCB(int a, int b)
        {
            if (b == 0)
                return a;
            return GCB(b, a % b);
        }

        private string GetFilePath(string inputPath, int flag)
        {
            string[] temp = inputPath.Split("\\");
            string fileName = temp[temp.Length - 1].Split('.')[0];
            string wPath = null;
            for (int i = 0; i < temp.Length - 1; i++)
            {
                wPath += temp[i] + "\\";
            }
            switch (flag)
            {
                case 1:
                    wPath += fileName + "_encoded.txt";
                    break;
                case 2:
                    wPath += fileName + "_decoded.txt";
                    break;
            }
            return wPath;
        }

        private void Encode()
        {
            string inpText;
            int a = 3, b = 6, currentIndex;
            foreach (string path in listOfFiles.Items)
            {
                inpText = File.ReadAllText(path);
                using (StreamWriter encodenFileWriter = new StreamWriter(GetFilePath(path, 1)))
                {
                    foreach (char c in inpText)
                    {
                        currentIndex = vocabulary.IndexOf(c);
                        if (currentIndex >= 0)
                            encodenFileWriter.Write(vocabulary[(a * currentIndex + b) % vocabulary.Count()]);
                    }
                }
            }
        }

        private void encodeButton_Click(object sender, EventArgs e)
        {
            Encode();
        }

        public int GetInversed(int a)
        {
            for (int i = 1; i < vocabulary.Count + 1; i++)
            {
                if ((a * i) % vocabulary.Count == 1)
                {
                    return i;
                }
            }
            return 0;
        }

        private void Decode()
        {
            string inpText;
            int a = 3, b = 6, currentIndex;
            foreach (string path in listOfFiles.Items)
            {
                inpText = File.ReadAllText(path);
                using (StreamWriter decodedFileWriter = new StreamWriter(GetFilePath(path, 2)))
                {
                    foreach (char c in inpText)
                    {
                        currentIndex = vocabulary.IndexOf(c);
                        if (currentIndex >= 0)
                        {
                            if (currentIndex - b < 0)
                                currentIndex += vocabulary.Count;
                            decodedFileWriter.Write(vocabulary[(GetInversed(a) * (currentIndex - b)) % vocabulary.Count]);
                        }
                    }
                }
            }
        }

        private void decodeButton_Click(object sender, EventArgs e)
        {
            Decode();
        }
    }
}
