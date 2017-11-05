using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Helper Class for handling images before and after OCR
    /// </summary>
    class RawSeatInfo
    {
        public RawSeatInfo(Bitmap nick, Bitmap ava)
        {
            nicknameBMP = nick;
            avatar = ava;
        }

        public System.Drawing.Bitmap nicknameBMP { set; get; }
        public System.Drawing.Bitmap avatar { set; get; }
        public string nicknameSTRING { set; get; }
    }

}
