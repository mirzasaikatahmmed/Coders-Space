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
    public partial class Profile : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        string userEmail;
        public Profile(string email)
        {
            InitializeComponent();
            userEmail = email;
            LoadUserData(userEmail);
        }
        private string GetSubscriptionDetails(string userId)
        {
            string subscriptionDetails = "Unlimited (-1)";
            SqlConnection con = new SqlConnection(cs);

            try
            {
                con.Open();
                string query = "SELECT TOP 1 * FROM Transactions WHERE USERID = @userId ORDER BY TRANSACTION_DATE DESC";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@userId", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int subValidMonth = (int)reader["SUB_VALID_MONTH"];
                    if (subValidMonth != -1)
                    {
                        DateTime transactionDate = (DateTime)reader["TRANSACTION_DATE"];
                        DateTime expiryDate = transactionDate.AddMonths(subValidMonth);
                        int daysLeft = (int)(expiryDate - DateTime.Now).TotalDays;

                        if (daysLeft >= 0)
                        {
                            subscriptionDetails = $"{daysLeft} days left";
                        }
                        else
                        {
                            subscriptionDetails = "Expired";
                        }
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching subscription details: " + ex.Message);
            }
            finally
            {
                con.Close();
            }

            return subscriptionDetails;
        }

        private void LoadUserData(string email)
        {
            SqlConnection con = new SqlConnection(cs);

            try
            {
                con.Open();
                string query = "SELECT * FROM USERS WHERE email = @email";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@email", email);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    if (reader["PHOTO"] != DBNull.Value)
                    {
                        byte[] imageData = (byte[])reader["PHOTO"];
                        MemoryStream ms = new MemoryStream(imageData);
                        pictureBoxUser.Image = Image.FromStream(ms);
                    }

                    labelName.Text = reader["NAME"].ToString();
                    labelName.TextAlignment = ContentAlignment.MiddleCenter;
                    labelContactNo.Text = reader["CONTACT_NO"].ToString();
                    labelEmail.Text = reader["EMAIL"].ToString();
                    labelDOB.Text = ((DateTime)reader["DOB"]).ToShortDateString();
                    labelGender.Text = reader["GENDER"].ToString();
                    labelUserType.Text = reader["ROLE"].ToString();

                    string userId = reader["ID"].ToString();
                    string subscriptionDetails = GetSubscriptionDetails(userId);
                    labelSub.Text = subscriptionDetails;
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


        private void editProfileBtn_Click(object sender, EventArgs e)
        {
            using (EditProfile editProfile = new EditProfile(userEmail))
            {
                editProfile.ShowDialog();
            }
        }

        private void changePasswordBtn_Click(object sender, EventArgs e)
        {
            using (ChangePassword  changePassword = new ChangePassword(userEmail))
            {
                changePassword.ShowDialog();
            }
        }
    }
}
