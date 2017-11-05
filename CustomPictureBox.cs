using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Extends PictureBox for storing TableHandle, SimilarNickList and PlayerButton references
    /// </summary>
    public class CustomPictureBox : PictureBox
    {
        /// <summary>
        /// Stores pointer of table the picturebox is attached to
        /// </summary>
        public IntPtr TableHandle { get; private set; }

        /// <summary>
        /// Stores String-List of SmilarNicks which then are going to get displayed if someone clicks on the picturebox
        /// </summary>
        public List<Note> SimilarNicks { get; set; }

        /// <summary>
        /// PlayerButton the picturebox relates to
        /// </summary>
        public PlayerButton PlayerButton { get; set; }

        /// <summary>
        /// Setting Parent for easier referencing later on
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
