using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba1c
{
    public partial class Form2 : Form
    {
        SqlConnection sqlConnection;
        int login_count = 3;

        public Form2()
        {
            TopMost = true;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (label3.Visible && label4.Visible)
            {
                label3.Visible = false;
                label4.Visible = false;
            }

            string ConnectionString = @"Data Source=(local)\SQLEXPRESS;Initial Catalog=Автосалон;Integrated Security=True";

            sqlConnection = new SqlConnection(ConnectionString);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(textBox1.Text) && (!string.IsNullOrEmpty(textBox2.Text)&&
                !string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text)))
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT Count(*) from [Login] where Username = '" + textBox1.Text + "' and Password = '" + textBox2.Text + "'", sqlConnection);

                DataTable dt = new DataTable();
                sda.Fill(dt);


                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        this.Hide();
                        Form1 f = new Form1();
                        f.Show();
                }
                    else
                {
                    
                    login_count--;
                    label3.Visible = true;
                    label3.Text = "Неверный логин или пароль";
                    label4.Visible = true;
                    label4.Text = "Осталось попыток:" + login_count.ToString();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    if (login_count == 0)
                    {
                        MessageBox.Show("Превышено максимально допестимое число неверных попыток");
                        Application.Exit();
                    }
                }
                
            }

            else
            {
                MessageBox.Show("Пожалуйста, введите имя пользователя и пароль!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


    }
}
