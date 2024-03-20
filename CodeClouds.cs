using Guna.UI2.WinForms;

namespace Coders_Space
{
    public partial class CodeClouds : Form
    {

        private int currentLabelIndex = 0;
        private String[] txts = { "#Create", "#Contribute", "#Store", "#Share" };

        private Guna2Button selectedButton;

        public CodeClouds()
        {
            InitializeComponent();
            InitWebView();
            selectedButton = buttonGitHub;
            UpdateButtonColors();

            label1.Text = txts[currentLabelIndex];
        }

        void timer_Tick(object sender, EventArgs e)
        {
            currentLabelIndex = (currentLabelIndex + 1) % txts.Length;
            label1.Text = txts[currentLabelIndex];
        }



        async void InitWebView()
        {
            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.Navigate("https://github.com/");

        }

        private void UpdateButtonColors()
        {

            foreach (Guna2Button button in guna2Panel2.Controls.OfType<Guna2Button>())
            {
                if (button != selectedButton)
                {
                    button.BackColor = Color.Transparent;
                    button.FillColor = Color.Transparent;
                }
                else
                {
                    button.BorderRadius = 15;
                    button.FillColor = Color.FromArgb(71, 207, 115);
                    button.BackColor = Color.Transparent;
                }
            }
        }

        private void buttonGitHub_Click(object sender, EventArgs e)
        {
            selectedButton = (Guna2Button)sender;
            UpdateButtonColors();
            webView21.CoreWebView2.Navigate("https://github.com/");
        }

        private void buttonGitLab_Click(object sender, EventArgs e)
        {
            selectedButton = (Guna2Button)sender;
            UpdateButtonColors();
            webView21.CoreWebView2.Navigate("https://gitlab.com/");
        }

        private void buttonBitBucket_Click(object sender, EventArgs e)
        {
            selectedButton = (Guna2Button)sender;
            UpdateButtonColors();
            webView21.CoreWebView2.Navigate("https://bitbucket.org/");
        }

        private void buttonCodePen_Click(object sender, EventArgs e)
        {
            selectedButton = (Guna2Button)sender;
            UpdateButtonColors();
            webView21.CoreWebView2.Navigate("https://codepen.io/");
        }

        private void buttonReplIt_Click(object sender, EventArgs e)
        {
            selectedButton = (Guna2Button)sender;
            UpdateButtonColors();
            webView21.CoreWebView2.Navigate("https://repl.it/");
        }

        private void buttonGist_Click(object sender, EventArgs e)
        {
            selectedButton = (Guna2Button)sender;
            UpdateButtonColors();
            webView21.CoreWebView2.Navigate("https://gist.github.com/");
        }

        private void buttonPasteCode_Click(object sender, EventArgs e)
        {
            selectedButton = (Guna2Button)sender;
            UpdateButtonColors();
            webView21.CoreWebView2.Navigate("https://pastecode.io/");
        }

        private void buttonJSFiddle_Click(object sender, EventArgs e)
        {
            selectedButton = (Guna2Button)sender;
            UpdateButtonColors();
            webView21.CoreWebView2.Navigate("https://jsfiddle.net/");
        }

        private void buttonCodeSandbox_Click(object sender, EventArgs e)
        {
            selectedButton = (Guna2Button)sender;
            UpdateButtonColors();
            webView21.CoreWebView2.Navigate("https://codesandbox.io/");
        }
    }
}
