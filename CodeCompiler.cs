using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coders_Space
{
    public partial class CodeCompiler : Form
    {
        private Guna2CircleButton selectedButton;
        public CodeCompiler()
        {
            InitializeComponent();
            InitWebView();
            selectedButton = buttonCPP;
            UpdateButtonColors();
        }
        async void InitWebView()
        {
            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.Navigate("https://www.programiz.com/csharp-programming/online-compiler/");

        }

        private void UpdateButtonColors()
        {
            foreach (Guna2CircleButton button in webView21.Controls.OfType<Guna2CircleButton>())
            {
                if (button != selectedButton)
                {
                    button.BackColor = Color.Transparent;
                    button.FillColor = Color.Transparent;
                }
                else
                {
                    button.FillColor = Color.FromArgb(0, 64, 221);
                }
            }
        }




        private void buttonCPP_Click(object sender, EventArgs e)
        {
            selectedButton = (Guna2CircleButton)sender;
            UpdateButtonColors();
            webView21.CoreWebView2.Navigate("https://www.programiz.com/csharp-programming/online-compiler/");
        }

        private void buttonCSharp_Click(object sender, EventArgs e)
        {
            selectedButton = (Guna2CircleButton)sender;
            UpdateButtonColors();
            webView21.CoreWebView2.Navigate("https://www.programiz.com/cpp-programming/online-compiler/");
        }

        private void buttonJava_Click(object sender, EventArgs e)
        {
            selectedButton = (Guna2CircleButton)sender;
            UpdateButtonColors();
            webView21.CoreWebView2.Navigate("https://www.programiz.com/java-programming/online-compiler/");
        }

        private void buttonPython_Click(object sender, EventArgs e)
        {
            selectedButton = (Guna2CircleButton)sender;
            UpdateButtonColors();
            webView21.CoreWebView2.Navigate("https://www.programiz.com/python-programming/online-compiler/");
        }

        private void buttonHTML_Click(object sender, EventArgs e)
        {
            selectedButton = (Guna2CircleButton)sender;
            UpdateButtonColors();
            webView21.CoreWebView2.Navigate("https://www.programiz.com/html/online-compiler/");
        }

        private void buttonJS_Click(object sender, EventArgs e)
        {
            selectedButton = (Guna2CircleButton)sender;
            UpdateButtonColors();
            webView21.CoreWebView2.Navigate("https://www.programiz.com/javascript/online-compiler/");
        }

        private void buttonPHP_Click(object sender, EventArgs e)
        {
            selectedButton = (Guna2CircleButton)sender;
            UpdateButtonColors();
            webView21.CoreWebView2.Navigate("https://www.programiz.com/php/online-compiler/");
        }
    }
}
