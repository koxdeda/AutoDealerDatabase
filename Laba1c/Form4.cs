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
    public partial class Form4 : Form
    {
        SqlConnection sqlConnection;
        string ConnectionString = @"Data Source=(local)\SQLEXPRESS;Initial Catalog=Автосалон;Integrated Security=True; MultipleActiveResultSets=True";
        int car_kit1, car_kit2, car_kit3, car_kit4, car_kit5, car_kit6, car_kit7, car_kit8;

        private void button2_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7();
            f7.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();
            f6.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.Show();

        }

        int tmp_price = StatClass.car_price;
        public Form4()
        {
            InitializeComponent();
        }

        private async void Form4_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;

            sqlConnection = new SqlConnection(ConnectionString);
            await sqlConnection.OpenAsync();

            label28.Text = StatClass.car_name;
            label2.Text = StatClass.car_year;
            label12.Text = StatClass.car_color;
            label14.Text = StatClass.car_corobka;
            label26.Text = StatClass.car_engine;
            label18.Text = StatClass.car_engine_power;
            label20.Text = StatClass.car_engine_type;
            label27.Text = StatClass.car_privod;
            label29.Text = Convert.ToString(StatClass.car_price);

        }

        // Очистка комплектации
        private void button5_Click(object sender, EventArgs e)
        {
            StatClass.car_price = tmp_price;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            label29.Text = Convert.ToString(StatClass.car_price);

        }
        // Применение комплектации(расчет цены в соотвествии с выбранной комплектацией)
        private async void button4_Click(object sender, EventArgs e)
        {

 
            SqlCommand command = new SqlCommand("Select * from ЦенаКомплектации", sqlConnection);


            SqlDataReader sqlReader = null;

            try
            {
                sqlReader = await command.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {

                    car_kit1 = Convert.ToInt32(sqlReader["Боковые_подушки_безопасности"]);
                    car_kit2 = Convert.ToInt32(sqlReader["Датчики_парковки"]);
                    car_kit3 = Convert.ToInt32(sqlReader["Тонировка_стекол_2го_ряда"]);
                    car_kit4 = Convert.ToInt32(sqlReader["Кожаная_отделка_салона"]);
                    car_kit5 = Convert.ToInt32(sqlReader["Климат_контроль"]);
                    car_kit6 = Convert.ToInt32(sqlReader["Датчик_света"]);
                    car_kit7 = Convert.ToInt32(sqlReader["Круиз_контроль"]);
                    car_kit8 = Convert.ToInt32(sqlReader["Аудиосистема_CD"]);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }


               
                if (checkBox1.Checked == true)
                {
                StatClass.car_kit_price += car_kit1;
                }
                if (checkBox2.Checked == true)
                {
                StatClass.car_kit_price += car_kit2;
                }
                if (checkBox3.Checked == true)
                {
                StatClass.car_kit_price += car_kit3;
                }
                if (checkBox4.Checked == true)
                {
                StatClass.car_kit_price += car_kit4;
                }
                if (checkBox5.Checked == true)
                {
                StatClass.car_kit_price += car_kit5;
                }
                if (checkBox6.Checked == true)
                {
                StatClass.car_kit_price += car_kit6;
                }
                if (checkBox7.Checked ==true)
                {
                StatClass.car_kit_price += car_kit7;
                }
                if (checkBox8.Checked == true)
                {
                StatClass.car_kit_price += car_kit8;
                }

            StatClass.car_price += StatClass.car_kit_price;
            label29.Text = Convert.ToString(StatClass.car_price);
            //StatClass.car_price = tmp_price;
            StatClass.car_kit_price = 0;


        }
    }
}

