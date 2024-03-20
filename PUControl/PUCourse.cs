using Coders_Space.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coders_Space.PUControl
{
    public partial class PUCourse : Form
    {
        public PUCourse()
        {
            InitializeComponent();
        }

        private void PUCourse_Load(object sender, EventArgs e)
        {
            courseList();
        }

        private void courseList()
        {
            flowLayoutPanel1.Controls.Clear();

            int itemsPerRow = 6;
            int itemCount = 0;

            Image defaultPicture = Properties.Resources.no_image_avaiable;
            string defaultCourseTitle = "Complete C# Course";
            string defaultLink = "https://google.com";

            for (int i = 0; i < 12; i++)
            {
                ListCourses listCoursesControl = new ListCourses();

                listCoursesControl.CoursePicture = defaultPicture;
                listCoursesControl.CourseTitle = defaultCourseTitle;

                listCoursesControl.viewCourseBTNClick += (sender, e) =>
                {
                    listCoursesControl.OpenBrowser(defaultLink);
                };

                flowLayoutPanel1.Controls.Add(listCoursesControl);
                itemCount++;

                if (itemCount % itemsPerRow == 0)
                {
                    flowLayoutPanel1.SetFlowBreak(listCoursesControl, true);
                }
            }
        }
    }
}
