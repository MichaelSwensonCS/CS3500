namespace SpreadsheetGUI
{
    partial class TutorialForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tutorialDescTxtBx = new System.Windows.Forms.TextBox();
            this.ConnectFourLink = new System.Windows.Forms.LinkLabel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(12, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 268);
            this.panel1.TabIndex = 4;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::SpreadsheetGUI.Properties.Resources.Slide2;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 268);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tutorialDescTxtBx
            // 
            this.tutorialDescTxtBx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tutorialDescTxtBx.Enabled = false;
            this.tutorialDescTxtBx.Location = new System.Drawing.Point(13, 303);
            this.tutorialDescTxtBx.Multiline = true;
            this.tutorialDescTxtBx.Name = "tutorialDescTxtBx";
            this.tutorialDescTxtBx.Size = new System.Drawing.Size(799, 65);
            this.tutorialDescTxtBx.TabIndex = 5;
            // 
            // ConnectFourLink
            // 
            this.ConnectFourLink.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ConnectFourLink.AutoSize = true;
            this.ConnectFourLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectFourLink.Location = new System.Drawing.Point(308, 422);
            this.ConnectFourLink.Name = "ConnectFourLink";
            this.ConnectFourLink.Size = new System.Drawing.Size(210, 24);
            this.ConnectFourLink.TabIndex = 6;
            this.ConnectFourLink.TabStop = true;
            this.ConnectFourLink.Text = "Want to play a game?";
            this.ConnectFourLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ConnectFourLink_LinkClicked);
            // 
            // TutorialForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(824, 471);
            this.Controls.Add(this.ConnectFourLink);
            this.Controls.Add(this.tutorialDescTxtBx);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TutorialForm";
            this.Text = "TutorialForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox tutorialDescTxtBx;
        private System.Windows.Forms.LinkLabel ConnectFourLink;
    }
}