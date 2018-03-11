using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Speiseplan
{
    public partial class Form2 : Form
    {

        private DatabaseAccess da;
        private OleDbDataReader dr;

        public OleDbCommand cmd;
        public string sql;
        public ListViewItem lvItem;
        public Form2()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            readGerichtIntoListView();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        internal void readGerichtIntoListView()
        {
            try
            {
                listView1.Items.Clear();
                dr = da.readData("Select * from Speisen;");
                while (dr.Read())
                {
                    lvItem = new ListViewItem(dr[1].ToString());
                    lvItem.SubItems.Add(dr[2].ToString());
                    listView1.Items.Add(lvItem);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
