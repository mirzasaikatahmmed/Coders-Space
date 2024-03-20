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
    public partial class AddCourse : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public AddCourse()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addCourseBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int nextID;
                //int contact = 1234567890;
                int maxID = GetMaxIDFromDatabase();
                nextID = maxID + 1;
                SqlConnection con = new SqlConnection(cs);
                string query = "Insert into COURSES (ID,BANNER,TITLE,DETAILS,PLAYLIST_LINK,CATEGORY,USERTYPE) values(@ID,@BANNER,@TITLE,@DETAILS,@PLAYLIST_LINK,@CATEGORY,@USERTYPE)";
                SqlCommand cnn = new SqlCommand(query, con);

                con.Open();

                cnn.Parameters.AddWithValue("@ID", nextID);
                cnn.Parameters.AddWithValue("@BANNER", SaveBanner());
                cnn.Parameters.AddWithValue("@TITLE", textBoxCourseName.Text);
                cnn.Parameters.AddWithValue("@DETAILS", textBoxDetails.Text);
                cnn.Parameters.AddWithValue("@PLAYLIST_LINK", textBoxPlayListLink.Text);
                cnn.Parameters.AddWithValue("@CATEGORY", comboBoxCategory.Text);
                cnn.Parameters.AddWithValue("@USERTYPE", comboBoxUserType.Text);

                cnn.ExecuteNonQuery();

                con.Close();

                MessageBox.Show("Course Added Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                string query = "SELECT MAX(ID) FROM COURSES";
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
            pictureBoxBanner.Image.Save(ms, pictureBoxBanner.Image.RawFormat);
            return ms.GetBuffer();
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
    }
}
