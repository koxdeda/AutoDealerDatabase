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
    public partial class Form6 : Form
    {
        SqlConnection sqlConnection;
        string ConnectionString = @"Data Source=(local)\SQLEXPRESS;Initial Catalog=Автосалон;Integrated Security=True; MultipleActiveResultSets=True";

        public Form6()
        {
            InitializeComponent();
        }

        private async void Form6_Load(object sender, EventArgs e)
        {
            textBox11.Text = StatClass.car_name;
            textBox13.Text = StatClass.car_typeTS;
            textBox14.Text = StatClass.car_VIN;
            textBox15.Text = StatClass.car_year;
            textBox16.Text = Convert.ToString(StatClass.car_price);


            sqlConnection = new SqlConnection(ConnectionString);
            await sqlConnection.OpenAsync();

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command3 = new SqlCommand("ПРКодКлиента", sqlConnection);
            command3.CommandType = CommandType.StoredProcedure;
            command3.Parameters.AddWithValue("@fioc", textBox10.Text);

            SqlDataReader sqlReader = null;
            try
            {
                sqlReader = await command3.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    StatClass.client_key = Convert.ToInt32(sqlReader[0]);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            SqlCommand command = new SqlCommand("INSERT INTO [Договор] (КодКлиента, ДатаДоговора) VALUES (@clcod, @date)", sqlConnection);
            command.Parameters.AddWithValue("@clcod", StatClass.client_key);
            command.Parameters.AddWithValue("@date",dateTimePicker1.Value );

            try
            {
                await command.ExecuteNonQueryAsync();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Close();
        }
    }
}
