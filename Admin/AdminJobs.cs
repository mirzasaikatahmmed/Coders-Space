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
    public partial class AdminJobs : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public AdminJobs()
        {
            InitializeComponent();
            BindGridView();
        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            AddJob addJob = new AddJob();
            addJob.ShowDialog();
        }

        private void guna2Button12_Click(object sender, EventArgs e)
        {
            ModifyJob modifyJob = new ModifyJob();
            modifyJob.ShowDialog();
        }

        void BindGridView()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = "select * from jobs";
                    using (SqlDataAdapter sda = new SqlDataAdapter(query, con))
                    {
                        DataTable data = new DataTable();
                        sda.Fill(data);

                        dataGridView1.DataSource = data;
                        DataGridViewImageColumn dgvImageColumn = (DataGridViewImageColumn)dataGridView1.Columns["COMPANYLOGO"];
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

        private void removeBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string jobId = dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString();

                DialogResult result = MessageBox.Show("Are you sure you want to delete this course?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DeleteJobs(jobId);
                    BindGridView();
                }
            }
            else
            {
                MessageBox.Show("Please select a course to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteJobs(string courseId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    string query = "DELETE FROM jobs WHERE ID = @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", courseId);
                        cmd.ExecuteNonQuery();
                    }

                    con.Close();
                }

                MessageBox.Show("Job deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting job: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            string searchJobId = textBoxJobID.Text.Trim();

            if (!string.IsNullOrEmpty(searchJobId) && dataGridView1.Rows != null)
            {
                bool userFound = false;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["ID"].Value != null)
                    {
                        string userId = row.Cells["ID"].Value.ToString();
                        if (userId.Equals(searchJobId))
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
                    MessageBox.Show($"No job found with ID: {searchJobId}", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please enter a job ID to search.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            BindGridView();
            MessageBox.Show("Data Refresh Successful");
        }
    }
}
