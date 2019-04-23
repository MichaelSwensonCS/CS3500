namespace Form1
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
            this.connectControlsPnl = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.playerNameTxtBx = new System.Windows.Forms.TextBox();
            this.ipInfoTxtBx = new System.Windows.Forms.TextBox();
            this.connectBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelGameHolder = new System.Windows.Forms.Panel();
            this.connectControlsPnl.SuspendLayout();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectControlsPnl
            // 
            this.connectControlsPnl.Controls.Add(this.label2);
            this.connectControlsPnl.Controls.Add(this.label1);
            this.connectControlsPnl.Controls.Add(this.playerNameTxtBx);
            this.connectControlsPnl.Controls.Add(this.ipInfoTxtBx);
            this.connectControlsPnl.Controls.Add(this.connectBtn);
            this.connectControlsPnl.Location = new System.Drawing.Point(13, 12);
            this.connectControlsPnl.Name = "connectControlsPnl";
            this.connectControlsPnl.Size = new System.Drawing.Size(583, 29);
            this.connectControlsPnl.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(260, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Server: ";
            // 
            // playerNameTxtBx
            // 
            this.playerNameTxtBx.Location = new System.Drawing.Point(304, 3);
            this.playerNameTxtBx.Name = "playerNameTxtBx";
            this.playerNameTxtBx.Size = new System.Drawing.Size(160, 20);
            this.playerNameTxtBx.TabIndex = 0;
            this.playerNameTxtBx.TabStop = false;
            this.playerNameTxtBx.Text = "Default";
            // 
            // ipInfoTxtBx
            // 
            this.ipInfoTxtBx.Location = new System.Drawing.Point(69, 3);
            this.ipInfoTxtBx.Name = "ipInfoTxtBx";
            this.ipInfoTxtBx.Size = new System.Drawing.Size(166, 20);
            this.ipInfoTxtBx.TabIndex = 0;
            this.ipInfoTxtBx.TabStop = false;
            this.ipInfoTxtBx.Text = "localHost";
            // 
            // connectBtn
            // 
            this.connectBtn.Location = new System.Drawing.Point(481, 3);
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Size = new System.Drawing.Size(75, 23);
            this.connectBtn.TabIndex = 0;
            this.connectBtn.TabStop = false;
            this.connectBtn.Text = "Connect";
            this.connectBtn.UseVisualStyleBackColor = true;
            this.connectBtn.Click += new System.EventHandler(this.connectBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Location = new System.Drawing.Point(614, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(174, 29);
            this.panel1.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(174, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.controlsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // controlsToolStripMenuItem
            // 
            this.controlsToolStripMenuItem.Name = "controlsToolStripMenuItem";
            this.controlsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.controlsToolStripMenuItem.Text = "Controls";
            this.controlsToolStripMenuItem.Click += new System.EventHandler(this.controlsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // panelGameHolder
            // 
            this.panelGameHolder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelGameHolder.Location = new System.Drawing.Point(13, 48);
            this.panelGameHolder.Margin = new System.Windows.Forms.Padding(0);
            this.panelGameHolder.Name = "panelGameHolder";
            this.panelGameHolder.Size = new System.Drawing.Size(750, 750);
            this.panelGameHolder.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(984, 862);
            this.Controls.Add(this.panelGameHolder);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.connectControlsPnl);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1000, 900);
            this.Name = "Form1";
            this.Text = "PS7";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.connectControlsPnl.ResumeLayout(false);
            this.connectControlsPnl.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel connectControlsPnl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox playerNameTxtBx;
        private System.Windows.Forms.TextBox ipInfoTxtBx;
        private System.Windows.Forms.Button connectBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem controlsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Panel panelGameHolder;
    }
}

