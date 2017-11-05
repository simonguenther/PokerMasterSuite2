using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Defines Constant String names for individual positions on every table
    /// (Top, TopLeft, TopRight,...)
    /// </summary>
    public static class TableSize
    {
        /// <summary>
        /// SixHanded Template
        /// </summary>
        public static class SixHanded
        {
            public const string Top = "Top";
            public const string UpperLeft = "UpperLeft";
            public const string UpperRight = "UpperRight";
            public const string LowerLeft = "LowerLeft";
            public const string LowerRight = "LowerRight";
            public const string Bottom = "Bottom";

            /// <summary>
            /// Retrieve constant seatnames
            /// </summary>
            /// <returns></returns>
            public static string[] getAll()
            {
                string[] all = new string[6];
                all[0] = Top;
                all[1] = UpperLeft;
                all[2] = UpperRight;
                all[3] = LowerLeft;
                all[4] = LowerRight;
                all[5] = Bottom;
                return all;
            }
        }

        /// <summary>
        /// SevenHanded template
        /// </summary>
        public static class SevenHanded
        {
            public const string TopLeft = "TopLeft";
            public const string TopRight = "TopRight";
            public const string UpperLeft = "UpperLeft";
            public const string UpperRight = "UpperRight";
            public const string LowerLeft = "LowerLeft";
            public const string LowerRight = "LowerRight";
            public const string Bottom = "Bottom";

            /// <summary>
            /// Retrieve constant seatnames
            /// </summary>
            /// <returns></returns>
            public static string[] getAll()
            {
                string[] all = new string[7];
                all[0] = TopLeft;
                all[1] = TopRight;
                all[2] = UpperLeft;
                all[3] = UpperRight;
                all[4] = LowerLeft;
                all[5] = LowerRight;
                all[6] = Bottom;
                return all;
            }
        }

        /// <summary>
        /// Eighthanded Template
        /// </summary>
        public static class EightHanded
        {
            public const string Top = "Top";
            public const string UpperLeft = "UpperLeft";
            public const string UpperRight = "UpperRight";
            public const string MiddleLeft = "MiddleLeft";
            public const string MiddleRight = "MiddleRight";
            public const string LowerLeft = "LowerLeft";
            public const string LowerRight = "LowerRight";
            public const string Bottom = "Bottom";

            /// <summary>
            /// Retrieve constant seatnames
            /// </summary>
            /// <returns></returns>
            public static string[] getAll()
            {
                string[] all = new string[8];
                all[0] = Top;
                all[1] = UpperLeft;
                all[2] = UpperRight;
                all[3] = MiddleLeft;
                all[4] = MiddleRight;
                all[5] = LowerLeft;
                all[6] = LowerRight;
                all[7] = Bottom;
                return all;
            }
        }

        /// <summary>
        ///  NineHanded Template
        /// </summary>
        public static class NineHanded
        {
            public const string TopLeft = "TopLeft";
            public const string TopRight = "TopRight";
            public const string UpperLeft = "UpperLeft";
            public const string UpperRight = "UpperRight";
            public const string MiddleLeft = "MiddleLeft";
            public const string MiddleRight = "MiddleRight";
            public const string LowerLeft = "LowerLeft";
            public const string LowerRight = "LowerRight";
            public const string Bottom = "Bottom";

            /// <summary>
            /// Retrieve constant seatnames
            /// </summary>
            /// <returns></returns>
            public static string[] getAll()
            {
                string[] all = new string[9];
                all[0] = TopLeft;
                all[1] = TopRight;
                all[2] = UpperLeft;
                all[3] = UpperRight;
                all[4] = MiddleLeft;
                all[5] = MiddleRight;
                all[6] = LowerLeft;
                all[7] = LowerRight;
                all[8] = Bottom;
                return all;
            }
        }
    }


}
