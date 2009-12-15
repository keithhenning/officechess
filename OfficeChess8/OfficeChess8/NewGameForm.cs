using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Globals;

namespace OfficeChess8
{
    public partial class NewGameForm : Form
    {
        public NewGameForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.m_Server.SetServerIP(textBox4.Text);
            Form1.m_Server.SetServerPort(Int32.Parse(textBox3.Text));

            Form1.m_Client.SetTargetIP(textBox1.Text);
            Form1.m_Client.SetTargetPort(Int32.Parse(textBox2.Text));
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}