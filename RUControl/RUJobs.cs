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

namespace Coders_Space
{
    public partial class RUJobs : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public RUJobs()
        {
            InitializeComponent();
        }

        private void RUJobs_Load(object sender, EventArgs e)
        {
            jobList();
        }

        private void jobList()
        {
            flowLayoutPanel1.Controls.Clear();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                string query = "SELECT * FROM JOBS WHERE USERTYPE = 'REGULAR_USER' ORDER BY CREATE_DATE DESC";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ListJobs jobControl = new ListJobs();

                    jobControl.CompanyLogo = ConvertByteArrayToImage(reader["COMPANYLOGO"] as byte[]);
                    jobControl.JobTitle = reader["JOBTITLE"].ToString();
                    jobControl.CompanyName = reader["COMPANYNAME"].ToString();
                    jobControl.Address = reader["ADDRESS"].ToString();
                    jobControl.Salary = "Salary: " + reader["SALARY"].ToString() + "BDT";
                    jobControl.Benefits = "Snacks, Lunch";
                    jobControl.ApplyLastDate = DateTime.Parse(reader["APPLYDEADLINE"].ToString());

                    string link = reader["A_LINK"].ToString();
                    jobControl.ApplyNowButtonClick += (sender, e) => jobControl.OpenBrowser(link);
                    flowLayoutPanel1.Controls.Add(jobControl);
                }
            }
        }

        private Image ConvertByteArrayToImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
            {
                return Properties.Resources.no_image_avaiable;
            }

            try
            {
                using (MemoryStream ms = new MemoryStream(byteArray))
                {
                    Image image = Image.FromStream(ms);
                    return image;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting byte array to Image: {ex.Message}");
                return Properties.Resources.no_image_avaiable;
            }
        }
    }
}
