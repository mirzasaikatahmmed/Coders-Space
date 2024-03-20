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
    public partial class ModifyUser : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public ModifyUser()
        {
            InitializeComponent();

            textBoxName.Clear();
            textBoxContact.Clear();
            textBoxEmail.Clear();
            DateTimePickerDOB.Value = DateTime.Now;
            comboBoxGender.SelectedItem = null;
            comboBoxRole.SelectedItem = null;
            textBoxEPassword.Clear();
            //pictureBox2.Image = null;

            textBoxName.Enabled = false;
            textBoxContact.Enabled = false;
            textBoxEmail.Enabled = false;
            DateTimePickerDOB.Enabled = false;
            comboBoxGender.Enabled = false;
            comboBoxRole.Enabled = false;
            textBoxEPassword.Enabled = false;
            browsePhoto.Enabled = false;
            UpdateBtn.Enabled = false;

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            AdminUser adminUser = new AdminUser();
            adminUser.ShowDialog();
            this.Close();
        }

        private void ModifyUser_Load(object sender, EventArgs e)
        {

        }

        private void dataSearch_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "SELECT * FROM users WHERE id=@id";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", textBoxID.Text);

            try
            {
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    textBoxName.Text = reader["name"].ToString();
                    textBoxContact.Text = reader["contact_no"].ToString();
                    textBoxEmail.Text = reader["email"].ToString();
                    DateTimePickerDOB.Text = reader["dob"].ToString();
                    comboBoxGender.SelectedItem = reader["gender"].ToString();
                    comboBoxRole.SelectedItem = reader["role"].ToString();
                    textBoxEPassword.Text = reader["password"].ToString();

                    byte[] imageData = (byte[])reader["photo"];
                    if (imageData != null && imageData.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            pictureBox2.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        pictureBox2.Image = null;
                    }

                    textBoxName.Enabled = true;
                    textBoxContact.Enabled = true;
                    textBoxEmail.Enabled = true;
                    DateTimePickerDOB.Enabled = true;
                    comboBoxGender.Enabled = true;
                    comboBoxRole.Enabled = true;
                    textBoxEPassword.Enabled = true;
                    browsePhoto.Enabled = true;
                    UpdateBtn.Enabled = true;
                }
                else
                {
                    textBoxName.Clear();
                    textBoxContact.Clear();
                    textBoxEmail.Clear();
                    DateTimePickerDOB.Value = DateTime.Now;
                    comboBoxGender.SelectedItem = null;
                    comboBoxRole.SelectedItem = null;
                    textBoxEPassword.Clear();
                    pictureBox2.Image = null;

                    textBoxName.Enabled = false;
                    textBoxContact.Enabled = false;
                    textBoxEmail.Enabled = false;
                    DateTimePickerDOB.Enabled = false;
                    comboBoxGender.Enabled = false;
                    comboBoxRole.Enabled = false;
                    textBoxEPassword.Enabled = false;
                    browsePhoto.Enabled = false;

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

        private byte[] SavePhoto()
        {
            if (pictureBox2.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox2.Image.Save(ms, pictureBox2.Image.RawFormat);
                    return ms.GetBuffer();
                }
            }
            else
            {
                return null;
            }
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

        private void registrationBtn_Click(object sender, EventArgs e)
        {
            string formattedDOB = DateTimePickerDOB.Value.ToString("yyyy-MM-dd");
            SqlConnection con = new SqlConnection(cs);
            string query = "UPDATE USERS SET NAME=@NAME, CONTACT_NO=@CONTACT_NO, EMAIL=@EMAIL, DOB=@DOB, GENDER=@GENDER, PHOTO=@PHOTO, PASSWORD=@PASSWORD, ROLE=@ROLE, UPDATE_DATE=@UPDATE_DATE WHERE ID=@ID";
            SqlCommand cnn = new SqlCommand(query, con);
            cnn.Parameters.AddWithValue("@ID", textBoxID.Text);

            try
            {
                con.Open();

                cnn.Parameters.AddWithValue("@NAME", textBoxName.Text);
                cnn.Parameters.AddWithValue("@CONTACT_NO", textBoxContact.Text);
                cnn.Parameters.AddWithValue("@EMAIL", textBoxEmail.Text);
                cnn.Parameters.AddWithValue("@DOB", formattedDOB);
                cnn.Parameters.AddWithValue("@GENDER", comboBoxGender.Text);
                cnn.Parameters.AddWithValue("@PHOTO", SavePhoto());
                cnn.Parameters.AddWithValue("@PASSWORD", textBoxEPassword.Text);
                cnn.Parameters.AddWithValue("@ROLE", comboBoxRole.Text);
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

        private void exitBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
