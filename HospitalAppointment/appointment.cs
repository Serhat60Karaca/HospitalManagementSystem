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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace HospitalAppointment
{
    public partial class appointment : UserControl
    {
        public int getid { get; set; }
        public appointment()
        {
            InitializeComponent();
            
            countyid.SelectedIndexChanged += countyid_SelectedIndexChanged;
            hospital.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            policlinic.SelectedIndexChanged += policlinic_SelectedIndexChanged;

        }
        NpgsqlConnection connection = new NpgsqlConnection("server=localhost;port=5432;UserId=postgres;password=tokatspor;database=HospitalAppointmentDb");
        private void button1_Click(object sender, EventArgs e)
        {
        connection.Open();
        string sql = "update available_appointments set patient_id = '"+ getid +"', is_available = 'false' where is_available = true and appointment_id = '"+ int.Parse(textBox1.Text) +"'";
        NpgsqlCommand command = new NpgsqlCommand(sql, connection);
        command.ExecuteNonQuery();
        connection.Close();
        MessageBox.Show("Appointment created");
        }

        private void appointment_Load(object sender, EventArgs e)
        {
            connection.Open();
            NpgsqlCommand cmdd = new NpgsqlCommand("select * from counties", connection);
            NpgsqlDataAdapter daa = new NpgsqlDataAdapter(cmdd);
            DataSet dss = new DataSet();
            daa.Fill(dss);
            cmdd.ExecuteNonQuery();
            connection.Close();
            countyid.DataSource = dss.Tables[0];
            countyid.DisplayMember = "name";
            countyid.ValueMember = "county_id";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            panel1.Visible = true;
            int hospital;
            if (int.TryParse(this.hospital.SelectedValue.ToString(), out hospital))
            {

            }
            else
            {

            }
            int doc;
            if (Int32.TryParse(comboBox3.SelectedValue.ToString(), out doc))
            {

            }
            else
            {

            }
            int pol;
            if (Int32.TryParse(policlinic.SelectedValue.ToString(), out pol))
            {

            }
            else
            {

            }
            //int gettext = int.Parse(comboBox3.Text);
            string querry = "select appointment_id as \"appointment no\", appointment_time as \"appointment time\", doctors.first_name || ' ' || doctors.last_name as \"doctor name\", hospitals.hospital_name as \"hospital\" from available_appointments inner join \"doctors\" on \"available_appointments\".\"doctor_id\" = \"doctors\".\"doctor_id\" inner join \"hospitals\" on \"doctors\".\"hospital_id\" = \"hospitals\".\"hospital_id\" inner join \"policlinics\" on \"hospitals\".\"hospital_id\" = \"policlinics\".\"hospital_id\" where \"available_appointments\".\"appointment_date\" = '" + dateTimePicker1.Value + "' and \"doctors\".\"doctor_id\" = '" + doc + "' and \"doctors\".\"hospital_id\"='"+hospital+"'";
            NpgsqlDataAdapter adapt = new NpgsqlDataAdapter(querry, connection);
            DataSet ds = new DataSet();
            adapt.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int das;
            if (Int32.TryParse(hospital.SelectedValue.ToString(), out das))
            {

            }
            else
            {

            }
            connection.Open();
            NpgsqlCommand cmddd = new NpgsqlCommand("select * from policlinics inner join \"hospitals\" on \"policlinics\".\"hospital_id\" = \"hospitals\".\"hospital_id\" where \"hospitals\".\"hospital_id\" = '" + das + "'", connection);
            NpgsqlDataAdapter daaa = new NpgsqlDataAdapter(cmddd);
            DataSet dsss = new DataSet();
            daaa.Fill(dsss);
            cmddd.ExecuteNonQuery();
            connection.Close();
            policlinic.DataSource = dsss.Tables[0];
            policlinic.DisplayMember = "policlinic_name";
            policlinic.ValueMember = "policlinic_id";
        }

        private void username_TextChanged(object sender, EventArgs e)
        {

        }

        private void countyid_SelectedIndexChanged(object sender, EventArgs e)
        {
            int das;
            if (Int32.TryParse(countyid.SelectedValue.ToString(), out das))
            {

            }
            else
            {

            }
            connection.Open();
            NpgsqlCommand cmddd = new NpgsqlCommand("select * from hospitals inner join \"counties\" on \"hospitals\".\"county_id\" = \"counties\".\"county_id\" where \"counties\".\"county_id\" = '" + das + "'", connection);
            NpgsqlDataAdapter daaa = new NpgsqlDataAdapter(cmddd);
            DataSet dsss = new DataSet();
            daaa.Fill(dsss);
            cmddd.ExecuteNonQuery();
            connection.Close();
            hospital.DataSource = dsss.Tables[0];
            hospital.DisplayMember = "hospital_name";
            hospital.ValueMember = "hospital_id";
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void policlinic_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void policlinic_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int das;
            if (Int32.TryParse(policlinic.SelectedValue.ToString(), out das))
            {

            }
            else
            {

            }
            connection.Open();
            NpgsqlCommand cmddd = new NpgsqlCommand("select * from doctors inner join \"policlinics\" on \"policlinics\".\"hospital_id\" = \"doctors\".\"hospital_id\" where \"policlinics\".\"policlinic_id\" = '" + das + "'", connection);
            NpgsqlDataAdapter daaa = new NpgsqlDataAdapter(cmddd);
            DataSet dsss = new DataSet();
            daaa.Fill(dsss);
            cmddd.ExecuteNonQuery();
            connection.Close();
            comboBox3.DataSource = dsss.Tables[0];
            comboBox3.DisplayMember = "first_name";
            comboBox3.ValueMember = "doctor_id";
        }
    }
}
