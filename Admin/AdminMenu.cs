using Guna.UI2.WinForms;
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
using System.Net;
using System.Net.Mail;
using System.Reflection.Emit;

namespace Coders_Space.Admin
{
    public partial class AdminMenu : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        private string userEmail;
        private Guna2Button selectedButton;


        public AdminMenu(string email)
        {
            InitializeComponent();
            userEmail = email;
            LoadUserData(userEmail);
            loadform(new AdminDashboard());

            if (selectedButton != null && selectedButton != AdminDashboardBTN)
            {
                ClearButtonColor(selectedButton);
            }
            SetButtonColor(AdminDashboardBTN, Color.White, Color.FromArgb(0, 64, 221));
            selectedButton = AdminDashboardBTN;
        }

        public void loadform(object Form)
        {
            if (this.ControlPanel.Controls.Count > 0)
            {
                this.ControlPanel.Controls.RemoveAt(0);
            }
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.ControlPanel.Controls.Add(f);
            this.ControlPanel.Tag = f;
            f.Show();
        }

        private void SetButtonColor(Guna2Button button, Color backgroundColor, Color textColor)
        {
            button.FillColor = backgroundColor;
            button.ForeColor = textColor;
        }

        private void ClearButtonColor(Guna2Button button)
        {
            button.FillColor = Color.FromArgb(0, 64, 221);
            button.ForeColor = Color.White;
        }

        private void AdminDashboardBTN_Click(object sender, EventArgs e)
        {
            loadform(new AdminDashboard());

            if (selectedButton != null && selectedButton != AdminDashboardBTN)
            {
                ClearButtonColor(selectedButton);
            }
            SetButtonColor(AdminDashboardBTN, Color.White, Color.FromArgb(0, 64, 221));
            selectedButton = AdminDashboardBTN;
        }

        private void adminUserBTN_Click(object sender, EventArgs e)
        {
            loadform(new AdminUser());

            if (selectedButton != null && selectedButton != adminUserBTN)
            {
                ClearButtonColor(selectedButton);
            }
            SetButtonColor(adminUserBTN, Color.White, Color.FromArgb(0, 64, 221));
            selectedButton = adminUserBTN;

        }

        private void adminCourseBTN_Click(object sender, EventArgs e)
        {
            loadform(new AdminCourse());

            if (selectedButton != null && selectedButton != adminCourseBTN)
            {
                ClearButtonColor(selectedButton);
            }
            SetButtonColor(adminCourseBTN, Color.White, Color.FromArgb(0, 64, 221));
            selectedButton = adminCourseBTN;
        }

        private void adminJobBTN_Click(object sender, EventArgs e)
        {
            loadform(new AdminJobs());

            if (selectedButton != null && selectedButton != adminJobBTN)
            {
                ClearButtonColor(selectedButton);
            }
            SetButtonColor(adminJobBTN, Color.White, Color.FromArgb(0, 64, 221));
            selectedButton = adminJobBTN;
        }

        private void adminTransactionBTN_Click(object sender, EventArgs e)
        {
            loadform(new Transaction());

            if (selectedButton != null && selectedButton != adminTransactionBTN)
            {
                ClearButtonColor(selectedButton);
            }
            SetButtonColor(adminTransactionBTN, Color.White, Color.FromArgb(0, 64, 221));
            selectedButton = adminTransactionBTN;
        }

        private void profileBTN_Click(object sender, EventArgs e)
        {
            loadform(new Profile(userEmail));

            if (selectedButton != null && selectedButton != profileBTN)
            {
                ClearButtonColor(selectedButton);
            }
            SetButtonColor(profileBTN, Color.White, Color.FromArgb(0, 64, 221));
            selectedButton = profileBTN;
        }

        private void logOutBTN_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.ShowDialog();
            this.Close();
        }


        private void LoadUserData(string email)
        {
            SqlConnection con = new SqlConnection(cs);

            try
            {
                con.Open();
                string query = "SELECT PHOTO, NAME FROM USERS WHERE email = @email";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@email", email);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string name = reader["Name"].ToString();
                    labelName.Text = name;
                    labelName.AutoSize = false;
                    labelName.TextAlign = ContentAlignment.MiddleRight;

                    if (reader["PHOTO"] != DBNull.Value)
                    {
                        byte[] imageData = (byte[])reader["PHOTO"];
                        MemoryStream ms = new MemoryStream(imageData);
                        titleProfilePicture.Image = Image.FromStream(ms);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void titleProfilePicture_Click(object sender, EventArgs e)
        {
            loadform(new Profile(userEmail));

            if (selectedButton != null && selectedButton != profileBTN)
            {
                ClearButtonColor(selectedButton);
            }
            SetButtonColor(profileBTN, Color.White, Color.FromArgb(0, 64, 221));
            selectedButton = profileBTN;
        }

        private void developerInfoBTN_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);

            try
            {
                con.Open();
                string query = "SELECT ROLL, ID, NAME, CONTRIBUTION FROM DevelopersInfo";
                SqlCommand cmd = new SqlCommand(query, con);

                SqlDataReader reader = cmd.ExecuteReader();

                StringBuilder developerInfo = new StringBuilder();

                while (reader.Read())
                {
                    int roll = (int)reader["Roll"];
                    string id = reader["Id"].ToString();
                    string name = reader["Name"].ToString();
                    string contribution = reader["Contribution"].ToString();

                    developerInfo.AppendLine($"Roll: {roll}");
                    developerInfo.AppendLine($"ID: {id}");
                    developerInfo.AppendLine($"Name: {name}");
                    developerInfo.AppendLine($"Contribution: {contribution}");
                    developerInfo.AppendLine();
                }

                reader.Close();
                MessageBox.Show(developerInfo.ToString(), "Developers Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void supportBTN_Click(object sender, EventArgs e)
        {
            using (Support supportForm = new Support(userEmail))
            {
                supportForm.ShowDialog();
            }
        }

        private void exitBTN_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void minimizeBTN_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
