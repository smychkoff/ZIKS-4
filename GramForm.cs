using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZIKS4
{
    public partial class GramForm : Form
    {
        public List<DataTable> gramTables;
        public GramForm()
        {
            InitializeComponent();
        }
        private void GramForm_Load(object sender, EventArgs e)
        {
            gram1DataGridView.DataSource = gramTables[0];
            gram2DataGridView.DataSource = gramTables[1];
            gram3DataGridView.DataSource = gramTables[2];
            gram1DataGridView.Sort(gram1DataGridView.Columns[1], ListSortDirection.Descending);
            gram2DataGridView.Sort(gram2DataGridView.Columns[1], ListSortDirection.Descending);
            gram3DataGridView.Sort(gram3DataGridView.Columns[1], ListSortDirection.Descending);
            numberOf1GramsLabel.Text = "Кількість 1-грам: " + gramTables[0].Rows.Count.ToString();
            numberOf2GramsLabel.Text = "Кількість 2-грам: " + gramTables[1].Rows.Count.ToString();
            numberOf3GramsLabel.Text = "Кількість 3-грам: " + gramTables[2].Rows.Count.ToString();
        }
    }
}
