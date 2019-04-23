namespace TipCalculator
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
            this.Title = new System.Windows.Forms.Label();
            this.TotalAmount = new System.Windows.Forms.TextBox();
            this.OutputTip = new System.Windows.Forms.TextBox();
            this.CalculateTip = new System.Windows.Forms.Button();
            this.TipPercentLbl = new System.Windows.Forms.Label();
            this.TipPercent = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Location = new System.Drawing.Point(52, 55);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(47, 13);
            this.Title.TabIndex = 0;
            this.Title.Text = "Total Bill";
            // 
            // TotalAmount
            // 
            this.TotalAmount.Location = new System.Drawing.Point(129, 52);
            this.TotalAmount.Name = "TotalAmount";
            this.TotalAmount.Size = new System.Drawing.Size(100, 20);
            this.TotalAmount.TabIndex = 1;
            this.TotalAmount.TextChanged += new System.EventHandler(this.TotalAmount_TextChanged);
            // 
            // OutputTip
            // 
            this.OutputTip.Location = new System.Drawing.Point(129, 112);
            this.OutputTip.Name = "OutputTip";
            this.OutputTip.Size = new System.Drawing.Size(100, 20);
            this.OutputTip.TabIndex = 2;
            // 
            // CalculateTip
            // 
            this.CalculateTip.BackColor = System.Drawing.SystemColors.Info;
            this.CalculateTip.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CalculateTip.Location = new System.Drawing.Point(245, 52);
            this.CalculateTip.Name = "CalculateTip";
            this.CalculateTip.Size = new System.Drawing.Size(173, 80);
            this.CalculateTip.TabIndex = 3;
            this.CalculateTip.Text = "CalculateTip";
            this.CalculateTip.UseVisualStyleBackColor = false;
            this.CalculateTip.Click += new System.EventHandler(this.CalculateTip_Click);
            // 
            // TipPercentLbl
            // 
            this.TipPercentLbl.AutoSize = true;
            this.TipPercentLbl.Location = new System.Drawing.Point(31, 193);
            this.TipPercentLbl.Name = "TipPercentLbl";
            this.TipPercentLbl.Size = new System.Drawing.Size(80, 13);
            this.TipPercentLbl.TabIndex = 4;
            this.TipPercentLbl.Text = "Tip Percentage";
            // 
            // TipPercent
            // 
            this.TipPercent.Location = new System.Drawing.Point(129, 190);
            this.TipPercent.Name = "TipPercent";
            this.TipPercent.Size = new System.Drawing.Size(100, 20);
            this.TipPercent.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TipPercent);
            this.Controls.Add(this.TipPercentLbl);
            this.Controls.Add(this.CalculateTip);
            this.Controls.Add(this.OutputTip);
            this.Controls.Add(this.TotalAmount);
            this.Controls.Add(this.Title);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.TextBox TotalAmount;
        private System.Windows.Forms.TextBox OutputTip;
        private System.Windows.Forms.Button CalculateTip;
        private System.Windows.Forms.Label TipPercentLbl;
        private System.Windows.Forms.TextBox TipPercent;
    }
}

