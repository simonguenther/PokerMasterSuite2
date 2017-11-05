using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerMasterSuite2
{
    /// <summary>
    /// SeatData Class
    /// </summary>
    class SeatData
    {
        /// <summary>
        /// Initialize SeatData object
        /// </summary>
        public SeatData()
        {
            avatar = new System.Drawing.Bitmap(42, 42); //random size, just to have sth
            locked = false;
            hasAlternates = false;
            color = System.Drawing.Color.Gray;
        }

        public string nickname { set; get; }
        public System.Drawing.Bitmap avatar { set; get; }
        public bool locked { set; get; }
        public bool hasAlternates { set; get; }
        public System.Drawing.Color color { set; get; }

    }
}
