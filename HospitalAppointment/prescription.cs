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
    public partial class prescription : UserControl
    {
        public int getValue { get; set; }
        public prescription()
        {
            InitializeComponent();
        }
        NpgsqlConnection connection = new NpgsqlConnection("server=localhost;port=5432;UserId=postgres;password=tokatspor;database=HospitalAppointmentDb");
        private void prescription_Load(object sender, EventArgs e)
        {
            connection.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("select * from patients", connection);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteNonQuery();
            connection.Close();
            comboBox2.DataSource = ds.Tables[0];
            comboBox2.DisplayMember = "patient_id";
            comboBox2.ValueMember = "patient_id";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            string sql = "insert into prescriptions (patient_id,doctor_id,medicines, dosage, frequency,duration) values(@patient_id,@doctor_id,@medicinetxt, @dosagetxt, @frequencytxt, @durationtxt)";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            int gettext = int.Parse(comboBox2.Text);
            NpgsqlCommand doctorsearch = new NpgsqlCommand("select doctor_id from doctors where user_id = '"+ getValue + "'", connection);
            object result = doctorsearch.ExecuteScalar();
            string a = result.ToString();
            int doctorid = int.Parse(a);
            command.Parameters.AddWithValue("@patient_id", gettext);
            command.Parameters.AddWithValue("@doctor_id", doctorid);
            command.Parameters.AddWithValue("@medicinetxt", medicinetxt.Text);
            command.Parameters.AddWithValue("@dosagetxt", dosagetxt.Text);
            command.Parameters.AddWithValue("@frequencytxt", frequencytxt.Text);
            command.Parameters.AddWithValue("@durationtxt", int.Parse(durationtxt.Text));
            command.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Prescription created");
            button3_Click(sender, EventArgs.Empty);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            connection.Open();
            NpgsqlCommand command2 = new NpgsqlCommand("update prescriptions set \"patient_id\"=@patient_id,\"medicines\"=@medicines, \"dosage\"=@dosage,\"frequency\"=@frequency where \"prescripton_id\"=@prescripton_id ", connection);
            
            int patient = int.Parse(comboBox2.Text);
            command2.Parameters.AddWithValue("@prescripton_id",int.Parse (textBox1.Text));
            command2.Parameters.AddWithValue("@patient_id", patient);
            command2.Parameters.AddWithValue("@medicines",medicinetxt.Text);
            command2.Parameters.AddWithValue("@dosage", dosagetxt.Text);
            command2.Parameters.AddWithValue("@frequency", frequencytxt.Text);
            
            command2.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("prescription updated successfully!");
            button3_Click(sender, EventArgs.Empty);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            connection.Open();
            NpgsqlCommand cmdd = new NpgsqlCommand("select * from prescriptions", connection);
            NpgsqlDataAdapter daa = new NpgsqlDataAdapter(cmdd);
            DataSet dss = new DataSet();
            daa.Fill(dss);
            cmdd.ExecuteNonQuery();
            dataGridView1.DataSource = dss.Tables[0];
            connection.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
