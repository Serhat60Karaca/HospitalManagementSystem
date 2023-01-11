using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalAppointment
{
    public partial class register : UserControl
    {
        public register()
        {
            InitializeComponent();
        }
        NpgsqlConnection connection = new NpgsqlConnection("server=localhost;port=5432;UserId=postgres;password=tokatspor;database=HospitalAppointmentDb");
        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            string sql = "insert into users (adress, first_name, gender, last_name, password, user_role, social_id) values(@adress, @first_name, @gender, @last_name, @password, @role, @social_id)";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@first_name", first_name.Text);
            command.Parameters.AddWithValue("@last_name", last_name.Text);
            command.Parameters.AddWithValue("@password", password.Text);
            command.Parameters.AddWithValue("@gender", gender.Text);
            command.Parameters.AddWithValue("@role", 'P');
            command.Parameters.AddWithValue("@adress", adress.Text);
            command.Parameters.AddWithValue("@social_id", social_id.Text);
            command.ExecuteNonQuery();
            string sql1 = "select user_id from users where social_id = '"+social_id.Text+"'";
            NpgsqlCommand idsearch = new NpgsqlCommand(sql1, connection);
            object result = idsearch.ExecuteScalar();
            string a = result.ToString();
            int userid = int.Parse(a);
            MessageBox.Show("your user id is " + userid);
            connection.Close();
            MessageBox.Show("Registration is successful");
            first_name.Clear();
            last_name.Clear();
            password.Clear();
            adress.Clear();
            social_id.Clear();
        }

        private void register_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
