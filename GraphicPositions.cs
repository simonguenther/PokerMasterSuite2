using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Preset/Hardcoded positions of both, OCR-Rectangle for reading playernames as well as Avatar-Rectangle 
    /// </summary>
    public class GraphicPositions
    {
         /// <summary>
         /// 6 max - SIX HANDED >> OCR and Avatar positions
         /// </summary>
        private Dictionary<String, Rectangle> ocrRectangle_SIX = new Dictionary<string, Rectangle>();
        Rectangle SIX_ocrTop = new Rectangle(240, 65, 113, 22);
        Rectangle SIX_ocrUpperLeft = new Rectangle(34, 254, 119, 25);
        Rectangle SIX_ocrUpperRight = new Rectangle(447, 254, 116, 25);
        Rectangle SIX_ocrLowerLeft = new Rectangle(34, 556, 114, 23);
        Rectangle SIX_ocrLowerRight = new Rectangle(447, 556, 116, 23);
        Rectangle SIX_ocrBottom = new Rectangle(237, 726, 130, 25);

        private Dictionary<string, Rectangle> avatar_6max;
        private Rectangle SIX_avatarTop = new Rectangle(265, 85, 65, 65);
        private Rectangle SIX_avatarTopLeft = new Rectangle(44, 279, 65, 65);
        private Rectangle SIX_avatarUpperRight = new Rectangle(490, 276, 65, 65);
        private Rectangle SIX_avatarLowerRight = new Rectangle(490, 579, 65, 65);
        private Rectangle SIX_avatarLowerLeft = new Rectangle(44, 579, 65, 65);
        private Rectangle SIX_avatarBottom = new Rectangle(266, 751, 65, 65);


        /// <summary>
        /// 7 max - SEVEN HANDED >> OCR and Avatar positions
        /// </summary>
        private Dictionary<string, Rectangle> ocrRectangle_SEVEN = new Dictionary<string, Rectangle>();
        Rectangle SEVEN_ocrTL = new Rectangle(151, 84, 120, 23);
        Rectangle SEVEN_ocrTR = new Rectangle(336, 84, 120, 23);
        Rectangle SEVEN_ocrUL = new Rectangle(35, 261, 120, 23); // upper middle left
        Rectangle SEVEN_ocrUR = new Rectangle(441, 261, 120, 23); // upper middle right
        Rectangle SEVEN_ocrLL = new Rectangle(35, 544, 120, 23); // lower middle left
        Rectangle SEVEN_ocrLR = new Rectangle(465, 544, 120, 23); // lower middle left
        Rectangle SEVEN_ocrBottom = new Rectangle(247, 729, 111, 22);

        private Dictionary<string, Rectangle> avatar_7max;
        private Rectangle SEVEN_avatarTL = new Rectangle(173, 105, 65, 65); // Top Left
        private Rectangle SEVEN_avatarTR = new Rectangle(362, 105, 65, 65); // Top Right
        private Rectangle SEVEN_avatarUL = new Rectangle(44, 282, 65, 65); // upper middle left
        private Rectangle SEVEN_avatarUR = new Rectangle(491, 281, 65, 65); // upper middle right
        private Rectangle SEVEN_avatarLR = new Rectangle(491, 565, 65, 65); // lower middle right
        private Rectangle SEVEN_avatarLL = new Rectangle(44, 565, 65, 65); // lower middle right
        private Rectangle SEVEN_avatarBottom = new Rectangle(266, 751, 65, 65);


        /// <summary>
        /// 8 max - EIGHT HANDED >> OCR and Avatar positions
        /// </summary>
        private Dictionary<String, Rectangle> ocrRectangle_EIGHT = new Dictionary<string, Rectangle>();
        Rectangle EIGHT_ocrTop = new Rectangle(240, 65, 113, 22);
        Rectangle EIGHT_ocrUpperLeft = new Rectangle(35, 218, 117, 24);
        Rectangle EIGHT_ocrUpperRight = new Rectangle(467, 216, 100, 26);
        Rectangle EIGHT_ocrMiddleLeft = new Rectangle(34, 390, 106, 21);
        Rectangle EIGHT_ocrMiddleRight = new Rectangle(460, 391, 103, 21);
        Rectangle EIGHT_ocrLowerLeft = new Rectangle(34, 564, 106, 21);
        Rectangle EIGHT_ocrLowerRight = new Rectangle(466, 558, 100, 28);
        Rectangle EIGHT_ocrBottom = new Rectangle(240, 727, 113, 24);

        private Dictionary<string, Rectangle> avatar_8max = new Dictionary<string, Rectangle>();
        private Rectangle EIGHT_avatarTop = new Rectangle(266, 85, 65, 65);
        private Rectangle EIGHT_avatarUpperLeft = new Rectangle(42, 241, 65, 65);
        private Rectangle EIGHT_avatarUpperRight = new Rectangle(489, 241, 65, 65);
        private Rectangle EIGHT_avatarMiddleLeft = new Rectangle(42, 409, 65, 65);
        private Rectangle EIGHT_avatarMiddleRight = new Rectangle(489, 409, 65, 65);
        private Rectangle EIGHT_avatarLowerLeft = new Rectangle(42, 583, 65, 65);
        private Rectangle EIGHT_avatarLowerRight = new Rectangle(489, 583, 65, 65);
        private Rectangle EIGHT_avatarBottom = new Rectangle(266, 751, 65, 65);

        /// <summary>
        /// 9 max - NINE HANDED >> OCR and Avatar positions
        /// </summary>
        private Dictionary<String, Rectangle> ocrRectangle_NINE = new Dictionary<string, Rectangle>();
        Rectangle NINE_ocrTopLeft = new Rectangle(151, 84, 120, 23);
        Rectangle NINE_ocrTopRight = new Rectangle(336, 84, 120, 23);
        Rectangle NINE_ocrUpperLeft = new Rectangle(35, 218, 117, 24);
        Rectangle NINE_ocrUpperRight = new Rectangle(467, 216, 100, 26);
        Rectangle NINE_ocrMiddleLeft = new Rectangle(34, 390, 106, 21);
        Rectangle NINE_ocrMiddleRight = new Rectangle(460, 391, 103, 21);
        Rectangle NINE_ocrLowerLeft = new Rectangle(34, 564, 106, 21);
        Rectangle NINE_ocrLowerRight = new Rectangle(466, 558, 100, 28);
        Rectangle NINE_ocrBottom = new Rectangle(240, 727, 113, 24);

        private Dictionary<string, Rectangle> avatar_9max;
        private Rectangle NINE_avatarTopLeft = new Rectangle(173, 105, 65, 65); // Top Left
        private Rectangle NINE_avatarTopRight = new Rectangle(362, 105, 65, 65); // Top Right
        private Rectangle NINE_avatarUpperLeft = new Rectangle(42, 241, 65, 65);
        private Rectangle NINE_avatarUpperRight = new Rectangle(489, 241, 65, 65);
        private Rectangle NINE_avatarMiddleLeft = new Rectangle(42, 409, 65, 65);
        private Rectangle NINE_avatarMiddleRight = new Rectangle(489, 409, 65, 65);
        private Rectangle NINE_avatarLowerLeft = new Rectangle(42, 583, 65, 65);
        private Rectangle NINE_avatarLowerRight = new Rectangle(489, 583, 65, 65);
        private Rectangle NINE_avatarBottom = new Rectangle(266, 751, 65, 65);


        /// <summary>
        /// Initialize ocr and avatar Dicitionaries
        /// </summary>
        public GraphicPositions()
        {
            // 6max OCR Rectangles
            ocrRectangle_SIX[TableSize.SixHanded.Top] = SIX_ocrTop;
            ocrRectangle_SIX[TableSize.SixHanded.UpperLeft] = SIX_ocrUpperLeft;
            ocrRectangle_SIX[TableSize.SixHanded.UpperRight] = SIX_ocrUpperRight;
            ocrRectangle_SIX[TableSize.SixHanded.LowerLeft] = SIX_ocrLowerLeft;
            ocrRectangle_SIX[TableSize.SixHanded.LowerRight] = SIX_ocrLowerRight;
            ocrRectangle_SIX[TableSize.SixHanded.Bottom] = SIX_ocrBottom;

            // 6max Avatar Rectangles
            avatar_6max = new Dictionary<string, Rectangle>();
            avatar_6max[TableSize.SixHanded.Top] = SIX_avatarTop;
            avatar_6max[TableSize.SixHanded.UpperLeft] = SIX_avatarTopLeft;
            avatar_6max[TableSize.SixHanded.UpperRight] = SIX_avatarUpperRight;
            avatar_6max[TableSize.SixHanded.LowerLeft] = SIX_avatarLowerLeft;
            avatar_6max[TableSize.SixHanded.LowerRight] = SIX_avatarLowerRight;
            avatar_6max[TableSize.SixHanded.Bottom] = SIX_avatarBottom;

            // 7max OCR Rectangles
            ocrRectangle_SEVEN[TableSize.SevenHanded.TopLeft] = SEVEN_ocrTL;
            ocrRectangle_SEVEN[TableSize.SevenHanded.TopRight] = SEVEN_ocrTR;
            ocrRectangle_SEVEN[TableSize.SevenHanded.UpperLeft] = SEVEN_ocrUL;
            ocrRectangle_SEVEN[TableSize.SevenHanded.UpperRight] = SEVEN_ocrUR;
            ocrRectangle_SEVEN[TableSize.SevenHanded.LowerLeft] = SEVEN_ocrLL;
            ocrRectangle_SEVEN[TableSize.SevenHanded.LowerRight] = SEVEN_ocrLR;
            ocrRectangle_SEVEN[TableSize.SevenHanded.Bottom] = SEVEN_ocrBottom;

            // 7max Avatar Rectangles
            avatar_7max = new Dictionary<string, Rectangle>();
            avatar_7max[TableSize.SevenHanded.TopLeft] = SEVEN_avatarTL;
            avatar_7max[TableSize.SevenHanded.TopRight] = SEVEN_avatarTR;
            avatar_7max[TableSize.SevenHanded.UpperLeft] = SEVEN_avatarUL;
            avatar_7max[TableSize.SevenHanded.UpperRight] = SEVEN_avatarUR;
            avatar_7max[TableSize.SevenHanded.LowerRight] = SEVEN_avatarLR;
            avatar_7max[TableSize.SevenHanded.LowerLeft] = SEVEN_avatarLL;
            avatar_7max[TableSize.SevenHanded.Bottom] = SEVEN_avatarBottom;

            // 8max OCR Rectangles
            ocrRectangle_EIGHT[TableSize.EightHanded.Top] = EIGHT_ocrTop;
            ocrRectangle_EIGHT[TableSize.EightHanded.UpperLeft] = EIGHT_ocrUpperLeft;
            ocrRectangle_EIGHT[TableSize.EightHanded.UpperRight] = EIGHT_ocrUpperRight;
            ocrRectangle_EIGHT[TableSize.EightHanded.MiddleLeft] = EIGHT_ocrMiddleLeft;
            ocrRectangle_EIGHT[TableSize.EightHanded.MiddleRight] = EIGHT_ocrMiddleRight;
            ocrRectangle_EIGHT[TableSize.EightHanded.LowerLeft] = EIGHT_ocrLowerLeft;
            ocrRectangle_EIGHT[TableSize.EightHanded.LowerRight] = EIGHT_ocrLowerRight;
            ocrRectangle_EIGHT[TableSize.EightHanded.Bottom] = EIGHT_ocrBottom;

            // 8max Avatar Rectangles
            avatar_8max[TableSize.EightHanded.Top] = EIGHT_avatarTop;
            avatar_8max[TableSize.EightHanded.UpperLeft] = EIGHT_avatarUpperLeft;
            avatar_8max[TableSize.EightHanded.UpperRight] = EIGHT_avatarUpperRight;
            avatar_8max[TableSize.EightHanded.MiddleLeft] = EIGHT_avatarMiddleLeft;
            avatar_8max[TableSize.EightHanded.MiddleRight] = EIGHT_avatarMiddleRight;
            avatar_8max[TableSize.EightHanded.LowerLeft] = EIGHT_avatarLowerLeft;
            avatar_8max[TableSize.EightHanded.LowerRight] = EIGHT_avatarLowerRight;
            avatar_8max[TableSize.EightHanded.Bottom] = EIGHT_avatarBottom;

            // 8max OCR Rectangles
            ocrRectangle_NINE[TableSize.NineHanded.TopLeft] = NINE_ocrTopLeft;
            ocrRectangle_NINE[TableSize.NineHanded.TopRight] = NINE_ocrTopRight;
            ocrRectangle_NINE[TableSize.NineHanded.UpperLeft] = NINE_ocrUpperLeft;
            ocrRectangle_NINE[TableSize.NineHanded.UpperRight] = NINE_ocrUpperRight;
            ocrRectangle_NINE[TableSize.NineHanded.MiddleLeft] = NINE_ocrMiddleLeft;
            ocrRectangle_NINE[TableSize.NineHanded.MiddleRight] = NINE_ocrMiddleRight;
            ocrRectangle_NINE[TableSize.NineHanded.LowerLeft] = NINE_ocrLowerLeft;
            ocrRectangle_NINE[TableSize.NineHanded.LowerRight] = NINE_ocrLowerRight;
            ocrRectangle_NINE[TableSize.NineHanded.Bottom] = NINE_ocrBottom;

            // 9max Avatar Rectangles
            avatar_9max = new Dictionary<string, Rectangle>();
            avatar_9max[TableSize.NineHanded.TopLeft] = NINE_avatarTopLeft;
            avatar_9max[TableSize.NineHanded.TopRight] = NINE_avatarTopRight;
            avatar_9max[TableSize.NineHanded.UpperLeft] = NINE_avatarUpperLeft;
            avatar_9max[TableSize.NineHanded.UpperRight] = NINE_avatarUpperRight;
            avatar_9max[TableSize.NineHanded.MiddleLeft] = NINE_avatarMiddleLeft;
            avatar_9max[TableSize.NineHanded.MiddleRight] = NINE_avatarMiddleRight;
            avatar_9max[TableSize.NineHanded.LowerLeft] = NINE_avatarLowerLeft;
            avatar_9max[TableSize.NineHanded.LowerRight] = NINE_avatarLowerRight;
            avatar_9max[TableSize.NineHanded.Bottom] = NINE_avatarBottom;

        }

        /// <summary>
        /// Creates Dictionary array which contains OCR and Avatar positions
        /// </summary>
        /// <returns>Dictionary-Array [0] => OCR Positions, [1] => Avatar Positions</returns>
        private Dictionary<string, Rectangle>[] SixHanded()
        {
            Dictionary<string, Rectangle>[] positions = new Dictionary<string, Rectangle>[2];
            positions[0] = ocrRectangle_SIX;
            positions[1] = avatar_6max;
            return positions;
        }

        /// <summary>
        /// Creates Dictionary array which contains OCR and Avatar positions
        /// </summary>
        /// <returns>Dictionary-Array [0] => OCR Positions, [1] => Avatar Positions</returns>
        private Dictionary<string, Rectangle>[] SevenHanded()
        {
            Dictionary<string,Rectangle>[] positions = new Dictionary<string, Rectangle>[2];
            positions[0] = ocrRectangle_SEVEN;
            positions[1] = avatar_7max;
            return positions;
        }

        /// <summary>
        /// Creates Dictionary array which contains OCR and Avatar positions
        /// </summary>
        /// <returns>Dictionary-Array [0] => OCR Positions, [1] => Avatar Positions</returns>
        private Dictionary<string, Rectangle>[] EightHanded()
        {
            Dictionary<string, Rectangle>[] positions = new Dictionary<string, Rectangle>[2];
            positions[0] = ocrRectangle_EIGHT;
            positions[1] = avatar_8max;
            return positions;
        }

        /// <summary>
        /// Creates Dictionary array which contains OCR and Avatar positions
        /// </summary>
        /// <returns>Dictionary-Array [0] => OCR Positions, [1] => Avatar Positions</returns>
        private Dictionary<string, Rectangle>[] NineHanded()
        {
            Dictionary<string, Rectangle>[] positions = new Dictionary<string, Rectangle>[2];
            positions[0] = ocrRectangle_NINE;
            positions[1] = avatar_9max;
            return positions;
        }

        /// <summary>
        /// Public interface for connection with other classes
        /// </summary>
        /// <param name="numberOfSeats">Number of Seats on the table</param>
        /// <returns>Dictionary-Array [0] => OCR Positions, [1] => Avatar Positions</returns>
        public Dictionary<string, Rectangle>[] getGraphicPositions(int numberOfSeats)
        {
            Dictionary<string, Rectangle>[] positions = new Dictionary<string, Rectangle>[2];
            switch(numberOfSeats)
            {
                case 6: return SixHanded();
                case 7: return SevenHanded();
                case 8: return EightHanded();
                case 9: return NineHanded();

            }
            throw new Exception("Unhandled number of Table positions in Graphics");
        }
    }
}
