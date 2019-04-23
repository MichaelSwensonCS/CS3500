namespace SpreadsheetGUI
{
    partial class SpreadSheetForm
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
            this.LayoutPanel = new System.Windows.Forms.Panel();
            this.InfoDisplayPanel = new System.Windows.Forms.Panel();
            this.checkBoxRed = new System.Windows.Forms.CheckBox();
            this.setButton = new System.Windows.Forms.Button();
            this.ContentsTxtBx = new System.Windows.Forms.TextBox();
            this.ValueTxtBx = new System.Windows.Forms.TextBox();
            this.SelectedTxtBx = new System.Windows.Forms.TextBox();
            this.ContentsLabel = new System.Windows.Forms.Label();
            this.ValueLabel = new System.Windows.Forms.Label();
            this.SelectedLabel = new System.Windows.Forms.Label();
            this.ssPanel = new System.Windows.Forms.Panel();
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extraFeatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.LayoutPanel.SuspendLayout();
            this.InfoDisplayPanel.SuspendLayout();
            this.ssPanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LayoutPanel
            // 
            this.LayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LayoutPanel.Controls.Add(this.InfoDisplayPanel);
            this.LayoutPanel.Controls.Add(this.ssPanel);
            this.LayoutPanel.Location = new System.Drawing.Point(13, 34);
            this.LayoutPanel.Name = "LayoutPanel";
            this.LayoutPanel.Size = new System.Drawing.Size(1075, 467);
            this.LayoutPanel.TabIndex = 0;
            // 
            // InfoDisplayPanel
            // 
            this.InfoDisplayPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoDisplayPanel.BackColor = System.Drawing.Color.White;
            this.InfoDisplayPanel.Controls.Add(this.checkBoxRed);
            this.InfoDisplayPanel.Controls.Add(this.setButton);
            this.InfoDisplayPanel.Controls.Add(this.ContentsTxtBx);
            this.InfoDisplayPanel.Controls.Add(this.ValueTxtBx);
            this.InfoDisplayPanel.Controls.Add(this.SelectedTxtBx);
            this.InfoDisplayPanel.Controls.Add(this.ContentsLabel);
            this.InfoDisplayPanel.Controls.Add(this.ValueLabel);
            this.InfoDisplayPanel.Controls.Add(this.SelectedLabel);
            this.InfoDisplayPanel.Location = new System.Drawing.Point(0, 3);
            this.InfoDisplayPanel.Name = "InfoDisplayPanel";
            this.InfoDisplayPanel.Size = new System.Drawing.Size(1075, 64);
            this.InfoDisplayPanel.TabIndex = 1;
            // 
            // checkBoxRed
            // 
            this.checkBoxRed.AutoSize = true;
            this.checkBoxRed.Location = new System.Drawing.Point(925, 28);
            this.checkBoxRed.Name = "checkBoxRed";
            this.checkBoxRed.Size = new System.Drawing.Size(89, 17);
            this.checkBoxRed.TabIndex = 7;
            this.checkBoxRed.Text = "Set Text Red";
            this.checkBoxRed.UseVisualStyleBackColor = true;
            // 
            // setButton
            // 
            this.setButton.Location = new System.Drawing.Point(738, 23);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(135, 23);
            this.setButton.TabIndex = 6;
            this.setButton.Text = "Set Contents";
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.SetButton_Click);
            // 
            // ContentsTxtBx
            // 
            this.ContentsTxtBx.Location = new System.Drawing.Point(420, 25);
            this.ContentsTxtBx.Name = "ContentsTxtBx";
            this.ContentsTxtBx.Size = new System.Drawing.Size(283, 20);
            this.ContentsTxtBx.TabIndex = 5;
            // 
            // ValueTxtBx
            // 
            this.ValueTxtBx.Location = new System.Drawing.Point(234, 25);
            this.ValueTxtBx.Name = "ValueTxtBx";
            this.ValueTxtBx.ReadOnly = true;
            this.ValueTxtBx.Size = new System.Drawing.Size(100, 20);
            this.ValueTxtBx.TabIndex = 4;
            // 
            // SelectedTxtBx
            // 
            this.SelectedTxtBx.Location = new System.Drawing.Point(73, 25);
            this.SelectedTxtBx.Name = "SelectedTxtBx";
            this.SelectedTxtBx.ReadOnly = true;
            this.SelectedTxtBx.Size = new System.Drawing.Size(100, 20);
            this.SelectedTxtBx.TabIndex = 3;
            // 
            // ContentsLabel
            // 
            this.ContentsLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ContentsLabel.AutoSize = true;
            this.ContentsLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ContentsLabel.Location = new System.Drawing.Point(363, 25);
            this.ContentsLabel.Name = "ContentsLabel";
            this.ContentsLabel.Size = new System.Drawing.Size(51, 15);
            this.ContentsLabel.TabIndex = 2;
            this.ContentsLabel.Text = "Contents";
            // 
            // ValueLabel
            // 
            this.ValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ValueLabel.AutoSize = true;
            this.ValueLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ValueLabel.Location = new System.Drawing.Point(192, 25);
            this.ValueLabel.Name = "ValueLabel";
            this.ValueLabel.Size = new System.Drawing.Size(36, 15);
            this.ValueLabel.TabIndex = 1;
            this.ValueLabel.Text = "Value";
            // 
            // SelectedLabel
            // 
            this.SelectedLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.SelectedLabel.AutoSize = true;
            this.SelectedLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SelectedLabel.Location = new System.Drawing.Point(16, 25);
            this.SelectedLabel.Name = "SelectedLabel";
            this.SelectedLabel.Size = new System.Drawing.Size(51, 15);
            this.SelectedLabel.TabIndex = 0;
            this.SelectedLabel.Text = "Selected";
            // 
            // ssPanel
            // 
            this.ssPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ssPanel.Controls.Add(this.spreadsheetPanel1);
            this.ssPanel.Location = new System.Drawing.Point(0, 73);
            this.ssPanel.Name = "ssPanel";
            this.ssPanel.Size = new System.Drawing.Size(1072, 401);
            this.ssPanel.TabIndex = 0;
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.Location = new System.Drawing.Point(4, 7);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(1068, 378);
            this.spreadsheetPanel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1100, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extraFeatureToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // extraFeatureToolStripMenuItem
            // 
            this.extraFeatureToolStripMenuItem.Name = "extraFeatureToolStripMenuItem";
            this.extraFeatureToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.extraFeatureToolStripMenuItem.Text = "Extra Feature";
            this.extraFeatureToolStripMenuItem.Click += new System.EventHandler(this.ExtraFeatureToolStripMenuItem_Click);
            // 
            // SpreadSheetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 513);
            this.Controls.Add(this.LayoutPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SpreadSheetForm";
            this.Text = "SPREADSHEET BIG SUPER FUN TIME !";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpreadSheetForm_FormClosing);
            this.LayoutPanel.ResumeLayout(false);
            this.InfoDisplayPanel.ResumeLayout(false);
            this.InfoDisplayPanel.PerformLayout();
            this.ssPanel.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel LayoutPanel;
        private System.Windows.Forms.Panel InfoDisplayPanel;
        private System.Windows.Forms.Label SelectedLabel;
        private System.Windows.Forms.Panel ssPanel;
        private System.Windows.Forms.Label ContentsLabel;
        private System.Windows.Forms.Label ValueLabel;
        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.TextBox SelectedTxtBx;
        private System.Windows.Forms.TextBox ContentsTxtBx;
        private System.Windows.Forms.TextBox ValueTxtBx;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extraFeatureToolStripMenuItem;
        private System.Windows.Forms.Button setButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox checkBoxRed;
    }
}

