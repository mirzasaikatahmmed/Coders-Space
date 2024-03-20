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
    public partial class Payment : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        string userEmail;
        decimal amount;
        public Payment(string email, decimal amount)
        {
            InitializeComponent();
            userEmail = email;
            this.amount = amount;
        }

        private void Payment_Load(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private bool PaymentInformationIsValid()
        {
            string defaultAccountName = "Mirza Saikat Ahmmed";
            string defaultCardNumber = "123456789012";
            string defaultExpirationMonth = "12";
            string defaultExpirationYear = "35";
            string defaultCVV = "354";

            return textBoxAccountName.Text.Trim() == defaultAccountName &&
                   textBoxCardNumber.Text.Trim() == defaultCardNumber &&
                   textBoxMM.Text.Trim() == defaultExpirationMonth &&
                   textBoxYY.Text.Trim() == defaultExpirationYear &&
                   textBoxCVV.Text.Trim() == defaultCVV;
        }

        private void buttonPay_Click(object sender, EventArgs e)
        {
            if (PaymentInformationIsValid())
            {
                UpdateUserRoleToPremium();
                InsertTransactionRecord();

                MessageBox.Show("Payment complete!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Login login = new Login();
                login.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Payment information is not valid.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateUserRoleToPremium()
        {
            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();

                string query = "UPDATE USERS SET Role = 'PREMIUM_USER', UPDATE_DATE = GETDATE() WHERE Email = @Email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", userEmail);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("User role updated to premium_user.");
                }
                else
                {
                    Console.WriteLine("Failed to update user role.");
                }
            }
        }

        private void InsertTransactionRecord()
        {
            int nextID;
            int maxID = GetMaxIDFromDatabase();
            nextID = maxID + 1;

            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();

                string queryUser = "SELECT ID, Name FROM USERS WHERE Email = @Email";
                SqlCommand commandUser = new SqlCommand(queryUser, connection);
                commandUser.Parameters.AddWithValue("@Email", userEmail);

                SqlDataReader reader = commandUser.ExecuteReader();

                int userId = 0;
                string userName = string.Empty;

                if (reader.Read())
                {
                    object idValue = reader["ID"];

                    if (idValue != null && idValue != DBNull.Value)
                    {
                        userId = Convert.ToInt32(idValue);
                    }
                    userName = reader["NAME"].ToString(); ;
                }

                reader.Close();

                string queryTransaction = "INSERT INTO TRANSACTIONS (ID, USERID, USERNAME, AMOUNT, TRXID, SUB_VALID_MONTH, TRANSACTION_DATE) " +
                                          "VALUES (@ID, @UserId, @UserName, @Amount, @TrxId, @SubValidMonth, GETDATE())";

                using (SqlCommand commandTransaction = new SqlCommand(queryTransaction, connection))
                {
                    commandTransaction.Parameters.AddWithValue("@ID", nextID);
                    commandTransaction.Parameters.AddWithValue("@UserId", userId);
                    commandTransaction.Parameters.AddWithValue("@UserName", userName);
                    commandTransaction.Parameters.AddWithValue("@Amount", amount);
                    commandTransaction.Parameters.AddWithValue("@TrxId", GenerateTrxId());
                    commandTransaction.Parameters.AddWithValue("@SubValidMonth", (amount == 5000) ? 12 : 1);

                    commandTransaction.ExecuteNonQuery();
                }
            }
        }

        private int GetMaxIDFromDatabase()
        {
            int maxID = 0;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT MAX(ID) FROM TRANSACTIONS";
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

        private string GenerateTrxId()
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string randomPart = Guid.NewGuid().ToString("N").Substring(0, 6);

            return $"{timestamp}{randomPart}";
        }

        private void exitBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void minimizeBTN_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
