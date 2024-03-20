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
    public partial class ModifyCourse : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public ModifyCourse()
        {
            InitializeComponent();

            textBoxID.Clear();
            textBoxCourseName.Clear();
            textBoxDetails.Clear();
            textBoxPlayListLink.Clear();
            comboBoxCategory.SelectedItem = null;
            comboBoxUserType.SelectedItem = null;
            pictureBoxBanner.Image = null;

            textBoxCourseName.Enabled = false;
            textBoxDetails.Enabled = false;
            textBoxPlayListLink.Enabled = false;
            comboBoxCategory.Enabled = false;
            comboBoxUserType.Enabled = false;
            browseBannerBtn.Enabled = false;
            updateBtn.Enabled = false;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            AdminCourse adminCourse = new AdminCourse();
            adminCourse.ShowDialog();
            this.Close();
        }

        private void dataSearch_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "SELECT * FROM courses WHERE id=@id";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", textBoxID.Text);

            try
            {
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    textBoxCourseName.Text = reader["title"].ToString();
                    textBoxDetails.Text = reader["details"].ToString();
                    textBoxPlayListLink.Text = reader["playlist_link"].ToString();
                    comboBoxCategory.Text = reader["category"].ToString();
                    comboBoxUserType.Text = reader["usertype"].ToString();

                    byte[] imageData = (byte[])reader["banner"];
                    if (imageData != null && imageData.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            pictureBoxBanner.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        pictureBoxBanner.Image = null;
                    }

                    textBoxCourseName.Enabled = true;
                    textBoxDetails.Enabled = true;
                    textBoxPlayListLink.Enabled = true;
                    comboBoxCategory.Enabled = true;
                    comboBoxUserType.Enabled = true;
                    browseBannerBtn.Enabled = true;
                    updateBtn.Enabled = true;
                }
                else
                {
                    textBoxID.Clear();
                    textBoxCourseName.Clear();
                    textBoxDetails.Clear();
                    textBoxPlayListLink.Clear();
                    comboBoxCategory.SelectedItem = null;
                    comboBoxUserType.SelectedItem = null;
                    pictureBoxBanner.Image = null;

                    textBoxCourseName.Enabled = false;
                    textBoxDetails.Enabled = false;
                    textBoxPlayListLink.Enabled = false;
                    comboBoxCategory.Enabled = false;
                    comboBoxUserType.Enabled = false;
                    browseBannerBtn.Enabled = false;
                    updateBtn.Enabled = false;

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
        private byte[] Savebanner()
        {
            if (pictureBoxBanner.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBoxBanner.Image.Save(ms, pictureBoxBanner.Image.RawFormat);
                    return ms.GetBuffer();
                }
            }
            else
            {
                return null;
            }
        }


        private void browseBannerBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Image";
            ofd.Filter = "ALL IMAGE FILE (*.*) | *.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBoxBanner.Image = new Bitmap(ofd.FileName);
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "UPDATE COURSES SET ID=@ID, BANNER=@BANNER, TITLE=@TITLE, DETAILS=@DETAILS, PLAYLIST_LINK=@PLAYLIST_LINK, CATEGORY=@CATEGORY, USERTYPE=@USERTYPE";
            SqlCommand cnn = new SqlCommand(query, con);
            cnn.Parameters.AddWithValue("@ID", textBoxID.Text);

            try
            {
                con.Open();

                cnn.Parameters.AddWithValue("@TITLE", textBoxCourseName.Text);
                cnn.Parameters.AddWithValue("@DETAILS", textBoxDetails.Text);
                cnn.Parameters.AddWithValue("@PLAYLIST_LINK", textBoxPlayListLink.Text);
                cnn.Parameters.AddWithValue("@CATEGORY", comboBoxCategory.Text);
                cnn.Parameters.AddWithValue("@USERTYPE", comboBoxUserType.Text);
                cnn.Parameters.AddWithValue("@BANNER", Savebanner());

                cnn.ExecuteNonQuery();

                con.Close();

                MessageBox.Show("Update Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exitBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
