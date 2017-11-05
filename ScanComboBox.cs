using System;
using System.Drawing;
using System.Windows.Forms;

namespace PokerMasterSuite2
{
    class ScanComboBox : System.Windows.Forms.ComboBox
    {
        public ScanComboBox()
        {
            this.Left = 350;
            this.Top = 7;
            this.Width = 35;
            this.Height = 25;
            this.BackColor = Color.FromArgb(255, 30, 30, 35);
            this.ForeColor = Color.White;
            this.FlatStyle = FlatStyle.Popup;

            this.Items.Add("7");
            this.Items.Add("6");
            this.Items.Add("8");
            this.Items.Add("2");
            this.Items.Add("9");

            this.SelectedIndex = 0;
        }

        public IntPtr TableHandle { set; get; }
        
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