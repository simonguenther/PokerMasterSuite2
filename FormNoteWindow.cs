using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Note Window Form
    /// Visualize Note- and Playerdata to the user
    /// </summary>
    class FormNoteWindow : FormTemplate
    {
        ComboBox colorBox;
        TextBox nameTextBox;
        TextBox tagsTextBox;
        Button btnSave;
        Button btnDelNote;
        Button btnUpdateAvatar;
        RichTextBox noteTextBox;
        PictureBox avatarBox;
        Label tagDescription;
        Note note;
        string table;
        
        /// <summary>
        /// Init note window with note
        /// 
        /// Going to get called by Similar Nick Icon
        /// </summary>
        /// <param name="singleNote"></param>
        /// <param name="tablename">needed for placing the window above the table of the player and not somewhere else on the screen</param>
        public FormNoteWindow(Note singleNote, string tablename)
        {
            initComponents();
            note = singleNote;
            table = tablename;
            loadValuesFromNote();
            setAppearanceLocation(tablename);
            this.Show();
        }

        /// <summary>
        /// Only one notewindow per player should be opened, set to foreground if double open
        /// !!! Bug: Seems like western nicknames work fine but chinese dont !!!+
        /// 
        /// Will get called from PlayerButton.Click
        /// 
        /// </summary>
        /// <param name="playername"></param>
        /// <param name="tablename"></param>
        public FormNoteWindow(string playername, string tablename) 
        {
            // Retrieve List of instances with the regex set to the playername 
            EmulatorHandler doubleCheck = new EmulatorHandler(playername);
            List<IntPtr> doubleList = doubleCheck.getEmulatorList();

            if(doubleList.Count == 0) // No window for the same player found
            {
                // Create new NoteWindow
                initComponents();
                //Console.WriteLine("FormNoteWindow from PlayerButton.Click");  // Debug

                // Set Note from Loaded Notes
                this.note = NoteHandler.getNote(playername);

                table = tablename;

                // Check if note exists, if not: construct new from tabledata
                if (note.Name == null) constructNewNote(playername, tablename);
                else loadValuesFromNote();

                // Set Location of the Form within table boundaries and not somewhere else on the screen
                setAppearanceLocation(tablename);
                this.Show();
            } else // Already one open => Set to foreground!
            {
                WinAPI.SetForegroundWindow(doubleList[0]);
            }
         
        }

        /// <summary>
        /// Retrieve Information from Note and add it to relevant components
        /// </summary>
        private void loadValuesFromNote()
        {
            Text = note.Name;
            nameTextBox.Text = note.Name;
            noteTextBox.Text = note.Text;
            colorBox.SelectedItem = note.getColor();
            tagsTextBox.Text = note.Tags;
            if(note.Avatar != null)
            {
                avatarBox.Image = NoteHandler.base64ToImage(note.Avatar);
            } 
        }

        /// <summary>
        /// If there is no Note stored for a player => Create a new note!
        /// </summary>
        /// <param name="playername"></param>
        /// <param name="tablename"></param>
        public void constructNewNote(string playername, string tablename)
        {
            // Construct Note from TableData
            TableData td = TableHandler.getTableDataFromTableName(tablename);
            string seatname = td.getSeatname(playername);
            note.Name = td.getNickname(seatname);

            if (td.getAvatar(seatname) == null) note.Avatar = null;
            else note.Avatar = NoteHandler.imageToBase64(td.getAvatar(seatname));

            loadValuesFromNote();
        }

        /// <summary>
        /// Hotkey => Call BtnSave_Click on Shift+Enter
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Save Changes on Shift+Enter
            if (keyData == (Keys.Enter | Keys.Shift))
            {
                BtnSave_Click(new object(), null);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Get Infos from Table to set NoteWindow appearance into the center of the table
        /// </summary>
        /// <param name="table"></param>
        private void setAppearanceLocation(string table)
        {
            // retrieve needed table from sessions list
            foreach(KeyValuePair<IntPtr, TableData> kvp in TableHandler.tableSessions)
            {
                TableData data = kvp.Value;
                if(data.tablename.Equals(table))
                {
                    IntPtr tableHandle = kvp.Key;
                    WinAPI.RECT rect = new WinAPI.RECT();
                    WinAPI.GetWindowRect(tableHandle, out rect);
                    int a = rect.Width / 4;
                    int b = rect.Height / 3;
                    WinAPI.MoveWindow(this.Handle, rect.X+a, rect.Y+b, this.Width, this.Height, true);
                    break;
                }
            }
        }

        /// <summary>
        /// Initialize all Components with default settings and add them to the Form
        /// </summary>
        private void initComponents()
        {
            this.Size = new System.Drawing.Size(355, 314);

            colorBox = new ComboBox { Left = 3, Top = 8 };
            this.Controls.Add(colorBox); // DO NOT MOVE THIS! box has to be added BEFORE datasource gets set!
            colorBox.DataSource = GlobalSettings.Notes.playercolors;
            
            nameTextBox = new TextBox { Left = 135, Top = 8 };

            tagsTextBox = new TextBox { Left = 75, Top = 222, Size = new Size(191, 20) };
            tagsTextBox.TabIndex = 1;

            btnSave = new Button { Left = 257, Top = 8, Size = new Size(75, 23), Text = "Save" };
            btnSave.Click += BtnSave_Click;

            btnDelNote = new Button { Left = 74, Top = 248, Size = new Size(93, 23), Text = "Delete Note" };
            btnDelNote.Click += BtnDelNote_Click;

            btnUpdateAvatar = new Button { Left = 173, Top = 248, Size = new Size(93, 23), Text = "Update Avatar" };
            btnUpdateAvatar.Click += BtnUpdateAvatar_Click;

            noteTextBox = new RichTextBox { Left = 3, Top = 35, Size = new Size(329, 165) };
            noteTextBox.TabIndex = 0;

            tagDescription = new Label { Left = 75, Top = 203, Size = new Size(138, 13), Text = "Tag(s) (seperate by spaces)" };

            avatarBox = new PictureBox { Left = 3, Top = 206, Size = new Size(65, 65) };
            
            this.Controls.Add(nameTextBox);
            this.Controls.Add(tagsTextBox);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnDelNote);
            this.Controls.Add(btnUpdateAvatar);
            this.Controls.Add(noteTextBox);
            this.Controls.Add(tagDescription);
            this.Controls.Add(avatarBox);
        }

        /// <summary>
        /// EventHandler - Delete Note from inside NoteWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelNote_Click(object sender, EventArgs e)
        {
            // Remove vom Note-File
            NoteHandler.deleteNote(note.Name);
            NoteHandler.saveNotesToFile();
            NoteHandler.refreshNotesFromFile();

            // Remove tabledata as well as button
            TableHandler.refreshTableData(note, table);
            TableHandler.refreshPlayerButtons(table);

            this.Close();
        }

        /// <summary>
        /// Saves updated note (also to file)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            /*
             * 1. Collect all info and save to list & save to file
             * 2. Check if avatar = null?
             */
            note.Color = colorBox.SelectedValue.ToString();
            note.Text = noteTextBox.Text.Trim();
            note.Tags = tagsTextBox.Text.Trim();

            // Convert Avatar to base64 encoded string
            if(note.Avatar != null)
            {
                note.Avatar = NoteHandler.imageToBase64(avatarBox.Image);
            }

            // Save Note
            NoteHandler.saveNote(note);
            NoteHandler.saveNotesToFile();
            NoteHandler.refreshNotesFromFile();

            // Update data representation as well as update playerbuttons
            TableHandler.refreshTableData(note, table);
            TableHandler.refreshPlayerButtons(table);

            this.Close();
        }

        /// <summary>
        /// Update Avatar from out of Note Window
        /// Basically just takes screenshot of Avatar-Rectangle and puts it into data representation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUpdateAvatar_Click(object sender, EventArgs e)
        {
            TableData td = TableHandler.getTableDataFromTableName(table);
            Bitmap screenshot = Screenshot.CaptureApplication(td.tablePointer);
            string seat = td.getSeatname(note.Name);
            Bitmap updatedAvatar = ScreenshotAnalyzer.getSingleAvatar(screenshot, seat, td.getTableSize());
            avatarBox.Image = updatedAvatar;
            note.Avatar = NoteHandler.imageToBase64(updatedAvatar);
        }
    }
}

