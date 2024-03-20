using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coders_Space
{
    public partial class ListCourses : UserControl
    {
        public ListCourses()
        {
            InitializeComponent();
        }

        private Image _coursePicture;
        public Image CoursePicture
        {
            get { return _coursePicture; }
            set
            {
                _coursePicture = value;
                picture.Image = value;
            }
        }
        private string _courseTitle;
        public string CourseTitle
        {
            get { return _courseTitle; }
            set
            {
                _courseTitle = value;
                courseTitle.Text = value;
            }
        }

        public event EventHandler viewCourseBTNClick
        {
            add { viewCourseBTN.Click += value; }
            remove { viewCourseBTN.Click -= value; }
        }
        public void OpenBrowser(string link)
        {
            try
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = link,
                    UseShellExecute = true
                });
            }
            catch (Win32Exception ex)
            {
                Console.WriteLine($"Error opening browser: {ex.Message}");
            }
        }
    }
}
