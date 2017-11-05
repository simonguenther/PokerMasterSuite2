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
    /// Custom Dialog which enables the user to change a player by avatar
    /// </summary>
    class FormChangeNickAvatar : FormTemplate
    {
        Label textLabel;
        TextBox search;
        ListView selectionTags;
        Button accept;
        string seatname;
        string tablename;

        /// <summary>
        /// Initialize Form parameters, define and add controls
        /// </summary>
        /// <param name="seatname"></param>
        /// <param name="tablename"></param>
        public FormChangeNickAvatar(string seatname, string tablename) {
            this.seatname = seatname;
            this.tablename = tablename;
            Width = GlobalSettings.ChangeNickByAvatar.Width;
            Height = GlobalSettings.ChangeNickByAvatar.Height;
            Text = GlobalSettings.ChangeNickByAvatar.Title;
            textLabel = new Label() { Left = 5, Top = 5, Height = 15, Text = "Search by tags" };
            search = new TextBox() { Left = 5, Top = 25, Width = 180 };
            selectionTags = new ListView() { Left = 5, Top = 48, Width = 180, Height = 180 };
            accept = new Button() { Left = 5, Top = 232, Width = 180, Height = 25, Text = "Change to this player" };
            accept.Click += Accept_Click;        
            search.KeyDown += new KeyEventHandler(searchByTags_KeyDown);
            this.Controls.Add(textLabel);
            this.Controls.Add(search);
            this.Controls.Add(selectionTags);
            this.Controls.Add(accept);
            this.Show();
        }

        /// <summary>
        /// EventHandler when Accepting change by tags
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Accept_Click(object sender, EventArgs e)
        {
            if (selectionTags.SelectedItems.Count > 0)
            {
                executeChangeByAvatar();
            }
        }

        /// <summary>
        /// Orchestrates change of nickname in data representation as well as in gui part
        /// </summary>
        private void executeChangeByAvatar()
        {
            IntPtr tableHandle = IntPtr.Zero;
            TableData td = new TableData(7);
            string nickname;
            string newNickname;

            // Change Nickname and information in Data Representation
            foreach (KeyValuePair<IntPtr, TableData> kvp in TableHandler.tableSessions)
            {
                td = kvp.Value;
                if (td.tablename.Equals(tablename))
                {
                    // Retrieve Nickname
                    nickname = td.getNickname(seatname);

                    // Show Adjustment Dialog
                    newNickname = selectionTags.SelectedItems[0].Text;

                    if (newNickname == "") newNickname = nickname; // new nickname cannot be empty!
                                                                   // change nickname in seatData (via tableData) here
                    td.setNickname(seatname, newNickname);
                    break;
                }
            }

            // Change Playerbutton based on updated Data Representation
            foreach (KeyValuePair<string, PlayerButtonHandler> kvp in TableHandler.buttonInventory)
            {
                if (kvp.Key.Equals(td.tablename))
                {
                    // Change button text here
                    kvp.Value.updateButtons(td, true);
                    break;
                }
            }
            this.Close();
        }

        /// <summary>
        /// Manages search and display of tags
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchByTags_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox tb = (TextBox)sender;
                ImageList tagFindings = new ImageList();
                tagFindings.ImageSize = new Size(65, 65);

                selectionTags.Items.Clear();
                selectionTags.ShowGroups = false;

                List<Note> findings = NoteHandler.searchNoteByTags(tb.Text);
                for (int i = 0; i < findings.Count; i++)
                {
                    tagFindings.Images.Add(NoteHandler.base64ToImage(findings.ElementAt(i).Avatar));
                }

                ListViewItem lvi;
                ListViewItem.ListViewSubItem lvsi;
                selectionTags.BeginUpdate();

                // Insert Items to ListView
                for (int x = 0; x < tagFindings.Images.Count; x++)
                {
                    lvi = new ListViewItem();
                    lvi.Text = findings.ElementAt(x).Name;
                    lvi.ImageIndex = x;
                    lvi.Name = findings.ElementAt(x).Name;
                    lvi.Tag = findings.ElementAt(x);

                    lvsi = new ListViewItem.ListViewSubItem();
                    lvsi.Text = findings.ElementAt(x).Name;

                    lvi.SubItems.Add(lvsi);
                    selectionTags.Items.Add(lvi);
                }

                selectionTags.EndUpdate();
                selectionTags.LargeImageList = tagFindings;
                selectionTags.View = View.LargeIcon;
                selectionTags.Refresh();
            }
        }
    }
}
