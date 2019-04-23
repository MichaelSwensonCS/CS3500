using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TipCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CalculateTip_Click(object sender, EventArgs e)
        {
            if (TotalAmount.Text != "")
            {
                string tipResult = TotalAmount.Text;
                string tipAmount = TipPercent.Text;
                double dblTipResult;
                double tipPercentage;

                if (Double.TryParse(tipAmount, out tipPercentage))
                {
                    if (Double.TryParse(tipResult, out dblTipResult))
                    {
                        dblTipResult = dblTipResult * (tipPercentage/10);
                        OutputTip.Text = dblTipResult + "";
                    }
                    else
                    {
                        MessageBox.Show("You need to input a number here");
                    }
                }
                else
                {
                    MessageBox.Show("The total bill is empty, please add a number");
                }
            }
            else
            {
                MessageBox.Show("Tip Percentage is not a number");
            }
        }

        private void TotalAmount_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
