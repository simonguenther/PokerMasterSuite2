using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Handles actions from Context Menu of each playerbutton by relaying the actions 
    /// </summary>
    class PlayerButtonContextMenuStrip : ContextMenuStrip
    {
        ToolStripMenuItem changeNicknameManually = new ToolStripMenuItem { Text = "Change nickname manually" };
        ToolStripMenuItem changeNicknameByAvatar = new ToolStripMenuItem { Text = "Change by avatar" };
        ToolStripMenuItem removeButton = new ToolStripMenuItem { Text = "Remove button" };
        ToolStripMenuItem lockButton = new ToolStripMenuItem { Text = "Lock" };
        ToolStripMenuItem displayDebugTableData = new ToolStripMenuItem { Text = "Display TableData" };
        public string playername { set; get; }
        public string tablename { set; get; }
        public string seatname { set; get;  }
        
        /// <summary>
        /// Init Components & EventHandlers
        /// </summary>
        public PlayerButtonContextMenuStrip()
        {
            changeNicknameManually.Click += changeNicknameManually_Click;
            changeNicknameByAvatar.Click += changeNicknameByAvatar_Click;
            removeButton.Click += RemoveButton_Click;
            lockButton.Click += LockButton_Click;
            displayDebugTableData.Click += DisplayDebugTableData_Click;
            this.Items.Add(changeNicknameManually);
            this.Items.Add(changeNicknameByAvatar);
            this.Items.Add(removeButton);
            this.Items.Add(lockButton);
            this.Items.Add(displayDebugTableData);
        }
        
        /// <summary>
        /// Change Nickname manually
        /// </summary>
        /// <param name="sender">should be ToolStripMenuItem</param>
        /// <param name="e"></param>
        private void changeNicknameManually_Click(object sender, EventArgs e)
        {
            /*
             *  Whats need to change?
             *  1. nickname in SeatData --> via tabledata
             *  2. text in PlayerButton --> via PlayerButtonHandler
             */
            if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem tsi = (ToolStripMenuItem)sender;
                PlayerButtonContextMenuStrip pbcms = (PlayerButtonContextMenuStrip)tsi.GetCurrentParent();
                PlayerButtonContextMenuActions.renameSeat(pbcms.seatname, pbcms.tablename);
            }
            else
            {
                throw new Exception("Sender is no ToolStripMenuItem!");
            }
        
        }

        /// <summary>
        /// Change Nickname by Avatar/Tags
        /// </summary>
        /// <param name="sender">should be ToolStripMenuItem</param>
        /// <param name="e"></param>
        private void changeNicknameByAvatar_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem tsi = (ToolStripMenuItem)sender;
                PlayerButtonContextMenuStrip pbcms = (PlayerButtonContextMenuStrip)tsi.GetCurrentParent();
                new FormChangeNickAvatar(pbcms.seatname, pbcms.tablename);
            }
            else
            {
                throw new Exception("Sender is no ToolStripMenuItem!");
            }
        }

        /// <summary>
        /// Remove Single Button 
        /// </summary>
        /// <param name="sender">should be ToolStripMenuItem</param>
        /// <param name="e"></param>
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem tsi = (ToolStripMenuItem)sender;
                PlayerButtonContextMenuStrip pbcms = (PlayerButtonContextMenuStrip)tsi.GetCurrentParent();
                PlayerButtonContextMenuActions.removeSeatFromTable(pbcms.seatname, pbcms.tablename);
            }
            else
            {
                throw new Exception("Sender is no ToolStripMenuItem!");
            }
        }

        /// <summary>
        /// Locks Single Button
        /// </summary>
        /// <param name="sender">should be ToolStripMenuItem</param>
        /// <param name="e"></param>
        private void LockButton_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem tsi = (ToolStripMenuItem)sender;
                PlayerButtonContextMenuStrip pbcms = (PlayerButtonContextMenuStrip)tsi.GetCurrentParent();

                // Set Caption correctly as it can also get changed by "LockAllButtons", etc
                string displayedText = tsi.Text;
                if(displayedText.Equals("Lock"))
                {
                    PlayerButtonContextMenuActions.lockSeat(pbcms.seatname, pbcms.tablename);
                    tsi.Text = "Unlock";
                } else
                {
                    PlayerButtonContextMenuActions.unlockSeat(pbcms.seatname, pbcms.tablename);
                    tsi.Text = "Lock";
                }
            }
            else
            {
                throw new Exception("Sender is no ToolStripMenuItem!");
            }
        }
        
        /// <summary>
        /// Display Debug Table Data Form 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayDebugTableData_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem tsi = (ToolStripMenuItem)sender;
                PlayerButtonContextMenuStrip pbcms = (PlayerButtonContextMenuStrip)tsi.GetCurrentParent();
                PlayerButtonContextMenuActions.displayDebugTableData(pbcms.tablename);
            }
            else
            {
                throw new Exception("Sender is no ToolStripMenuItem!");
            }
        }
    }
}
