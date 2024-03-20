namespace Coders_Space
{
    partial class ListCourses
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            picture = new PictureBox();
            courseTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            viewCourseBTN = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)picture).BeginInit();
            SuspendLayout();
            // 
            // picture
            // 
            picture.BackColor = Color.Transparent;
            picture.Image = Properties.Resources.no_image_avaiable;
            picture.Location = new Point(3, 5);
            picture.Name = "picture";
            picture.Size = new Size(184, 125);
            picture.SizeMode = PictureBoxSizeMode.Zoom;
            picture.TabIndex = 6;
            picture.TabStop = false;
            // 
            // courseTitle
            // 
            courseTitle.BackColor = Color.Transparent;
            courseTitle.Font = new Font("Times New Roman", 14.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            courseTitle.Location = new Point(13, 142);
            courseTitle.Name = "courseTitle";
            courseTitle.Size = new Size(166, 24);
            courseTitle.TabIndex = 7;
            courseTitle.Text = "Complete C# Course";
            // 
            // viewCourseBTN
            // 
            viewCourseBTN.BorderRadius = 20;
            viewCourseBTN.CustomizableEdges = customizableEdges1;
            viewCourseBTN.DisabledState.BorderColor = Color.DarkGray;
            viewCourseBTN.DisabledState.CustomBorderColor = Color.DarkGray;
            viewCourseBTN.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            viewCourseBTN.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            viewCourseBTN.FillColor = Color.FromArgb(0, 64, 221);
            viewCourseBTN.Font = new Font("Times New Roman", 14.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            viewCourseBTN.ForeColor = Color.White;
            viewCourseBTN.Location = new Point(6, 174);
            viewCourseBTN.Name = "viewCourseBTN";
            viewCourseBTN.ShadowDecoration.CustomizableEdges = customizableEdges2;
            viewCourseBTN.Size = new Size(180, 45);
            viewCourseBTN.TabIndex = 8;
            viewCourseBTN.Text = "View Course";
            // 
            // ListCourses
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            Controls.Add(picture);
            Controls.Add(courseTitle);
            Controls.Add(viewCourseBTN);
            Name = "ListCourses";
            Size = new Size(190, 225);
            ((System.ComponentModel.ISupportInitialize)picture).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picture;
        private Guna.UI2.WinForms.Guna2HtmlLabel courseTitle;
        private Guna.UI2.WinForms.Guna2Button viewCourseBTN;
    }
}
