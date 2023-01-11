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
    public partial class doctorlogin : UserControl
    {
        public doctorlogin()
        {
            InitializeComponent();
        }
        NpgsqlConnection connection = new NpgsqlConnection("server=localhost;port=5432;UserId=postgres;password=tokatspor;database=HospitalAppointmentDb");
        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            int user_id;
            string user_password;
            user_id = int.Parse(docusername.Text);
            user_password = docpassword.Text;

            try
            {
                string querry = "select * from users where user_role = 'D' and user_id = '" + int.Parse(docusername.Text) + "' and password = '" + docpassword.Text + "'";
                NpgsqlDataAdapter adapt = new NpgsqlDataAdapter(querry, connection);
                DataTable dtable = new DataTable();
                adapt.Fill(dtable);

                if (dtable.Rows.Count == 1)
                {
                    user_id = int.Parse(docusername.Text);
                    user_password = docpassword.Text;
                    MessageBox.Show("Successful");
                    doctorhome ho = new doctorhome(user_id);
                    prescription send = new prescription();
                    send.getValue = user_id;
                    this.Hide();
                    ho.Show();
                    
                }
                else
                {
                    MessageBox.Show("invalid userid and password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    docusername.Clear();
                    docpassword.Clear();

                    docusername.Focus();

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

        private void doctorlogin_Load(object sender, EventArgs e)
        {

        }
    }
}
