using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Template and Default initialization of PlayerButtons
    /// </summary>
    public class ButtonPositions
    {
        Dictionary<string, PlayerButton> sixButtons = new Dictionary<string, PlayerButton>();
        Dictionary<string, PlayerButton> sevenButtons = new Dictionary<string, PlayerButton>();
        Dictionary<string, PlayerButton> eightButtons = new Dictionary<string, PlayerButton>();
        Dictionary<string, PlayerButton> nineButtons = new Dictionary<string, PlayerButton>();
        
        public ButtonPositions()
        {
            sixButtons[TableSize.SixHanded.Top] = new PlayerButton { SeatName = TableSize.SixHanded.Top, Left = 250, Top = 40 };
            sixButtons[TableSize.SixHanded.UpperLeft] = new PlayerButton { SeatName = TableSize.SixHanded.UpperLeft, Left = 35, Top = 214 };
            sixButtons[TableSize.SixHanded.UpperRight] = new PlayerButton { SeatName = TableSize.SixHanded.UpperRight, Left = 454, Top = 214 };
            sixButtons[TableSize.SixHanded.LowerLeft] = new PlayerButton { SeatName = TableSize.SixHanded.LowerLeft, Left = 35, Top = 514 };
            sixButtons[TableSize.SixHanded.LowerRight] = new PlayerButton { SeatName = TableSize.SixHanded.LowerRight, Left = 454, Top = 514 };
            sixButtons[TableSize.SixHanded.Bottom] = new PlayerButton { SeatName = TableSize.SixHanded.Bottom, Left = 250, Top = 690 };

            sevenButtons[TableSize.SevenHanded.TopLeft] = new PlayerButton { SeatName = TableSize.SevenHanded.TopLeft, Left = 141, Top = 61 };
            sevenButtons[TableSize.SevenHanded.TopRight] = new PlayerButton { SeatName = TableSize.SevenHanded.TopRight, Left = 350, Top = 61 };
            sevenButtons[TableSize.SevenHanded.UpperLeft] = new PlayerButton { SeatName = TableSize.SevenHanded.UpperLeft, Left = 35, Top = 214 };
            sevenButtons[TableSize.SevenHanded.UpperRight] = new PlayerButton { SeatName = TableSize.SevenHanded.UpperRight, Left = 454, Top = 214 };
            sevenButtons[TableSize.SevenHanded.LowerLeft] = new PlayerButton { SeatName = TableSize.SevenHanded.LowerLeft, Left = 35, Top = 514 };
            sevenButtons[TableSize.SevenHanded.LowerRight] = new PlayerButton { SeatName = TableSize.SevenHanded.LowerRight, Left = 454, Top = 514 };
            sevenButtons[TableSize.SevenHanded.Bottom] = new PlayerButton { SeatName = TableSize.SevenHanded.Bottom, Left = 239, Top = 703 };

            eightButtons[TableSize.EightHanded.Top] = new PlayerButton { SeatName = TableSize.EightHanded.Top, Left = 250, Top = 40 };
            eightButtons[TableSize.EightHanded.UpperLeft] = new PlayerButton { SeatName = TableSize.EightHanded.UpperLeft, Left = 35, Top = 195 };
            eightButtons[TableSize.EightHanded.UpperRight] = new PlayerButton { SeatName = TableSize.EightHanded.UpperRight, Left = 454, Top = 195 };
            eightButtons[TableSize.EightHanded.MiddleLeft] = new PlayerButton { SeatName = TableSize.EightHanded.MiddleLeft, Left = 35, Top = 363 };
            eightButtons[TableSize.EightHanded.MiddleRight] = new PlayerButton { SeatName = TableSize.EightHanded.MiddleRight, Left = 454, Top = 363 };
            eightButtons[TableSize.EightHanded.LowerLeft] = new PlayerButton { SeatName = TableSize.EightHanded.LowerLeft, Left = 35, Top = 541 };
            eightButtons[TableSize.EightHanded.LowerRight] = new PlayerButton { SeatName = TableSize.EightHanded.LowerRight, Left = 454, Top = 541 };
            eightButtons[TableSize.EightHanded.Bottom] = new PlayerButton { SeatName = TableSize.EightHanded.Bottom, Left = 239, Top = 703 };

            nineButtons[TableSize.NineHanded.TopLeft] = new PlayerButton { SeatName = TableSize.NineHanded.TopLeft, Left = 141, Top = 61 };
            nineButtons[TableSize.NineHanded.TopRight] = new PlayerButton { SeatName = TableSize.NineHanded.TopRight, Left = 350, Top = 61 };
            nineButtons[TableSize.NineHanded.UpperLeft] = new PlayerButton { SeatName = TableSize.NineHanded.UpperLeft, Left = 35, Top = 195 };
            nineButtons[TableSize.NineHanded.UpperRight] = new PlayerButton { SeatName = TableSize.NineHanded.UpperRight, Left = 454, Top = 195 };
            nineButtons[TableSize.NineHanded.MiddleLeft] = new PlayerButton { SeatName = TableSize.NineHanded.MiddleLeft, Left = 35, Top = 363 };
            nineButtons[TableSize.NineHanded.MiddleRight] = new PlayerButton { SeatName = TableSize.NineHanded.MiddleRight, Left = 454, Top = 363 };
            nineButtons[TableSize.NineHanded.LowerLeft] = new PlayerButton { SeatName = TableSize.NineHanded.LowerLeft, Left = 35, Top = 541 };
            nineButtons[TableSize.NineHanded.LowerRight] = new PlayerButton { SeatName = TableSize.NineHanded.LowerRight, Left = 454, Top = 541 };
            nineButtons[TableSize.NineHanded.Bottom] = new PlayerButton { SeatName = TableSize.NineHanded.Bottom, Left = 239, Top = 703 };
        }

        public Dictionary<string, PlayerButton> SixHanded()
        {
            return sixButtons;
        }

        public Dictionary<string, PlayerButton> SevenHanded()
        {
            return sevenButtons;
        }

        public Dictionary<string, PlayerButton> EightHanded()
        {
            return eightButtons;
        }

        public Dictionary<string, PlayerButton> NineHanded()
        {
            return nineButtons;
        }

    }
}


