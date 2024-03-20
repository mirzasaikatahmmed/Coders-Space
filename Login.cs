using Coders_Space.Admin;
using Coders_Space.PUControl;
using Coders_Space.RUControl;
using System.Configuration;
using System.Data.SqlClient;

namespace Coders_Space
{
    public partial class Login : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void registrationBtn_Click(object sender, EventArgs e)
        {
            Registration registration = new Registration();
            registration.ShowDialog();
            this.Close();
        }

        private void forgotPassLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgotPassword forgotPassword = new ForgotPassword();
            forgotPassword.ShowDialog();
            this.Close();
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            if (textBoxEmail.Text != "" && textBoxPassword.Text != "")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = "SELECT * FROM USERS WHERE EMAIL = @email AND PASSWORD = @password";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@email", textBoxEmail.Text);
                        cmd.Parameters.AddWithValue("@password", textBoxPassword.Text);

                        con.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    string Role = dr["ROLE"]?.ToString() ?? "";
                                    string email = textBoxEmail.Text;

                                    if (Role == "ADMIN")
                                    {
                                        AdminMenu adminMenu = new AdminMenu(email);
                                        adminMenu.ShowDialog();
                                        this.Close();
                                    }
                                    if (Role == "REGULAR_USER")
                                    {
                                        RUMenu rUMenu = new RUMenu(email);
                                        rUMenu.ShowDialog();
                                        this.Close();
                                    }
                                    if (Role == "PREMIUM_USER")
                                    {
                                        PUMenu pUMenu = new PUMenu(email);
                                        pUMenu.ShowDialog();
                                        this.Close();
                                    }

                                }
                            }
                            else
                            {
                                MessageBox.Show("No user found with the given credentials", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Enter Fields", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void minimizeBTN_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}