using System;
using Npgsql;
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
    public partial class patientlogin : UserControl
    {
        public patientlogin()
        {
            InitializeComponent();
        }
        NpgsqlConnection connection = new NpgsqlConnection("server=localhost;port=5432;UserId=postgres;password=tokatspor;database=HospitalAppointmentDb");
        

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            int user_id;
            string user_password;
            user_id = int.Parse(username.Text);
            user_password = password.Text;

            try
            {
                string querry = "select * from users where user_id = '" + int.Parse(username.Text) + "' and password = '" + password.Text + "'";
                NpgsqlDataAdapter adapt = new NpgsqlDataAdapter(querry, connection);
                DataTable dtable = new DataTable();
                adapt.Fill(dtable);

                if (dtable.Rows.Count == 1)
                {
                    user_id = int.Parse(username.Text);
                    user_password = password.Text;
                    MessageBox.Show("Successful");
                    patienthome ho = new patienthome(user_id);
                    ho.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("invalid userid and password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    username.Clear();
                    password.Clear();

                    username.Focus();

                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
            finally
            {
                connection.Close();
            }
        }
        /*private int data;
        public int user_id
        {

            get { return data; }
            set { data = int.Parse(username.Text); }
        }*/
        private void patientlogin_Load(object sender, EventArgs e)
        {

        }
    }
}
