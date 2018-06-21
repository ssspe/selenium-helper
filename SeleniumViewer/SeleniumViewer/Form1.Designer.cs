namespace WindowsFormsApplication1
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.metroButton2 = new MetroFramework.Controls.MetroButton();
            this.navigationBar = new MetroFramework.Controls.MetroTextBox();
            this.textEditorButton = new MetroFramework.Controls.MetroButton();
            this.languageSelection = new MetroFramework.Controls.MetroComboBox();
            this.seleniumList = new MetroFramework.Controls.MetroListView();
            this.metroContextMenu1 = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.navigateToggle = new MetroFramework.Controls.MetroToggle();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(12, 63);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(578, 483);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser1_Navigated);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(12, 63);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(578, 483);
            this.richTextBox1.TabIndex = 7;
            this.richTextBox1.Text = "";
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(0, 0);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(75, 23);
            this.metroButton1.TabIndex = 0;
            this.metroButton1.UseSelectable = true;
            // 
            // metroButton2
            // 
            this.metroButton2.Location = new System.Drawing.Point(0, 0);
            this.metroButton2.Name = "metroButton2";
            this.metroButton2.Size = new System.Drawing.Size(75, 23);
            this.metroButton2.TabIndex = 0;
            this.metroButton2.UseSelectable = true;
            // 
            // navigationBar
            // 
            this.navigationBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.navigationBar.CustomButton.Image = null;
            this.navigationBar.CustomButton.Location = new System.Drawing.Point(459, 1);
            this.navigationBar.CustomButton.Name = "";
            this.navigationBar.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.navigationBar.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.navigationBar.CustomButton.TabIndex = 1;
            this.navigationBar.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.navigationBar.CustomButton.UseSelectable = true;
            this.navigationBar.CustomButton.Visible = false;
            this.navigationBar.Lines = new string[0];
            this.navigationBar.Location = new System.Drawing.Point(109, 25);
            this.navigationBar.MaxLength = 32767;
            this.navigationBar.Name = "navigationBar";
            this.navigationBar.PasswordChar = '\0';
            this.navigationBar.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.navigationBar.SelectedText = "";
            this.navigationBar.SelectionLength = 0;
            this.navigationBar.SelectionStart = 0;
            this.navigationBar.ShortcutsEnabled = true;
            this.navigationBar.Size = new System.Drawing.Size(481, 23);
            this.navigationBar.TabIndex = 8;
            this.navigationBar.UseSelectable = true;
            this.navigationBar.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.navigationBar.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.navigationBar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.navigationBar_KeyDown);
            // 
            // textEditorButton
            // 
            this.textEditorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textEditorButton.Location = new System.Drawing.Point(610, 25);
            this.textEditorButton.Name = "textEditorButton";
            this.textEditorButton.Size = new System.Drawing.Size(187, 23);
            this.textEditorButton.TabIndex = 9;
            this.textEditorButton.Text = "Text Editor";
            this.textEditorButton.UseSelectable = true;
            this.textEditorButton.Click += new System.EventHandler(this.textEditorButton_Click);
            // 
            // languageSelection
            // 
            this.languageSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.languageSelection.FormattingEnabled = true;
            this.languageSelection.ItemHeight = 23;
            this.languageSelection.Location = new System.Drawing.Point(610, 517);
            this.languageSelection.Name = "languageSelection";
            this.languageSelection.Size = new System.Drawing.Size(187, 29);
            this.languageSelection.TabIndex = 10;
            this.languageSelection.UseSelectable = true;
            this.languageSelection.TextChanged += new System.EventHandler(this.languageSelection_TextChanged);
            // 
            // seleniumList
            // 
            this.seleniumList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seleniumList.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.seleniumList.FullRowSelect = true;
            this.seleniumList.Location = new System.Drawing.Point(610, 63);
            this.seleniumList.Name = "seleniumList";
            this.seleniumList.OwnerDraw = true;
            this.seleniumList.Size = new System.Drawing.Size(187, 439);
            this.seleniumList.TabIndex = 11;
            this.seleniumList.UseCompatibleStateImageBehavior = false;
            this.seleniumList.UseSelectable = true;
            this.seleniumList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.seleniumList_MouseDoubleClick);
            // 
            // metroContextMenu1
            // 
            this.metroContextMenu1.Name = "metroContextMenu1";
            this.metroContextMenu1.Size = new System.Drawing.Size(61, 4);
            // 
            // navigateToggle
            // 
            this.navigateToggle.Location = new System.Drawing.Point(12, 25);
            this.navigateToggle.Name = "navigateToggle";
            this.navigateToggle.Size = new System.Drawing.Size(80, 23);
            this.navigateToggle.TabIndex = 13;
            this.navigateToggle.Text = "Off";
            this.navigateToggle.UseSelectable = true;
            this.navigateToggle.CheckedChanged += new System.EventHandler(this.metroToggle1_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 556);
            this.Controls.Add(this.navigateToggle);
            this.Controls.Add(this.seleniumList);
            this.Controls.Add(this.languageSelection);
            this.Controls.Add(this.textEditorButton);
            this.Controls.Add(this.navigationBar);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.webBrowser1);
            this.MinimumSize = new System.Drawing.Size(809, 556);
            this.Name = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private MetroFramework.Controls.MetroButton metroButton1;
        private MetroFramework.Controls.MetroButton metroButton2;
        private MetroFramework.Controls.MetroTextBox navigationBar;
        private MetroFramework.Controls.MetroButton textEditorButton;
        private MetroFramework.Controls.MetroComboBox languageSelection;
        private MetroFramework.Controls.MetroListView seleniumList;
        private MetroFramework.Controls.MetroContextMenu metroContextMenu1;
        private MetroFramework.Controls.MetroToggle navigateToggle;
    }
}

