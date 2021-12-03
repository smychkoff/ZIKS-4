using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZIKS4
{
    public partial class BigramMatrixForm : Form
    {
        public List<DataTable> gramTables;

        public BigramMatrixForm()
        {
            InitializeComponent();
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

        private void BigramMatrixForm_Load(object sender, EventArgs e)
        {
            DataTable sortedTable = gramTables[1];
            sortedTable.DefaultView.Sort = "Вірогідність";
            sortedTable = sortedTable.DefaultView.ToTable();
            float maxP = (float)sortedTable.Rows[sortedTable.Rows.Count - 1][1];
            float minP = (float)sortedTable.Rows[0][1];
            DataTable bigramMatrix = new DataTable();
            bigramMatrix.Columns.Add("   ");
            for (int i = 0; i < gramTables[0].Rows.Count; i++)
            {
                bigramMatrix.Columns.Add($"{gramTables[0].Rows[i][0]}");
            }
            for (int i = 0; i < gramTables[0].Rows.Count; i++)
            {
                bigramMatrix.Rows.Add($"{gramTables[0].Rows[i][0]}");
            }
            bigramMatrixView.DataSource = bigramMatrix;
            bigramMatrixView.Columns[0].Frozen = true;
            foreach (DataGridViewColumn c in bigramMatrixView.Columns)
            {
                c.Width = 20;
            }
            for (int i = 0; i < bigramMatrixView.Rows.Count; i++)
            {
                for (int j = 1; j < bigramMatrixView.Columns.Count; j++)
                {
                    float p = 0;
                    string bigram = $"{gramTables[0].Rows[i][0]}{gramTables[0].Rows[j - 1][0]}";
                    for (int k = 0; k < sortedTable.Rows.Count; k++)
                    {
                        if ((string)sortedTable.Rows[k][0] == bigram)
                            p = (float)sortedTable.Rows[k][1];
                    }
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    if (p != 0)
                    {
                        string ss = GetColour(minP, maxP, p);
                        style.BackColor = Color.FromArgb(int.Parse("FF" + GetColour(minP, maxP, p), System.Globalization.NumberStyles.HexNumber));
                    }
                    else
                    {
                        style.BackColor = Color.Black;
                    }
                    bigramMatrixView.Rows[i].Cells[j].Style = style;
                }
            }
            bigramMatrixView.Refresh();
        }
    }
}
