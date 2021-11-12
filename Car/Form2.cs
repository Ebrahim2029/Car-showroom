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
    public partial class Form2 : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Xhemo\source\repos\Car\Car\Database\DB_Server.mdf;Integrated Security=True;Connect Timeout=30");
        public String imageLocation = "";
        public Form2()
        {
            InitializeComponent();
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            display_Data();
        }

        /// <summary>
        /// Buttons Insert , Edit , Display and Delete 
        /// </summary>
        
        private void Insert_Click(object sender, EventArgs e)
        {
            if (ID.Text != "" && Name_company.Text != "" && price.Text != "" && color.Text != "" && made_in.Text != "" && Number_of_seats.Text != "" && maximum_speed.Text != "" && Engine_power.Text != "" && fuel_tank.Text != "" )
            {
                int check = check_Id(ID.Text , "Insert");

               
               if (check <  0)
                    {
                    return;
                }
              
               else if (imageLocation == "")
                {
                    DialogResult dialogResult = MessageBox.Show("الاستمرار في ادخال البيانات دون تحميل صورة", " تحميل صورة", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        
                       
                        INsert_data_to_DB();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }


                }
                else if (Convert.ToInt32(ID.Text) > check)
                {
                    DialogResult dialogResult = MessageBox.Show("  لم يتم ادخال رقم " + check.ToString() + " هل تريد الاستمرار  ", check.ToString() + " رقم ", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                      
                       INsert_data_to_DB();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }

                }
                else
                {
                    
                      INsert_data_to_DB();
                }
              

               
               
            }
            else
            {
                MessageBox.Show("تحقق من ادخال جميع البيانات المطلوبة");
            }
          

        }
       
        private void Delete_Click(object sender, EventArgs e)
        {
            if (ID.Text != "")
            {
                connect.Open();
                SqlCommand cmd = connect.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from  [Table1] where ID= '" + ID.Text + "' ";
                cmd.ExecuteNonQuery();
                connect.Close();
                ID.Text = "";
                Name_company.Text = "";
                price.Text = "";
                color.Text = "";
                made_in.Text = "";
                Number_of_seats.Text = "";
                maximum_speed.Text = "";
                Engine_power.Text = "";
                fuel_tank.Text = "";
                pic1.Image = null;
                display_Data();
                MessageBox.Show("تم مسح البيانات  ");
            }
            else
            {
                MessageBox.Show("ادخل رقم الحقل المطلوب مسحه ");
            }
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            int check = check_Id(ID.Text , "Edit");

          


             if ( Convert.ToInt32(ID.Text) >  check )
            {
                 MessageBox.Show("  هذا الرقم لايوجد في قاعدة البيانات  ", check.ToString() + " رقم ");
                return;
                

            }
            else if (imageLocation == "")
            {
                DialogResult dialogResult = MessageBox.Show("الاستمرار في ادخال البيانات دون تحميل صورة", " تحميل صورة", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Edit_data_to_DB();
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }


            }
            else
            {
                Edit_data_to_DB();
            }

        }

        private void Display_Click(object sender, EventArgs e)
        {
            display_Data();
        }

        private void Upload_Image_Click(object sender, EventArgs e)
        {
           
            
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image files(*.jpg; *.png; *.gif; *.bmp; )| *.jpg; *.png; *.gif; *.bmp;";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imageLocation = dialog.FileName;
                  

                   pic1.Image = Image.FromFile(imageLocation);
                   
                    

                }
            

        }

      /// <summary>
      /// DataGridView Event " When Uer Click To Cell Get Data From DataGridview To TextBoxs 
      /// </summary>
     
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;

         ID.Text              = dataGridView1.Rows[index].Cells[0].Value.ToString();
         Name_company.Text    = dataGridView1.Rows[index].Cells[1].Value.ToString();
         price.Text           = dataGridView1.Rows[index].Cells[2].Value.ToString();
         color.Text           = dataGridView1.Rows[index].Cells[3].Value.ToString();
         made_in.Text         = dataGridView1.Rows[index].Cells[4].Value.ToString();
         Number_of_seats.Text = dataGridView1.Rows[index].Cells[5].Value.ToString();
         maximum_speed.Text   = dataGridView1.Rows[index].Cells[6].Value.ToString();
         Engine_power.Text    = dataGridView1.Rows[index].Cells[7].Value.ToString();
         fuel_tank.Text       = dataGridView1.Rows[index].Cells[8].Value.ToString();

            if (dataGridView1.Rows[index].Cells[9].Value.ToString() != "")
            {
                MemoryStream ms = new MemoryStream((byte[])dataGridView1.Rows[index].Cells[9].Value);
                pic1.Image = Image.FromStream(ms);
            }
            else
            {
                pic1.Image = null;
            }
        }

       

        /// <summary>
        /// Check ID from Database if User Make Error When Insert Or Edit Data
        /// </summary>
        /// <param name="ID"> Send Number from TextBox (ID) </param>
        /// <param name="Get_Fun"> Check The Function From Insert OR Edit </param>
        /// <returns> The Number </returns>

        public int check_Id(string ID , string Get_Fun)
        {

            SqlCommand cmd1 = new SqlCommand("SELECT [ID]  FROM [Table1] ");
            cmd1.CommandType = CommandType.Text;
            cmd1.Connection = connect;

            connect.Open();
            SqlDataReader sdr = cmd1.ExecuteReader();

            int i = 1;


            while (sdr.Read())
            {
                if (ID == sdr["ID"].ToString() && Get_Fun == "Insert")
                {
                    MessageBox.Show("هذا الرقم موجود سابقا في قاعدة البيانات ");
                    i = -1;
                    break;
                    
                }
              else  if ( ( i == Convert.ToInt32(sdr["ID"].ToString()) || Convert.ToInt32(sdr["ID"].ToString()) == 0 )  && Get_Fun == "Insert")

                {
                    i++; continue;
                }
                else if (Get_Fun == "Edit")
                {
                    i++;
                }
               

              

            }

            connect.Close();

            return i;

        }




        /// <summary>
        /// Functions to Insert , Dispaly and Edit Data To DataBase 
        /// </summary>
        public void display_Data()
        {
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = " select * from [Table1]";
            cmd.ExecuteNonQuery();
            DataTable dta = new DataTable();
            SqlDataAdapter datadp = new SqlDataAdapter(cmd);
            datadp.Fill(dta);
            dataGridView1.DataSource = dta;
            connect.Close();


        }
        public void INsert_data_to_DB()
        {
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into [Table1] " +
           "(ID ,[Name company],price , color , [made in] , [Number of seats] , [maximum speed] , [Engine power] , [fuel tank]) " +
           " values ('" + ID.Text + "' , '" + Name_company.Text + "' , '" + price.Text + "' , '" + color.Text + "' , '" + made_in.Text + "' , '" + Number_of_seats.Text + "' , '" + maximum_speed.Text + "' , '" + Engine_power.Text + "' , '" + fuel_tank.Text + "')";
            cmd.ExecuteNonQuery();
            connect.Close();
          
            if (imageLocation != "")
            {

                byte[] images = null;

                FileStream stream = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
                BinaryReader binread = new BinaryReader(stream);
                images = binread.ReadBytes((int)stream.Length);


                connect.Open();
                string sqlquery = "update Table1 set Image = @images  where ID ='" + ID.Text + "' ";
                cmd = new SqlCommand(sqlquery, connect);
                cmd.Parameters.Add(new SqlParameter("@images", images));
                cmd.ExecuteNonQuery();
                connect.Close();

               
            }

            MessageBox.Show("تم  ادخال البيانات ");
            ID.Text = "";
            Name_company.Text = "";
            price.Text = "";
            color.Text = "";
            made_in.Text = "";
            Number_of_seats.Text = "";
            maximum_speed.Text = "";
            Engine_power.Text = "";
            fuel_tank.Text = "";
            display_Data();




        }
        public void Edit_data_to_DB()
        {
            if (ID.Text != "")
            {
                connect.Open();
                SqlCommand cmd = connect.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update [Table1] set [Name company] = '" + Name_company.Text + "' , price = '" + price.Text + "' , color =  '" + color.Text + "' ," +
                    " [made in] = '" + made_in.Text + "' ,   [Number of seats] =  '" + Number_of_seats.Text + "' , " +
                    " [maximum speed] = '" + maximum_speed.Text + "', [Engine power] =  '" + Engine_power.Text + "' , [fuel tank] = '" + fuel_tank.Text + "'  where ID= '" + ID.Text + "' ";
                cmd.ExecuteNonQuery();
                connect.Close();

                if (imageLocation != "")
                {

                    byte[] images = null;

                    FileStream stream = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
                    BinaryReader binread = new BinaryReader(stream);
                    images = binread.ReadBytes((int)stream.Length);


                    connect.Open();
                    string sqlquery = "update Table1 set Image = @images  where ID ='" + ID.Text + "' ";
                    cmd = new SqlCommand(sqlquery, connect);
                    cmd.Parameters.Add(new SqlParameter("@images", images));
                    cmd.ExecuteNonQuery();
                    connect.Close();

                  
                }
                ID.Text = "";
                Name_company.Text = "";
                price.Text = "";
                color.Text = "";
                made_in.Text = "";
                Number_of_seats.Text = "";
                maximum_speed.Text = "";
                Engine_power.Text = "";
                fuel_tank.Text = "";
                pic1.Image = null;
                display_Data();
                MessageBox.Show("تم تعديل  البيانات  ");
            }
            else
            {
                MessageBox.Show("ادخل رقم الحقل المطلوب تعديله ");
            }
        }

       



        //////////////////////// Event  For write Just Number ////////////////////////////
        private void ID_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {

                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void price_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void Number_of_seats_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void maximum_speed_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void Engine_power_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void fuel_tank_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////


        private void button4_Click(object sender, EventArgs e)
        {

            Form1 f = new Form1();
            f.Show();
            this.Close();

        }

        /// <summary>
        ///  Emptying All TextBox and PictureBox 
        /// </summary>
       
        private void Emptying_Click(object sender, EventArgs e)
        {

            ID.Text = "";
            Name_company.Text = "";
            price.Text = "";
            color.Text = "";
            made_in.Text = "";
            Number_of_seats.Text = "";
            maximum_speed.Text = "";
            Engine_power.Text = "";
            fuel_tank.Text = "";
            pic1.Image = null;
        }

    }
}






