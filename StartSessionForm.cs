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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ServiceTest
{
    public partial class StartSessionForm : Form
    {
        /// <summary>
        /// Contains the entered username.
        /// </summary>
        public string Username
        {
            get { return (usernameTextBox.Text); }
        }

        /// <summary>
        /// Contains the entered password
        /// </summary>
        public string Password
        {
            get { return (passwordTextBox.Text); }
        }

        /// <summary>
        /// Creates a new form instance.
        /// </summary>
        public StartSessionForm ()
        {
            InitializeComponent ();
        }

        /// <summary>
        /// Regular expression used to check if the username is an email address.
        /// </summary>
        private static readonly Regex   emailPattern
            = new Regex (@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        /// <summary>
        /// If the username changes check if the accept button should be enabled.
        /// </summary>
        /// <param name="sender">The object causing the event.</param>
        /// <param name="e">The event details.</param>
        private void usernameTextBox_TextChanged (object sender, EventArgs e)
        {
            acceptButton.Enabled = emailPattern.IsMatch (usernameTextBox.Text);
        }
    }
}