using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Extends Windows.Forms.Button and implements all necessary methods to manage those during runtime (changing, deleting,...)
    /// </summary>
    public class PlayerButton : System.Windows.Forms.Button
    {
        public IntPtr TableHandle { set; get; }
        public string SeatName { set; get; }
        public CustomPictureBox lockImage { set; get; }
        public CustomPictureBox similarImage { set; get; }

        /// <summary>
        /// Init Button with default values from GlobalSettings
        /// </summary>
        public PlayerButton()
        {
            Width = GlobalSettings.ButtonSettings.Width;
            Height = GlobalSettings.ButtonSettings.Height;
            Visible = true;
            this.Click += PlayerButton_Click;
        }

        /// <summary>
        /// Open Note Window on Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerButton_Click(object sender, EventArgs e)
        {
            PlayerButtonContextMenuStrip pbcms = (PlayerButtonContextMenuStrip)this.ContextMenuStrip;
            new FormNoteWindow(pbcms.playername, pbcms.tablename);
        }

        /// <summary>
        /// Set Parent => Makes it easier later to navigate through changes made on the table
        /// </summary>
        /// <param name="newParent"></param>
        public void SetParent(IntPtr newParent)
        {
            this.TableHandle = newParent;
            if (!TableHandle.Equals(IntPtr.Zero))
            {
                WinAPI.SetParent(this.Handle, TableHandle);
            }
        }

        /// <summary>
        /// If Player has a note (real text, not just color), display note icon on the left side of the button
        /// </summary>
        /// <param name="hasNote"></param>
        public void setNoteImage(bool hasNote)
        {
            if (hasNote)
            {
                this.Image = GlobalSettings.Images.financeIcon.ToBitmap();
                this.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            } else
            {
                this.Image = null;
                this.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
        }

        /// <summary>
        /// pass information through to context menu
        /// --> necessary for adjusting nickname and avatars, locking & removing buttons
        /// </summary>
        /// <param name="nickname">nickname</param>
        /// <param name="seat">seatname</param>
        /// <param name="tableName">tablename</param>
        /// <param name="locked">locked or not?</param>
        public void SetPlayerNameContextMenu(string nickname, string seat, string tableName, bool locked)
        {
            PlayerButtonContextMenuStrip pbcms = (PlayerButtonContextMenuStrip)ContextMenuStrip;
            pbcms.playername = nickname;
            pbcms.seatname = seat;
            pbcms.tablename = tableName;

            if(locked)
            {
                foreach(ToolStripMenuItem tsmi in pbcms.Items)
                {
                    if (tsmi.Text.Equals("Lock")) tsmi.Text = "Unlock";
                }
            }
        }

        /// <summary>
        /// Handles actions when clicked on SimilarImage-Icon
        /// * RightClick => Removes Icon
        /// * Left Click => If there is only one similar nick, it will adjust the player from the button to the one from the similar icon
        ///                 If there are more than one => Open NoteWindows for all similar nicks!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void similarImage_Click(object sender, MouseEventArgs e)
        {
            // LeftClick
            if (e.Button.Equals(MouseButtons.Left))
            {
                if (sender is CustomPictureBox)
                {
                    CustomPictureBox cbp = (CustomPictureBox)sender;
                    PlayerButton pb = cbp.PlayerButton;
                    List<Note> similar = cbp.SimilarNicks;

                    IntPtr pointer = pb.TableHandle;
                    TableData td = TableHandler.tableSessions[pointer];
                    
                    // if only 1 similar nick, set player on this button to the similar player on LeftClick
                    if(similar.Count.Equals(1))
                    {
                        // Adjust TableData
                        string seatname = td.getSeatname(pb.Text);
                        td.setNickname(seatname, similar[0].Name);

                        // Repaint Buttons
                        PlayerButtonHandler pbh = TableHandler.buttonInventory[td.tablename];
                        pbh.updateButtons(td, true);
                        cbp.Dispose();
                    }
                    else
                    {
                        // if more than 1 similar nick, open NoteWindows for every of those nicks
                        foreach (Note n in similar)
                        {
                            Console.WriteLine(n.Name);
                            FormNoteWindow fnw = new FormNoteWindow(n, td.tablename);
                            fnw.Show();
                        }
                    }
                    
                }
            }
            // RightClick
            else if (e.Button.Equals(MouseButtons.Right))
            {
                if(sender is CustomPictureBox)
                {
                    CustomPictureBox cbp = (CustomPictureBox)sender;
                    cbp.Dispose();
                }
            }
        }

        /// <summary>
        /// Remove Similar Nick Image
        /// </summary>
        /// <param name="b"></param>
        public void removeSimilarNickImage(PlayerButton b)
        {
            CustomPictureBox cpb = b.similarImage;
            //b.similarImage = null;
            if (cpb != null)
            {
                cpb.Dispose();
            }
            
        }

        /// <summary>
        ///  Remove Lock Image
        /// </summary>
        /// <param name="b"></param>
        public void removeLockImage(PlayerButton b)
        {
            CustomPictureBox cpb = b.lockImage;
            if (cpb != null)
            {
                cpb.Visible = false;
                cpb = null;
                //cpb.Dispose();
            }
            if(b.lockImage != null) b.lockImage.Dispose();
            //Console.WriteLine("Removed for " + b.SeatName + " Status: " + b.lockImage.IsDisposed); // Debug
        }
    }
}
