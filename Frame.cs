//==============================================================================
//  _____      __  __ _                               
// |  ___| __ |  \/  | |                              
// | |_ | '_ \| |\/| | |                              
// |  _|| |_) | |  | | |___                           
// |_|__| .__/|_|  |_|_____|        _____         _   
// / ___|_|___ _ ____   _(_) ___ __|_   _|__  ___| |_ 
// \___ \ / _ \ '__\ \ / / |/ __/ _ \| |/ _ \/ __| __|
//  ___) |  __/ |   \ V /| | (_|  __/| |  __/\__ \ |_ 
// |____/ \___|_|    \_/ |_|\___\___||_|\___||___/\__|
//                                                                                                               
// An FpML WebServices Test Application
//==============================================================================
// Copyright (C)2014-2016 ISDA and HandCoded Software Ltd.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
//
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR AND CONTRIBUTORS ''AS IS'' AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
// OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
// OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
// SUCH DAMAGE.
//
// This work is licensed under a Creative Commons Attribution 4.0 International
// License. See http://creativecommons.org/licenses/by/4.0/ for details.
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ServiceTest.ServiceReference;

namespace ServiceTest
{
    public partial class Frame : Form
    {
        public Frame ()
        {
            InitializeComponent ();

            SetMenuState ();
        }

        /// <summary>
        /// Form used to get session connection parameters.
        /// </summary>
        private StartSessionForm startSessionForm
            = new StartSessionForm ();

        private ResponseIdForm responseIdForm
            = new ResponseIdForm ();

        /// <summary>
        /// The client side of the webservice interface.
        /// </summary>
        private  WebInterfaceSoapClient client = null;

        /// <summary>
        /// Set the meni item states to match the state of the connection.
        /// </summary>
        private void SetMenuState ()
        {
            bool connected = (client != null);

            startSessionToolStripMenuItem.Enabled = !connected;

            endSessionToolStripMenuItem.Enabled = connected;
            retrieveNextNotificationToolStripMenuItem.Enabled = connected;
            submitMessageToolStripMenuItem.Enabled = connected;
            retrieveNextResponseToolStripMenuItem.Enabled = connected;
            retrieveReponseByIDToolStripMenuItem.Enabled = connected;
            processMessageToolStripMenuItem.Enabled = connected;
        }

        /// <summary>
        /// Reads the entire contents of a <see cref="Stream"/> and returns it
        /// as a string.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to be read.</param>
        /// <returns>The entire contents as a string.</returns>
        private string ReadFileContent (Stream stream)
        {
            TextReader reader = new StreamReader (stream);
            StringBuilder buffer = new StringBuilder ();
            String line;

            while ((line = reader.ReadLine ()) != null)
                buffer.AppendLine (line);
            reader.Close ();

            return (buffer.ToString ());
        }

        /// <summary>
        /// Handle the File/Exit menu item.
        /// </summary>
        /// <param name="sender">The object raising the event.</param>
        /// <param name="e">The event parameters.</param>
        private void exitToolStripMenuItem_Click (object sender, EventArgs e)
        {
            Close ();
        }

        private void copyToolStripMenuItem_Click (object sender, EventArgs e)
        {
            Clipboard.SetText (richTextBox.SelectedText);
        }

        private void richTextBox_SelectionChanged (object sender, EventArgs e)
        {
            copyToolStripMenuItem.Enabled = richTextBox.SelectedText.Length != 0;
        }

        private void startSessionToolStripMenuItem_Click (object sender, EventArgs e)
        {
            if (startSessionForm.ShowDialog (this) == DialogResult.OK) {
                WebInterfaceSoapClient temp = new WebInterfaceSoapClient ();

                statusLabel.Text = "Connecting";
                Refresh ();

                try {
                    if (temp.StartSession (startSessionForm.Username, startSessionForm.Password))
                        client = temp;

                    statusLabel.Text = "Ready";
                }
                catch (Exception error) {
                    statusLabel.Text = "Error";
                    MessageBox.Show (error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            SetMenuState ();
        }

        private void endSessionToolStripMenuItem_Click (object sender, EventArgs e)
        {
            statusLabel.Text = "Connecting";
            Refresh ();

            try {
                client.EndSession ();

                statusLabel.Text = "Ready";
            }
            catch (Exception error) {
                statusLabel.Text = "Error";

                MessageBox.Show (error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                client = null;
            }

            SetMenuState ();
        }

        private void retrieveNextNotificationToolStripMenuItem_Click (object sender, EventArgs e)
        {
            statusLabel.Text = "Connecting";
            Refresh ();

            try {
                string result = client.RetrieveNotificationMessage (false);

                if (result != null)
                    richTextBox.Lines = result.Split (new char [] { '\n' });
                else {
                    richTextBox.Clear ();

                    MessageBox.Show (this, "No more notification messages", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                statusLabel.Text = "Ready";
            }
            catch (Exception error) {
                statusLabel.Text = "Error";

                MessageBox.Show (error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

 
        private void submitMessageToolStripMenuItem_Click (object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog (this) == DialogResult.OK) {
                String payload = ReadFileContent (openFileDialog.OpenFile ());

                statusLabel.Text = "Connecting";
                Refresh ();

                try {
                    string result = client.SubmitMessage (payload);

                    if (result != null)
                        richTextBox.Text = result;
                    else
                        richTextBox.Clear ();

                    statusLabel.Text = "Ready";
                }
                catch (Exception error) {
                    statusLabel.Text = "Error";

                    MessageBox.Show (error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void retrieveNextResponseToolStripMenuItem_Click (object sender, EventArgs e)
        {
            statusLabel.Text = "Connecting";
            Refresh ();

            try {
                string result = client.RetrieveMessage (false);

                if (result != null)
                    richTextBox.Lines = result.Split (new char [] { '\n' });
                else {
                    richTextBox.Clear ();

                    MessageBox.Show (this, "No more response messages", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception error) {
                statusLabel.Text = "Error";

                MessageBox.Show (error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void retrieveReponseByIDToolStripMenuItem_Click (object sender, EventArgs e)
        {
            if (responseIdForm.ShowDialog (this) == DialogResult.OK) {
                string responseId = responseIdForm.ResponseId;

                statusLabel.Text = "Connecting";
                Refresh ();

                try {
                    string result = client.RetrieveResponseMessage (responseId, false);

                    if (result != null)
                        richTextBox.Lines = result.Split (new char [] { '\n' });
                    else {
                        richTextBox.Clear ();

                        MessageBox.Show (this, "No such response message", "Information",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    statusLabel.Text = "Ready";
                }
                catch (Exception error) {
                    statusLabel.Text = "Error";

                    MessageBox.Show (error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void processMessageToolStripMenuItem_Click (object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog (this) == DialogResult.OK) {
                String payload = ReadFileContent (openFileDialog.OpenFile ());

                statusLabel.Text = "Connecting";
                Refresh ();

                try {
                    string result = client.ProcessMessage (payload);

                    if (result != null)
                        richTextBox.Lines = result.Split (new char [] { '\n' });
                    else
                        richTextBox.Clear ();

                    statusLabel.Text = "Ready";
                }
                catch (Exception error) {
                    statusLabel.Text = "Error";

                    MessageBox.Show (error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}