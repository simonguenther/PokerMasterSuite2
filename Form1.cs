using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokerMasterSuite2
{
    public partial class Form1 : Form
    {
        bool isRunning = false;
        static SessionHandler sessionHandler = new SessionHandler();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //sessionHandler.attachScanControls();
            btnRefresh.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(isRunning) // Click => Stop => Idle
            {
                // Check for running seating scripts
                sessionHandler.detachScanControls();
                btnStart.Text = "Start";
                isRunning = false;
                btnRefresh.Visible = false;
            }
            else // Click => Start => Running
            {
                sessionHandler.attachScanControls();
                btnStart.Text = "Stop";
                isRunning = true;
                btnRefresh.Visible = true;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            sessionHandler.attachScanControls();
        }
    }
}
