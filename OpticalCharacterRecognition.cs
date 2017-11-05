using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Holds all necessary methods for OCRing information from tablescreenshots
    /// </summary>
    public static class OpticalCharacterRecognition
    {
        /// <summary>
        /// Main OCR function (based on Google's open TesseractEngine)
        /// </summary>
        /// <param name="engine">Tesseract Instance</param>
        /// <param name="bmp">Image in question</param>
        /// <returns>ocr'd string from image</returns>
        public static String ocr(TesseractEngine engine, Bitmap bmp)
        {
            // Scale size of bitmap to increase ocr quality
            int width = bmp.Width * 5;
            int height = bmp.Height * 5;

            Bitmap newImage = new Bitmap(width, height);

            // Enhance quality of image
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(bmp, new Rectangle(0, 0, width, height));
            }

            var ocrtext = string.Empty;

            // Turn colored image into grayscale Image (improves OCR success percentage)
            using (var img = PixConverter.ToPix(MakeGrayscale3(newImage)))
            {
                // Read Text from Image
                using (var page = engine.Process(img))
                {
                    ocrtext = page.GetText();
                }
            }
            // Get Rid off whitespaces and newlines (often too many whitespaces included)
            return ocrtext.ToString().Replace(" ", "").Replace("\n", "");
        }


        /// <summary>
        /// Turn colord image into grayscale image
        /// </summary>
        /// <param name="original">color image</param>
        /// <returns>grey image</returns>
        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            float brightness = 1.0f; // no change in brightness
            float contrast = 3.0f; // twice the contrast
            float gamma = 1.0f; // no change in gamma
            float adjustedBrightness = brightness - 1.0f;

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
                 //new float[] {contrast, .3f, .3f, 0, 0}, // scale red
                 //new float[] {.59f, contrast, .59f, 0, 0}, // scale green
                 //new float[] {.11f, .11f, contrast, 0, 0}, // scale blue
                 //new float[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
                 //new float[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new imaeg
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }


    }
}
