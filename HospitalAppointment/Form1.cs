using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.DesignerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalAppointment
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle= FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            
            
        }

        private void btnpatient_Click(object sender, EventArgs e)
        {
            patientlogin1.Visible = true;
            doctorlogin1.Visible = false;
            register1.Visible = false;
            if (btnpatient.Focused == true)
            {
                btnpatient.BackColor = Color.Gold;
                btndoctor.BackColor = Color.CadetBlue;
                btnregister.BackColor = Color.CadetBlue;
            }
        }

        private void patientlogin1_Load(object sender, EventArgs e)
        {
            
        }

        private void btndoctor_Click(object sender, EventArgs e)
        {
            patientlogin1.Visible = false;
            doctorlogin1.Visible = true;
            register1.Visible = false;
            if (btndoctor.Focused == true)
            {
                btndoctor.BackColor = Color.Gold;
                btnpatient.BackColor = Color.CadetBlue;
                btnregister.BackColor = Color.CadetBlue;
            }
        }

        private void btnregister_Click(object sender, EventArgs e)
        {
            register1.Visible = true;
            patientlogin1.Visible = false;
            doctorlogin1.Visible = false;
            if (btnregister.Focused == true)
            {
                btnregister.BackColor = Color.Gold;
                btndoctor.BackColor = Color.CadetBlue;
                btnpatient.BackColor = Color.CadetBlue;
            }
        }

        private void register1_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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
    }
}
