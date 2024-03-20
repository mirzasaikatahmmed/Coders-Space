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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Coders_Space
{
    public partial class EditProfile : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        string userEmail;
        public EditProfile(string email)
        {
            InitializeComponent();
            userEmail = email;
            LoadUserData(userEmail);
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
                        pictureBox2.Image = Image.FromStream(ms);
                    }

                    textBoxName.Text = reader["NAME"].ToString();
                    textBoxContact.Text = reader["CONTACT_NO"].ToString();
                    DateTimePickerDOB.Text = ((DateTime)reader["DOB"]).ToShortDateString();
                    comboBoxGender.Text = reader["GENDER"].ToString();
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

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            string formattedDOB = DateTimePickerDOB.Value.ToString("yyyy-MM-dd");
            SqlConnection con = new SqlConnection(cs);
            string query = "UPDATE USERS SET NAME=@NAME, CONTACT_NO=@CONTACT_NO, DOB=@DOB, GENDER=@GENDER, PHOTO=@PHOTO, UPDATE_DATE=@UPDATE_DATE WHERE EMAIL=@EMAIL";
            SqlCommand cnn = new SqlCommand(query, con);
            cnn.Parameters.AddWithValue("@EMAIL", userEmail);

            try
            {
                con.Open();

                cnn.Parameters.AddWithValue("@NAME", textBoxName.Text);
                cnn.Parameters.AddWithValue("@CONTACT_NO", textBoxContact.Text);
                cnn.Parameters.AddWithValue("@DOB", formattedDOB);
                cnn.Parameters.AddWithValue("@GENDER", comboBoxGender.Text);
                cnn.Parameters.AddWithValue("@PHOTO", SavePhoto());
                cnn.Parameters.AddWithValue("@UPDATE_DATE", DateTime.Now);

                cnn.ExecuteNonQuery();

                con.Close();

                MessageBox.Show("Update Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private byte[] SavePhoto()
        {
            MemoryStream ms = new MemoryStream();
            pictureBox2.Image.Save(ms, pictureBox2.Image.RawFormat);
            return ms.GetBuffer();
        }

        private void browsePhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Image";
            ofd.Filter = "ALL IMAGE FILE (*.*) | *.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image = new Bitmap(ofd.FileName);
            }
        }

        private void exitBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
