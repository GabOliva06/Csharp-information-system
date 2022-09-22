using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;

namespace superduperfinal.thesis
{
    public partial class Form3 : Form
    {
        PrintDocument printdoc1 = new PrintDocument();
        PrintPreviewDialog previewdlg = new PrintPreviewDialog();
        string cs = @"Data Source=C:\Users\Admin\Documents\Gabriell-Oliva\b190\C# system\superduperfinal.thesis\superduperfinal.thesis\dbcon.sdf";
        public Form3()
        {
            InitializeComponent();
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

        public void setButton1Visible(Boolean flag)
        {
            this.button7.Visible = flag;
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            String forms = "This contains the logs of the users activities in the system.";
            groupBox1.Text = forms;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            groupBox1.ResetText();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //residents button
            Form2 x = new Form2();
            x.Show();
            this.Close();
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            SqlCeDataAdapter adapt = new SqlCeDataAdapter("Select * from logs", con);
            DataTable table = new DataTable();
            adapt.Fill(table);
            logsrefresh();
            con.Close();

        }

        private void button9_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
            panel8.Visible = false;
        }
        private void button2_MouseEnter(object sender, EventArgs e)
        {
            String records = "This contains records of each residents in barangay";
            groupBox1.Text = records;
        }
        private void button9_MouseEnter(object sender, EventArgs e)
        {
            String records = "This contains settings of Accounts registered in the system";
            groupBox1.Text = records;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            groupBox1.ResetText();
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            string inv = "This contains inventory of equipments of barangay such as: Tables, Chairs, Tent";
            groupBox1.Text = inv;
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            groupBox1.ResetText();
        }

        private void button7_MouseEnter(object sender, EventArgs e)
        {
            String set = "Settings of the system";
            groupBox1.Text = set;
        }

        private void button7_MouseLeave(object sender, EventArgs e)
        {
            groupBox1.ResetText();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            loaddata4();
            Refreshlist();
            panel13.Visible = false;
            panel3.Visible = true;
            panel8.Visible = true;
        }

        private void tbl_accountsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.tbl_accountsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dbconDataSet);

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dbconDataSet.logs' table. You can move, or remove it, as needed.
            this.logsTableAdapter.Fill(this.dbconDataSet.logs);
            // TODO: This line of code loads data into the 'dbconDataSet.tbl_people' table. You can move, or remove it, as needed.
            this.tbl_peopleTableAdapter.Fill(this.dbconDataSet.tbl_people);
            // TODO: This line of code loads data into the 'dbconDataSet.tbl_budget' table. You can move, or remove it, as needed.
            this.tbl_budgetTableAdapter.Fill(this.dbconDataSet.tbl_budget);
            // TODO: This line of code loads data into the 'dbconDataSet.tbl_rent' table. You can move, or remove it, as needed.
            this.tbl_rentTableAdapter.Fill(this.dbconDataSet.tbl_rent);
            // TODO: This line of code loads data into the 'dbconDataSet.tbl_onrent' table. You can move, or remove it, as needed.
            // TODO: This line of code loads data into the 'dbconDataSet.tbl_inventory' table. You can move, or remove it, as needed.
            this.tbl_inventoryTableAdapter.Fill(this.dbconDataSet.tbl_inventory);
            // TODO: This line of code loads data into the 'dbconDataSet.tbl_user' table. You can move, or remove it, as needed.
            this.tbl_userTableAdapter.Fill(this.dbconDataSet.tbl_user);
            // TODO: This line of code loads data into the 'dbconDataSet.tbl_accounts' table. You can move, or remove it, as needed.
            this.tbl_accountsTableAdapter.Fill(this.dbconDataSet.tbl_accounts);
            this.tbl_onrentTableAdapter.Fill(this.dbconDataSet.tbl_onrent);

            var date = DateTime.Now;
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            SqlCeCommand cmd2 = new SqlCeCommand("Select * from tbl_rent where ReturnDate <= @dt", con);
            cmd2.Parameters.AddWithValue("@dt", SqlDbType.Date).Value = date.Date.ToString("MM-dd-yyyy");
            SqlCeDataAdapter adapt2 = new SqlCeDataAdapter(cmd2);
            DataTable dtb = new DataTable();
            adapt2.Fill(dtb);
            int cn1 = dtb.Rows.Count;
            tbl_rentDataGridView.DataSource = dtb;
            con.Close();
            label71.Text = cn1.ToString();

            SqlCeConnection cn = new SqlCeConnection(cs);
            cn.Open();
            SqlCeCommand cmd = new SqlCeCommand("Select * from tbl_budget ORDER by ID ASC", cn);
            try
            {
                SqlCeDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListViewItem item = new ListViewItem(dr[0].ToString());
                    item.SubItems.Add(dr[1].ToString());
                    item.SubItems.Add(dr[2].ToString());
                    item.SubItems.Add(dr[3].ToString());
                    item.SubItems.Add(dr[4].ToString());
                    item.SubItems.Add(dr[5].ToString());
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            SqlCeCommand cm = new SqlCeCommand("Select * from tbl_rent ORDER by FirstName ASC", cn);
            try
            {
                SqlCeDataReader dr1 = cm.ExecuteReader();
                while (dr1.Read())
                {
                    ListViewItem item1 = new ListViewItem(dr1["ID"].ToString());
                    item1.SubItems.Add(dr1["FirstName"].ToString());
                    item1.SubItems.Add(dr1["MiddleName"].ToString());
                    item1.SubItems.Add(dr1["LastName"].ToString());
                    item1.SubItems.Add(dr1["StreetNo"].ToString());
                    item1.SubItems.Add(dr1["StreetName"].ToString());
                    item1.SubItems.Add(dr1["Baranggay"].ToString());
                    item1.SubItems.Add(dr1["City"].ToString());
                    item1.SubItems.Add(dr1["DateRented"].ToString());
                    item1.SubItems.Add(dr1["ReturnDate"].ToString());
                    item1.SubItems.Add(dr1["Purpose"].ToString());
                    listView2.Items.Add(item1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            SqlCeCommand cmd1 = new SqlCeCommand("Select * from tbl_accounts ORDER by ID ASC", cn);
            try
            {
                SqlCeDataReader dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    ListViewItem item1 = new ListViewItem(dr1[1].ToString());
                    item1.SubItems.Add(dr1[2].ToString());
                    listView3.Items.Add(item1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            SqlCeCommand com1 = new SqlCeCommand("Select * from Councils", cn);
            SqlCeDataReader reader2;
            reader2 = com1.ExecuteReader();
            reader2.Read();
            label81.Text = reader2[1].ToString();
            label82.Text = reader2[2].ToString();
            label83.Text = reader2[3].ToString();
            label84.Text = reader2[4].ToString();
            label85.Text = reader2[5].ToString();
            label86.Text = reader2[6].ToString();
            label87.Text = reader2[7].ToString();
            label88.Text = reader2[8].ToString();
            label89.Text = reader2[9].ToString();
            label90.Text = reader2[10].ToString();
            label91.Text = reader2[11].ToString();
            reader2.Close();
            //for logs
            SqlCeDataAdapter adapt = new SqlCeDataAdapter("Select * from logs ORDER by ID DESC", cn);
            DataTable dt = new DataTable();
            adapt.Fill(dt);
            logsDataGridView.DataSource = dt;
            cn.Close();
            logsrefresh();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int count = 0;
            if (textBox1.Text != "" || textBox2.Text != "")
            {
                try
                {
                    SqlCeConnection con = new SqlCeConnection(cs);
                    con.Open();
                    SqlCeCommand cmd = new SqlCeCommand("Insert into tbl_user (Username,Password,[First Name],[Last Name],[Job Position],Role) values (@user,@pass,@fn,@ln,@jb,@role)", con);
                    cmd.Parameters.AddWithValue("@user", textBox1.Text);
                    cmd.Parameters.AddWithValue("@Pass", textBox2.Text);
                    cmd.Parameters.AddWithValue("@fn", textBox4.Text);
                    cmd.Parameters.AddWithValue("@ln", textBox15.Text);
                    cmd.Parameters.AddWithValue("@jb", textBox5.Text);
                    cmd.Parameters.AddWithValue("@role", textBox24.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Registration Successfully!");
                    count++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    loaddata();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox4.Text = "";
                    textBox15.Text = "";
                    textBox5.Text = "";
                    textBox24.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Please Enter Correct Details!");
            }
            if (count == 1)
            {
                //logs tbl_user *insert*
                SqlCeConnection con = new SqlCeConnection(cs);
                con.Open();
                SqlCeCommand c = new SqlCeCommand("Select * from logs ORDER by Date_Time DESC", con);
                SqlCeDataReader reader;
                reader = c.ExecuteReader();
                reader.Read();
                string name = reader[0].ToString();
                var date = DateTime.Now;
                string activity = "Insert New Account";
                SqlCeCommand cd = new SqlCeCommand("Insert into logs (Name,Role,Date_time,Activity) values (@nm,@rl,@dt,@act)", con);
                cd.Parameters.AddWithValue("@nm", name);
                cd.Parameters.AddWithValue("@rl", label80.Text);
                cd.Parameters.AddWithValue("@dt", date);
                cd.Parameters.AddWithValue("@act", activity);
                cd.ExecuteNonQuery();
                con.Close();
            }
        }

        private void loaddata()
        {
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            SqlCeDataAdapter adapt3 = new SqlCeDataAdapter("select * from tbl_user", con);
            DataTable dt3 = new DataTable();
            adapt3.Fill(dt3);
            tbl_userDataGridView1.DataSource = dt3;
            con.Close();
        }

        private void tbl_userDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                textBox3.Text = tbl_userDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBox1.Text = tbl_userDataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBox4.Text = tbl_userDataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                textBox15.Text = tbl_userDataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                textBox5.Text = tbl_userDataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                textBox24.Text = tbl_userDataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            int count = 0;
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                SqlCeConnection con = new SqlCeConnection(cs);
                SqlCeCommand cmd = new SqlCeCommand("update tbl_user set Username=@user,Password=@pass,[First Name]=@fn,[Last Name]=@ln,[Job Position]=@jb,Role=@rl where ID=@id", con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", textBox3.Text);
                cmd.Parameters.AddWithValue("@user", textBox1.Text);
                cmd.Parameters.AddWithValue("@pass", textBox2.Text);
                cmd.Parameters.AddWithValue("@fn", textBox4.Text);
                cmd.Parameters.AddWithValue("@ln", textBox15.Text);
                cmd.Parameters.AddWithValue("@jb", textBox5.Text);
                cmd.Parameters.AddWithValue("@rl", textBox24.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Updated Successfully");
                con.Close();
                loaddata();
                textBox1.ResetText(); textBox15.ResetText();
                textBox2.ResetText(); textBox5.ResetText(); textBox24.ResetText();
                count++;
            }
            else
            {
                MessageBox.Show("Please Select Record to Update");
            }

            //logs tbl_user *iupdate*
            if (count == 1)
            {
                SqlCeConnection con1 = new SqlCeConnection(cs);
                con1.Open();
                SqlCeCommand c = new SqlCeCommand("Select * from logs ORDER by Date_Time DESC", con1);
                SqlCeDataReader reader;
                reader = c.ExecuteReader();
                reader.Read();
                string name = reader[0].ToString();
                var date = DateTime.Now;
                var nm = textBox4.Text;
                string activity = "Update user account: " + nm;
                SqlCeCommand cd = new SqlCeCommand("Insert into logs (Name,Role,Date_time,Activity) values (@nm,@rl,@dt,@act)", con1);
                cd.Parameters.AddWithValue("@nm", name);
                cd.Parameters.AddWithValue("@rl", label80.Text);
                cd.Parameters.AddWithValue("@dt", date);
                cd.Parameters.AddWithValue("@act", activity);
                cd.ExecuteNonQuery();
                con1.Close();
            }
            textBox4.ResetText();

        }

        private void button16_Click(object sender, EventArgs e)
        {
            int count = 0;
            try
            {
                if (textBox3.Text != "")
                {
                    SqlCeConnection con = new SqlCeConnection(cs);
                    SqlCeCommand cmd = new SqlCeCommand("delete tbl_user where ID=@id", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@id", textBox3.Text);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record Deleted Successfully!");
                    count++;
                }
                else
                {
                    MessageBox.Show("Please Select Record to Delete");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                loaddata();
                textBox1.ResetText(); textBox15.ResetText();
                textBox2.ResetText(); textBox5.ResetText(); textBox24.ResetText();
            }

            //logs tbl_user *delete*
            if (count == 1)
            {
                SqlCeConnection con1 = new SqlCeConnection(cs);
                con1.Open();
                SqlCeCommand c = new SqlCeCommand("Select * from logs ORDER by Date_Time DESC", con1);
                SqlCeDataReader reader;
                reader = c.ExecuteReader();
                reader.Read();
                string name = reader[0].ToString();
                var date = DateTime.Now;
                var nm = textBox4.Text;
                string activity = "Delete user account: " + nm;
                SqlCeCommand cd = new SqlCeCommand("Insert into logs (Name,Role,Date_time,Activity) values (@nm,@rl,@dt,@act)", con1);
                cd.Parameters.AddWithValue("@nm", name);
                cd.Parameters.AddWithValue("@rl", label80.Text);
                cd.Parameters.AddWithValue("@dt", date);
                cd.Parameters.AddWithValue("@act", activity);
                cd.ExecuteNonQuery();
                con1.Close();
            }
            textBox4.ResetText();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label7.Text = DateTime.Now.ToString();
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            //logs
            var date = DateTime.Now;
            string role = string.Empty;
            string name = string.Empty;
            Form1 x = new Form1();
            x.Show();
            this.Hide();
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            SqlCeCommand cd = new SqlCeCommand("Select * from logs ORDER by ID DESC", con);
            SqlCeDataReader reader;
            reader = cd.ExecuteReader();
            reader.Read();
            role = reader["Role"].ToString();
            Name = reader["Name"].ToString();
            SqlCeCommand cmd = new SqlCeCommand("Insert into logs (Name,Role,Date_time,Activity) values (@nm,@rl,@dt,@act)", con);
            cmd.Parameters.AddWithValue("@nm", Name);
            cmd.Parameters.AddWithValue("@rl", role);
            cmd.Parameters.AddWithValue("@dt", date);
            cmd.Parameters.AddWithValue("@act", "LOGOUT");
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            panel3.Visible = false;
            textBox1.ResetText(); textBox4.ResetText(); textBox15.ResetText();
            textBox2.ResetText(); textBox5.ResetText(); textBox24.ResetText();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            panel9.Visible = true;
            Stockrefresh();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            panel9.Visible = false;
        }

        private void clear()
        {
            textBox7.ResetText(); textBox28.ResetText(); textBox9.ResetText(); richTextBox1.ResetText();
            textBox8.ResetText(); textBox30.ResetText(); textBox31.ResetText(); textBox14.ResetText();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            int count = 0;
            if (textBox7.Text != "" && comboBox2.Text != "")
            {
                try
                {
                    SqlCeCommand cmd = new SqlCeCommand("Insert into tbl_rent (FirstName,MiddleName,LastName,StreetNo,StreetName,Baranggay,City,DateRented,ReturnDate,Purpose) values (@fn,@mn,@ln,@sno,@sn,@brgy,@ct,@dr,@rd,@pp)", con);
                    cmd.Parameters.AddWithValue("@fn", textBox7.Text);
                    cmd.Parameters.AddWithValue("@mn", textBox8.Text);
                    cmd.Parameters.AddWithValue("@ln", textBox28.Text);
                    cmd.Parameters.AddWithValue("@sno", comboBox4.GetItemText(comboBox4.SelectedItem));
                    cmd.Parameters.AddWithValue("@sn", textBox9.Text);
                    cmd.Parameters.AddWithValue("@brgy", textBox30.Text);
                    cmd.Parameters.AddWithValue("@ct", textBox31.Text);
                    cmd.Parameters.AddWithValue("@dr", SqlDbType.Date).Value = dateTimePicker1.Value.Date.ToString("MM-dd-yyyy");
                    cmd.Parameters.AddWithValue("@rd", SqlDbType.Date).Value = dateTimePicker2.Value.Date.ToString("MM-dd-yyyy");
                    cmd.Parameters.AddWithValue("@pp", richTextBox1.Text);
                    cmd.ExecuteNonQuery();
                    var date = DateTime.Now;
                    SqlCeCommand cd = new SqlCeCommand("Insert into tbl_onrent (ChairsRented,TablesRented,TentsRented,ApprovedBy,[Date Encoded]) values (@cr,@tbr,@ter,@ab,@de)", con);
                    cd.Parameters.AddWithValue("@cr", comboBox6.GetItemText(comboBox1.SelectedItem));
                    cd.Parameters.AddWithValue("@tbr", comboBox7.GetItemText(comboBox2.SelectedItem));
                    cd.Parameters.AddWithValue("@ter", comboBox8.GetItemText(comboBox3.SelectedItem));
                    cd.Parameters.AddWithValue("@ab", textBox14.Text);
                    cd.Parameters.AddWithValue("@de", date);
                    cd.ExecuteNonQuery();
                    MessageBox.Show("Registration Successfully!");
                    count++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Stockrefresh();
                    Refreshlist();
                    panel9.Visible = false;
                    PerOp();
                    con.Close();
                }
            }
            //logs tbl_rent *insert*
            if (count == 1)
            {
                SqlCeConnection con1 = new SqlCeConnection(cs);
                con1.Open();
                SqlCeCommand c = new SqlCeCommand("Select * from logs ORDER by Date_Time DESC", con1);
                SqlCeDataReader reader;
                reader = c.ExecuteReader();
                reader.Read();
                string name = reader[0].ToString();
                var date = DateTime.Now;
                string nme = textBox7.Text;
                string activity = "ADDED NEW RENTER: " + nme;
                SqlCeCommand cd = new SqlCeCommand("Insert into logs (Name,Role,Date_time,Activity) values (@nm,@rl,@dt,@act)", con1);
                cd.Parameters.AddWithValue("@nm", name);
                cd.Parameters.AddWithValue("@rl", label80.Text);
                cd.Parameters.AddWithValue("@dt", date);
                cd.Parameters.AddWithValue("@act", activity);
                cd.ExecuteNonQuery();
                con1.Close();
            }
            clear();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            Stockrefresh();
            Refreshlist();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (textBox13.Text != "")
            {
                Stockrefresh();
                panel10.Visible = true;
                perup();
            }
            else
            {
                MessageBox.Show("Please Select record to UPDATE!");
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            int count = 0;
            var date = DateTime.Now;
            if (textBox12.Text != "" && comboBox6.Text != "")
            {
                
                SqlCeCommand cmd = new SqlCeCommand("UPDATE tbl_rent set FirstName=@fn,MiddleName=@mn,LastName=@ln,StreetNo=@sno,StreetName=@sn,Baranggay=@brgy,City=@ct,DateRented=@dr,ReturnDate=@rd,Purpose=@pp where ID=@id", con);
                cmd.Parameters.AddWithValue("id", textBox13.Text);
                cmd.Parameters.AddWithValue("@fn", textBox22.Text);
                cmd.Parameters.AddWithValue("@mn", textBox21.Text);
                cmd.Parameters.AddWithValue("@ln", textBox20.Text);
                cmd.Parameters.AddWithValue("@sno", comboBox5.GetItemText(comboBox5.SelectedItem));
                cmd.Parameters.AddWithValue("@sn", textBox19.Text);
                cmd.Parameters.AddWithValue("@brgy", textBox18.Text);
                cmd.Parameters.AddWithValue("@ct", textBox11.Text);
                cmd.Parameters.AddWithValue("@dr", SqlDbType.Date).Value = dateTimePicker4.Value.Date.ToString("MM-dd-yyyy");
                cmd.Parameters.AddWithValue("@rd", SqlDbType.Date).Value = dateTimePicker5.Value.Date.ToString("MM-dd-yyyy");
                cmd.Parameters.AddWithValue("@pp", richTextBox2.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Updated Successfully");
                panel10.Visible = false;
                //
               
                SqlCeCommand cd = new SqlCeCommand("Update tbl_onrent set ChairsRented=@cr,TablesRented=@tbr,TentsRented=@ter,ApprovedBy=@ab,[Date Encoded]=@de where ID = @id", con);
                cd.Parameters.AddWithValue("@id", textBox13.Text);
                cd.Parameters.AddWithValue("@cr", comboBox6.GetItemText(comboBox6.SelectedItem));
                cd.Parameters.AddWithValue("@tbr", comboBox7.GetItemText(comboBox7.SelectedItem));
                cd.Parameters.AddWithValue("@ter", comboBox8.GetItemText(comboBox8.SelectedItem));
                cd.Parameters.AddWithValue("@ab", textBox12.Text);
                cd.Parameters.AddWithValue("@de", date);
                cd.ExecuteNonQuery();
                PerOp1();
                count++;
                Stockrefresh();
                Refreshlist();
                con.Close();
            }
            else
            {
                MessageBox.Show("Please Complete the fields!");
            }

            //logs tbl_rent *update*
            if (count == 1)
            {
                SqlCeConnection con1 = new SqlCeConnection(cs);
                con1.Open();
                SqlCeCommand c = new SqlCeCommand("Select * from logs ORDER by Date_Time DESC", con1);
                SqlCeDataReader reader;
                reader = c.ExecuteReader();
                reader.Read();
                string name = reader[0].ToString();
                var nm = textBox22.Text;
                string activity = "Update rental items of: " +nm;
                SqlCeCommand cd = new SqlCeCommand("Insert into logs (Name,Role,Date_time,Activity) values (@nm,@rl,@dt,@act)", con1);
                cd.Parameters.AddWithValue("@nm", name);
                cd.Parameters.AddWithValue("@rl", label80.Text);
                cd.Parameters.AddWithValue("@dt", date);
                cd.Parameters.AddWithValue("@act", activity);
                cd.ExecuteNonQuery();
                con1.Close();
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            panel10.Visible = false;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            panel11.Visible = true;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            int count = 0;
            var date = DateTime.Now;
            if (textBox25.Text != "" || textBox26.Text != "")
            {
                try
                {
                    SqlCeCommand cmd = new SqlCeCommand("Insert into tbl_budget (Date,[Expense Category],[Expense Amount],[Mode Of Payment],Notes) values (@mb,@de,@te,@am,@nt)", con);
                    cmd.Parameters.AddWithValue("@mb", SqlDbType.Date).Value = dateTimePicker1.Value.Date.ToString("MM-dd-yyyy");
                    cmd.Parameters.AddWithValue("@de", textBox25.Text);
                    cmd.Parameters.AddWithValue("@te", textBox26.Text);
                    cmd.Parameters.AddWithValue("@am", textBox10.Text);
                    cmd.Parameters.AddWithValue("@nt", richTextBox3.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Inserted Successfully!");
                    count++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    panel11.Visible = false;
                    refresh1();
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("Please fill in the blanks");
            }
            //logs tbl_budget *insert*
            if (count == 1)
            {
                SqlCeConnection con1 = new SqlCeConnection(cs);
                con1.Open();
                SqlCeCommand c = new SqlCeCommand("Select * from logs ORDER by Date_Time DESC", con1);
                SqlCeDataReader reader;
                reader = c.ExecuteReader();
                reader.Read();
                string name = reader[0].ToString();
                var nm = textBox25.Text;
                string activity = "ADDED NEW Expense: " + nm;
                SqlCeCommand cd = new SqlCeCommand("Insert into logs (Name,Role,Date_time,Activity) values (@nm,@rl,@dt,@act)", con1);
                cd.Parameters.AddWithValue("@nm", name);
                cd.Parameters.AddWithValue("@rl", label80.Text);
                cd.Parameters.AddWithValue("@dt", date);
                cd.Parameters.AddWithValue("@act", activity);
                cd.ExecuteNonQuery();
                con1.Close();
            }
            textBox25.ResetText();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            panel11.Visible = false;
        }

        private void fillToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.tbl_budgetTableAdapter.Fill(this.dbconDataSet.tbl_budget);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void button27_Click_1(object sender, EventArgs e)
        {
            PerOp1();
            panel10.Visible = false;
            Refreshlist();
            Stockrefresh();
            clearlist();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (textBox34.Text != "")
            {
                panel4.Visible = true;
            }
            else
            {
                MessageBox.Show("Please Select Records To Update!");
            }
        }

        private void loaddata4()
        {
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            SqlCeDataAdapter adapt = new SqlCeDataAdapter("select * from tbl_people WHERE Stats = '"+"Active"+"'", con);
            DataTable dt = new DataTable();
            adapt.Fill(dt);
            tbl_peopleDataGridView.DataSource = dt;
            con.Close();
        }

        private void tbl_peopleDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                textBox7.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBox8.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                textBox28.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
                textBox9.Text = tbl_peopleDataGridView.Rows[e.RowIndex].Cells[5].Value.ToString();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                textBox34.Text = item.SubItems[0].Text;
                dateTimePicker6.Value = Convert.ToDateTime(item.SubItems[1].Text);
                textBox49.Text = item.SubItems[2].Text;
                textBox50.Text = item.SubItems[3].Text;
                textBox51.Text = item.SubItems[4].Text;
                richTextBox4.Text = item.SubItems[5].Text;
            }
            else
            {
                textBox34.ResetText();
                textBox49.Text = string.Empty;
                textBox50.Text = string.Empty;
                textBox51.Text = string.Empty;
            }
        }

        private void button30_Click_1(object sender, EventArgs e)
        {
            panel8.Visible = true;
            panel12.Visible = false;
        }

        private void button31_Click(object sender, EventArgs e)
        {
            textBox22.ResetText();
            panel9.Visible = false;
            panel12.Visible = true;
        }

        private void button32_Click(object sender, EventArgs e)
        {
            panel10.Visible = false;
            panel12.Visible = true;
        }

        private void button33_Click(object sender, EventArgs e)
        {
            panel11.Visible = false;
            panel4.Visible = false;
            panel10.Visible = false;
            panel8.Visible = true;
            panel12.Visible = false;
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                ListViewItem item = listView2.SelectedItems[0];
                textBox13.Text = item.SubItems[0].Text;
                textBox22.Text = item.SubItems[1].Text;
                textBox21.Text = item.SubItems[2].Text;
                textBox20.Text = item.SubItems[3].Text;
                int cb;
                cb = this.comboBox5.FindString(listView2.SelectedItems[0].SubItems[4].Text);
                this.comboBox5.SelectedIndex = cb;
                textBox19.Text = item.SubItems[5].Text;
                textBox18.Text = item.SubItems[6].Text;
                textBox11.Text = item.SubItems[7].Text;
                dateTimePicker4.Value = Convert.ToDateTime(item.SubItems[8].Text);
                dateTimePicker5.Value = Convert.ToDateTime(item.SubItems[9].Text);
                richTextBox2.Text = item.SubItems[10].Text;
            }
            else
            {
                clearlist();
            }
        }

        private void clearlist()
        {
            textBox22.Text = String.Empty;
            textBox21.Text = String.Empty;
            textBox20.Text = String.Empty;
            textBox19.Text = String.Empty;
            textBox18.Text = String.Empty;
            textBox11.Text = String.Empty;
            comboBox5.Text = String.Empty;
            richTextBox2.Text = String.Empty;
            dateTimePicker5.Value = DateTime.Now;
            dateTimePicker6.Value = DateTime.Now;
        }

        private void Refreshlist()
        {
            try
            {
                listView2.Items.Clear();
                SqlCeConnection con = new SqlCeConnection(cs);
                con.Open();
                SqlCeCommand cmd = new SqlCeCommand("SELECT * FROM tbl_rent ORDER by ID ASC", con);
                SqlCeDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListViewItem item2 = new ListViewItem(dr[10].ToString());
                    item2.SubItems.Add(dr[0].ToString());
                    item2.SubItems.Add(dr[1].ToString());
                    item2.SubItems.Add(dr[2].ToString());
                    item2.SubItems.Add(dr[3].ToString());
                    item2.SubItems.Add(dr[4].ToString());
                    item2.SubItems.Add(dr[5].ToString());
                    item2.SubItems.Add(dr[6].ToString());
                    item2.SubItems.Add(dr[7].ToString());
                    item2.SubItems.Add(dr[8].ToString());
                    item2.SubItems.Add(dr[9].ToString());
                    listView2.Items.Add(item2);
                }
                SqlCeDataAdapter ad = new SqlCeDataAdapter("Select * from tbl_onrent", con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                tbl_onrentDataGridView.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void refresh1()
        {
            listView1.Items.Clear();
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            SqlCeCommand cmd = new SqlCeCommand("SELECT * FROM tbl_budget ORDER by ID ASC", con);
            SqlCeDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListViewItem item1 = new ListViewItem(dr[0].ToString());
                item1.SubItems.Add(dr[1].ToString());
                item1.SubItems.Add(dr[2].ToString());
                item1.SubItems.Add(dr[3].ToString());
                item1.SubItems.Add(dr[4].ToString());
                item1.SubItems.Add(dr[5].ToString());
                listView1.Items.Add(item1);
            }
        }

        private void button34_Click(object sender, EventArgs e)
        {
            //expenses
            refresh1();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            int count = 0;
            var date = DateTime.Now;
          
                if (textBox13.Text != "")
                {
                    DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                    {
                    perDel();
                    try
                    {
                        SqlCeConnection con = new SqlCeConnection(cs);
                        con.Open();
                        SqlCeCommand cmd = new SqlCeCommand("Delete tbl_rent Where ID = @id", con);
                        cmd.Parameters.AddWithValue("@id", textBox13.Text);
                        cmd.ExecuteNonQuery();
                        SqlCeCommand cd = new SqlCeCommand("Delete tbl_onrent Where ID = @id1", con);
                        cd.Parameters.AddWithValue("@id1", textBox13.Text);
                        cd.ExecuteNonQuery();
                        MessageBox.Show("Deleted Successfully!");
                        count++;
                        Stockrefresh();
                        Refreshlist();
                        con.Close();
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
                else
                    {
                    MessageBox.Show("Please Select Record to delete!");
                    }
            
            //logs tbl_rent *delete*
            if (count == 1)
            {
                SqlCeConnection con1 = new SqlCeConnection(cs);
                con1.Open();
                SqlCeCommand c = new SqlCeCommand("Select * from logs ORDER by Date_Time DESC", con1);
                SqlCeDataReader reader;
                reader = c.ExecuteReader();
                reader.Read();
                string name = reader[0].ToString();
                var nm = textBox22.Text;
                string activity = "Deleted Rent Records of: " + nm;
                SqlCeCommand cd = new SqlCeCommand("Insert into logs (Name,Role,Date_time,Activity) values (@nm,@rl,@dt,@act)", con1);
                cd.Parameters.AddWithValue("@nm", name);
                cd.Parameters.AddWithValue("@rl", label80.Text);
                cd.Parameters.AddWithValue("@dt", date);
                cd.Parameters.AddWithValue("@act", activity);
                cd.ExecuteNonQuery();
                con1.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel6.Visible = false;
            panel10.Visible = false;
            panel3.Visible = true;
            panel8.Visible = true;
            panel12.Visible = true;
            panel13.Visible = true;
        }

        private void Stockrefresh()
        {
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            SqlCeDataAdapter ad = new SqlCeDataAdapter("Select * from tbl_inventory", con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            tbl_inventoryDataGridView.DataSource = dt;
            tbl_inventoryDataGridView1.DataSource = dt;
            con.Close();
            textBox29.ResetText();
            textBox32.ResetText();
            textBox33.ResetText();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            int count = 0;
            var date = DateTime.Now;

            if (textBox29.Text != "")
            {
                DialogResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    SqlCeConnection con = new SqlCeConnection(cs);
                    con.Open();
                    SqlCeCommand cm = new SqlCeCommand("Update tbl_inventory set Chairs=@ch,Tables=@tb,Booth=@bt", con);
                    cm.Parameters.AddWithValue("@ch", textBox29.Text);
                    cm.Parameters.AddWithValue("@tb", textBox32.Text);
                    cm.Parameters.AddWithValue("@bt", textBox33.Text);
                    cm.ExecuteNonQuery();
                    Stockrefresh();
                    MessageBox.Show("Updated Successfully!");
                    count++;
                    con.Close();
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Please enter correct details!");
            }

            //logs stocks *update*
            if (count == 1)
            {
                SqlCeConnection con1 = new SqlCeConnection(cs);
                con1.Open();
                SqlCeCommand c = new SqlCeCommand("Select * from logs ORDER by Date_Time DESC", con1);
                SqlCeDataReader reader;
                reader = c.ExecuteReader();
                reader.Read();
                string name = reader[0].ToString();
                string activity = "Updated the Stocks";
                SqlCeCommand cd = new SqlCeCommand("Insert into logs (Name,Role,Date_time,Activity) values (@nm,@rl,@dt,@act)", con1);
                cd.Parameters.AddWithValue("@nm", name);
                cd.Parameters.AddWithValue("@rl", label80.Text);
                cd.Parameters.AddWithValue("@dt", date);
                cd.Parameters.AddWithValue("@act", activity);
                cd.ExecuteNonQuery();
                con1.Close();
            }
        }

        private void button37_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var textBox in groupBox3.Controls.OfType<TextBox>())
                    textBox.Enabled = true;
                foreach (var lbl in groupBox3.Controls.OfType<Label>())
                    lbl.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button37.Visible = false;
                MessageBox.Show("You can now edit the Council's");
                button35.Visible = true;
            }
        }

        private void button35_Click(object sender, EventArgs e)
        {
            int count = 0;
            var date = DateTime.Now;
            try
            {
                foreach (var textBox in groupBox3.Controls.OfType<TextBox>())
                {
                    textBox.Enabled = false;
                }
                SqlCeConnection con = new SqlCeConnection(cs);
                con.Open();
                SqlCeCommand cmd = new SqlCeCommand("Update Councils set c1=@tb1,c2=@tb2,c3=@tb3,c4=@tb4,c5=@tb5,c6=@tb6,c7=@tb7,c8=@tb8,c9=@tb9,c10=@tb10,c11=@tb11 where ID = 1", con);
                cmd.Parameters.AddWithValue("@tb1", textBox35.Text);
                cmd.Parameters.AddWithValue("@tb2", textBox36.Text);
                cmd.Parameters.AddWithValue("@tb3", textBox37.Text);
                cmd.Parameters.AddWithValue("@tb4", textBox38.Text);
                cmd.Parameters.AddWithValue("@tb5", textBox39.Text);
                cmd.Parameters.AddWithValue("@tb6", textBox40.Text);
                cmd.Parameters.AddWithValue("@tb7", textBox41.Text);
                cmd.Parameters.AddWithValue("@tb8", textBox42.Text);
                cmd.Parameters.AddWithValue("@tb9", textBox43.Text);
                cmd.Parameters.AddWithValue("@tb10", textBox44.Text);
                cmd.Parameters.AddWithValue("@tb11", textBox45.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                count++;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button37.Visible = true;
                button35.Visible = false;
                MessageBox.Show("Successfully Updated!");
                foreach (var lbl in groupBox3.Controls.OfType<Label>())
                    lbl.Show();
                foreach (var textBox in groupBox3.Controls.OfType<TextBox>())
                {
                    textBox.ResetText();
                }
            }

            //logs council *update*
            if (count == 1)
            {
                SqlCeConnection con1 = new SqlCeConnection(cs);
                con1.Open();
                SqlCeCommand c = new SqlCeCommand("Select * from logs ORDER by Date_Time DESC", con1);
                SqlCeDataReader reader;
                reader = c.ExecuteReader();
                reader.Read();
                string name = reader[0].ToString();
                string activity = "Updated the Councils";
                SqlCeCommand cd = new SqlCeCommand("Insert into logs (Name,Role,Date_time,Activity) values (@nm,@rl,@dt,@act)", con1);
                cd.Parameters.AddWithValue("@nm", name);
                cd.Parameters.AddWithValue("@rl", label80.Text);
                cd.Parameters.AddWithValue("@dt", date);
                cd.Parameters.AddWithValue("@act", activity);
                cd.ExecuteNonQuery();
                con1.Close();
            }

        }

        private void PerOp()
        {
            SqlCeConnection con = new SqlCeConnection(cs);
            try
            {
                con.Open();
                SqlCeCommand cmd = new SqlCeCommand("Select Chairs,Tables,Booth from tbl_inventory", con);
                SqlCeDataReader reader = cmd.ExecuteReader();
                reader.Read();
                int a = Convert.ToInt32(reader[0].ToString());
                int b = Convert.ToInt32(reader[1].ToString());
                int c = Convert.ToInt32(reader[2].ToString());

                int x = Convert.ToInt32(comboBox1.SelectedItem);
                int y = Convert.ToInt32(comboBox2.SelectedItem);
                int z = Convert.ToInt32(comboBox3.SelectedItem);

                int ch = a - x;
                int tb = b - y;
                int bh = c - z;
                //for update
                SqlCeCommand cm = new SqlCeCommand("Update tbl_inventory set Chairs=@c,Tables=@t,Booth=@b where ID = 1", con);
                cm.Parameters.AddWithValue("@c", ch);
                cm.Parameters.AddWithValue("@t", tb);
                cm.Parameters.AddWithValue("@b", bh);
                cm.ExecuteNonQuery();
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

        private void perup()
        {
            //temp
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            SqlCeCommand cmd = new SqlCeCommand("Select Chairs,Tables,Booth from tbl_inventory where ID = 1", con);
            SqlCeDataReader rd = cmd.ExecuteReader();
            rd.Read();
            int a = Convert.ToInt32(rd[0].ToString());
            int b = Convert.ToInt32(rd[1].ToString());
            int c = Convert.ToInt32(rd[2].ToString());

            SqlCeCommand cd = new SqlCeCommand("Select ChairsRented,TablesRented,TentsRented from tbl_onrent where ID=@id", con);
            cd.Parameters.AddWithValue("@id", textBox13.Text);
            SqlCeDataReader rd1 = cd.ExecuteReader();
            rd1.Read();
            int x = Convert.ToInt32(rd1[0].ToString());
            int y = Convert.ToInt32(rd1[1].ToString());
            int z = Convert.ToInt32(rd1[2].ToString());

            int temp1 = a + x;
            int temp2 = b + y;
            int temp3 = c + z;
            //insert new stocks
            SqlCeCommand cd1 = new SqlCeCommand("Update tbl_inventory set Chairs=@t1,Tables=@t2,Booth=@t3 where ID = 1", con);
            cd1.Parameters.AddWithValue("t1", temp1);
            cd1.Parameters.AddWithValue("t2", temp2);
            cd1.Parameters.AddWithValue("t3", temp3);
            cd1.ExecuteNonQuery();
            con.Close();
        }

        private void perDel()
        {
            SqlCeConnection con = new SqlCeConnection(cs);
            try
            {
                con.Open();
                SqlCeCommand cmd = new SqlCeCommand("Select * from tbl_inventory", con);
                SqlCeDataReader reader = cmd.ExecuteReader();
                reader.Read();
                int a = Convert.ToInt32(reader[0].ToString());
                int b = Convert.ToInt32(reader[1].ToString());
                int c = Convert.ToInt32(reader[2].ToString());

                SqlCeCommand cd = new SqlCeCommand("Select ChairsRented,TablesRented,TentsRented from tbl_onrent where ID=@id", con);
                cd.Parameters.AddWithValue("@id", textBox13.Text);
                SqlCeDataReader reader1 = cd.ExecuteReader();
                reader1.Read();
                int x = Convert.ToInt32(reader1[0].ToString());
                int y = Convert.ToInt32(reader1[1].ToString());
                int z = Convert.ToInt32(reader1[2].ToString());

                int ch = a + x;
                int tb = b + y;
                int bh = c + z;
                //for update
                SqlCeCommand cm = new SqlCeCommand("Update tbl_inventory set Chairs=@c,Tables=@t,Booth=@b where ID = 1", con);
                cm.Parameters.AddWithValue("@c", ch);
                cm.Parameters.AddWithValue("@t", tb);
                cm.Parameters.AddWithValue("@b", bh);
                cm.ExecuteNonQuery();
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

        private void textBox35_TextChanged(object sender, EventArgs e)
        {
            this.textBox35.Update();
        }

        private void PerOp1()
        {
            SqlCeConnection con = new SqlCeConnection(cs);
            try
            {
                con.Open();
                SqlCeCommand cmd = new SqlCeCommand("Select Chairs,Tables,Booth from tbl_inventory", con);
                SqlCeDataReader reader = cmd.ExecuteReader();
                reader.Read();
                int a = Convert.ToInt32(reader[0].ToString());
                int b = Convert.ToInt32(reader[1].ToString());
                int c = Convert.ToInt32(reader[2].ToString());

                SqlCeCommand cd = new SqlCeCommand("Select ChairsRented,TablesRented,TentsRented from tbl_onrent where ID=@id", con);
                cd.Parameters.AddWithValue("@id", textBox13.Text);
                SqlCeDataReader reader1 = cd.ExecuteReader();
                reader1.Read();
                int x = Convert.ToInt32(reader1[0].ToString());
                int y = Convert.ToInt32(reader1[1].ToString());
                int z = Convert.ToInt32(reader1[2].ToString());

                int ch2 = a - x;
                int tb2 = b - y;
                int bh2 = c - z;
                //for update
                SqlCeCommand cm = new SqlCeCommand("Update tbl_inventory set Chairs=@c2,Tables=@t2,Booth=@b2 where ID = 1", con);
                cm.Parameters.AddWithValue("@c2", ch2);
                cm.Parameters.AddWithValue("@t2", tb2);
                cm.Parameters.AddWithValue("@b2", bh2);
                cm.ExecuteNonQuery();
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

        private void getTotal()
        {
            int colTotal = 0;
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            SqlCeDataAdapter adapt = new SqlCeDataAdapter("Select Amount from tbl_budget", con);
            DataTable dt = new DataTable();
            adapt.Fill(dt);
            foreach (DataColumn col in dt.Columns)
            {
                foreach (DataRow row in col.Table.Rows)
                {
                    colTotal += Int32.Parse(row[col].ToString());
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            int count = 0;
            var date = DateTime.Now;
            if (textBox49.Text != "" || textBox50.Text != "")
            {
                try
                {
                    SqlCeCommand cmd = new SqlCeCommand("Update tbl_budget set Date=@mb,[Expense Category]=@de,[Expense Amount]=@te,[Mode Of Payment]=@am, Notes=@nt where ID = @id", con);
                    cmd.Parameters.AddWithValue("@id", textBox34.Text);
                    cmd.Parameters.AddWithValue("@mb", SqlDbType.Date).Value = dateTimePicker1.Value.Date.ToString("MM-dd-yyyy");
                    cmd.Parameters.AddWithValue("@de", textBox49.Text);
                    cmd.Parameters.AddWithValue("@te", textBox50.Text);
                    cmd.Parameters.AddWithValue("@am", textBox51.Text);
                    cmd.Parameters.AddWithValue("@nt", richTextBox4.Text);
                    cmd.ExecuteNonQuery();
                    if (textBox48.Text != "")
                    {
                        SqlCeCommand cd = new SqlCeCommand("Update tbl_accounts set Month=@mt,[Monthly Expenses]=@me where ID=@id", con);
                        cd.Parameters.AddWithValue("@id", textBox34.Text);
                        cd.Parameters.AddWithValue("@mt", comboBox9.GetItemText(comboBox9.SelectedItem));
                        cd.Parameters.AddWithValue("@me", textBox49.Text);
                        cd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Updated Successfully!");
                    count++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    panel4.Visible = false;
                    refresh1();
                    con.Close();
                }

                //logs budget *update*
                if (count == 1)
                {
                    SqlCeConnection con1 = new SqlCeConnection(cs);
                    con1.Open();
                    SqlCeCommand c = new SqlCeCommand("Select * from logs ORDER by Date_Time DESC", con1);
                    SqlCeDataReader reader;
                    reader = c.ExecuteReader();
                    reader.Read();
                    string name = reader[0].ToString();
                    var nm = textBox49.Text;
                    string activity = "Updated expense: " + nm;
                    SqlCeCommand cd = new SqlCeCommand("Insert into logs (Name,Role,Date_time,Activity) values (@nm,@rl,@dt,@act)", con1);
                    cd.Parameters.AddWithValue("@nm", name);
                    cd.Parameters.AddWithValue("@rl", label80.Text);
                    cd.Parameters.AddWithValue("@dt", date);
                    cd.Parameters.AddWithValue("@act", activity);
                    cd.ExecuteNonQuery();
                    con1.Close();
                }
            }
        }

        private void tbl_userDataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.Value != null)
            {
                e.Value = new String('*', e.Value.ToString().Length);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
            panel8.Visible = true;
            panel12.Visible = true;
            panel13.Visible = true;
            panel6.Visible = true;
        }

        //for print
        private System.Drawing.Printing.PrintDocument document = new System.Drawing.Printing.PrintDocument();

        Bitmap bm;

        private void button12_Click_2(object sender, EventArgs e)
        {
            int height = logsDataGridView.Height;
            logsDataGridView.Height = logsDataGridView.RowHeadersWidth * logsDataGridView.RowTemplate.Height * 2;
            bm = new Bitmap(this.logsDataGridView.Width, this.logsDataGridView.Height);
            logsDataGridView.DrawToBitmap(bm, new Rectangle(0, 0, logsDataGridView.Width, logsDataGridView.Height));
            printPreviewDialog1.ShowDialog();
        }

        private void logsrefresh()
        {
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            SqlCeDataAdapter ad = new SqlCeDataAdapter("Select * from logs ORDER by ID DESC", con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            logsDataGridView.DataSource = dt;
            con.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            logsrefresh();
        }

        private void textBox52_TextChanged(object sender, EventArgs e)
        {
            if (textBox52.Text != string.Empty)
            {
                SqlCeConnection con = new SqlCeConnection(cs);
                con.Open();
                SqlCeDataAdapter adapt = new SqlCeDataAdapter("Select * from logs where Name like '" + textBox52.Text + "'", con);
                DataTable dt = new DataTable();
                adapt.Fill(dt);
                logsDataGridView.DataSource = dt;
                con.Close();
            }
            else
            {
                return;
            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            panel14.Visible = false;
        }

        private void button38_Click(object sender, EventArgs e)
        {
            panel14.Visible = true;
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bm, 0, 0);
        }

        private void button40_MouseEnter(object sender, EventArgs e)
        {
            groupBox1.Text = "Generated Reports";
        }

        private void button40_MouseLeave(object sender, EventArgs e)
        {
            groupBox1.Text = "";
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {
                ListViewItem item = listView3.SelectedItems[0];
                textBox6.Text = item.SubItems[0].Text;
                textBox48.Text = item.SubItems[1].Text;
                int xcb; 
                xcb = this.comboBox9.FindString(listView3.SelectedItems[0].SubItems[0].Text); 
                this.comboBox9.SelectedIndex = xcb;
            }
            else
            {
                textBox6.ResetText();
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (textBox48.Text != "")
            {
                DialogResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo);
    
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            SqlCeConnection con = new SqlCeConnection(cs);
                            SqlCeCommand cd = new SqlCeCommand("Insert into tbl_accounts ([Month],[Monthly Expenses]) values (@mt,@me)", con);
                            cd.Parameters.AddWithValue("@mt", comboBox9.GetItemText(comboBox9.SelectedItem));
                            cd.Parameters.AddWithValue("@me", textBox48.Text);
                            cd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            textBox48.ResetText();
                            monthrefresh();
                        }
                    }
                    else
                    {
                        return;
                    }
            }
                
                else
                {
                    MessageBox.Show("Please Enter records");
                }
        
        }

        private void button41_Click(object sender, EventArgs e)
        {
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            if (textBox6.Text != "" || textBox48.Text != "")
            {
                DialogResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        SqlCeCommand cd = new SqlCeCommand("Update tbl_accounts set [Month]=@mt,[Monthly Expenses]=@me where Month=@id", con);
                        cd.Parameters.AddWithValue("@id", textBox6.Text);
                        cd.Parameters.AddWithValue("@mt", comboBox9.GetItemText(comboBox9.SelectedItem));
                        cd.Parameters.AddWithValue("@me", textBox48.Text);
                        cd.ExecuteNonQuery();
                        MessageBox.Show("Updated Successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        textBox48.ResetText();
                        monthrefresh();
                    }
                }
                else
                {
                    return;
                }
            }

                else
                {
                    MessageBox.Show("Please Select Records to update!");
                }
            }

        private void monthrefresh()
        {
            listView3.Items.Clear();
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            SqlCeCommand cmd = new SqlCeCommand("SELECT * FROM tbl_accounts ORDER by ID ASC", con);
            SqlCeDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListViewItem item2 = new ListViewItem(dr[1].ToString());
                item2.SubItems.Add(dr[2].ToString());
                listView3.Items.Add(item2);
            }
            con.Close();
            dr.Close();
        }

        private void button39_Click(object sender, EventArgs e)
        {
            string datefrom = dateTimePicker7.Value.ToString();
            string dateto = dateTimePicker8.Value.ToString();
            try
            {
                SqlCeConnection con = new SqlCeConnection(cs);
                con.Open();
                SqlCeCommand cmd = new SqlCeCommand("Select * from logs where Date_time BETWEEN @sd and @ed ORDER BY ID DESC", con);
                cmd.Parameters.AddWithValue("@sd", datefrom);
                cmd.Parameters.AddWithValue("@ed", dateto);
                cmd.ExecuteNonQuery();
                SqlCeDataAdapter adapt = new SqlCeDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapt.Fill(dt);
                logsDataGridView.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        static int counter1 = 0;

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            counter1++;
            var date = DateTime.Now;
            groupBox11.Visible = true;
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            SqlCeCommand cmd = new SqlCeCommand("Select * from tbl_rent where ReturnDate <= @dt", con);
            cmd.Parameters.AddWithValue("@dt", SqlDbType.Date).Value = date.Date.ToString("MM-dd-yyyy");
            SqlCeDataAdapter adapt = new SqlCeDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapt.Fill(dt);
            int cn = dt.Rows.Count;
            tbl_rentDataGridView.DataSource = dt;
            con.Close();
            //another click
            if (counter1 == 2)
            {
                groupBox11.Visible = false;
                counter1 = 0;
            }
            label71.Text = cn.ToString();
            pictureBox3.Visible = false;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            counter1++;
            var date = DateTime.Now;
            groupBox11.Visible = true;
            //another click
            if (counter1 == 2)
            {
                groupBox11.Visible = true;
                counter1 = 0;
            }
            pictureBox3.Visible = true;
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();
            SqlCeCommand cd = new SqlCeCommand("SELECT * from tbl_onrent WHERE ID IN (Select ID from tbl_rent WHERE ReturnDate <= @dt)", con);
            cd.Parameters.AddWithValue("@dt", SqlDbType.Date).Value = date.Date.ToString("MM-dd-yyyy");
            SqlCeDataAdapter adp = new SqlCeDataAdapter(cd);
            DataTable dt2 = new DataTable();
            adp.Fill(dt2);
            tbl_onrentDataGridView1.DataSource = dt2;
            con.Close();
        }

        private void button40_Click(object sender, EventArgs e)
        {
            Form5 x = new Form5();
            x.Show();
            this.Hide();
        }

        private void tbl_onrentDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int cb,cb2,cb3;
            cb = this.comboBox6.FindString(tbl_onrentDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
            this.comboBox6.SelectedIndex = cb;
            cb2 = this.comboBox7.FindString(tbl_onrentDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString());
            this.comboBox7.SelectedIndex = cb2;
            cb3 = this.comboBox8.FindString(tbl_onrentDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString());
            this.comboBox8.SelectedIndex = cb3;
            textBox12.Text = tbl_onrentDataGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
        }
    }
}
