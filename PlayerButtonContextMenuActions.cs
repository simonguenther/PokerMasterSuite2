using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Handles all the actions which belong to the context menu of each PlayerButton (representing the player on the screen)
    /// </summary>
    class PlayerButtonContextMenuActions
    {
        /// <summary>
        /// Lock Seat - Since we usually need to re-scan the table (because of those call/fold/raise bubbles) in order to get info on ALL players, 
        ///             we need to lock those recognized players where everything is fine in order to prevent re-scanning them
        ///             (Especially if we had to adjust the nickname because of some OCR mistake)
        /// </summary>
        /// <param name="seatName">Which seat should get locked?</param>
        /// <param name="tableName">Which table are we talking about?</param>
        public static void lockSeat(string seatName, string tableName)
        {
            IntPtr tableHandle = IntPtr.Zero;
            TableData td = new TableData(7);

            // Retrieve active tables from tablehandler and check for each active table
            foreach (KeyValuePair<IntPtr, TableData> kvp in TableHandler.tableSessions)
            {
                td = kvp.Value;
                // find requested session/table
                if (td.tablename.Equals(tableName))
                {
                    // Retrieve pointer and pass it to lock-Method in table Handler
                    tableHandle = td.tablePointer;
                    td.lockSeat(seatName);
                    break;
                }
            }
            
            // Find corresponding PlayerButtonHandler and set Lock image in HUD
            foreach (KeyValuePair<string, PlayerButtonHandler> kvp in TableHandler.buttonInventory)
            {
                if (kvp.Key.Equals(td.tablename))
                {
                    kvp.Value.setLockImage(seatName, tableHandle);
                    break;
                }
            }
        }
         
        /// <summary>
        /// Unlock locked seat (in data represantation and in graphical representation buy removing the Lock-Image)
        /// </summary>
        /// <param name="seatName"></param>
        /// <param name="tableName"></param>
        public static void unlockSeat(string seatName, string tableName)
        {
            TableData td = new TableData(7);
            foreach (KeyValuePair<IntPtr, TableData> kvp in TableHandler.tableSessions)
            {
                td = kvp.Value;
                if (td.tablename.Equals(tableName))
                {
                    td = kvp.Value;
                    td.unlockSeat(seatName);
                    break;
                }
            }
            foreach (KeyValuePair<string, PlayerButtonHandler> kvp in TableHandler.buttonInventory)
            {
                if (kvp.Key.Equals(td.tablename))
                {
                    kvp.Value.removeLockImage(seatName);
                    break;
                }
            }
        }


        /// <summary>
        /// When a new player sits on a prior occupied seat, it makes sense to completely remove every loaded info on that seat
        /// </summary>
        /// <param name="seatName"></param>
        /// <param name="tableName"></param>
        public static void removeSeatFromTable(string seatName, string tableName)
        {
            //Console.WriteLine(String.Format("Removing Seat: {0} from {1}", seatName, tableName)); // Debug
            
            // Find PlayerButtonHandler for this seat/table and remove Button, LockImage and similarNickImage
            foreach (KeyValuePair<string, PlayerButtonHandler> kvp in TableHandler.buttonInventory)
            {
                if (kvp.Key.Equals(tableName))
                {
                    kvp.Value.removeButton(seatName);
                    kvp.Value.removeLockImage(seatName);
                    kvp.Value.removeSimilarNickImage(seatName);
                }
            }

            // Remove seat from data-representation in table sessions
            foreach (KeyValuePair<IntPtr, TableData> kvp in TableHandler.tableSessions)
            {
                TableData td = kvp.Value;
                if (td.tablename.Equals(tableName))
                {
                    td.removeSeat(seatName);
                }
            }
        }

        /// <summary>
        /// Rename playername at a specific seat. Used by adjusting either manually
        /// </summary>
        /// <param name="seatname"></param>
        /// <param name="tablename"></param>
        public static void renameSeat(string seatname, string tablename)
        {
            IntPtr tableHandle = IntPtr.Zero;
            TableData td = new TableData(7); // dummy necessary to prevent empty constructor, will get overriden anyways
            string nickname;
            string newNickname;

            // Change nickname in data representation
            foreach (KeyValuePair<IntPtr, TableData> kvp in TableHandler.tableSessions)
            {
                td = kvp.Value;
                if (td.tablename.Equals(tablename))
                {
                    // Retrieve Nickname
                    nickname = td.getNickname(seatname);

                    // Show Adjustment Dialog
                    FormChangeNickManually fanm = new FormChangeNickManually(nickname);
                    newNickname = fanm.run();

                    if (newNickname == "") newNickname = nickname; // new nickname cannot be empty!
                    // change nickname in seatData (via tableData) here
                    td.setNickname(seatname, newNickname);
                    break;
                }
            }

            // Update button caption/text to new nickname
            foreach (KeyValuePair<string, PlayerButtonHandler> kvp in TableHandler.buttonInventory)
            {
                if (kvp.Key.Equals(td.tablename))
                {
                    // Change button text here
                    kvp.Value.updateButtons(td, true);
                    break;
                }
            }
        }

        /// <summary>
        /// Debug method for displaying retrieved table data
        /// </summary>
        /// <param name="tableName"></param>
        public static void displayDebugTableData(string tableName)
        {
            foreach (KeyValuePair<IntPtr, TableData> kvp in TableHandler.tableSessions)
            {
                TableData td = kvp.Value;
                if (td.tablename.Equals(tableName))
                {
                    new TableDataDebugDisplay(td);
                    break;
                }
            }
        }
    }
}
