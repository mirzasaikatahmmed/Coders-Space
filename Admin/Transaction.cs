using Coders_Space.Admin;
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
    public partial class Transaction : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public Transaction()
        {
            InitializeComponent();
            BindGridView();
        }

        void BindGridView()
        {
            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();

                string query = "SELECT * FROM TRANSACTIONS";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            AddTransaction addTransaction = new AddTransaction();
            addTransaction.ShowDialog();
        }

        private void removeBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string Id = dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString();

                DialogResult result = MessageBox.Show("Are you sure you want to delete this transaction?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DeleteTransaction(Id);
                    BindGridView();
                }
            }
            else
            {
                MessageBox.Show("Please select a transaction to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteTransaction(string Id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    string query = "DELETE FROM transactions WHERE ID = @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", Id);
                        cmd.ExecuteNonQuery();
                    }

                    con.Close();
                }

                MessageBox.Show("Transaction deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting transaction: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            BindGridView();
            MessageBox.Show("Data Refresh Successful");
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            string searchTrxID = textBoxTrxID.Text.Trim();

            if (!string.IsNullOrEmpty(searchTrxID) && dataGridView1.Rows != null)
            {
                bool userFound = false;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["TRXID"].Value != null)
                    {
                        string trxID = row.Cells["TRXID"].Value.ToString();
                        if (trxID.Equals(searchTrxID))
                        {
                            dataGridView1.ClearSelection();
                            row.Selected = true;
                            dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                            MessageBox.Show($"User found with ID: {trxID}", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            userFound = true;
                            break;
                        }
                    }
                }

                if (!userFound)
                {
                    MessageBox.Show($"No transaction found with ID: {searchTrxID}", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please enter a transaction ID to search.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
