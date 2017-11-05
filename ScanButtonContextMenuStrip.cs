using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokerMasterSuite2
{
    class ScanButtonContextMenuStrip : ContextMenuStrip
    {
        ToolStripMenuItem scan = new ToolStripMenuItem { Text = "Scan" };
        ToolStripMenuItem seatingScript = new ToolStripMenuItem { Text = "Start SeatingScript" };
        ToolStripMenuItem removeAllLocks = new ToolStripMenuItem { Text = "Unlock all buttons" };
        ToolStripMenuItem lockAllButtons = new ToolStripMenuItem { Text = "Lock all buttons" };
        ToolStripMenuItem removeAllButtons = new ToolStripMenuItem { Text = "Remove buttons" };
        ToolStripMenuItem noteBrowser = new ToolStripMenuItem { Text = "NoteBrowser" };
        ToolStripMenuItem removeScanButton = new ToolStripMenuItem { Text = "Remove scan button" };

        public IntPtr tableHandle { set; get;  }
        public ScanButton parentButton { set; get; }

       public ScanButtonContextMenuStrip()
        {
            removeAllButtons.Click += RemoveAllButtons_Click;
            removeAllLocks.Click += RemoveAllLocks_Click;
            seatingScript.Click += SeatingScript_Click;
            scan.Click += SessionHandler.ControlButton_Click;
            lockAllButtons.Click += LockAllButtons_Click;
            removeScanButton.Click += RemoveScanButton_Click;
            noteBrowser.Click += NoteBrowser_Click;
            this.Items.Add(scan);
            this.Items.Add(seatingScript);
            this.Items.Add(removeAllLocks);
            this.Items.Add(lockAllButtons);
            this.Items.Add(removeAllButtons);
            this.Items.Add(noteBrowser);
            this.Items.Add(removeScanButton);
        }

        private void NoteBrowser_Click(object sender, EventArgs e)
        {
           

        }

        public void RemoveScanButton_Click(object sender, EventArgs e)
        {
            SessionHandler.removeScanButton(parentButton);
        }

        private void LockAllButtons_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem tsi = (ToolStripMenuItem)sender;
                ScanButtonContextMenuStrip pbcms = (ScanButtonContextMenuStrip)tsi.GetCurrentParent();
                IntPtr pointer = pbcms.tableHandle;
                TableData td = TableHandler.tableSessions[pointer];
                
                // Lock Seats in TableData
                td.lockAllSeats();

                // Update Buttons => Show lock!
                string tablename = td.tablename;
                PlayerButtonHandler pbh = TableHandler.buttonInventory[tablename];
                pbh.lockAllButtons();
            }
        }

        public static void SeatingScript_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                SeatingScript ss = new SeatingScript();

                ToolStripMenuItem tsi = (ToolStripMenuItem)sender;
                ScanButtonContextMenuStrip pbcms = (ScanButtonContextMenuStrip)tsi.GetCurrentParent();
                ScanButton scanButton = pbcms.parentButton;
                
                if (tsi.Text.Equals(GlobalSettings.SeatingScript.StartSeatingScriptLabel))
                {
                    ss.startSeating(scanButton);
                    tsi.Text = GlobalSettings.SeatingScript.StopSeatingScriptLabel;
                }
                else if (tsi.Text.Equals(GlobalSettings.SeatingScript.StopSeatingScriptLabel))
                {
                    ss.stopSeating(scanButton);
                    tsi.Text = GlobalSettings.SeatingScript.StartSeatingScriptLabel;
                    scanButton.Text = GlobalSettings.ButtonSettings.ScanButtonLabel;
                }
                else throw new Exception("Labeling of SeatingScript Buttons is wrong!");
            }
            else
            {
                throw new Exception("Sender is no ToolStripMenuItem!");
            }
        }

        private void RemoveAllButtons_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem tsi = (ToolStripMenuItem)sender;
                ScanButtonContextMenuStrip pbcms = (ScanButtonContextMenuStrip)tsi.GetCurrentParent();
                IntPtr pointer = pbcms.tableHandle;
                TableData td = TableHandler.tableSessions[pointer];

                // Remove in TableData
                td.removeAllSeats();

                // Remove Buttons
                string tablename = td.tablename;
                PlayerButtonHandler pbh = TableHandler.buttonInventory[tablename];
                pbh.removeAllButtons();
            }
            else
            {
                throw new Exception("Sender is no ToolStripMenuItem!");
            }
        }

        private void RemoveAllLocks_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem tsi = (ToolStripMenuItem)sender;
                ScanButtonContextMenuStrip pbcms = (ScanButtonContextMenuStrip)tsi.GetCurrentParent();
                IntPtr pointer = pbcms.tableHandle;
                TableData td = TableHandler.tableSessions[pointer];

                // Unlock in TableData
                td.unlockAllSeats();

                // Unlock buttons => remove lock icons
                string tablename = td.tablename;
                PlayerButtonHandler pbh = TableHandler.buttonInventory[tablename];
                pbh.removeAllLockImages();


            }
            else
            {
                throw new Exception("Sender is no ToolStripMenuItem!");
            }
        }
    }

   
}
