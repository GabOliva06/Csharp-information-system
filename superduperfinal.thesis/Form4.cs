using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.Configuration;
using System.IO;
using System.Diagnostics;

namespace superduperfinal.thesis
{
    public partial class Form4 : Form
    {
        String cs = @"Data Source=C:\Users\Admin\Documents\Gabriell-Oliva\b190\C# system\superduperfinal.thesis\superduperfinal.thesis\dbcon.sdf";   
        SqlCeCommand cmd;   
        SqlCeDataAdapter adapt;

        public Form4()
        {
            InitializeComponent();
        }

        private void loaddata()
        {
            SqlCeConnection con = new SqlCeConnection(cs);
            con.Open();   
            DataTable dt=new DataTable();   
            adapt=new SqlCeDataAdapter("select * from tbl_people",con);   
            adapt.Fill(dt);      
            con.Close();   
        }
        private void button3_Click(object sender, EventArgs e)
        {
            int count = 0;
            if (textBox1.Text != "" || textBox2.Text != "")
            {
                try
                {
                    SqlCeConnection con = new SqlCeConnection(cs);
                    cmd = new SqlCeCommand("insert into tbl_people([Nick Name],[First Name],[Middle Name],[Last Name],[Street Name],[House #],[Birthday],[Gender],[Status],[Birthplace],[Occupation],[Records],[Age],[Image],[Stats]) values (@Name,@Fname,@Mname,@Lname,@Street,@House,@Birthdate,@Gender,@Status,@Bplace,@Occ,@Rec,@Age,@Pic,@st)", con);
                    con.Open();
                    MemoryStream stream = new MemoryStream();
                    pictureBox2.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] pic = stream.ToArray();
                    int age = DateTime.Now.Year - dateTimePicker1.Value.Year - (DateTime.Now.DayOfYear < dateTimePicker1.Value.DayOfYear ? 1 : 0);
                    cmd.Parameters.AddWithValue("@Name", textBox12.Text);
                    cmd.Parameters.AddWithValue("@Fname", textBox1.Text);
                    cmd.Parameters.AddWithValue("@Mname", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Lname", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Street", textBox4.Text);
                    cmd.Parameters.AddWithValue("@House", textBox5.Text);
                    cmd.Parameters.AddWithValue("@Birthdate", SqlDbType.Date).Value = dateTimePicker1.Value.Date.ToString("MMddyyyy");
                    cmd.Parameters.AddWithValue("@Gender", comboBox1.GetItemText(comboBox1.SelectedItem));
                    cmd.Parameters.AddWithValue("@Status", comboBox2.GetItemText(comboBox2.SelectedItem));
                    cmd.Parameters.AddWithValue("@Bplace", textBox9.Text);
                    cmd.Parameters.AddWithValue("@Occ", textBox10.Text);
                    cmd.Parameters.AddWithValue("@Rec", textBox11.Text);
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@Pic", pic);
                    cmd.Parameters.AddWithValue("@st", "Active");
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("RECORDS INSERTED SUCCESSFULLY!");
                    loaddata();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    this.Close();
                    loaddata();
                    count++;
                }
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
                var nm = textBox1.Text;
                string activity = "ADDED RECORDS OF: " + nm;
                SqlCeCommand cd = new SqlCeCommand("Insert into logs (Name,Role,Date_time,Activity) values (@nm,@rl,@dt,@act)", con1);
                cd.Parameters.AddWithValue("@nm", name);
                cd.Parameters.AddWithValue("@rl", role);
                cd.Parameters.AddWithValue("@dt", date);
                cd.Parameters.AddWithValue("@act", activity);
                cd.ExecuteNonQuery();
                con1.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Choose Image";
            open.Filter = "Images (*.JPEG;*.BMP;*.JPG;*.GIF;*.PNG;*.)|*.JPEG;*.BMP;*.JPG;*.GIF;*.PNG";
            if (open.ShowDialog() == DialogResult.OK)
            {
                Image img = new Bitmap(open.FileName);
                pictureBox2.Image = img; //resizeImage(img);
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}