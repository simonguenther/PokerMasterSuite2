using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Recieves open windows from operating system and returns a list of pointers to Emulator Tables based on a regular expression (like placemint)
    /// Using WinAPI-Calls
    /// </summary>
    class EmulatorHandler
    {
        /// <summary>
        /// Init handle-list and set regular expression of emulator windows
        /// </summary>
        /// <param name="regExpression"></param>
        public EmulatorHandler(string regExpression)
        {
            regex = regExpression;
            emulatorHandles = new List<IntPtr>();
        }

        /// <summary>
        /// Holds all open windows 
        /// </summary>
        public List<IntPtr> emulatorHandles { get; set; }

        /// <summary>
        /// Get Windows from API
        /// </summary>
        private void getWindows()
        {
            WinAPI.EnumWindows(new WinAPI.WinCallBack(EnumWindowCallBack), 0);
        }

        public void setRegex(string re) { regex = re; }
        string regex;

        /// <summary>
        /// Gets called by Delegate function WinCallBack
        /// interates through all open windows
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private bool EnumWindowCallBack(int hwnd, int lParam)
        {
            IntPtr windowHandle = (IntPtr)hwnd;
            StringBuilder sb = new StringBuilder(1024);

            // Get Title of window & check if empty
            WinAPI.GetWindowText((int)windowHandle, sb, sb.Capacity);
            if (sb.Length > 0)
            {
                // Check if regex matches and add to open emulator windows list
                Match preg = Regex.Match(sb.ToString(), regex, RegexOptions.IgnoreCase);
                if (preg.Success)
                {
                    emulatorHandles.Add(windowHandle);
                }
            }
            return true;
        }

        /// <summary>
        /// Returns list with all open emulator windows (aka tables)
        /// </summary>
        /// <returns>List of Emulator pointers</returns>
        public List<IntPtr> getEmulatorList()
        {
            getWindows();
            return emulatorHandles;
        }
        
    }
}
