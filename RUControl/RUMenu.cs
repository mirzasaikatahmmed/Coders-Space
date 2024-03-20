using Coders_Space.PUControl;
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

namespace Coders_Space.RUControl
{
    public partial class RUMenu : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        private string userEmail;
        private Guna2Button selectedButton;
        public RUMenu(string email)
        {
            InitializeComponent();
            userEmail = email;

            LoadUserData(userEmail);
            loadform(new RUDashboard());

            if (selectedButton != null && selectedButton != RUDashboardBTN)
            {
                ClearButtonColor(selectedButton);
            }
            SetButtonColor(RUDashboardBTN, Color.White, Color.FromArgb(0, 64, 221));
            selectedButton = RUDashboardBTN;

            exitBTN.Click += exitBTN_Click;
            minimizeBTN.Click += minimizeBTN_Click;
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

        private void RUDashboardBTN_Click(object sender, EventArgs e)
        {
            loadform(new RUDashboard());

            if (selectedButton != null && selectedButton != RUDashboardBTN)
            {
                ClearButtonColor(selectedButton);
            }
            SetButtonColor(RUDashboardBTN, Color.White, Color.FromArgb(0, 64, 221));
            selectedButton = RUDashboardBTN;
        }

        private void RUCourseBTN_Click(object sender, EventArgs e)
        {
            loadform(new RUCourse());

            if (selectedButton != null && selectedButton != RUCourseBTN)
            {
                ClearButtonColor(selectedButton);
            }
            SetButtonColor(RUCourseBTN, Color.White, Color.FromArgb(0, 64, 221));
            selectedButton = RUCourseBTN;
        }

        private void RUDriveBTN_Click(object sender, EventArgs e)
        {
            loadform(new CodeClouds());

            if (selectedButton != null && selectedButton != RUDriveBTN)
            {
                ClearButtonColor(selectedButton);
            }
            SetButtonColor(RUDriveBTN, Color.White, Color.FromArgb(0, 64, 221));
            selectedButton = RUDriveBTN;
        }

        private void RUCompilerBTN_Click(object sender, EventArgs e)
        {
            loadform(new CodeCompiler());

            if (selectedButton != null && selectedButton != RUCompilerBTN)
            {
                ClearButtonColor(selectedButton);
            }
            SetButtonColor(RUCompilerBTN, Color.White, Color.FromArgb(0, 64, 221));
            selectedButton = RUCompilerBTN;
        }

        private void RUJobBTN_Click(object sender, EventArgs e)
        {
            loadform(new RUJobs());

            if (selectedButton != null && selectedButton != RUJobBTN)
            {
                ClearButtonColor(selectedButton);
            }
            SetButtonColor(RUJobBTN, Color.White, Color.FromArgb(0, 64, 221));
            selectedButton = RUJobBTN;
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

        private void developerInfoBTN_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);

            try
            {
                con.Open();
                string query = "SELECT * FROM DevelopersInfo";
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
                    labelName.TextAlignment = ContentAlignment.MiddleRight;

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

        private void exitBTN_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void minimizeBTN_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void subscriptionBTN_Click(object sender, EventArgs e)
        {
            Subscription subscription = new Subscription(userEmail);
            subscription.Show();
        }
    }
}
