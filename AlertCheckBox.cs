using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Extension of Windows.Forms.CheckBox to meet requirements
    /// </summary>
    class AlertCheckBox : System.Windows.Forms.CheckBox
    {
        public IntPtr TableHandle { set; get; }

        /// <summary>
        /// Init with default settings 
        /// </summary>
        public AlertCheckBox()
        {
            this.Width = 20;
            this.Text = "";
            this.Left = 200 - Width+2;
            this.Top = 6;
            this.BackColor = System.Drawing.Color.FromArgb(255, 30, 30, 35);
            this.CheckedChanged += AlertCheckBox_CheckedChanged;
        }

        /// <summary>
        /// If Checked, Alerter will monitor the table if buttons will appear => Player has options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlertCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Checked)
            {
                TableHandler.alerter[TableHandle] = true;
                Console.WriteLine(String.Format("Scanning: {0}", WinAPI.GetWindowTitle((int)TableHandle)));
            }
            else
            {
                TableHandler.alerter[TableHandle] = false;
                Console.WriteLine(String.Format("Stopped: {0}", WinAPI.GetWindowTitle((int)TableHandle)));
            }
        }

        /// <summary>
        /// Sets Parent of the CheckBox
        /// Usually going to be the TablePointer
        /// </summary>
        /// <param name="newParent"></param>
        public void SetParent(IntPtr newParent)
        {
            this.TableHandle = newParent;
            if (!TableHandle.Equals(IntPtr.Zero))
            {
                WinAPI.SetParent(this.Handle, TableHandle);
            }
        }

    }
}
