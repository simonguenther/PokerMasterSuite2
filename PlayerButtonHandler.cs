using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Handles everything related to displaying PlayerButtons on a specific table
    /// </summary>
    class PlayerButtonHandler
    {
        TableData table;
        List<PlayerButton> buttons;
        Dictionary<string, PlayerButton> buttonTemplate;
        ButtonPositions positions = new ButtonPositions();
        ToolTip tip = new ToolTip();

        /// <summary>
        /// Init handler with tableinformation
        /// </summary>
        /// <param name="tableDataInformation"></param>
        public PlayerButtonHandler(TableData tableDataInformation)
        {
            table = tableDataInformation;
            // retrieve/init buttonTemplate based on actual size of table retrieved from data
            buttonTemplate = retrieveButtonTemplate(table.getTableSize());
            updateButtons(table,false);
        }

        // Paint every button new from scratch
        private List<PlayerButton> paintButtons(Dictionary<string, PlayerButton> template,TableData tableData, bool force) // force = true enables repainting of buttons which are locked
        {
            foreach (PlayerButton button in buttonTemplate.Values)
            {
                if (!force) // if repainting is not forced, skip locked seats, otherwise force repaint for all buttons (necessary for completely new tables f.e.)
                {
                    if (tableData.isSeatLocked(button.SeatName)) // only (re)paint unlocked seats
                    {
                        continue;
                    }
                }

                string playername = tableData.getNickname(button.SeatName);
                if (playername == null) continue; // skip empty/open seats

                if (GlobalSettings.ButtonSettings.HideEmptyButtons)     // Show Empty Buttons? yes/no
                {
                    try
                    {
                        if (playername.Equals(""))
                        {
                            table.removeSeat(button.SeatName);
                            continue;
                        }

                    } catch(Exception ex) { Console.WriteLine("HideEmptyButtons: \n"+ex.ToString()); }
                }

                // Create ContextMenuStrip for playerbutton and initialize with necessary information
                PlayerButtonContextMenuStrip pbcms = new PlayerButtonContextMenuStrip();
                button.ContextMenuStrip = pbcms;
                button.Text = playername;
                button.BackColor = System.Drawing.Color.FromName(NoteHandler.getNote(playername).getColor());
                button.SetPlayerNameContextMenu(playername, button.SeatName, tableData.tablename, tableData.isSeatLocked(button.SeatName));
                button.SetParent(tableData.tablePointer);
                button.Visible = true;

                /*
                * Create ToolTip & NoteIcon if there are notes
                */
                createToolTipAndNoteIcon(button, playername);

                /*
                 * Check for alternate spellings in playernames ==> Levenshtein!
                 */
                if (tableData.isSeatLocked(button.SeatName)) continue; // Makes no sense to update this for locked buttons [would popup icon if other player gets renamed]
                if (playername.Length > GlobalSettings.Notes.PlayernameMinLengthForDistanceCheck) // Only check for playernames with 3+ characters
                {
                    List<Note> similarNicks = NoteHandler.findSimilarNicknames(playername, GlobalSettings.Notes.LevenshteinMaxDistance);
                    if (similarNicks.Count > 0) // Players with similar nicks have notes already
                    {
                        setSimilarNickImage(button, similarNicks);
                    }
                }

             
            }
            return buttonTemplate.Values.ToList();
        }

        /// <summary>
        /// Removes similar nickname image from screen
        /// </summary>
        /// <param name="seatName"></param>
        public void removeSimilarNickImage(string seatName)
        {
            foreach (PlayerButton b in buttons)
            {
                if (b.SeatName.Equals(seatName))
                {
                    b.removeSimilarNickImage(b);
                    break;
                }
            }
        }

        /// <summary>
        /// Remove unlocked buttons - called at the beginning of the scan-Method in order to remove all unlocked seats/buttons which then will get re-scanned
        /// </summary>
        /// <param name="scanTheseSeats">Stack of unlocked seats retrieved from tabledata</param>
        public void removeUnlockedButtons(Stack<string> scanTheseSeats)
        {
            foreach(String seat in scanTheseSeats)
            {
                removeButton(seat);
            }
        }

        /// <summary>
        /// Set Similar images for buttons if necessary
        /// </summary>
        /// <param name="button">Which button should we attach similar nicks to?</param>
        /// <param name="similar">List of similar nicks</param>
        private void setSimilarNickImage(PlayerButton button, List<Note> similar)
        {
            // Create picture box with settings from GlobalSettings and Set location according to PlayerButton
            CustomPictureBox imageSimilar = new CustomPictureBox();
            imageSimilar.Image = GlobalSettings.Images.questionImage;
            imageSimilar.Width = GlobalSettings.Images.lockImage.Width;
            imageSimilar.Height = GlobalSettings.Images.lockImage.Height;
            imageSimilar.Location = new System.Drawing.Point(button.Location.X, button.Location.Y - imageSimilar.Width-1);
            imageSimilar.Visible = true;
            button.similarImage = imageSimilar;
            imageSimilar.SimilarNicks = similar;
            imageSimilar.PlayerButton = button;
            imageSimilar.MouseDown += new MouseEventHandler(PlayerButton.similarImage_Click);
            imageSimilar.SetParent(button.TableHandle);
        }

        /// <summary>
        /// Set LockImage for single seat
        /// </summary>
        /// <param name="seatName"></param>
        /// <param name="tablePointer"></param>
        public void setLockImage(string seatName, IntPtr tablePointer)
        {
            foreach (PlayerButton b in buttons)
            {
                if (b.SeatName.Equals(seatName))
                {
                    CustomPictureBox imageLock = new CustomPictureBox();
                    imageLock.Image = GlobalSettings.Images.lockImage;
                    imageLock.Width = GlobalSettings.Images.lockImage.Width;
                    imageLock.Height = GlobalSettings.Images.lockImage.Height;
                    imageLock.Location = new System.Drawing.Point(b.Location.X - imageLock.Width - 1, b.Location.Y);
                    imageLock.Visible = true;
                    b.lockImage = imageLock;
                    imageLock.SetParent(tablePointer);
                    break;
                }
            }
        }

        /// <summary>
        /// C# has a bug where tooltips for Buttons whicha re created during runtime will fail to show up
        /// Workaround is adding a transparent label on that button and attach the tooltip to that label
        /// If a Player has a note, we add a Note Icon to the left side of the button and implement the workaround for the tooltip
        /// </summary>
        /// <param name="button"></param>
        /// <param name="playername"></param>
        private void createToolTipAndNoteIcon(PlayerButton button, string playername)
        {
            string hasNote = NoteHandler.playerHasNote(playername);
            // check if note in note database
            if (hasNote == null || hasNote.Equals(""))
            {
                button.setNoteImage(false);
            }
            else
            {
                // if note-text was found, add note image to playerbutton
                button.setNoteImage(true);

                // clean up before adding new tooltip
                int controls = button.Controls.Count;
                if (controls > 0)
                {
                    foreach (Control c in button.Controls)
                    {
                        c.Dispose();
                    }
                }

                // Create workaround/dummy Label
                Label dummy = new Label { Top = 3, Left = 3, Width = 20, Height = 20 };
                dummy.BackColor = System.Drawing.Color.Transparent;
                dummy.ForeColor = System.Drawing.Color.Transparent;
                button.Controls.Add(dummy);

                // Set custom EventHandlers
                button.MouseHover += Button_MouseHover;
                button.MouseLeave += Button_MouseLeave;
            }
        }

        /// <summary>
        /// Deal with showing/hiding tooltip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_MouseLeave(object sender, EventArgs e)
        {
            tip.Active = false;
            tip.Active = true;
        }

        /// <summary>
        /// Implementing the Tooltip-Workaround found on StackOverflow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_MouseHover(object sender, EventArgs e)
        {
            /*
             * Workaround found on StackOverflow since Button Tooltips are (still) buggy
             */
            Button button = (Button)sender;
            string display = NoteHandler.getNote(button.Text).Text;
            Label dummy = (Label)button.Controls[0];
            tip.Show(display, dummy);
            tip.AutoPopDelay = 0;
            tip.AutomaticDelay = 0;
            tip.ShowAlways = true;
        }

        /// <summary>
        /// Remove LockImage from PlayerButton
        /// </summary>
        /// <param name="seatName"></param>
        public void removeLockImage(string seatName)
        {
            foreach (PlayerButton b in buttons)
            {
                if (b.SeatName.Equals(seatName))
                {
                    b.removeLockImage(b);
                    break;
                }
            }
        }

        /// <summary>
        /// Update PlayerButtons triggers a repaint of PlayerButtons
        /// </summary>
        /// <param name="tableData">provide tabledata in order to update buttons</param>
        /// <param name="force">true = also update locked buttons (necessary for new tables), false = repaints only unlocked buttons</param>
        public void updateButtons(TableData tableData, bool force) // force = true enables repainting of buttons which are locked
        {
            table = tableData;
            buttons = paintButtons(buttonTemplate, table, force);
        }

        /// <summary>
        /// Retrieve Button template depending on size of the table
        /// </summary>
        /// <param name="numberOfSeats"></param>
        /// <returns></returns>
        public Dictionary<string, PlayerButton> retrieveButtonTemplate(int numberOfSeats)
        {
            switch(numberOfSeats)
            {
                case 6: return positions.SixHanded();
                case 7: return positions.SevenHanded();
                case 8: return positions.EightHanded();
                case 9: return positions.NineHanded();
            }
            throw new Exception("No Button template for this number of seats");
        }

        /// <summary>
        /// Remove Single Button from Table
        /// </summary>
        /// <param name="seatName"></param>
        public void removeButton(string seatName)
        {
            foreach(PlayerButton b in buttons)
            {
                if(b.SeatName.Equals(seatName))
                {
                    b.Text = "";
                    removeSimilarNickImage(seatName);
                    b.Visible = false;
                }
            }
        }

        /// <summary>
        /// Remove all Locks from single Table
        /// </summary>
        public void removeAllLockImages()
        {
            resetLockToolstrip("Lock");
            foreach (PlayerButton b in buttons)
            {
                if (b.lockImage != null)
                {
                    
                    b.lockImage.Visible = false;
                    b.lockImage = null;
                }
                
            }
        }

        /// <summary>
        /// Remove all similar nick images from table
        /// </summary>
        public void removeAllSimilarNickImages()
        {
            foreach(PlayerButton b in buttons)
            {
                if(b.similarImage != null)
                {
                    b.similarImage.Dispose();
                }
            }
        }

        /// <summary>
        /// Reset PlayerButton context-menu to "text" if ScanButton context-menu "Remove Locks"/"Lock All" has been used before
        /// </summary>
        /// <param name="text"></param>
        public void resetLockToolstrip(String text) 
        {
            foreach (PlayerButton b in buttons)
            {
                PlayerButtonContextMenuStrip newPBCMS = (PlayerButtonContextMenuStrip)b.ContextMenuStrip;
                if (newPBCMS == null)
                {
                    // Happens with empty seats if there are no buttons created at all but buttons has those positions still in
                    continue;
                }
                foreach (ToolStripMenuItem tsmi in newPBCMS.Items)
                {
                    if (tsmi.Text.ToLower().Contains("lock"))
                    {
                        tsmi.Text = text;
                    }
                }
            }
        }

        /// <summary>
        /// Remove all Buttons and Images from Table
        /// </summary>
        public void removeAllButtons()
        {
            foreach (PlayerButton b in buttons)
            {
                b.Text = "";
                b.Visible = false;
                
            }
            removeAllLockImages();
            removeAllSimilarNickImages();
        }

        /// <summary>
        /// Lock all Buttons for a single table
        /// </summary>
        public void lockAllButtons()
        {
            foreach (PlayerButton b in buttons)
            {
                if (b.Text != null)
                {
                    CustomPictureBox cpbOld = (CustomPictureBox)b.lockImage;
                    if (cpbOld == null) // dont attach two lock icons
                    {
                        CustomPictureBox imageLock = new CustomPictureBox();
                        imageLock.Image = GlobalSettings.Images.lockImage;
                        imageLock.Width = GlobalSettings.Images.lockImage.Width;
                        imageLock.Height = GlobalSettings.Images.lockImage.Height;
                        imageLock.Location = new System.Drawing.Point(b.Location.X - imageLock.Width - 1, b.Location.Y);
                        imageLock.Visible = true;
                        b.lockImage = imageLock;
                        imageLock.SetParent(b.TableHandle);

                        PlayerButtonContextMenuStrip pbcms = (PlayerButtonContextMenuStrip)b.ContextMenuStrip;
                        if (pbcms == null)
                        {
                            continue;
                        }
                        // Reset toolstrip to "unlock" text
                        resetLockToolstrip("Unlock");
                    }

                } 
            }
        }

        /// <summary>
        /// Disposes lock and similarNick images
        /// </summary>
        public void Dispose()
        {
            foreach(PlayerButton b in buttons)
            {
                if (b.similarImage != null) b.similarImage.Dispose();
                if (b.lockImage != null) b.lockImage.Dispose();
                b.Dispose();
            }
        }
    }
}
