using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Coordinates the interaction between Data (from TableData) and GUI (from PlayerButtonHandler)
    /// </summary>
    class TableHandler
    {
        static TesseractEngine engine;

        /// <summary>
        /// Set Tesseract Instance
        /// </summary>
        /// <param name="ocrEngine"></param>
        public TableHandler(TesseractEngine ocrEngine)
        {
            engine = ocrEngine;
        }
        
        /// <summary>
        /// Holds all scanned tables
        /// </summary>
        public static Dictionary<IntPtr, TableData> tableSessions = new Dictionary<IntPtr, TableData>();

        /// <summary>
        /// Holds PlayerButtonHandlers for every table
        /// </summary>
        public static Dictionary<string, PlayerButtonHandler> buttonInventory = new Dictionary<string, PlayerButtonHandler>();

        /// <summary>
        /// Handles Alerter activity (true = alerts, false = quiet)
        /// </summary>
        public static Dictionary<IntPtr, bool> alerter = new Dictionary<IntPtr, bool>();

        /// <summary>
        /// Requests Tabledata for certain table
        /// </summary>
        /// <param name="tablename">Tablename in focus</param>
        /// <returns>TableData Instance</returns>
        public static TableData getTableDataFromTableName(string tablename)
        {
            foreach (TableData td in tableSessions.Values)
            {
                if (td.tablename.Equals(tablename))
                {
                    return td;
                }
            }
            throw new Exception("TableData not found!");
        }

        /// <summary>
        /// Triggers Tablescan where all info will be gathered and retrieve information will get separated into Data and GUI Parts
        /// </summary>
        /// <param name="button">ScanButton of table in focus</param>
        public static void scan(ScanButton button)
        {
            IntPtr handle = button.TableHandle;
            if (handle.Equals(IntPtr.Zero)) throw new Exception("Can't scan Zero Pointer");

            // Get TableSize and check if supported by toolbox
            int tableSize = int.Parse(button.scanComboBox.SelectedItem.ToString());
            if (!GlobalSettings.General.SupportedTableSizes.Contains(tableSize)) throw new Exception("Unsupported Tablesize: " + tableSize);

            // Init TableData with tablesize
            TableData table = new TableData(tableSize);

            // Retrieve Screenshot for table
            Bitmap screenshot = Screenshot.CaptureApplication(handle);

            // Checks if tablesize between scans has changed
            // If changed, means that we have to perform a completely new table (and get rid of locked seats and stuff)
            bool tableHasSameNumberOfSeats = true;

            // Table was scanned before
            if (tableSessions.ContainsKey(handle)) 
            {
                table = tableSessions[handle];

                // if tableSize from sessions is different than new one => start over with a new table since selection has changed (we sit on a new table)
                if (!table.getTableSize().Equals(tableSize))
                {
                    table = new TableData(tableSize);
                    table.tablename = button.TableName;
                    table.tablePointer = handle;
                    tableSessions[handle] = table;
                    table.scanButton = button;
                    tableHasSameNumberOfSeats = false;
                }
            }
            else // New Table aka First Scan of a table
            {
                table.tablename = button.TableName;
                table.tablePointer = handle;
                tableSessions[handle] = table;
                table.scanButton = button;
            }
            
            // Get all unlocked Seats, as they are the only ones that should be scanned/updated from scratch
            Stack<string> scanTheseSeats = table.getUnlockedSeats();

            // Analyze those seats from screenshot
            ScreenshotAnalyzer scAnalyze = new ScreenshotAnalyzer(engine, screenshot, scanTheseSeats, tableSize);

            // Retrieve usable (aka string) information from the scan
            Dictionary<string, RawSeatInfo> unlockedSeatsInformation = scAnalyze.getRawSeatInfo();

            // Update/Set Retrieved info in TableData
            foreach (KeyValuePair<string, RawSeatInfo> kvp in unlockedSeatsInformation)
            {
                string seat = kvp.Key;
                RawSeatInfo rsi = kvp.Value;
                table.setNickname(seat, rsi.nicknameSTRING);
                table.setAvatar(seat, rsi.avatar);
            }

            // Run Button Creation at this point
            if (buttonInventory.ContainsKey(table.tablename))
            {
                if (tableHasSameNumberOfSeats) // Same table as before (probably)
                {
                    buttonInventory[table.tablename].removeUnlockedButtons(scanTheseSeats);
                    buttonInventory[table.tablename].updateButtons(table, false);
                }
                else // if there is a new table size than scan before => new table/remove all old buttons from display
                {
                    buttonInventory[table.tablename].removeAllButtons();
                    tableHasSameNumberOfSeats = true;
                    buttonInventory[table.tablename] = new PlayerButtonHandler(table);
                }
            }  else
            {
                buttonInventory[table.tablename] = new PlayerButtonHandler(table);
            }
        }

        /// <summary>
        /// Remove TableHandle from Handler
        /// </summary>
        /// <param name="tableHandle"></param>
        public static void Dispose(IntPtr tableHandle)
        {
            tableSessions.Remove(tableHandle);
        }

        /// <summary>
        /// Refresh Table Data - Used after Deleting a Note and resetting defaults
        /// Especially needed when we delete from Note Window that was not open by Click from table,
        /// but from NoteBrowser or from Click on SimilarNickName-Icon
        /// </summary>
        /// <param name="note"></param>
        /// <param name="tablename"></param>
        public static void refreshTableData(Note note, string tablename)
        {
            TableData td = TableHandler.getTableDataFromTableName(tablename);
            if (td.playerIsSeated(note.Name))
            {
                string seatname = td.getSeatname(note.Name);
                td.setColor(seatname, System.Drawing.Color.FromName(note.getColor()));
                td.setNickname(seatname, note.Name);
                td.setAvatar(seatname, (Bitmap)NoteHandler.base64ToImage(note.Avatar));
            }
        }

        /// <summary>
        /// Refresh Player Buttons - Used after Deleting a Note and resetting defaults
        /// Especially needed when we delete from Note Window that was not open by Click from table,
        /// but from NoteBrowser or from Click on SimilarNickName-Icon
        /// </summary>
        /// <param name="tablename"></param>
        public static void refreshPlayerButtons(string tablename)
        {
            PlayerButtonHandler pbh = TableHandler.buttonInventory[tablename];
            TableData td = TableHandler.getTableDataFromTableName(tablename);
            pbh.updateButtons(td,true);
        }
    }
}
