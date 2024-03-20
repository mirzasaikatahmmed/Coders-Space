using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Coders_Space
{
    public partial class ResetPassword : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        string emailToReset;
        public ResetPassword(string emailToReset)
        {
            InitializeComponent();
            this.emailToReset = emailToReset;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.ShowDialog();
            this.Close();
        }

        private void changepasswordBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string newPassword = textBoxNewPassword.Text;
                string confirmPassword = textBoxConfirmNewPassword.Text;

                if (IsEmailExist(this.emailToReset))
                {
                    if (newPassword == confirmPassword)
                    {
                        UpdatePassword(emailToReset, newPassword);

                        MessageBox.Show("Password reset successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Login login = new Login();
                        login.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("New password and confirm password do not match. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Email not found. Please enter a valid email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdatePassword(string email, string newPassword)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "UPDATE USERS SET PASSWORD = @NewPassword WHERE EMAIL = @Email";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@NewPassword", newPassword);
                cmd.Parameters.AddWithValue("@Email", email);

                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        private bool IsEmailExist(string email)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT COUNT(*) FROM USERS WHERE EMAIL = @Email";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);

                con.Open();

                int count = (int)cmd.ExecuteScalar();

                con.Close();

                return count > 0;
            }
        }

        private void minimizeBTN_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
