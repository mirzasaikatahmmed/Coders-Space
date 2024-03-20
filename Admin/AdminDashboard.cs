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
using Guna.UI2.WinForms;
using Guna.Charts.WinForms;



namespace Coders_Space
{
    public partial class AdminDashboard : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public AdminDashboard()
        {
            InitializeComponent();
            aDashboard();
        }

        private void aDashboard()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    connection.Open();

                    SqlCommand totalUsersCommand = new SqlCommand("SELECT COUNT(*) FROM Users", connection);
                    int totalUsers = (int)totalUsersCommand.ExecuteScalar();
                    labelTotalUsers.Text = totalUsers.ToString();

                    SqlCommand premiumUsersCommand = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Role = 'premium_user'", connection);
                    int premiumUsers = (int)premiumUsersCommand.ExecuteScalar();
                    labelTotalPUsers.Text = premiumUsers.ToString();

                    SqlCommand regularUsersCommand = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Role = 'regular_user'", connection);
                    int regularUsers = (int)regularUsersCommand.ExecuteScalar();
                    labelTotalRUsers.Text = regularUsers.ToString();

                    SqlCommand totalCoursesCommand = new SqlCommand("SELECT COUNT(*) FROM COURSES", connection);
                    int totalCourses = (int)totalCoursesCommand.ExecuteScalar();
                    labeltotalCourses.Text = totalCourses.ToString();

                    SqlCommand totalJobsCommand = new SqlCommand("SELECT COUNT(*) FROM JOBS", connection);
                    int totalJobs = (int)totalCoursesCommand.ExecuteScalar();
                    labelTotalJobs.Text = totalJobs.ToString();

                    SqlCommand totalRevenueCommand = new SqlCommand("SELECT SUM(Amount) FROM TRANSACTIONS", connection);
                    object totalRevenueResult = totalRevenueCommand.ExecuteScalar();

                    if (totalRevenueResult != DBNull.Value)
                    {
                        decimal totalRevenue = Convert.ToDecimal(totalRevenueResult);
                        labelTotalRevenue.Text = totalRevenue.ToString() + " BDT";
                    }
                    else
                    {
                        labelTotalRevenue.Text = "0 BDT";
                    }
                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
