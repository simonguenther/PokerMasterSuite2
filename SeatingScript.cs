using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokerMasterSuite2
{
    /// <summary>
    /// SeatingScript - Monitoring open table and take seat once it opens
    /// </summary>
    class SeatingScript
    {
        BackgroundSeatingWorker worker;
        SoundPlayer start = new SoundPlayer(GlobalSettings.SeatingScript.StartSoundPath);
        SoundPlayer success = new SoundPlayer(GlobalSettings.SeatingScript.SuccessSoundPath);
        SoundPlayer stop = new SoundPlayer(GlobalSettings.SeatingScript.StopSoundPath);
        
        // Worker Holder - Keep track of running workers as those show issues with not stopping if not kept track off!
        static Dictionary<IntPtr, BackgroundSeatingWorker> activeWorkers = new Dictionary<IntPtr, BackgroundSeatingWorker>();

        /// <summary>
        /// Initialize Worker
        /// </summary>
        public SeatingScript()
        {
            /*
            * What does the seating script need to start?
            * 1. Pointer to the table
            * 2. Positions of avatar rectangles since these are the spots to check on and click if open seat
            */

            worker = new BackgroundSeatingWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        /// <summary>
        /// Evaluation of seating success after worker completed/got cancelled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundSeatingWorker result = (BackgroundSeatingWorker)sender;

            if (e.Error != null)
            {
                //MessageBox.Show(e.Error.StackTrace.ToString()); // Debug
            }
            // Reset Buttons after worker got cancelled (either b/c we got a seat or something went wrong)
            else if (e.Cancelled)
            {
                // Adjust Click Delegates and Text after Seat was successfully found
                ScanButton scan = result.scanButton;
                scan.Text = GlobalSettings.ButtonSettings.ScanButtonLabel;

                // Clean up delegates and adjust accordingly
                // If seating script runs, clicking on ScanButton stops seating script
                // If seating script is off, clicking on ScanButton scans table
                RemoveClickEvent(scan);
                scan.Click += SessionHandler.ControlButton_Click;
                scan.Refresh();
                activeWorkers.Remove(scan.TableHandle);

                // Update ContextMenu for ScanButton (switch between "start seatingscript" and "stop seatingscript"
                foreach (ToolStripMenuItem tsmi in scan.ContextMenuStrip.Items)
                {
                    if (tsmi.Text.Contains("SeatingScript"))
                    {
                        tsmi.Text = GlobalSettings.SeatingScript.StartSeatingScriptLabel;
                        break;
                    }
                }
            }
            else
            {
                // No exception in DoWork.
                try
                {
                    // Seat was found!
                    success.Play();

                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Start SeatingScript
        /// </summary>
        /// <param name="button">ScanButton from table where scripts will start running (necessary to update GUI)</param>
        public void startSeating(ScanButton button)
        {
            // Init
            int tableSize = getTableSize(button);
            IntPtr pointer = button.TableHandle;
            GraphicPositions graphics = new GraphicPositions();

            // Chefk if emulator has preset size, if not => Resize
            SessionHandler.checkEmulatorSize(pointer);

            // Retrieve avatar rectangles for specific tablesize (avatar positions are seating buttons if nobody sits)
            Dictionary<string, Rectangle>[] positions = graphics.getGraphicPositions(tableSize);
            Dictionary<string, Rectangle> avatarPositions = positions[1];
            List<Rectangle> avaPosi = avatarPositions.Values.ToList();

            // BackgroundWorker needs Pointer of Table and positions of avatar rectangles to check
            // Construct parameters passed to Worker-Thread
            object[] parameters = new object[2];
            parameters[0] = pointer;
            parameters[1] = avaPosi;

            activeWorkers[pointer] = worker;
            worker.RunWorkerAsync(parameters);
            worker.scanButton = button;

            // Update ScanButton text
            button.Text = GlobalSettings.SeatingScript.StopSeatingScriptLabel;

            // Remove Click Events from ScanButton, otherwise it Scans the table before stopping the seating script
            RemoveClickEvent(button);

            // Set EventHandler on Click to Stopping SeatingScript instead of Scanning Table
            button.Click += ButtonStopSeatingScript_Click;
        }

        /// <summary>
        /// StopSeating Script Button
        /// </summary>
        /// <param name="sender">ScanButton</param>
        /// <param name="e"></param>
        private void ButtonStopSeatingScript_Click(object sender, EventArgs e)
        {
            // Stop Seating Script
            ScanButton scan = (ScanButton)sender;
            stopSeating(scan);

            // Reset ScanButton Text & Update EventHandlers (Scan Table on ScanButton.Click)
            scan.Text = GlobalSettings.ButtonSettings.ScanButtonLabel;
            RemoveClickEvent(scan);
            scan.Click += SessionHandler.ControlButton_Click;
            scan.Refresh();

            // Remove worker from active worker list
            activeWorkers.Remove(scan.TableHandle);
        }

        /// <summary>
        /// Stop SeatingScript - Accessible from rest of toolbox, handles stop-requests
        /// </summary>
        /// <param name="b">ScanButton of table which seatingscript will stop</param>
        public void stopSeating(ScanButton b)
        {
            // Get Worker-Object from active workerlist
            BackgroundWorker finishHim = activeWorkers[b.TableHandle];
            try
            {
                
                if (finishHim.WorkerSupportsCancellation)
                {
                    // Send Cancel-Command to 
                    finishHim.CancelAsync();

                    // Reset Text in ScanButton Context Menu from "Stop SeatingScript" to "Start SeatingScript"
                    foreach(ToolStripMenuItem tsmi in b.ContextMenuStrip.Items)
                    {
                        if(tsmi.Text.Contains("SeatingScript"))
                        {
                            tsmi.Text = GlobalSettings.SeatingScript.StartSeatingScriptLabel;
                        }
                    }
                    // Play Abort-Sound
                    stop.Play();
                }
                else throw new Exception("Cancellation not Supported");
            } catch(Exception ex) { System.Windows.Forms.MessageBox.Show(ex.ToString()); }
        }

        /// <summary>
        /// Try to seat every pre-set interval if seat opened up
        /// </summary>
        /// <param name="handle">Pointer of table which gets scanned</param>
        /// <param name="ava">List of avatar positions which need to get checked and eventually clicked on</param>
        /// <returns></returns>
        private bool attemptSeating(IntPtr handle, List<Rectangle> ava)
        {
            // Get new Screenshot every interval
            Bitmap asd = Screenshot.CaptureApplication(handle);

            // Check for each Avatar Position 
            foreach (Rectangle r in ava)
            {
                // Crop Image to specific avatar size/position in order to check if seat is open or not
                using (Bitmap tmp = Screenshot.CropImage(Screenshot.CaptureApplication(handle), r))
                {
                    bool escape = false;
                    int count = 0;

                    for (int x = 0; x < r.Width; x++)
                    {
                        for (int y = 0; y < r.Height; y++)
                        {
                            // Check every pixel from cropped image if it fits the range of color an open seat usually has
                            if (Helper.colorIsInRange(GlobalSettings.SeatingScript.OpenSeatColor, tmp.GetPixel(x, y),10))
                            {
                                // Keep track of how many pixels matched the open seat color
                                count++;
                                
                                // If count exceeds threshold of open seat pixels we can assume the seat is open
                                if (count > GlobalSettings.SeatingScript.SeatOpenCounts) // Seat is open
                                {
                                    escape = true;
                                    /*
                                     * Problem: If we are using the betslider on a table while a seat pops open, 
                                     *          clicking on the open seat will often move us all-in on the other table
                                     *          (yes, it cost me some money figuring that out xD -.-)
                                     * Solution: Before clicking on the seat, Check if Left MouseButton is pressed
                                     */
                                    if (WinAPI.GetAsyncKeyState(WinAPI.VK_LBUTTON) != -32767) // Left Mouse Button is NOT pressed
                                    {
                                        // click on open seat
                                        clickSeat(handle, r); 

                                        // Cancel Background Worker
                                        BackgroundSeatingWorker bgWorker = activeWorkers[handle];
                                        ScanButton sb = bgWorker.scanButton;
                                        worker.CancelAsync();

                                        // Play success-Sound
                                        success.Play();
                                        return true;
                                    }
                                    break;
                                }
                                if (escape) break;
                            }
                            if (escape) break;
                        }
                        if (escape) break;
                    }
                    if (escape) break;
                }
            }
            return false;
        }

        /// <summary>
        /// Worker Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Pointer of table which seatingscript should run on, List of Avatar-Rectangles which should get monitored</param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // get pointer from EventArguments
            object[] init = (object[])e.Argument;
            IntPtr tableHandle = (IntPtr)init[0]; 
            List<Rectangle> positions = (List<Rectangle>)init[1];

            // Play start-sound
            start.Play();

            // Infinite Loop until something cancels the worker
            // Attemp to seat every preset interval (decreasing the interval significantly will at this stage increase the lag as screenshot engine has no good performance)
            while (worker.CancellationPending == false)
            {
                attemptSeating(tableHandle, positions);
                System.Threading.Thread.Sleep(GlobalSettings.SeatingScript.Interval);
            }
            e.Cancel = true;
            e.Result = tableHandle;
        }

        /// <summary>
        /// Clicks on predefined rectangle in order to sit down
        /// </summary>
        /// <param name="table"></param>
        /// <param name="clickBox">position for click</param>
        private void clickSeat(IntPtr table, Rectangle clickBox)
        {
            // Get Table Rectangle
            Rectangle tablePosition = new Rectangle();
            WinAPI.GetWindowRect(new HandleRef(this, table), out tablePosition);

            // Save current Cursor Position
            int old_x = Cursor.Position.X;
            int old_y = Cursor.Position.Y;

            // Set Cursor on OpenSeat and Click
            WinAPI.SetCursorPos(tablePosition.Left + (clickBox.X + 20), tablePosition.Top + (clickBox.Y + 20));
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);//make left button down
            WinAPI.mouse_event(WinAPI.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);//make left button up   

            // Reset Cursor Position position before the click
            WinAPI.SetCursorPos(old_x, old_y);
        }

        /// <summary>
        /// Get TableSize from ScanButton
        /// </summary>
        /// <param name="button">ScanButton</param>
        /// <returns>TableSize</returns>
        private int getTableSize(ScanButton button)
        {
            int selectedIndex = button.scanComboBox.SelectedIndex;
            int numberOfSeats = int.Parse(button.scanComboBox.Items[selectedIndex].ToString());
            return numberOfSeats;
        }

        /// <summary>
        /// Remove Click Events from Buttons
        /// </summary>
        /// <param name="b"></param>
        private void RemoveClickEvent(Button b)
        {
            FieldInfo f1 = typeof(Control).GetField("EventClick", BindingFlags.Static | BindingFlags.NonPublic);
            object obj = f1.GetValue(b);
            PropertyInfo pi = b.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
            EventHandlerList list = (EventHandlerList)pi.GetValue(b, null);
            list.RemoveHandler(obj, list[obj]);
        }
    }
}
