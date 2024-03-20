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
    public partial class ModifyJob : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public ModifyJob()
        {
            InitializeComponent();

            textBoxJobTitle.Clear();
            textBoxJobSalary.Clear();
            textBoxCompanyName.Clear();
            textBoxCompanyAddress.Clear();
            comboBoxJobUserType.SelectedItem = null;
            applyDeadLine.Value = DateTime.Now;
            textBoxApplyLink.Clear();

            textBoxJobTitle.Enabled = false;
            textBoxJobSalary.Enabled = false;
            textBoxCompanyName.Enabled = false;
            textBoxCompanyAddress.Enabled = false;
            comboBoxJobUserType.Enabled = false;
            applyDeadLine.Enabled = false;
            textBoxApplyLink.Enabled = false;
            jPosterBtn.Enabled = false;
            updateJobBtn.Enabled = false;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            AdminJobs adminJobs = new AdminJobs();
            adminJobs.ShowDialog();
            this.Close();
        }

        private void updateJobBtn_Click(object sender, EventArgs e)
        {

        }

        private void dataSearch_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "SELECT * FROM JOBS WHERE id=@id";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", textBoxID.Text);

            try
            {
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    textBoxJobTitle.Text = reader["JOBTITLE"].ToString();
                    textBoxJobSalary.Text = reader["SALARY"].ToString();
                    textBoxCompanyName.Text = reader["COMPANYNAME"].ToString();
                    textBoxCompanyAddress.Text = reader["ADDRESS"].ToString();
                    textBoxApplyLink.Text = reader["A_LINK"].ToString();
                    comboBoxJobUserType.SelectedItem = reader["USERTYPE"].ToString();
                    applyDeadLine.Text = reader["APPLYDEADLINE"].ToString();

                    byte[] imageData = (byte[])reader["COMPANYLOGO"];
                    if (imageData != null && imageData.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            pictureBoxJPoster.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        pictureBoxJPoster.Image = null;
                    }

                    textBoxJobTitle.Enabled = true;
                    textBoxJobSalary.Enabled = true;
                    textBoxCompanyName.Enabled = true;
                    textBoxCompanyAddress.Enabled = true;
                    comboBoxJobUserType.Enabled = true;
                    applyDeadLine.Enabled = true;
                    textBoxApplyLink.Enabled = true;
                    jPosterBtn.Enabled = true;
                    updateJobBtn.Enabled = true;

                }
                else
                {
                    textBoxJobTitle.Clear();
                    textBoxJobSalary.Clear();
                    textBoxCompanyName.Clear();
                    textBoxCompanyAddress.Clear();
                    comboBoxJobUserType.SelectedItem = null;
                    applyDeadLine.Value = DateTime.Now;
                    textBoxApplyLink.Clear();

                    textBoxJobTitle.Enabled = false;
                    textBoxJobSalary.Enabled = false;
                    textBoxCompanyName.Enabled = false;
                    textBoxCompanyAddress.Enabled = false;
                    comboBoxJobUserType.Enabled = false;
                    applyDeadLine.Enabled = false;
                    textBoxApplyLink.Enabled = false;
                    jPosterBtn.Enabled = false;
                    updateJobBtn.Enabled = false;

                    MessageBox.Show("Data not found for the given ID.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private byte[] SaveBanner()
        {
            MemoryStream ms = new MemoryStream();
            pictureBoxJPoster.Image.Save(ms, pictureBoxJPoster.Image.RawFormat);
            return ms.GetBuffer();
        }

        private void jPosterBtn_Click(object sender, EventArgs e)
        {
            
        }

        private DateTime UpdateDate()
        {
            return DateTime.Now;
        }

        private void exitBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void updateJobBtn_Click_1(object sender, EventArgs e)
        {
            string formattedApplyDeadLine = applyDeadLine.Value.ToString("dd-MM-yyyy");
            SqlConnection con = new SqlConnection(cs);
            string query = "UPDATE JOBS SET COMPANYLOGO=@COMPANYLOGO, JOBTITLE=@JOBTITLE, COMPANYNAME=@COMPANYNAME, ADDRESS=@ADDRESS, SALARY=@SALARY, APPLYDEADLINE=@APPLYDEADLINE, USERTYPE=@USERTYPE, A_LINK=@A_LINK, UPDATE_DATE=@UPDATE_DATE WHERE ID=@ID";

            SqlCommand cnn = new SqlCommand(query, con);
            cnn.Parameters.AddWithValue("@ID", textBoxID.Text);

            try
            {
                con.Open();

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
                cnn.Parameters.AddWithValue("@UPDATE_DATE", UpdateDate());

                cnn.ExecuteNonQuery();

                con.Close();

                MessageBox.Show("Update Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void jPosterBtn_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Image";
            ofd.Filter = "ALL IMAGE FILE (*.*) | *.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBoxJPoster.Image = new Bitmap(ofd.FileName);
            }
        }
    }
}
