using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;

namespace superduperfinal.thesis
{
    public partial class Form1 : Form
    {
        
        String cs = @"Data Source=C:\Users\Admin\Documents\Gabriell-Oliva\b190\C# system\superduperfinal.thesis\superduperfinal.thesis\dbcon.sdf";
        public Form1()
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


        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.ResetText();
            pictureBox1.Visible = true;
            
            if (textBox2.Text == "")
            {
                textBox2.Text = "Password";
                pictureBox2.Visible = false;
            }
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.ResetText();
            pictureBox2.Visible = true;
            
            if (textBox1.Text == "")
            {
                textBox1.Text = "Username";
                pictureBox1.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var date = DateTime.Now;
            SqlCeConnection con = new SqlCeConnection(cs);
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Please provide Username and Password");
                return;
            }
            else if (textBox1.Text == "Username" || textBox2.Text == "Password")
            {
                MessageBox.Show("Please provide Username and Password");
                return;
            }
            else if(textBox1.Text != "")
            {
                try
                {
                    string login = "Login";
                    SqlCeCommand cmd = new SqlCeCommand("Select * from tbl_user where Username=@user and Password=@pass", con);
                    cmd.Parameters.AddWithValue("@user", textBox1.Text);
                    cmd.Parameters.AddWithValue("@pass", textBox2.Text);
                    con.Open();
                    SqlCeDataAdapter adapt = new SqlCeDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapt.Fill(ds);
                    int count = ds.Tables[0].Rows.Count;
                    SqlCeDataReader myReader;
                    myReader = cmd.ExecuteReader();
                    string userRole = string.Empty;
                    while (myReader.Read())
                    {
                        userRole = myReader["Role"].ToString();
                    }
                    if (count == 1)
                    {
                        MessageBox.Show("Login Successful!");
                        if (userRole == "Admin")
                        {
                            Form3 fm = new Form3();
                            fm.label80.Text = userRole.ToString();
                            fm.Show();
                            this.Hide();
                        }

                        else if (userRole == "User")
                        {
                            string role = string.Empty;
                            string Uname = string.Empty;
                            Form3 fm = new Form3();
                            fm.Show();
                            fm.label80.Text = userRole.ToString();
                            fm.button4.Enabled = false;
                            fm.button7.Enabled = false;
                            fm.button9.Enabled = false;
                            fm.label14.Enabled = false;
                            fm.label15.Enabled = false;
                            fm.label11.Enabled = false;
                            SqlCeCommand cd = new SqlCeCommand("Select [First Name] from tbl_user where Username = @user", con);
                            cd.Parameters.AddWithValue("@user", textBox1.Text);
                            SqlCeDataReader reader;
                            reader = cd.ExecuteReader();
                            reader.Read();
                            Uname = reader["First Name"].ToString();
                            SqlCeCommand com = new SqlCeCommand("Insert into logs (Name,Role,Date_Time,Activity) values (@nm,@role,@date,@log)", con);
                            com.Parameters.AddWithValue("@nm", Uname);
                            com.Parameters.AddWithValue("@role", userRole);
                            com.Parameters.AddWithValue("@date", date);
                            com.Parameters.AddWithValue("@log", login);
                            com.ExecuteNonQuery();
                            this.Hide();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Incorrect username or password");
                        pictureBox1.Visible = false;
                        pictureBox2.Visible = false;
                    }
                    if (userRole == "Admin")
                    {
                        SqlCeCommand cd = new SqlCeCommand("Select * from tbl_user where Username=@id", con);
                        cd.Parameters.AddWithValue("@id", textBox1.Text);
                        SqlCeDataReader read;
                        read = cd.ExecuteReader();
                        string Uname = string.Empty;
                        read.Read();
                        Uname = read[3].ToString();
                        SqlCeCommand com = new SqlCeCommand("Insert into logs (Name,Role,Date_Time,Activity) values (@nm,@role,@date,@log)", con);
                        com.Parameters.AddWithValue("@nm", Uname);
                        com.Parameters.AddWithValue("@role", userRole);
                        com.Parameters.AddWithValue("@date", date);
                        com.Parameters.AddWithValue("@log", login);
                        com.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();
                    ClearData();
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            pictureBox2.Visible = true;
            if (textBox2.Text == "Password")
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            ClearData();
        }
    
        private void ClearData()
        {
            textBox1.Text = "Username";
            textBox2.Text = "Password";
        }  

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            pictureBox1.Visible = true;
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            pictureBox2.Visible = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            ClearData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
