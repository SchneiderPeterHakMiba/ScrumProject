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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

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
        private int btn;
        private string v = "";

        public Form1()
        {
            da = new DatabaseAccess();
            f1 = this;
            btn = 0;
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
            System.Threading.Thread.Sleep(200);
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
            System.Threading.Thread.Sleep(200);
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
            int v = 0;
            int h = 0;
            int n = 0;
            if (btn == 0)
            {
                for (int l = 0; l <= listView1.Items.Count - 1; l++)
                {
                    if (listView1.Items[l].SubItems[1].Text.Equals("Vorspeise"))
                    {
                        v++;
                    }
                    else if (listView1.Items[l].SubItems[1].Text.Equals("Hauptspeise"))
                    {
                        h++;
                    }
                    else if (listView1.Items[l].SubItems[1].Text.Equals("Nachspeise"))
                    {
                        n++;
                    }
                }
                if (v < 5)
                {
                    MessageBox.Show("Zu wenig Vorspeisen vorhanden");
                }
                else if (h < 5)
                {
                    MessageBox.Show("Zu wenig Hauptspeisen vorhanden");
                }
                else if (n < 5)
                {
                    MessageBox.Show("Zu wenig Nachspeisen vorhanden");
                }
                else
                {
                    cmd = new OleDbCommand();
                    cmd.CommandText = "Delete from Wochenplan";
                    da.executeQuery(cmd);
                    int i = 3;
                    while (i < 18)
                    {
                        string z = "";
                        int p = i % 3;
                        if (p == 0)
                            z = "Vorspeise";
                        if (p == 1)
                            z = "Hauptspeise";
                        if (p == 2)
                            z = "Nachspeise";
                        bool b = true;
                        while (b)
                        {

                            Random r = new Random();
                            int j = r.Next(0, Convert.ToInt32(listView1.Items.Count));
                            string o = listView1.Items[j].SubItems[0].Text;
                            //MessageBox.Show(z + " " + listView1.Items[j].SubItems[1].Text + "." + i + " " + j);
                            bool q = true;

                            for (int l = 1; l <= i-2; l++)
                            {
                                if(o.Equals(da.executeScalarString("Select WSpeise from Wochenplan where WID=" + l + ";")))
                                { q = false; }
                               
                            }

                            if (q == true)
                            {
                                if (z.Equals(listView1.Items[j].SubItems[1].Text))
                                {

                                    cmd = new OleDbCommand();
                                    cmd.CommandText = "Insert into Wochenplan (WID, WSpeise) values(?,?);";
                                    cmd.Parameters.Add(new OleDbParameter("WID", (i - 2)));
                                    cmd.Parameters.Add(new OleDbParameter("WSpeise", o));
                                    listView2.Items.Clear();
                                    dr = da.readData("Select WID, WSpeise from Wochenplan");
                                    while (dr.Read())
                                    {
                                       lvItem = new ListViewItem(dr[1].ToString());
                                       listView2.Items.Add(lvItem);
                                    }
                                    da.executeQuery(cmd);
                                    b = false;
                                }
                            }
                        }
                        
                        i++;
                        
                    }
                    readWochenplanIntoList();
                }
            }
            else if (btn == 1)
            {
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button8.Enabled = true;
                button6.Text = "Wochenmenü zusammenstellen";
                button5.Text = "Speise austauschen";
                btn = 0;
            }
            
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            int i = 0;
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream("Wochenplan.pdf", FileMode.Create));
            doc.Open();

            Paragraph paragraph = new Paragraph("                                   Wochenmenü\n\n");
            doc.Add(paragraph);

            PdfPTable table = new PdfPTable(3);
            int m = 1;
            string p = "Montag";
            dr = da.readData("Select WID, WSpeise from Wochenplan");
            while (dr.Read())
            {
               

                if (i == 0)
                {
                    if (m == 2)
                        p = "Dienstag";
                    else if (m == 3)
                        p = "Mittwoch";
                    else if (m == 4)
                        p = "Donnerstag";
                    else if (m == 5)
                        p = "Freitag";
                    table.AddCell(p);
                    table.AddCell("Vorspeise");
                    table.AddCell(dr[1].ToString());
                    i++;
                    m++;
                }
                else if(i==1)
                {
                    table.AddCell("");
                    table.AddCell("Hauptspeise");
                    table.AddCell(dr[1].ToString());
                    i++;
                }
                else if(i==2)
                {
                    table.AddCell("");
                    table.AddCell("Nachspeise");
                    table.AddCell(dr[1].ToString());
                    i = 0;
                }
               
            }           
            

            doc.Add(table);
            doc.Close();
            MessageBox.Show("PDF erfolgreich erstellt");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            if(btn == 0)
            {
                try
            {
                    v = listView1.SelectedItems[0].SubItems[0].Text;
                    MessageBox.Show("Wählen Sie die auszutauschende Speise");
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button8.Enabled = false;
                    button6.Text = "Abbrechen";
                    button5.Text = "Bestätigen";
                    btn = 1;

            }
            catch
            {
                MessageBox.Show("Bitte wähle eine Speise in der Speiseliste");
            }
            }
            else if(btn ==1)
            {
                int c=0;
                try
                {
                    if (listView2.SelectedItems.Count > 0)
                    {
                        for (int lcount = 0; lcount <= listView2.Items.Count - 1; lcount++)
                        {
                            if (listView2.Items[lcount].Selected == true)
                            {
                                c = lcount+1;
                                break;
                            }
                        }
                    }
                    
                    string u = da.executeScalarString("Select Speisenart from Speisen where Speise='" + v + "';");
                    string z = "";
                    int p = (c+2 )% 3;
                    if (p == 0)
                        z = "Vorspeise";
                    if (p == 1)
                        z = "Hauptspeise";
                    if (p == 2)
                        z = "Nachspeise";
                    if(z.Equals(u))
                    {
                    string sql = "Update Wochenplan set WSpeise=? where WID=" + c + ";";
                    cmd = new OleDbCommand();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new OleDbParameter("WSpeise", v));
                    da.executeQuery(cmd);


                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button8.Enabled = true;
                    button6.Text = "Wochenmenü zusammenstellen";
                    button5.Text = "Speise austauschen";
                    btn = 0;
                    readWochenplanIntoList();
                    }
                    else
                    {
                        MessageBox.Show("Speisenart muss übereinstimmen: " + u + " =/= " + z);
                    }


                    
                }
                catch
                {
                    MessageBox.Show("Bitte wähle eine Speise in der Wochenliste");
                }

                
            }
            

        }
    }
}
