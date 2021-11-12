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
using System.IO;
namespace Car
{
    public partial class Form1 : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Xhemo\source\repos\Car\Car\Database\DB_Server.mdf;Integrated Security=True;Connect Timeout=30");
        public int i =1; 
        public Form1()
        {
            InitializeComponent();
        }

       

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
            this.Hide();
        }

       

      

        private void front_Click(object sender, EventArgs e)
        {
           
          
            if (GET_Id("front"))
            {  
                SqlCommand cmd1 = new SqlCommand("SELECT  ID ,[Name company],price , color , [made in] , [Number of seats] , [maximum speed] , [Engine power] , [fuel tank] , Image  FROM Table1  Where ID = '" + i + "'");

                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = connect;
                connect.Open();
                SqlDataReader sdr = cmd1.ExecuteReader();

                sdr.Read();

                

                    ID_no.Text = sdr["ID"].ToString();
                    Name_com.Text = sdr["Name company"].ToString();
                    Price.Text = sdr["price"].ToString();

                    color.Text = sdr["color"].ToString();
                    Made_in.Text = sdr["made in"].ToString();
                    no_of_seat.Text = sdr["Number of seats"].ToString();
                    mx_speed.Text = sdr["maximum speed"].ToString();
                    Power.Text = sdr["Engine power"].ToString();
                    fuel_tank.Text = sdr["fuel tank"].ToString();
                    if (sdr["Image"].ToString() != "")
                    {
                        
                       

                        MemoryStream mIamge = new MemoryStream((byte[])sdr["Image"]);
                        pictureBox1.Image = Image.FromStream(mIamge);
                    }
                    else
                    {
                        pictureBox1.Image = null;
                    }
                

                connect.Close();

            }
            
        }

        private void behind_Click(object sender, EventArgs e)
        {
         

            if (GET_Id("behind"))
            {
                SqlCommand cmd1 = new SqlCommand("SELECT  ID ,[Name company],price , color , [made in] , [Number of seats] , [maximum speed] , [Engine power] , [fuel tank] , Image  FROM Table1  Where ID = '" + i + "'");

                cmd1.CommandType = CommandType.Text;
                cmd1.Connection = connect;
                connect.Open();
                SqlDataReader sdr = cmd1.ExecuteReader();

                sdr.Read();
                

                    ID_no.Text = sdr["ID"].ToString();
                    Name_com.Text = sdr["Name company"].ToString();
                    Price.Text = sdr["price"].ToString();

                    color.Text = sdr["color"].ToString();
                    Made_in.Text = sdr["made in"].ToString();
                    no_of_seat.Text = sdr["Number of seats"].ToString();
                    mx_speed.Text = sdr["maximum speed"].ToString();
                    Power.Text = sdr["Engine power"].ToString();
                    fuel_tank.Text = sdr["fuel tank"].ToString();
                    if (sdr["Image"].ToString() != "")
                    {
                        byte[] image = ((byte[])sdr["Image"]);
                        pictureBox1.Image = null;

                        MemoryStream mIamge = new MemoryStream(image);
                        pictureBox1.Image = Image.FromStream(mIamge);
                     
                  
                    }
                    else
                    {
                        pictureBox1.Image = null;
                    }
                

               

                connect.Close();
            }
        }

      
          public bool GET_Id(string Fun_Mode)
        {
            bool check = false;
            int num = 0 ; 
            SqlCommand cmd1 = new SqlCommand("SELECT [ID]  FROM [Table1] ");
            cmd1.CommandType = CommandType.Text;
            cmd1.Connection = connect;

            connect.Open();
            SqlDataReader sdr = cmd1.ExecuteReader();


            if (Fun_Mode == "front")      { i++; }
            else  { i--; }

            
            
            while (sdr.Read())
            {
                  
                if (i == Convert.ToInt32(sdr["ID"]))
                {
                    check = true;
                    break;
                }
                else
                {
                    if(Convert.ToInt32(sdr["ID"]) > i && Fun_Mode == "behind" && i > -1)
                    {
                      
                        i--;
                        break;
                    }
                    
                    else if (Convert.ToInt32(sdr["ID"]) > i)
                      {


                        i = Convert.ToInt32(sdr["ID"]);
                        check = true;

                      
                        break;

                      }
                    

                }
               
              

                num = Convert.ToInt32(sdr["ID"]);
            }


             /// true           true
             if (!check && Fun_Mode == "front") { i = num;  }
            

            connect.Close();

            return check;

        }
       
    }
}
