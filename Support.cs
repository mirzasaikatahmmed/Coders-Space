using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Coders_Space.Admin;
using System.Configuration;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Coders_Space
{
    public partial class Support : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        string userEmail;
        public Support(String email)
        {
            InitializeComponent();
            userEmail = email;
        }

        private async void send_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxName.Text) ||
                    string.IsNullOrWhiteSpace(textBoxEmail.Text) ||
                    string.IsNullOrWhiteSpace(textBoxMessage.Text))
                {
                    MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int nextID;
                int maxID = GetMaxIDFromDatabase();
                nextID = maxID + 1;
                SqlConnection con = new SqlConnection(cs);
                int userId = GetUserIdFromDatabase(userEmail);
                string query = "Insert into SUPPORT (ID,USERID,NAME,EMAIL,MESSAGE,SEND_TIME) values(@ID,@USERID,@NAME,@EMAIL,@MESSAGE,@SEND_TIME)";
                SqlCommand cnn = new SqlCommand(query, con);

                con.Open();

                cnn.Parameters.AddWithValue("@ID", nextID);
                cnn.Parameters.AddWithValue("@USERID", userId);
                cnn.Parameters.AddWithValue("@NAME", textBoxName.Text);
                cnn.Parameters.AddWithValue("@EMAIL", textBoxEmail.Text);
                cnn.Parameters.AddWithValue("@MESSAGE", textBoxMessage.Text);
                cnn.Parameters.AddWithValue("@SEND_TIME", SendTime());

                cnn.ExecuteNonQuery();

                con.Close();


                string smtpServer = "";
                int smtpPort = 587;
                string smtpUsername = "";
                string smtpPassword = "";

                string adminEmail = "contact@saikat.com.bd";

                string name = textBoxName.Text;
                string email = textBoxEmail.Text;
                string message = textBoxMessage.Text;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(email);
                mail.To.Add(new MailAddress(adminEmail));
                mail.Subject = "New Message from Contact Form";
                mail.Body = $"Name: {name}\nEmail: {email}\nMessage:\n{message}";

                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;
                smtpClient.Timeout = 30000;

                await smtpClient.SendMailAsync(mail);

                MessageBox.Show("Message sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBoxName.Text = "";
                textBoxEmail.Text = "";
                textBoxMessage.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetMaxIDFromDatabase()
        {
            int maxID = 0;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT MAX(ID) FROM SUPPORT";
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

        private DateTime SendTime()
        {
            return DateTime.Now;
        }
        private int GetUserIdFromDatabase(string userEmail)
        {
            int userId = 0;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                string query = "SELECT ID FROM Users WHERE EMAIL = @EMAIL";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@EMAIL", userEmail);

                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    userId = Convert.ToInt32(result);
                }

                con.Close();
            }

            return userId;
        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Support_Load(object sender, EventArgs e)
        {
            textBoxMessage.Text = "Enetr Your Message";
        }

        private void textBoxMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxMessage_Enter(object sender, EventArgs e)
        {
            if (textBoxMessage.Text == "Enetr Your Message")
            {
                textBoxMessage.Text = "";
            }
        }

        private void textBoxMessage_Leave(object sender, EventArgs e)
        {
            if (textBoxMessage.Text == "Enetr Your Message")
            {
                textBoxMessage.Text = "";
            }
        }
    }
}
