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


    public partial class Form1 : Form
    {
        SqlConnection sqlConnection;
        //int city_key = 0;
        string machine_characteristics, selected_element;
        public Form3 form;

        string ConnectionString = @"Data Source=(local)\SQLEXPRESS;Initial Catalog=Автосалон;Integrated Security=True; MultipleActiveResultSets=True";

        public Form1()
        {
            InitializeComponent();

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "автосалонDataSet.Дилер". При необходимости она может быть перемещена или удалена.
            this.дилерTableAdapter.Fill(this.автосалонDataSet.Дилер);


            sqlConnection = new SqlConnection(ConnectionString);
            await sqlConnection.OpenAsync();
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("select distinct Марка.НазваниеМарки from [Марка] ", sqlConnection);


            try
            {
                sqlReader = await command.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    listBox2.Items.Add(Convert.ToString(Convert.ToString(sqlReader["НазваниеМарки"]) + "   "));
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
        }



        private async void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox5.Items.Clear();
            try
            {
                selected_element = listBox2.SelectedItem.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Пожалуйста, выберите марку находящуюся в данном списке");
            }
            SqlCommand command = new SqlCommand("МоделиМарки", sqlConnection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@selected_element", selected_element);

            SqlDataReader sqlReader = null;

            try
            {
                sqlReader = await command.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    listBox5.Items.Add(Convert.ToString(sqlReader["НазваниеМодели"]) + "   ");
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
        }

        private async void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                label12.Text = "";
                label14.Text = "";
                label16.Text = "";
                label18.Text = "";
                label20.Text = "";
                label26.Text = "";
                label27.Text = "";
                label31.Text = "";

            try
            {
                machine_characteristics = listBox5.SelectedItem.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                StatClass.car_name = selected_element + " " + machine_characteristics;
            
                

                SqlCommand command = new SqlCommand("ПрХарактеристикиМашины", sqlConnection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@name", machine_characteristics);

                SqlDataReader sqlReader = null;

                try
                {
                    sqlReader = await command.ExecuteReaderAsync();

                    while (await sqlReader.ReadAsync())
                    {

                    StatClass.car_color= Convert.ToString(sqlReader["Цвет"]);
                    StatClass.car_corobka = Convert.ToString(sqlReader["КоробкаПередач"]);
                    StatClass.car_engine= Convert.ToString(sqlReader["РабочийОбъем"]);
                    StatClass.car_engine_power = Convert.ToString(sqlReader["Мощность"]);
                    StatClass.car_engine_type = Convert.ToString(sqlReader["ТипДвигателя"]);
                    StatClass.car_privod = Convert.ToString(sqlReader["Привод"]);
                    StatClass.car_price = Convert.ToInt32(sqlReader["Цена"]);
                    StatClass.car_year = Convert.ToString(sqlReader["ГодВыпуска"]);
                    StatClass.car_typeTS = Convert.ToString(sqlReader["ТипТС"]);
                    StatClass.car_VIN = Convert.ToString(sqlReader["VIN"]);


                    label28.Text = StatClass.car_name;
                        label12.Text = StatClass.car_color;
                        label14.Text = StatClass.car_corobka;
                        label26.Text = StatClass.car_engine;
                        label18.Text = StatClass.car_engine_power;
                        label20.Text = StatClass.car_engine_type;
                        label27.Text = StatClass.car_privod;
                        label31.Text = StatClass.car_typeTS;

                        label29.Text = Convert.ToString(StatClass.car_price);



                }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Пожалуйста, выберите существующую модель");

                }
                finally
                {
                    if (sqlReader != null)
                        sqlReader.Close();
                }

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            SqlCommand command = new SqlCommand("фильтр_моделей", sqlConnection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@min_value", textBox8.Text);
            command.Parameters.AddWithValue("@max_value", textBox9.Text);

            SqlDataReader sqlReader = null;

            try
            {
                sqlReader = await command.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    listBox2.Items.Add(Convert.ToString(sqlReader["НазваниеМарки"]) + "   ");
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox8.Text = "";
            textBox9.Text = "";
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (listBox5.SelectedItem != null)
            {


                SqlCommand command = new SqlCommand("ДилерыМодели", sqlConnection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@model", listBox5.SelectedItem);

                SqlDataReader sqlReader = null;

                try
                {
                    sqlReader = await command.ExecuteReaderAsync();

                    while (await sqlReader.ReadAsync())
                    {
                        StatClass.model_key = Convert.ToInt32(sqlReader[0]);

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

                Form3 f3 = new Form3();
                f3.Show();
            }
            else
            {
                MessageBox.Show("Необходимо выбрать модель");
            }
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            if (listBox5.SelectedItem != null)
            {

                Form4 f4 = new Form4();
                f4.Show();
            }
            else
            {
                MessageBox.Show("Необходимо выбрать модель");
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {


            SqlCommand command1 = new SqlCommand("ДилерыМодели", sqlConnection);

            command1.CommandType = CommandType.StoredProcedure;
            command1.Parameters.AddWithValue("@model", textBox7.Text);

            SqlDataReader sqlReader = null;
            SqlDataReader sqlReader1 = null;

            try
            {

                sqlReader1 = command1.ExecuteReader();

                while (sqlReader1.Read())
                {
                    StatClass.model_key = Convert.ToInt32(sqlReader[0]);
                    sqlReader1.Close();

                }
                дилерBindingSource.Filter = "КодМодели = ' " + StatClass.model_key + " '";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



        }

        private async void button9_Click(object sender, EventArgs e)
        {
            int model_key;

            SqlCommand command = new SqlCommand("ДилерыМодели", sqlConnection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@model", textBox7.Text);

            SqlDataReader sqlReader = null;

            try
            {
                sqlReader = await command.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    model_key = Convert.ToInt32(sqlReader[0]);
                    дилерBindingSource.Filter = "КодМодели = ' " + model_key + " '";

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
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            int city_key;

            SqlCommand command = new SqlCommand("ДилерыГорода", sqlConnection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@city", textBox2.Text);

            SqlDataReader sqlReader = null;

            try
            {
                sqlReader = await command.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    city_key = Convert.ToInt32(sqlReader[0]);
                    дилерBindingSource.Filter = "КодГорода = ' " + city_key + " '";

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
        }





        // Запись в таблицу Марка
        private async void button5_Click(object sender, EventArgs e)
        {
            if (label7.Visible)
                label7.Visible = false;

            if (!string.IsNullOrEmpty(textBox4.Text) && !string.IsNullOrWhiteSpace(textBox4.Text) &&
                !string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox5.Text) &&
                !string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox5.Text))
            {
                SqlCommand command = new SqlCommand("INSERT INTO [Марка] (КодМарки, НазваниеМарки, СтранаПроизводителя) VALUES(@number, @name, @manufacturer) ", sqlConnection);

                command.Parameters.AddWithValue("@number", textBox6.Text);
                command.Parameters.AddWithValue("@name", textBox5.Text);
                command.Parameters.AddWithValue("@manufacturer", textBox4.Text);

                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                label7.Visible = true;
                label7.Text = "Все поля должны быть заполнены";
            }
        }
        // Вставка в таблицу "Продажи"
        private async void button4_Click(object sender, EventArgs e)
        {
            if (label8.Visible)
                label8.Visible = false;

            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) &&
                !string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox3.Text) &&
                !string.IsNullOrEmpty(dateTimePicker1.Text) && !string.IsNullOrWhiteSpace(dateTimePicker1.Text))
            {
                SqlCommand command = new SqlCommand("INSERT INTO [Продажа1] (КодПродажи, Дата, ФИОПокупателя) VALUES(@cod, @date, @FIO) ", sqlConnection);

                command.Parameters.AddWithValue("@cod", textBox1.Text);
                command.Parameters.AddWithValue("@date", dateTimePicker1.Value);
                command.Parameters.AddWithValue("@FIO", textBox3.Text);


                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                textBox1.Text = "";
                textBox3.Text = "";

            }
            else
            {
                label8.Visible = true;
                label8.Text = "Все поля должны быть заполнены";
            }

        }

        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
            {
                sqlConnection.Close();
                Application.Exit();
            }
        }
    }

    public static class StatClass
    {
        //Данная переменная статического класса будет доступна откуда угодно в пределах проекта
        public static int model_key = 0, trim_price = 0, car_kit_price, car_price, diler_key;
        public static int client_key;

        public static string car_name,
            car_color,
            car_corobka,
            car_engine,
            car_engine_power,
            car_engine_type,
            car_privod,
            car_typeTS,
            car_VIN,
            car_year;

    }
}





