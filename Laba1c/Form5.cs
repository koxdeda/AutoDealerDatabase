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
    public partial class Form5 : Form
    {
        SqlConnection sqlConnection;
        string ConnectionString = @"Data Source=(local)\SQLEXPRESS;Initial Catalog=Автосалон;Integrated Security=True; MultipleActiveResultSets=True";
        int stoimost = 0;
        string diler_name;
        public Form5()
        {
            InitializeComponent();
        }

        private async void Form5_Load(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();

            sqlConnection = new SqlConnection(ConnectionString);
            await sqlConnection.OpenAsync();


            SqlCommand command = new SqlCommand("select Дилер.Название  from [Дилер] ", sqlConnection);

            SqlDataReader sqlReader = null;

            try
            {
                sqlReader = await command.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    diler_name = Convert.ToString(sqlReader["Название"]);
                    comboBox2.Items.Add(diler_name);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        private async void button1_Click(object sender, EventArgs e)
        {


            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) &&
                !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text) &&
                !string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox3.Text) &&
                !string.IsNullOrEmpty(textBox4.Text) && !string.IsNullOrWhiteSpace(textBox4.Text) &&
                !string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox5.Text) &&
                !string.IsNullOrEmpty(textBox6.Text) && !string.IsNullOrWhiteSpace(textBox6.Text))
            {

                SqlCommand command = new SqlCommand("INSERT INTO [Клиент] (ФИО, Телефон) VALUES(@f, @ph) ", sqlConnection);
                command.Parameters.AddWithValue("@f", textBox1.Text);
                command.Parameters.AddWithValue("@ph", textBox2.Text);

                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                SqlCommand command3 = new SqlCommand("ДилерыМодели", sqlConnection);
                command3.CommandType = CommandType.StoredProcedure;
                command3.Parameters.AddWithValue("@model", textBox6.Text);

                SqlCommand command4 = new SqlCommand("ПрКодДилера", sqlConnection);
                command4.CommandType = CommandType.StoredProcedure;
                command4.Parameters.AddWithValue("@diler_name", diler_name);
                SqlCommand command5 = new SqlCommand("ПРКодКлиента", sqlConnection);
                command5.CommandType = CommandType.StoredProcedure;
                command5.Parameters.AddWithValue("@fioc", textBox1.Text);

                
 



                SqlDataReader sqlReader3 = null;
                SqlDataReader sqlReader4 = null;
                SqlDataReader sqlReader5 = null;
                try
                {
                    sqlReader3 = await command3.ExecuteReaderAsync();
                    sqlReader4 = await command4.ExecuteReaderAsync();
                    sqlReader5 = await command5.ExecuteReaderAsync();

                    while (await sqlReader5.ReadAsync())
                    {
                        StatClass.client_key = Convert.ToInt32(sqlReader5[0]);
                    }

                    while (await sqlReader3.ReadAsync())
                    {
                        StatClass.model_key = Convert.ToInt32(sqlReader3[0]);

                    }
                    while (await sqlReader4.ReadAsync())
                    {
                        StatClass.diler_key = Convert.ToInt32(sqlReader4[0]);

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            else
            {
                MessageBox.Show("Все поля должны быть заполнены!");
            }


 
            SqlCommand command2 = new SqlCommand("INSERT INTO [TradeIN] (КодМодели, КодДилера,КодКлиента, ГодВыпуска, Пробег, Комментарий, ОценочнаяСтоимость) VALUES(@c1, @c2, @c3, @year1, @probeg, @comments, @stoimost) ", sqlConnection);



            command2.Parameters.AddWithValue("@c1", StatClass.model_key);
            command2.Parameters.AddWithValue("@c2", StatClass.diler_key);
            command2.Parameters.AddWithValue("@c3", StatClass.client_key);
            command2.Parameters.AddWithValue("@year1", textBox3.Text);
            command2.Parameters.AddWithValue("@probeg", textBox4.Text);
            command2.Parameters.AddWithValue("@comments", textBox5.Text);
            command2.Parameters.AddWithValue("@stoimost", stoimost);


            try
            {
                await command2.ExecuteNonQueryAsync();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        

            this.Close();
        }
    }
}
