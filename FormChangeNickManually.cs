using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Custom Dialog when wanting to change the Playername by hand
    /// </summary>
    class FormChangeNickManually : FormTemplate
    {
        Label textLabel;
        TextBox textBox;
        Button confirmation;
        string nick = "";

        /// <summary>
        /// Init with old nickname 
        /// </summary>
        /// <param name="nickname"></param>
        public FormChangeNickManually(string nickname)
        {
            nick = nickname;
        }
        
        /// <summary>
        /// Adds controls to form and handles interaction with the user
        /// </summary>
        /// <returns></returns>
        public string run()
        {
            Width = GlobalSettings.ChangeNickManually.Width;
            Height = GlobalSettings.ChangeNickManually.Height;
            FormBorderStyle = GlobalSettings.ChangeNickManually.BorderStyle;
            StartPosition = GlobalSettings.ChangeNickManually.StartPosition;
            textLabel = new Label() { Left = 5, Top = 5, Text = GlobalSettings.ChangeNickManually.Title };
            textBox = new TextBox() { Left = 5, Top = 25, Width = 100, Text = nick };
            confirmation = new Button() { Text = "Change!", Left = 5, Width = 80, Top = 50, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { this.Close(); };
            this.Controls.Add(textBox);
            this.Controls.Add(confirmation);
            this.Controls.Add(textLabel);
            this.AcceptButton = confirmation;
            return this.ShowDialog() == DialogResult.OK ? textBox.Text : Text;
        }
        
}
}
