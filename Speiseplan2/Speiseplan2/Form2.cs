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
    public partial class Form2 : Form
    {
        private DatabaseAccess da;
        private OleDbDataReader dr;
        private OleDbCommand cmd;
        private string sql;

        public Form2()
        {
            da = new DatabaseAccess();
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("") || comboBox1.Text.Equals(""))
                MessageBox.Show("Bitte vollständig ausfüllen");
            else
            {
                if (button1.Text.Equals("Hinzufügen"))
                {
                    int x = 0;
                    int f = 0;
                    dr = da.readData("Select Speise from Speisen");
                    while (dr.Read())
                    {
                        if (dr[x].ToString().ToLower() == textBox1.Text.ToLower())
                        {
                            f = 1;
                        }
                    }
                    if (f == 1)
                        MessageBox.Show("Speise schon vorhanden");
                    else
                    {
                        sql = "Insert into Speisen (Speise, Speisenart) values(?,?);";
                        cmd = new OleDbCommand();
                        cmd.CommandText = sql;
                        cmd.Parameters.Add(new OleDbParameter("Speise", textBox1.Text));
                        cmd.Parameters.Add(new OleDbParameter("Speisenart", comboBox1.Text));

                        da.executeQuery(cmd);
                        Form1.f1.readSpeiseIntoList();
                        this.Close();
                        Form1.f1.readSpeiseIntoList();
                    }
                }
                else if (button1.Text.Equals("Bearbeiten"))
                {
                    string sql = "Update Speisen set Speise=?, Speisenart=? where Speise='" + Form1.f1.listView1.SelectedItems[0].SubItems[0].Text + "';";
                    cmd = new OleDbCommand();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new OleDbParameter("Speise", textBox1.Text.ToString()));
                    cmd.Parameters.Add(new OleDbParameter("Speisenart", comboBox1.Text.ToString()));
                    da.executeQuery(cmd);
                    Form1.f1.readSpeiseIntoList();
                    this.Close();
                    Form1.f1.readSpeiseIntoList();

                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Text = Form1.f1.t;
            button1.Text = Form1.f1.t;
            if(this.Text.Equals("Bearbeiten"))
            {
                textBox1.Text = Form1.f1.listView1.SelectedItems[0].SubItems[0].Text;
                comboBox1.Text = Form1.f1.listView1.SelectedItems[0].SubItems[1].Text;
            }

        }
    }
}
