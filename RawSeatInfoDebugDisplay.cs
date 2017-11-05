using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Debug Info display 
    /// </summary>
    class RawSeatInfoDebugDisplay
    {
        public RawSeatInfoDebugDisplay(string tableName, Dictionary<string, RawSeatInfo> input)
        {
            Form debug = new Form { Size = new System.Drawing.Size(400,800), Text = "Debug for: " + tableName } ;

            Label seatLabel;
            Label nickLabel;
            PictureBox nicknameBox;
            PictureBox avatarBox;
            for (int i = 0; i < input.Count; i++) 
            {
                string seat = input.ElementAt(i).Key;
                RawSeatInfo rsi = input.ElementAt(i).Value;
                Console.WriteLine(seat);

                seatLabel = new Label { Text = seat, Width = 100, Height = 25, Left = 5, Top = i*100  };
                seatLabel.Show();

                nickLabel = new Label { Text = rsi.nicknameSTRING, Width = 100, Height = 25, Left = 105, Top = i * 100 };
                nickLabel.Show();

                avatarBox = new PictureBox { Image = rsi.avatar, Width = 62, Height = 62, Left = 250, Top = i * 100 };
                avatarBox.Show();

                nicknameBox = new PictureBox { Image = rsi.nicknameBMP, Width = 120, Height = 23, Left = 105, Top = nickLabel.Top + 30 };
                nicknameBox.Show();

                debug.Controls.Add(seatLabel);
                debug.Controls.Add(nickLabel);
                debug.Controls.Add(nicknameBox);
                debug.Controls.Add(avatarBox);
                
            }
            debug.Show();
        }
    }
}
