using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Globals;
using System.Net;

namespace OfficeChess8
{
    public partial class NetworkSettingsForm : Form
    {
        public NetworkSettingsForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // test input sanity
            IPAddress outIP;
            int outInt;
            if ( !IPAddress.TryParse(textBox4.Text, out outIP) || 
                 !IPAddress.TryParse(textBox1.Text, out outIP) )
            {
                MessageBox.Show("One of the IP adresses is invalid", "Error", MessageBoxButtons.OK);
                return;
            }                
            else if (!Int32.TryParse(textBox2.Text, out outInt) || 
                     !Int32.TryParse(textBox3.Text, out outInt) )
            {
                MessageBox.Show("One of the port numbers is invalid", "Error", MessageBoxButtons.OK);
                return;
            }                 
            else if (textBox4.Text == textBox1.Text)
            {
                MessageBox.Show("Target IP can't be the same as the server IP", "Error", MessageBoxButtons.OK);
                return;
            }
            else
            {
                // setup server and client
                try
                {
                    // configure server
                    Form1.m_Server.SetServerIP(textBox4.Text);
                    Form1.m_Server.SetServerPort(Int32.Parse(textBox3.Text));

                    // restart server
                    Form1.m_Server.Stop();
                    Form1.m_Server.Start();

                    // configure client
                    Form1.m_Client.SetTargetIP(textBox1.Text);
                    Form1.m_Client.SetTargetPort(Int32.Parse(textBox2.Text));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NetworkSettingsForm_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // close on enter pressed
            if (e.KeyChar == 13)
            {
                button1_Click(sender, e);
            }
        }
    }
}