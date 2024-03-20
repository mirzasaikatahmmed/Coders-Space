using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coders_Space
{
    public partial class ListJobs : UserControl
    {
        public ListJobs()
        {
            InitializeComponent();
        }
        public Image CompanyLogo
        {
            get { return companyLogo.Image; }
            set { companyLogo.Image = value; }
        }

        public string JobTitle
        {
            get { return labelJobTitle.Text; }
            set { labelJobTitle.Text = value; }
        }

        public string CompanyName
        {
            get { return labelCompanyName.Text; }
            set { labelCompanyName.Text = value; }
        }

        public string Address
        {
            get { return labelAddress.Text; }
            set { labelAddress.Text = value; }
        }

        public string Salary
        {
            get { return labelSalary.Text; }
            set { labelSalary.Text = value; }
        }

        public string Benefits
        {
            get { return labelBenefits.Text; }
            set { labelBenefits.Text = value; }
        }

        public DateTime ApplyLastDate
        {
            get { return DateTime.ParseExact(labelDeadLine.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture); }
            set 
            { 
                labelDeadLine.Text = value.ToString("dd/MM/yyyy");
                applyNowBtn.Enabled = DateTime.Now < value;
            }
        }

        public event EventHandler ApplyNowButtonClick
        {
            add { applyNowBtn.Click += value; }
            remove { applyNowBtn.Click -= value; }
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
