using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Visual Debug Form for TableData information
    /// </summary>
    class TableDataDebugDisplay
    {

        public TableDataDebugDisplay(TableData input)
        {
            Form debug = new Form { Size = new System.Drawing.Size(600, 800), Text = "Debug for: " + input.tablename };
            Dictionary<string, SeatData> seatList = input.getSeatList();

            Label nameLabel = new Label { Text = input.tablename, Width = 100, Height = 25, Left = 5, Top = 5 };
            nameLabel.Show();

            Label numberOfSeatsLabel = new Label { Text = "Number of Seats: " +input.getTableSize().ToString(), Width = 100, Height = 30, Left = 5, Top = 35 };
            numberOfSeatsLabel.Show();

            Label seatLabel;
            Label nickLabel;
            Label lockedLabel;
            PictureBox avatarBox;
            for (int i = 0; i < seatList.Count; i++)
            {
                string seatName = seatList.ElementAt(i).Key;
                SeatData seat = seatList.ElementAt(i).Value;

                seatLabel = new Label { Text = seatName, Width = 100, Height = 25, Left = 5, Top = i*100+50 };
                seatLabel.Show();

                nickLabel = new Label { Text = seat.nickname, Width = 100, Height = 25, Left = 105, Top = i * 100 + 50 };
                nickLabel.Show();

                string isLocked = "unlocked";
                if (seat.locked) isLocked = "locked";

                lockedLabel = new Label { Text = isLocked, Width = 100, Height = 25, Left = 210, Top = i * 100 + 50 };
                lockedLabel.Show();

                avatarBox = new PictureBox { Image = seat.avatar, Width = 62, Height = 62, Left = 315, Top = i * 100 +50, BackColor = System.Drawing.Color.Black};
                avatarBox.Show();

                //seat.avatar.Save("c:\\DEBUG_avatar_" + seatName + ".png");

                debug.Controls.Add(seatLabel);
                debug.Controls.Add(nickLabel);
                debug.Controls.Add(lockedLabel);
                debug.Controls.Add(avatarBox);
            }

            debug.Controls.Add(nameLabel);
            debug.Controls.Add(numberOfSeatsLabel);
            debug.Show();
        }
        
    }
}
