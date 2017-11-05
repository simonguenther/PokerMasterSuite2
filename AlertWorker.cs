using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Handles alerts if players has options
    /// ==> Plays a sound if buttons for actions are enabled
    /// </summary>
    class AlertWorker
    {
        static BackgroundWorker worker;
        EmulatorHandler emu;
        static SoundPlayer simpleSound = new SoundPlayer(GlobalSettings.General.AlerterSoundPath);

        /// <summary>
        /// Init & Start BackgroundWorker which monitors table for action buttons
        /// </summary>
        public AlertWorker()
        {
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            start();
        }

        /// <summary>
        /// Worker Starte
        /// </summary>
        public void start()
        {
            Console.WriteLine("AlerterWorker has started"); // Debug
            worker.RunWorkerAsync();
        }
        

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Implement some sound too?
        }


        /// <summary>
        /// Check every 1000ms if players has options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (worker.CancellationPending.Equals(false))
            {
                playerHasOptions();
                System.Threading.Thread.Sleep(1000);
            }
            Console.WriteLine("off"); // Debug
            e.Cancel = true;
        }

        /// <summary>
        /// Check for action buttons on the table with the help of preset Color in hardcoded place
        /// </summary>
        private void playerHasOptions()
        {
            // Get open tables
            emu = new EmulatorHandler(GlobalSettings.General.EmulatorRegex);
            List<IntPtr> openTables = emu.getEmulatorList();
            Color options = GlobalSettings.General.AlerterButtonColor;

            // Check for each open table...
            foreach (IntPtr pointer in openTables)
            {
                if (TableHandler.alerter.ContainsKey(pointer))
                {
                    // ...if CheckBox on top was ticked
                    bool tick = TableHandler.alerter[pointer];
                    if (!tick) continue;
                    
                    // Get Area of the table with buttons
                    WinAPI.RECT r = new WinAPI.RECT();
                    r.Width = 848;
                    r.Height = 1318;

                    // Get table screenshot and crop image roughly to the area where buttons will appear
                    Bitmap bmp = Screenshot.CaptureApplication(pointer);
                    Bitmap crop = Screenshot.CropImage(bmp, 0, Math.Abs((bmp.Height / 100) * 85), bmp.Width, 30);

                    // Check if actions are available
                    bool itsonme = checkColor(crop, options);
                    if (itsonme) // Play sound on available options
                    {
                        simpleSound.Play();
                        System.Threading.Thread.Sleep(1500);
                    }
                }
            }
        }

        /// <summary>
        /// Check if options are available
        /// </summary>
        /// <param name="bmp">Cropped image of the part of the table where buttons will appear</param>
        /// <param name="col">Average color of buttons</param>
        /// <returns></returns>
        public Boolean checkColor(Bitmap bmp, Color col)
        {
            Dictionary<String, int> test = new Dictionary<String, int>();

            // Go through each pixel and check if in range
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color check = bmp.GetPixel(x, y);
                    if (Helper.colorIsInRange(col, check,15))
                    {
                        // bmp.SetPixel(x, y, Color.Red); // Debug
                        return true;
                    }
               }
            }
            return false;
        }
    }
}
