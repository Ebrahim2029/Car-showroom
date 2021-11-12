using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Car
{
   
    public partial class EditUsers : Form
    {

        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Xhemo\source\repos\Car\Car\Database\DB_Server.mdf;Integrated Security=True;Connect Timeout=30");

        public string username = "";
       

        public EditUsers()
        {
            InitializeComponent();
        }

        private void EditUsers_Load(object sender, EventArgs e)
        {
            display();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          int rowindex = e.RowIndex;
            
           

                username = dataGridView1.Rows[rowindex].Cells[0].Value.ToString();
                textBox1.Text = dataGridView1.Rows[rowindex].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.Rows[rowindex].Cells[1].Value.ToString();


        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "" && textBox2.Text != "")
            {
                connect.Open();
                SqlCommand cmd = connect.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update  [users] set [user] = '" + textBox1.Text + "' ,  [password] = '" + textBox2.Text + "'  where  [user] = '" + username + "'  ";
                cmd.ExecuteNonQuery();
                connect.Close();
                display();

            }
            else
            {
                MessageBox.Show(" اختر الحقل من الجدول ");
            }
        }

        public void display()
        {

            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT  [user] ,  [password]  FROM users  ";
            cmd.ExecuteNonQuery();
            DataTable dta = new DataTable();
            SqlDataAdapter datadp = new SqlDataAdapter(cmd);
            datadp.Fill(dta);
            dataGridView1.DataSource = dta;
            connect.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "" && textBox2.Text != "")
            {
                connect.Open();
                SqlCommand cmd = connect.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from  [users]  where [user]= '" + username + "' ";
                cmd.ExecuteNonQuery();
                connect.Close();
                display();
                textBox1.Text = "";
                textBox2.Text = "";

            }
            else
            {
                MessageBox.Show(" اختر الحقل من الجدول ");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                connect.Open();
                SqlCommand cmd = connect.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into [users] ([user] ,  [password]) values ('" + textBox1.Text + "' , '" + textBox2.Text + "')";
                cmd.ExecuteNonQuery();
                connect.Close();
                display();
                textBox1.Text = "";
                textBox2.Text = "";

            }
            else
            {
                MessageBox.Show(" ادخل كلمة اسم المستخدم وكلمة المرور ");
            }

        }
    }
}
