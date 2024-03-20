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
    public partial class AddJob : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public AddJob()
        {
            InitializeComponent();
        }

        private void addJobBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int nextID;
                int maxID = GetMaxIDFromDatabase();
                nextID = maxID + 1;
                string formattedApplyDeadLine = applyDeadLine.Value.ToString("dd-MM-yyyy");
                SqlConnection con = new SqlConnection(cs);
                string query = "Insert into JOBS (ID, COMPANYLOGO, JOBTITLE, COMPANYNAME, ADDRESS, SALARY, APPLYDEADLINE, USERTYPE, A_LINK, CREATE_DATE) values(@ID, @COMPANYLOGO, @JOBTITLE, @COMPANYNAME, @ADDRESS, @SALARY, @APPLYDEADLINE, @USERTYPE, @A_LINK, @CREATE_DATE)";
                SqlCommand cnn = new SqlCommand(query, con);

                con.Open();

                cnn.Parameters.AddWithValue("@ID", nextID);
                cnn.Parameters.AddWithValue("@COMPANYLOGO", SaveBanner());
                cnn.Parameters.AddWithValue("@JOBTITLE", textBoxJobTitle.Text);
                cnn.Parameters.AddWithValue("@COMPANYNAME", textBoxCompanyName.Text);
                cnn.Parameters.AddWithValue("@ADDRESS", textBoxCompanyAddress.Text);

                if (decimal.TryParse(textBoxJobSalary.Text, out decimal salaryValue))
                {
                    cnn.Parameters.AddWithValue("@SALARY", salaryValue);
                }

                cnn.Parameters.AddWithValue("@APPLYDEADLINE", formattedApplyDeadLine);
                cnn.Parameters.AddWithValue("@USERTYPE", comboBoxJobUserType.Text);
                cnn.Parameters.AddWithValue("@A_LINK", textBoxApplyLink.Text);
                cnn.Parameters.AddWithValue("@CREATE_DATE", CreateDate());


                cnn.ExecuteNonQuery();

                con.Close();

                MessageBox.Show("Job Added Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetMaxIDFromDatabase()
        {
            int maxID = 0;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT MAX(ID) FROM JOBS";
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();

                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    maxID = Convert.ToInt32(result);
                }

                con.Close();
            }

            return maxID;
        }

        private byte[] SaveBanner()
        {
            MemoryStream ms = new MemoryStream();
            pictureBoxJPoster.Image.Save(ms, pictureBoxJPoster.Image.RawFormat);
            return ms.GetBuffer();
        }

        private void jPosterBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Image";
            ofd.Filter = "ALL IMAGE FILE (*.*) | *.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBoxJPoster.Image = new Bitmap(ofd.FileName);
            }
        }

        private DateTime CreateDate()
        {
            return DateTime.Now;
        }

        private void exitBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
