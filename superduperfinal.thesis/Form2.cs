using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Data.SqlServerCe;
using System.IO; 
using System.Drawing.Drawing2D;


namespace superduperfinal.thesis
{
    public partial class Form2 : Form
    {
        String cs = @"Data Source=C:\Users\Admin\Documents\Gabriell-Oliva\b190\C# system\superduperfinal.thesis\superduperfinal.thesis\dbcon.sdf";
        SqlCeConnection con;
        SqlCeDataAdapter adapt;
        DataTable dt;
        SqlCeCommand cmd;
        SqlCeDataReader reader;
        PrintDocument printdoc1 = new PrintDocument();
        PrintPreviewDialog previewdlg = new PrintPreviewDialog();
        Panel pannel = null;

        public Form2()
        {
            InitializeComponent();
            printdoc1.PrintPage += new PrintPageEventHandler(printdoc1_PrintPage);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;
        private void loaddata()
        {
            con = new SqlCeConnection(cs);
            con.Open();
            string stat = "Active";
            SqlCeCommand cd = new SqlCeCommand("select * from tbl_people where Stats = @stat", con);
            cd.Parameters.AddWithValue("@stat", stat);
            adapt = new SqlCeDataAdapter(cd);
            dt = new DataTable();
            adapt.Fill(dt);
            tbl_peopleDataGridView.DataSource = dt;
            con.Close();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            cleardata();
            loaddata();
            string st = "Inactive";
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            SqlCeCommand cmd = new SqlCeCommand("Select * from tbl_people where Stats = @st", con);
            cmd.Parameters.AddWithValue("@st", st);
            SqlCeDataAdapter adapt = new SqlCeDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapt.Fill(dt);
            tbl_peopleDataGridView1.DataSource = dt;
            con.Close();
        }

        private void tbl_peopleBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.tbl_peopleBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dbconDataSet);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 x = new Form4();
            x.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            con = new SqlCeConnection(cs);
            con.Open();
            try
            {
                if (textBox1.Text != "")
                {
                    adapt = new SqlCeDataAdapter("select * from tbl_people where [First Name] like '" + textBox1.Text + "%'", con);
                    dt = new DataTable();
                    adapt.Fill(dt);
                    tbl_peopleDataGridView.DataSource = dt;
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (textBox1.Text == string.Empty)
                {
                    con.Close();
                    //loaddata();
                }
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            Print(this.panel3);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            string st = "Active";
            listView1.Items.Clear();
            listView1.Visible = true;
            panel3.Visible = false;
            panel4.Visible = false;
            SqlCeConnection cn = new SqlCeConnection(cs);
            cn.Open();
            SqlCeCommand cmd = new SqlCeCommand("Select * from tbl_people where stats = @st ORDER by ID ASC ", cn);
            cmd.Parameters.AddWithValue("@st", st);
            try
            {
                SqlCeDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ListViewItem item = new ListViewItem(dr["ID"].ToString());
                    item.SubItems.Add(dr["Nick Name"].ToString());
                    item.SubItems.Add(dr["First Name"].ToString());
                    item.SubItems.Add(dr["Middle Name"].ToString());
                    item.SubItems.Add(dr["Last Name"].ToString());
                    item.SubItems.Add(dr["Street Name"].ToString());
                    item.SubItems.Add(dr["House #"].ToString());
                    item.SubItems.Add(dr["Birthday"].ToString());
                    item.SubItems.Add(dr["Gender"].ToString());
                    item.SubItems.Add(dr["Status"].ToString());
                    item.SubItems.Add(dr["Birthplace"].ToString());
                    item.SubItems.Add(dr["Occupation"].ToString());
                    item.SubItems.Add(dr["Records"].ToString());
                    item.SubItems.Add(dr["Age"].ToString());
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn.Close();
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                con = new SqlCeConnection(cs);
                cmd = new SqlCeCommand("Select Role from logs ORDER by ID DESC", con);
                con.Open();
                reader = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                SqlCeDataAdapter adapt = new SqlCeDataAdapter(cmd);
                adapt.Fill(ds);
                int count = ds.Tables[0].Rows.Count;
                string users1 = string.Empty;
                reader.Read();
                users1 = reader["Role"].ToString();
                if (users1 == "User")
                {
                    Form3 x = new Form3();
                    x.Show();
                    x.button7.Enabled = false;
                    x.button9.Enabled = false;
                    x.label14.Enabled = false;
                    x.label15.Enabled = false;
                    x.button4.Enabled = false;
                    x.label11.Enabled = false;
                    x.label80.Text = "User";
                    this.Close();
                }
                else if (users1 == "Admin")
                {
                    Form3 x = new Form3();
                    x.Show();
                    x.label80.Text = "Admin";
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int counter = 0;
            string st = "Inactive";
            if (textBox2.Text != "")
            {
                try
                {
                    DialogResult dr = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo);
                    if (textBox2.Text != "")
                    {
                        if (dr == DialogResult.Yes)
                        {
                            SqlCeConnection con = new SqlCeConnection(cs);
                            con.Open();
                            SqlCeCommand cmd = new SqlCeCommand("Update tbl_people set Stats=@st where ID=@id", con);
                            cmd.Parameters.AddWithValue("@st", st);
                            cmd.Parameters.AddWithValue("@id", textBox2.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();
                            counter++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Select Records to Delete!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    loaddata();
                    refarchive();
                }
            }

            if (counter == 1)
            {
                var date = DateTime.Now;
                SqlCeConnection con1 = new SqlCeConnection(cs);
                con1.Open();
                SqlCeCommand c = new SqlCeCommand("Select * from logs ORDER by Date_Time DESC", con1);
                SqlCeDataReader reader;
                reader = c.ExecuteReader();
                reader.Read();
                string name = reader[0].ToString();
                string role = reader[1].ToString();
                var nm = textBox2.Text;
                string activity = "SET INACTIVE STATUS RESIDENT ID#: " +nm;
                SqlCeCommand cd = new SqlCeCommand("Insert into logs (Name,Role,Date_time,Activity) values (@nm,@rl,@dt,@act)", con1);
                cd.Parameters.AddWithValue("@nm", name);
                cd.Parameters.AddWithValue("@rl", role);
                cd.Parameters.AddWithValue("@dt", date);
                cd.Parameters.AddWithValue("@act", activity);
                cd.ExecuteNonQuery();
                con1.Close();
            }
        }

        private void tbl_peopleDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            panel4.Visible = false;
            panel3.Visible = false;
            foreach (DataGridViewRow dgRow in tbl_peopleDataGridView.Rows)
            {
                var cell = dgRow.Cells[0];
                if (cell.Value != null)
                {
                    if (e.RowIndex == -1)
                    {
                        return;
                    }
                    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                    {
                        panel3.Visible = true;
                        textBox2.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                        textBox14.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                        textBox15.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                        textBox3.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
                        textBox12.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
                        textBox5.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[5].Value.ToString();
                        textBox4.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[6].Value.ToString();
                        textBox9.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[10].Value.ToString();
                        textBox10.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[11].Value.ToString();
                        textBox11.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString();
                        //for image
                        int x = int.Parse(textBox2.Text);
                        if (x >= 0)
                        {
                            try
                            {
                                SqlCeConnection con = new SqlCeConnection(cs);
                                con.Open();
                                SqlCeCommand command = new SqlCeCommand("select image from tbl_people where ID=@id", con);
                                command.Parameters.AddWithValue("@id", textBox2.Text);
                                SqlCeDataAdapter dp = new SqlCeDataAdapter(command);
                                DataSet ds = new DataSet();
                                byte[] MyData = new byte[0];
                                dp.Fill(ds);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    MyData = (byte[])ds.Tables[0].Rows[0]["Image"];
                                    MemoryStream stream = new MemoryStream(MyData);
                                    pictureBox1.Image = Image.FromStream(stream);
                                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    return;
                }

                foreach (DataGridViewRow dgRow1 in tbl_peopleDataGridView.Rows)
                {
                    var cell1 = dgRow1.Cells[1];
                    if (cell1.Value != null)
                    {
                        label40.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                        label39.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                        label37.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
                        label38.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
                        label35.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[5].Value.ToString();
                        label36.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[7].Value.ToString();
                        label34.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[6].Value.ToString();
                        label32.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[8].Value.ToString();
                        label33.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[9].Value.ToString();
                        label31.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[10].Value.ToString();
                        label30.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[11].Value.ToString();
                        label29.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                panel2.Visible = true;
            }
            else
            {
                MessageBox.Show("Please Select Record to UPDATE");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int count = 0;
            if (textBox2.Text != "" || textBox15.Text != "")
            {
                try
                {
                    con = new SqlCeConnection(cs);
                    SqlCeCommand cmd = new SqlCeCommand("update tbl_people set [Nick Name]=@Name,[First Name]=@Fname,[Middle Name]=@Mname,[Last Name]=@Lname,[Street Name]=@Street,[House #]=@House,[Birthday]=@Bday,[Gender]=@Gender,[Status]=@Status,[Birthplace]=@Bplace,[Occupation]=@Occ,[Records]=@Rec where ID=@id", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@id", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Name", textBox14.Text);
                    cmd.Parameters.AddWithValue("@Fname", textBox15.Text);
                    cmd.Parameters.AddWithValue("@Mname", textBox12.Text);
                    cmd.Parameters.AddWithValue("@Lname", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Street", textBox5.Text);
                    cmd.Parameters.AddWithValue("@House", textBox4.Text);
                    cmd.Parameters.AddWithValue("@Bday", SqlDbType.Date).Value = dateTimePicker1.Value.Date.ToString("MMddyyyy");
                    cmd.Parameters.AddWithValue("@Gender", comboBox1.GetItemText(comboBox1.SelectedItem));
                    cmd.Parameters.AddWithValue("@Status", comboBox2.GetItemText(comboBox2.SelectedItem));
                    cmd.Parameters.AddWithValue("@Bplace", textBox9.Text);
                    cmd.Parameters.AddWithValue("@Occ", textBox10.Text);
                    cmd.Parameters.AddWithValue("@Rec", textBox11.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record Updated Successfully");
                    con.Close();
                    count++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    panel2.Visible = false;
                }
            }
            else
            {
                MessageBox.Show("Please fill the blanks");
            }

            if (count == 1)
            {
                var date = DateTime.Now;
                SqlCeConnection con1 = new SqlCeConnection(cs);
                con1.Open();
                SqlCeCommand c = new SqlCeCommand("Select * from logs ORDER by Date_Time DESC", con1);
                SqlCeDataReader reader;
                reader = c.ExecuteReader();
                reader.Read();
                string name = reader[0].ToString();
                string role = reader[1].ToString();
                var nm = textBox2.Text;
                string activity = "UPDATED RESIDENT ID#: " +nm;
                SqlCeCommand cd = new SqlCeCommand("Insert into logs (Name,Role,Date_time,Activity) values (@nm,@rl,@dt,@act)", con1);
                cd.Parameters.AddWithValue("@nm", name);
                cd.Parameters.AddWithValue("@rl", role);
                cd.Parameters.AddWithValue("@dt", date);
                cd.Parameters.AddWithValue("@act", activity);
                cd.ExecuteNonQuery();
                con1.Close();
                cleardata();
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel2.Visible = false;
        }
        private void cleardata()
        {
            textBox14.ResetText(); textBox15.ResetText(); textBox12.ResetText(); textBox3.ResetText();
            textBox5.ResetText(); textBox4.ResetText();
            textBox9.ResetText(); textBox10.ResetText(); textBox11.ResetText();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label16.Text = DateTime.Now.ToString();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            loaddata();
        }

        private System.Drawing.Printing.PrintDocument document =
        new System.Drawing.Printing.PrintDocument();

        Bitmap MemoryImage;
        public void GetPrintArea(Panel pnl)
        {
            MemoryImage = new Bitmap(pnl.Width, pnl.Height);
            pnl.DrawToBitmap(MemoryImage, new Rectangle(0, 0, pnl.Width, pnl.Height));
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (MemoryImage != null)
            {
                e.Graphics.DrawImage(MemoryImage, 0, 0);
                base.OnPaint(e);
            }
        }
        void printdoc1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Rectangle pagearea = e.PageBounds;
            e.Graphics.DrawImage(MemoryImage, (pagearea.Width / 2) - (this.panel3.Width / 2), this.panel3.Location.Y);
        }
        public void Print(Panel pnl)
        {
            pannel = pnl;
            GetPrintArea(pnl);
            previewdlg.Document = printdoc1;
            previewdlg.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Print(this.panel3);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            panel3.Visible = true;
            panel4.Visible = true;
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            string st = "Active";
            int count = 0;
            try
            {
                DialogResult dr = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo);
                if (textBox6.Text != "")
                {
                    if (dr == DialogResult.Yes)
                    {
                        SqlCeConnection con = new SqlCeConnection(cs);
                        con.Open();
                        SqlCeCommand cmd = new SqlCeCommand("Update tbl_people set Stats=@st where ID=@id", con);
                        cmd.Parameters.AddWithValue("@st", st);
                        cmd.Parameters.AddWithValue("@id", textBox6.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Success!");
                        count++;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Please Select Record");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                refarchive();
            }

            if (count == 1)
            {
                var date = DateTime.Now;
                SqlCeConnection con1 = new SqlCeConnection(cs);
                con1.Open();
                SqlCeCommand c = new SqlCeCommand("Select * from logs ORDER by Date_Time DESC", con1);
                SqlCeDataReader reader;
                reader = c.ExecuteReader();
                reader.Read();
                string name = reader[0].ToString();
                string role = reader[1].ToString();
                var nm = textBox6.Text;
                string activity = "SET ACTIVE STATUS FOR ID#: " +nm;
                SqlCeCommand cd = new SqlCeCommand("Insert into logs (Name,Role,Date_time,Activity) values (@nm,@rl,@dt,@act)", con1);
                cd.Parameters.AddWithValue("@nm", name);
                cd.Parameters.AddWithValue("@rl", role);
                cd.Parameters.AddWithValue("@dt", date);
                cd.Parameters.AddWithValue("@act", activity);
                cd.ExecuteNonQuery();
                con1.Close();
            }

        }

        private void tbl_peopleDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox6.Text = tbl_peopleDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
        }

        private void refarchive()
        {
            string st = "Inactive";
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            SqlCeCommand cmd = new SqlCeCommand("Select * from tbl_people where stats=@st", con);
            cmd.Parameters.AddWithValue("@st", st);
            SqlCeDataAdapter ad = new SqlCeDataAdapter(cmd);
            DataTable tbl = new DataTable();
            ad.Fill(tbl);
            tbl_peopleDataGridView1.DataSource = tbl;
        }
    }
}
