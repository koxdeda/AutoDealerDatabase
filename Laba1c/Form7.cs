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
    public partial class Form7 : Form
    {
        SqlConnection sqlConnection;
        string ConnectionString = @"Data Source=(local)\SQLEXPRESS;Initial Catalog=Автосалон;Integrated Security=True; MultipleActiveResultSets=True";

        public Form7()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConnectionString);
            await sqlConnection.OpenAsync();
            SqlDataReader sqlReader = null;
            int tmp;

            SqlCommand command = new SqlCommand("INSERT into [Клиент] (ФИО, Телефон) VALUES (@fio, @phone)", sqlConnection);
            command.Parameters.AddWithValue("@fio", textBox1.Text);
            command.Parameters.AddWithValue("@phone", textBox2.Text);


            SqlCommand command3 = new SqlCommand("ПРКодКлиента", sqlConnection);
            command3.CommandType = CommandType.StoredProcedure;
            command3.Parameters.AddWithValue("@fioc", textBox1.Text);


            try
            {
                await command.ExecuteNonQueryAsync();
                sqlReader = await command3.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    StatClass.client_key= Convert.ToInt32(sqlReader[0]);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            SqlCommand command2 = new SqlCommand("INSERT into [ЗаявкаНаКредит] (КодПродажи, КодКлиента) VALUES (@cod1, @cod2)", sqlConnection);
            command2.Parameters.AddWithValue("@cod1", StatClass.client_key);
            command2.Parameters.AddWithValue("@cod2", StatClass.client_key);

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
