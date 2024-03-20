using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coders_Space
{
    public partial class Subscription : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        string userEmail;
        decimal amount;
        public Subscription(string email)
        {
            InitializeComponent();
            userEmail = email;
            checkUserType(userEmail);
            guna2ToggleSwitch1.CheckedChanged += guna2ToggleSwitch1_CheckedChanged;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            UpdateUserRoleToRegular();
            MessageBox.Show("Downgrade complete!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Hide();
            Login login = new Login();
            login.ShowDialog();
            this.Close();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Payment payment = new Payment(userEmail, amount);
            payment.ShowDialog();
            this.Close();
        }

        private void checkUserType(string email)
        {
            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();

                string query = "SELECT Role FROM Users WHERE Email = @Email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                object role = command.ExecuteScalar();

                if (role != null)
                {
                    string userRole = role.ToString();
                    bool isRegularUser = string.Equals(userRole, "REGULAR_USER", StringComparison.OrdinalIgnoreCase);

                    guna2Button2.Enabled = !isRegularUser;
                    guna2Button3.Enabled = !(userRole.Equals("PREMIUM_USER", StringComparison.OrdinalIgnoreCase) || userRole.Equals("ADMIN", StringComparison.OrdinalIgnoreCase));
                }
            }
        }


        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch1.Checked)
            {
                labelPremiumPrice.Text = "5000BDT";
                amount = 5000;
            }
            else
            {
                labelPremiumPrice.Text = "500BDT";
                amount = 5000;
            }
        }

        private void UpdateUserRoleToRegular()
        {
            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();

                string query = "UPDATE USERS SET Role = 'REGULAR_USER', UPDATE_DATE = GETDATE() WHERE Email = @Email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", userEmail);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("User role downgrade to regular_user.");
                }
                else
                {
                    Console.WriteLine("Failed to downgrade user role.");
                }
            }
        }

        private void exitBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
