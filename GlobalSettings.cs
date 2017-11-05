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
    /// Library of settings for every part of the toolbox
    /// </summary>
    public static class GlobalSettings
    {
        /// <summary>
        /// General Preset Settings
        /// </summary>
        public static class General
        {
            public static int EmulatorWidth = 600;
            public static int EmulatorHeight = 975;
            public static string EmulatorRegex = @"PM[0-9]";
            public static readonly string JSONpath = "notes.json";
            public static int[] SupportedTableSizes = { 2, 6, 7, 8, 9 };
            public static string AlerterSoundPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "/sounds/alert.wav";
            public static Color AlerterButtonColor = Color.FromArgb(255, 45, 182, 192);

        }

        /// <summary>
        /// Preset settings for PlayerButtons
        /// </summary>
        public static class ButtonSettings
        {
            public static readonly bool HideEmptyButtons = true;
            public static readonly int Height = 25;
            public static readonly int Width = 100;
            public static readonly string ScanButtonLabel = "Scan!";
        }

        /// <summary>
        /// Init of images (lock-image, etc)
        /// </summary>
        public static class Images
        {
            public static readonly System.Drawing.Bitmap lockImage = new System.Drawing.Bitmap(AppDomain.CurrentDomain.BaseDirectory.ToString() + "/images/lock.png");
            public static readonly System.Drawing.Bitmap questionImage = new System.Drawing.Bitmap(AppDomain.CurrentDomain.BaseDirectory.ToString() + "/images/qm.png");
            public static readonly System.Drawing.Icon financeIcon = new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory.ToString() + "/images/finance.ico");
        }

        /// <summary>
        /// Note preset (colors, LevenshteinDistance, etc)
        /// </summary>
        public static class Notes
        {
            public static string[] playercolors = { "LightGreen", "Red", "LightBlue", "Yellow", "Gray", "Orange", "Pink", "Green", "Blue" };
            public static int LevenshteinMaxDistance = 2;
            public static int PlayernameMinLengthForDistanceCheck = 2;
        }

        /// <summary>
        /// Presets for Dialog when changing Nick manually
        /// </summary>
        public static class ChangeNickManually
        {
            public static readonly int Width = 150;
            public static readonly int Height = 105;
            public static readonly FormBorderStyle BorderStyle = FormBorderStyle.FixedDialog;
            public static readonly FormStartPosition StartPosition = FormStartPosition.CenterParent;
            public static readonly string Title = "Change Nickname";
        }

        /// <summary>
        /// Presets for Dialog when changing Nick by avatar
        /// </summary>
        public static class ChangeNickByAvatar
        {
            public static readonly int Width = 300;
            public static readonly int Height = 300;
            public static readonly FormBorderStyle BorderStyle = FormBorderStyle.FixedDialog;
            public static readonly string Title = "Change by avatar";
            public static readonly FormStartPosition StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// Presets seating script (sounds, interval,...)
        /// </summary>
        public static class SeatingScript
        {
            public static string StartSoundPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "/sounds/start.wav";
            public static string StopSoundPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "/sounds/stop.wav";
            public static string SuccessSoundPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "/sounds/success.wav";
            public static int Interval = 1000;
            public static Color OpenSeatColor = Color.FromArgb(255, 1, 27, 44);
            public static int SeatOpenCounts = 200;
            public static string StartSeatingScriptLabel = "Start SeatingScript";
            public static string StopSeatingScriptLabel = "Stop SeatingScript";
        }

    }
}
