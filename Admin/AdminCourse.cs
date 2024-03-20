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
    public partial class AdminCourse : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public AdminCourse()
        {
            InitializeComponent();
            BindGridView();
        }

        private void AdminCourse_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            AddCourse addCourse = new AddCourse();
            addCourse.ShowDialog();
        }

        private void guna2Button12_Click(object sender, EventArgs e)
        {
            ModifyCourse modifyCourse = new ModifyCourse();
            modifyCourse.ShowDialog();
        }
        void BindGridView()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = "select * from courses";
                    using (SqlDataAdapter sda = new SqlDataAdapter(query, con))
                    {
                        DataTable data = new DataTable();
                        sda.Fill(data);

                        dataGridView1.DataSource = data;
                        DataGridViewImageColumn dgvImageColumn = (DataGridViewImageColumn)dataGridView1.Columns["Banner"];
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

        private void removeCourseBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string courseId = dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString();

                DialogResult result = MessageBox.Show("Are you sure you want to delete this course?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DeleteCourse(courseId);
                    BindGridView();
                }
            }
            else
            {
                MessageBox.Show("Please select a course to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void DeleteCourse(string courseId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    string query = "DELETE FROM courses WHERE ID = @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", courseId);
                        cmd.ExecuteNonQuery();
                    }

                    con.Close();
                }

                MessageBox.Show("Course deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting course: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button14_Click(object sender, EventArgs e)
        {
            BindGridView();
            MessageBox.Show("Data Refresh Successful");
        }

        private void guna2Button13_Click(object sender, EventArgs e)
        {

        }
    }
}
