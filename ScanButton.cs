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
    /// ScanButton Object - extends regular Button 
    /// </summary>
    class ScanButton : System.Windows.Forms.Button
    {
        public IntPtr TableHandle { set; get; }
        public ScanComboBox scanComboBox { set; get; }
        public string TableName { set; get; }
        public AlertCheckBox checkBox { set; get; }

        public ScanButton()
        {
            this.Text = "Scan!";
            Left = 200;
            Top = 5;
            Width = 150;
            Height = 25;
            BackColor = Color.FromArgb(255, 30, 30, 35);
            ForeColor = Color.White;
            FlatStyle = FlatStyle.Popup;
        }

        public void SetParent(IntPtr newParent)
        {
            this.TableHandle = newParent;
            ScanButtonContextMenuStrip sbcms = (ScanButtonContextMenuStrip)this.ContextMenuStrip;
            sbcms.tableHandle = newParent;
            sbcms.parentButton = this;

            if (!TableHandle.Equals(IntPtr.Zero))
            {
                WinAPI.SetParent(this.Handle, TableHandle);
            }
        }




    }
}
