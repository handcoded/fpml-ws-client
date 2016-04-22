using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ServiceTest
{
    public partial class ResponseIdForm : Form
    {
        public string ResponseId
        {
            get
            {
                return (responseIdTextBox.Text);
            }
        }

        public ResponseIdForm ()
        {
            InitializeComponent ();
        }

        private void responseIdTextBox_TextChanged (object sender, EventArgs e)
        {
            acceptButton.Enabled = responseIdTextBox.Text.Length > 0;
        }
    }
}
