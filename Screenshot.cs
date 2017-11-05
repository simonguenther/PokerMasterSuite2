using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Handles evereything related to Capturing and cropping Screenshots
    /// </summary>
    static class Screenshot
    {
        /// <summary>
        /// Capture Screenshot from window-pointer
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        public static Bitmap CaptureApplication(IntPtr procName)
        {
            WinAPI.RECT rect = new WinAPI.RECT();
            WinAPI.GetWindowRect(procName, out rect);
            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                bmp.SetResolution(graphics.DpiX, graphics.DpiY);
                graphics.CopyFromScreen(rect.Left, rect.Top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
                return bmp;
            }
        }

        /// <summary>
        /// Crop Image
        /// </summary>
        /// <param name="source">Big image</param>
        /// <param name="crop">Cropping size/Rectangle</param>
        /// <returns>Cropped image</returns>
        public static Bitmap CropImage(Image source, Rectangle crop)
        {
            Bitmap bmp = new Bitmap(crop.Width, crop.Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);

                return bmp;
            }
        }
        
        /// <summary>
        /// Alternative Cropping Method
        /// </summary>
        /// <param name="source"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap CropImage(Image source, int x, int y, int width, int height)
        {
            Rectangle crop = new Rectangle(x, y, width, height);

            var bmp = new Bitmap(crop.Width, crop.Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
            }

            return bmp;
        }
    }
}
