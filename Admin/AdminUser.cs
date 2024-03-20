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
    public partial class AdminUser : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public AdminUser()
        {
            InitializeComponent();
            BindGridView();
        }

        private void addUserBtn_Click(object sender, EventArgs e)
        {
            AddUser addUser = new AddUser();
            addUser.ShowDialog();
        }

        private void modifyUserBtn_Click(object sender, EventArgs e)
        {
            ModifyUser modifyUser = new ModifyUser();
            modifyUser.ShowDialog();
        }

        private void AdminUser_Load(object sender, EventArgs e)
        {

        }

        

        void BindGridView()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = "select ID, NAME, CONTACT_NO, EMAIL, DOB, GENDER, PHOTO, ROLE from users";
                    using (SqlDataAdapter sda = new SqlDataAdapter(query, con))
                    {
                        DataTable data = new DataTable();
                        sda.Fill(data);

                        dataGridView1.DataSource = data;
                        DataGridViewImageColumn dgvImageColumn = (DataGridViewImageColumn)dataGridView1.Columns["PHOTO"];
                        dgvImageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;

                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dataGridView1.RowTemplate.Height = 80;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ModifyUser modifyUser = new ModifyUser();
            modifyUser.ShowDialog();
        }

        private void guna2Button14_Click(object sender, EventArgs e)
        {
            BindGridView();
            MessageBox.Show("Data Refresh Successful");
        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string userId = dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString();

                if (HasUserTransactions(userId))
                {
                    DialogResult transactionResult = MessageBox.Show("This user has transactions. Deleting the user will also delete associated transactions. Are you sure you want to proceed?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (transactionResult == DialogResult.Yes)
                    {
                        DeleteUserTransactions(userId);
                        DeleteUser(userId);
                        BindGridView();
                    }
                }
                else
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        DeleteUser(userId);
                        BindGridView();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool HasUserTransactions(string userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM transactions WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking transactions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void DeleteUserTransactions(string userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    string deleteQuery = "DELETE FROM transactions WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting transactions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteUser(string userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    string query = "DELETE FROM users WHERE ID = @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", userId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("User deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string userId = dataGridView1.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                string userName = dataGridView1.Rows[e.RowIndex].Cells["NAME"].Value.ToString();
                MessageBox.Show($"Clicked on user: {userName} (ID: {userId})", "Cell Clicked", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            string searchUserId = userSearch.Text.Trim();

            if (!string.IsNullOrEmpty(searchUserId) && dataGridView1.Rows != null)
            {
                bool userFound = false;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["ID"].Value != null)
                    {
                        string userId = row.Cells["ID"].Value.ToString();
                        if (userId.Equals(searchUserId))
                        {
                            dataGridView1.ClearSelection();
                            row.Selected = true;
                            dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                            MessageBox.Show($"User found with ID: {userId}", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            userFound = true;
                            break;
                        }
                    }
                }

                if (!userFound)
                {
                    MessageBox.Show($"No user found with ID: {searchUserId}", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please enter a user ID to search.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
