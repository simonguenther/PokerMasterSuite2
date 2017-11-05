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
    /// This class is where the party starts as it handles every other part of the toolbox.
    /// The toolbox by design splits plain data representation (playernames, seatnames, etc) and visual components (GUI: PlayerButtons displayed on the table)
    /// All data-relevant actions (updating info on players) are handled by the "TableHandler" which has control over SeatData-Instances.
    /// All GUI-relevant actions (display Button on Table, change Color based on Colorcode of the player) are handled by PlayerButtonHandler and subsequent classes.
    /// The Class "TableHandler" is where Data and GUI-Actions are coordinated and handled
    /// </summary>
    class SessionHandler
    {
        EmulatorHandler emuHandler;     
        List<IntPtr> emulatorList;      // keeps track of all open Emulator instances
        TableHandler tableHandler;      // keeps track of all scanned tables
        TesseractEngine engine;         // Googles TesseractEngine needed for OCR
        AlertWorker alerterWorker;      // keeps track of all activated Alerters (notifies player if it is his turn)
        static Dictionary<IntPtr, ScanButton> scanButtonList;   // keeps track of all attached ScanButtons

        /// <summary>
        /// Initialize all of the above components
        /// </summary>
        public SessionHandler()
        {
            launchOCREngine();
            emuHandler = new EmulatorHandler(GlobalSettings.General.EmulatorRegex); // Only retrieve pointers from windows which match regular expression
            emulatorList = emuHandler.getEmulatorList();    
            tableHandler = new TableHandler(engine);
            NoteHandler.refreshNotesFromFile(); // Load Notes from JSON-Notefile
            alerterWorker = new AlertWorker();
            scanButtonList = new Dictionary<IntPtr, ScanButton>();
        }

        /// <summary>
        /// Check for new Emulator instances
        /// </summary>
        public void updateSession()
        {
            emulatorList = emuHandler.getEmulatorList();
        }

        /// <summary>
        /// Check if Size of Emulator windows match with the loaded/pre-defined windows size
        /// Resize window if it doesnt match the pre-defined sizes
        /// </summary>
        /// <param name="emulator"></param>
        public static void checkEmulatorSize(IntPtr emulator)
        {
            // Get window parameters
            WinAPI.RECT rect = new WinAPI.RECT();
            WinAPI.GetWindowRect(emulator, out rect);

            // Check if match and resize if necessary
            if(rect.Width != GlobalSettings.General.EmulatorWidth || rect.Height != GlobalSettings.General.EmulatorWidth)
            {
                WinAPI.MoveWindow(emulator, rect.X, rect.Y, GlobalSettings.General.EmulatorWidth, GlobalSettings.General.EmulatorHeight, true);
                System.Threading.Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Launch Google's Tesseract OCR Engine from /tessadata directory
        /// Preset languages: english, simplified and traditional chinese
        /// </summary>
        public void launchOCREngine()
        {
            string dataPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"tessdata";
            engine = new TesseractEngine(dataPath, "eng+chi_sim+chi_tra", EngineMode.Default);
        }

        /// <summary>
        /// Attach ScanButton to Emulator instances
        /// </summary>
        public void attachScanControls()
        {
            updateSession(); // check for new emulator instances
            foreach (IntPtr emulator in emulatorList)
            {
                if (scanButtonList == null) scanButtonList = new Dictionary<IntPtr, ScanButton>();

                // check if ScanButton for this emulator is already getting tracked
                if (scanButtonList.ContainsKey(emulator)) continue;
                //else Console.WriteLine("new emulator found");     // Debug

                // Check & Set Emulator Window Size
                checkEmulatorSize(emulator);

                // Init ScanButton with all necessary components (ComboBox for chosing tablesize, custom context menu, etc)
                ScanButton scanButton = new ScanButton();
                ScanComboBox scanButtonComboBox = new ScanComboBox();
                AlertCheckBox box = new AlertCheckBox();
                ScanButtonContextMenuStrip scanButtonCMS = new ScanButtonContextMenuStrip();

                scanButton.ContextMenuStrip = scanButtonCMS;
                scanButton.scanComboBox = scanButtonComboBox;
                scanButton.checkBox = box;
                scanButton.TableName = WinAPI.GetWindowTitle((int)emulator);
                scanButton.Click += ControlButton_Click;

                scanButton.SetParent(emulator);
                scanButtonComboBox.SetParent(emulator);
                box.SetParent(emulator);

                // Add Emulator/Button to tracking
                scanButtonList[emulator] = scanButton;
            }
        }

        /// <summary>
        /// Start TableModule ==> Scan Button Was clicked 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ControlButton_Click(object sender, EventArgs e)
        {
            ScanButton sb = new ScanButton();

            // Get ScanButton instance depending on where the request came from
            if (sender is ToolStripMenuItem) // From Context Menu
            {
                ToolStripMenuItem tsi = (ToolStripMenuItem)sender;
                ScanButtonContextMenuStrip pbcms = (ScanButtonContextMenuStrip)tsi.GetCurrentParent();
                sb = pbcms.parentButton;
            }
            else if (sender is ScanButton) // From LeftClick on ScanButton
            {
                sb = (ScanButton)sender;
            }
            else throw new Exception("ControlButton_Click: Unknown Sender");

            // Check Again for Size of the table
            checkEmulatorSize(sb.TableHandle);

            // Scan the table
            TableHandler.scan(sb);
        }

        /// <summary>
        /// Detach ScanButton from every Instance
        /// </summary>
        public void detachScanControls()
        {
            for(int i = 0; i < scanButtonList.Count; i++)
            {
                removeScanButton(scanButtonList.ElementAt(i).Value);
            }
            scanButtonList = null;
        }

        /// <summary>
        /// Remove ScanButton from single Instance
        /// </summary>
        /// <param name="sb"></param>
        public static void removeScanButton(ScanButton sb)
        {
            scanButtonList.Remove(sb.TableHandle);
            // bug if table hasnt been scanned
            if (!TableHandler.tableSessions.ContainsKey(sb.TableHandle))
            {
                //MessageBox.Show("Table was not scanned yet!");    // Debug
            }
            else
            {
                // Clear TableData and Button Inventory
                string tablename = TableHandler.tableSessions[sb.TableHandle].tablename;
                TableHandler.Dispose(sb.TableHandle);
                TableHandler.buttonInventory[tablename].Dispose();
            }

            // Dispose components
            sb.Dispose();
            sb.scanComboBox.Dispose();
            sb.checkBox.Dispose();
            
        }
    }
}
