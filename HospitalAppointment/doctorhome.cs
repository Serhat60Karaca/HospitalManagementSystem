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
    public partial class doctorhome : Form
    {
        NpgsqlConnection connection = new NpgsqlConnection("server=localhost;port=5432;UserId=postgres;password=tokatspor;database=HospitalAppointmentDb");
        public doctorhome(int doc_id)
        {
            connection.Open();
            InitializeComponent();
            int docid = doc_id;
            prescription1.getValue = docid;
            NpgsqlCommand command = new NpgsqlCommand("select first_name || ' ' || last_name as \"username\" from users where user_id = " + docid + " limit 1", connection);
            object result = command.ExecuteScalar();
            string a = result.ToString();
            docname.Text = a;
            connection.Close();
        }
        
        private void doctorhome_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            connection.Open();
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            prescription1.Visible = true;
            if (button2.Focused == true)
            {
                button2.BackColor = Color.Gold;
                button3.BackColor = Color.CadetBlue;
            }

        }

        private void prescription1_Load(object sender, EventArgs e)
        {

        }

        private void docname_Click(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            prescription1.Visible = false;

            Form1 doc = new Form1();
            doc.Show();
            this.Hide();
        }
    }
}
