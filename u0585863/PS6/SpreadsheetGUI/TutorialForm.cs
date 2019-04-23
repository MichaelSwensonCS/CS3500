/*
 * U0585863
 * Author: Michael Swenson
 * 10/18/18
 * CS3500
 */

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Small Functionality form that describes my extra features
    /// </summary>
    public partial class TutorialForm : Form
    {
        public TutorialForm()
        {
            InitializeComponent();
            tutorialDescTxtBx.Text = "Hello, My extra features are the red text option, " +
                "this form itself(closes when main form closes)," +
                "if you open a new spreadsheet a speech synthesizer in microsoft anna's voice will read a message," +
                " and a console game I made if you click the link below";
        }

        /// <summary>
        /// A little game I made that demonstrates some of the cool stuff you can do with
        /// C# consoles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectFourLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("ConnectFour.exe");
        }


    }
}
