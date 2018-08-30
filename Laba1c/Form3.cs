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

    public partial class Form3 : Form
    {
        SqlConnection sqlConnection;
        string ConnectionString = @"Data Source=(local)\SQLEXPRESS;Initial Catalog=Автосалон;Integrated Security=True; MultipleActiveResultSets=True";
        public Form3()
        {
            InitializeComponent();
        }

        private async void Form3_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConnectionString);
            await sqlConnection.OpenAsync();

            comboBox1.Items.Clear();

            SqlCommand command = new SqlCommand("ДляПросмотраМодели", sqlConnection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@modelkey", Convert.ToString(StatClass.model_key));

            SqlDataReader sqlReader = null;

            try
            {
                sqlReader = await command.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    comboBox1.Items.Add(Convert.ToString(sqlReader["Название"]) + "   " + Convert.ToString(sqlReader["Адрес"]) + "   ");
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

    

        private async void button1_Click(object sender, EventArgs e)
        {
            int clients_count = 1;
            if (label5.Visible)
                label5.Visible = false;

            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) &&
            !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                SqlCommand command = new SqlCommand("INSERT INTO [Клиент] (КодКлиента, ФИО, Телефон) VALUES(@cod,@fio, @phone) ", sqlConnection);

                command.Parameters.AddWithValue("@cod", clients_count);
                command.Parameters.AddWithValue("@fio", textBox1.Text);
                command.Parameters.AddWithValue("@phone", textBox2.Text);

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
                label5.Visible = true;
                label5.Text = "Все поля должны быть заполнены";
            }
            


            this.Close();
        }
    }
}
