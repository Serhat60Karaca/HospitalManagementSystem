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
    public partial class patienthome : Form
    {
        NpgsqlConnection connection = new NpgsqlConnection("server=localhost;port=5432;UserId=postgres;password=tokatspor;database=HospitalAppointmentDb");
        public patienthome(int user_id)
        {
            connection.Open();
            InitializeComponent();
            int patientid = user_id;
            myappointments1.getnewValue = user_id;
            appointment1.getid = patientid;
            NpgsqlCommand command = new NpgsqlCommand("select first_name || ' ' || last_name as \"username\" from users where user_id = " + patientid + " limit 1", connection);
            object result = command.ExecuteScalar();
            string a = result.ToString();
            patientname.Text = a;
            connection.Close();
        }


        private void patienthome_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            appointment1.Visible = true;
            myappointments1.Visible = false;
            if (button1.Focused == true)
            {
                button1.BackColor = Color.Gold;
                button2.BackColor = Color.CadetBlue;
                button3.BackColor = Color.CadetBlue;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            myappointments1.Visible = true;
            appointment1.Visible = false;
            if (button2.Focused == true)
            {
                button2.BackColor = Color.Gold;
                button1.BackColor = Color.CadetBlue;
                button3.BackColor = Color.CadetBlue;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult sure;
            sure = MessageBox.Show("Do you want to exit", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sure == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                this.Show();
            }
        }

        private void myappointments1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void patientname_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            myappointments1.Visible = false;
            appointment1.Visible = false;
            Form1 ptnt = new Form1();
            ptnt.Show();
            this.Hide();
        }
    }
}
