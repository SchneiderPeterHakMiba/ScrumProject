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

namespace Speiseplan2
{
    public partial class Form1 : Form
    {
        private DatabaseAccess da;
        private OleDbDataReader dr;
        public OleDbCommand cmd;
        public ListViewItem lvItem;
        public ListViewItem lvItem2;
        internal static Form1 f1 = new Form1();
        internal string t = "";
        internal int g;

        public Form1()
        {
            da = new DatabaseAccess();
            f1 = this;
            InitializeComponent();
            listView1.FullRowSelect = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            readSpeiseIntoList();
            readWochenplanIntoList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            t = "Hinzufügen";
            Form2 f2 = new Form2();
            f2.ShowDialog();
            readSpeiseIntoList();

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void readSpeiseIntoList()
        {
            listView1.Items.Clear();
            dr = da.readData("Select ID, Speise, Speisenart from Speisen");
            while (dr.Read())
            {
                lvItem = new ListViewItem(dr[1].ToString());
                lvItem.SubItems.Add(dr[2].ToString());
                listView1.Items.Add(lvItem);
            }
        }
        public void readWochenplanIntoList()
        {
            listView2.Items.Clear();
            dr = da.readData("Select WID, WSpeise from Wochenplan");
            while (dr.Read())
            {
                lvItem = new ListViewItem(dr[1].ToString());
                listView2.Items.Add(lvItem);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                 g = Convert.ToInt32(da.executeScalar("Select ID from Speisen where Speise='" + listView1.SelectedItems[0].Text + "';"));
                
                
                    DialogResult dre = MessageBox.Show("Wollen Sie die ausgewählte Speise " + listView1.SelectedItems[0].Text + " löschen?", "Achtung: ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dre == DialogResult.Yes)
                    {
                        
                        cmd = new OleDbCommand();
                        cmd.CommandText = "Delete from Speisen where ID=" + g + ";";
                        da.executeQuery(cmd);
                        readSpeiseIntoList();
                    }
                
            }
            catch
            {
                MessageBox.Show("Bitte wähle eine Speise");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                g = Convert.ToInt32(da.executeScalar("Select ID from Speisen where Speise='" + listView1.SelectedItems[0].Text + "';"));
                t = "Bearbeiten";
            Form2 f2 = new Form2();
            f2.ShowDialog();
            readSpeiseIntoList();
            }
            catch
            {
                MessageBox.Show("Bitte wähle eine Speise");
            }


}

        private void button7_Click(object sender, EventArgs e)
        {
            readSpeiseIntoList();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            cmd = new OleDbCommand();
                cmd.CommandText = "Delete from Wochenplan";
                da.executeQuery(cmd);
            int i = 3;
            while(i<18)
            {
                string z= "";
                int p = i % 3;
                if (p == 0)
                    z = "Vorspeise";
                if (p == 1)
                    z = "Hauptspeise";
                if (p == 2)
                    z = "Nachspeise";
                bool b = true;
                while(b)
                    {
                   
                    Random r = new Random();
                    int j = r.Next(0, Convert.ToInt32(listView1.Items.Count));
                    string o = listView1.Items[j].SubItems[0].Text;
                    //MessageBox.Show(z + " " + listView1.Items[j].SubItems[1].Text + "." + i + " " + j);
                    if(z.Equals(listView1.Items[j].SubItems[1].Text))
                    {
                   
                        cmd = new OleDbCommand();
                        cmd.CommandText = "Insert into Wochenplan ( WSpeise) values(?);"; ;
                        cmd.Parameters.Add(new OleDbParameter("WSpeise", o));
                    
                        da.executeQuery(cmd);
                        b = false;
                    }
                    }

                i++;
                
            }
            readWochenplanIntoList();
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
