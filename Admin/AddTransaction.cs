using Microsoft.VisualBasic.ApplicationServices;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Coders_Space.Admin
{
    public partial class AddTransaction : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        decimal amount;
        public AddTransaction()
        {
            InitializeComponent();

            textBoxName.Clear();
            comboBoxPackage.SelectedIndex = 0;

            textBoxName.Enabled = false;
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

                    string userRole = reader["Role"].ToString();
                    SetFieldAccessibility(userRole == "REGULAR_USER");

                    MessageBox.Show($"Data found!\nUserID: {reader["id"].ToString()}\nUserName: {reader["name"].ToString()}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    textBoxName.Clear();
                    SetFieldAccessibility(false);

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

        private void selectPackage()
        {
            string selectedPackage = comboBoxPackage.SelectedItem.ToString();

            if (selectedPackage == "Monthly")
            {
                labelPrice.Text = "500 BDT";
                amount = 500;
            }
            else if (selectedPackage == "Annually")
            {
                labelPrice.Text = "5000 BDT";
                amount = 5000;
            }
        }


        private void comboBoxPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectPackage();
        }

        private void addTransactionBtn_Click(object sender, EventArgs e)
        {
            if (FieldValidation())
            {
                UpdateUserRoleToPremium();
                InsertTransactionRecord();

                MessageBox.Show("Add Transaction Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBoxID.Clear();
                textBoxName.Clear();
                comboBoxPackage.SelectedIndex = 0;
                labelPrice.Text = "0 BDT";
            }
        }

        private bool FieldValidation()
        {
            if (string.IsNullOrEmpty(textBoxID.Text))
            {
                MessageBox.Show("Please enter a valid ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Please enter a valid name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (comboBoxPackage.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a package.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }


        private void UpdateUserRoleToPremium()
        {
            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();

                string query = "UPDATE USERS SET Role = 'PREMIUM_USER', UPDATE_DATE = GETDATE() WHERE ID = @ID";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ID", textBoxID.Text);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("User role updated to premium_user.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to update user role.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                string queryUser = "SELECT ID, Name FROM USERS WHERE ID = @ID";
                SqlCommand commandUser = new SqlCommand(queryUser, connection);

                commandUser.Parameters.AddWithValue("@ID", textBoxID.Text);

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

        private void SetFieldAccessibility(bool enabled)
        {
            comboBoxPackage.Enabled = enabled;
            comboBoxPackage.SelectedIndex = 0;
            labelPrice.Text = "0 BDT";
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Transaction adminTransaction = new Transaction();
            adminTransaction.ShowDialog();
            this.Close();
        }

        private void exitBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
