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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HospitalAppointment
{
    public partial class myappointments : UserControl
    {
        public int getnewValue{ get; set; }
        public myappointments()
        {
            InitializeComponent();
        }
        NpgsqlConnection connection = new NpgsqlConnection("server=localhost;port=5432;UserId=postgres;password=tokatspor;database=HospitalAppointmentDb");
        private void myappointments_Load(object sender, EventArgs e)
        {
            connection.Open();
            
            NpgsqlCommand cmdd = new NpgsqlCommand("select * from hospitals", connection);
            NpgsqlCommand cmddd = new NpgsqlCommand("select * from doctors", connection);
            NpgsqlDataAdapter daa = new NpgsqlDataAdapter(cmdd);
            NpgsqlDataAdapter daaa = new NpgsqlDataAdapter(cmddd);
            
            DataSet dss = new DataSet();
            DataSet dsss = new DataSet();
            daa.Fill(dss);
            daaa.Fill(dsss);
            cmdd.ExecuteNonQuery();
            cmddd.ExecuteNonQuery();
            connection.Close();
            comboBox1.DataSource = dss.Tables[0];
            comboBox1.DisplayMember = "hospital_id";
            comboBox1.ValueMember = "hospital_id";
            comboBox3.DataSource = dsss.Tables[0];
            comboBox3.DisplayMember = "doctor_id";
            comboBox3.ValueMember = "doctor_id";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            panel1.Visible = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            NpgsqlCommand command2 = new NpgsqlCommand("update available_appointments set \"appointment_date\"=@appointment_date,\"doctor_id\"=@doctor_id, \"patient_id\"=@patient_id, \"hospital_id\"=@hospital_id where \"appointment_id\"=@appointmentid", connection);
            command2.Parameters.AddWithValue("@appointmentid", int.Parse(textBox3.Text));
            command2.Parameters.AddWithValue("@appointment_date", dateTimePicker1.Value);
            int laa = int.Parse(comboBox3.Text);
            int gettext = int.Parse(comboBox1.Text);
            command2.Parameters.AddWithValue("@doctor_id", laa);
            command2.Parameters.AddWithValue("@patient_id",getnewValue);
            command2.Parameters.AddWithValue("@hospital_id", gettext);
            command2.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("appointment updated successfully!");
            panel1.Visible = false;
            button4_Click(sender, EventArgs.Empty);
            button3.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            connection.Open();
            NpgsqlCommand command3 = new NpgsqlCommand("update unavailable_appointments set is_available = 'true' where appointment_id = '"+ int.Parse(textBox1.Text) + "'", connection);
            command3.Parameters.AddWithValue("@appointmentid", int.Parse(textBox1.Text));
            command3.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("appointment cancelled successfully!");
            button4_Click(sender, EventArgs.Empty);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //basarısız loginden id alma girisimi
            
            dataGridView2.Visible = true;
            string querry = "select book_date as \"book date\", appointment_date as \"appointment date\", appointment_id as \"appointment no\", start_time as \"appoitment hour\", doctors.first_name || ' ' || doctors.last_name as \"doctor name\", hospitals.\"hospital_name\" as \"hospital name\" from unavailable_appointments inner join doctors on unavailable_appointments.\"doctor_id\" = doctors.\"doctor_id\" inner join \"hospitals\" on doctors.\"hospital_id\" = hospitals.\"hospital_id\" where unavailable_appointments.\"patient_id\" = '" + getnewValue + "'";
            NpgsqlDataAdapter adapt = new NpgsqlDataAdapter(querry, connection);
            DataSet ds = new DataSet();
            adapt.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
