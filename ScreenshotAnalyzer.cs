using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Handles everything related to cropping Tablescreenshots to relevant parts of the screen (nick, avatar) 
    /// </summary>
        /*
         * 1. Get Positiontemplate based on numberOfSeats   < check
         * 2. Crop Screenshot to relevant parts             < check
         * 3. OCR                                           < check
         * 4. Build Dictionary output                       < check
         */

    class ScreenshotAnalyzer
    {
        Dictionary<string, RawSeatInfo> rawInfo;
        int tableSize;
        Bitmap screenshot;
        Dictionary<string, Rectangle> nicknamePositions;
        Dictionary<string, Rectangle> avatarPositions;
        
        /// <summary>
        /// Initialize with info provided
        /// </summary>
        /// <param name="ocrEngine">TesseractEngine Instance</param>
        /// <param name="screen">TableScreenshot</param>
        /// <param name="seats">Stack of Seats which will get analyzed (f.e. only unlocked seats)</param>
        /// <param name="numberOfSeats">Tablesize (important for picking correct positions)</param>
        public ScreenshotAnalyzer(TesseractEngine ocrEngine, Bitmap screen, Stack<string> seats, int numberOfSeats)
        {
            screenshot = screen;
            tableSize = numberOfSeats;
            GraphicPositions graphics = new GraphicPositions();
            // 0 = positions of nickname rectangles for tablesize
            // 1 = positions of avatar rectangles for tablesize
            Dictionary<string, Rectangle>[] positions = graphics.getGraphicPositions(tableSize);
            nicknamePositions = positions[0];
            avatarPositions = positions[1];

            // Construct raw info from provided Seat-Stack
            rawInfo = constructRawSeatinfo(seats);
            // Perform OCR 
            rawInfo = runOpticalCharacterRecognition(ocrEngine, rawInfo);
        }

        /// <summary>
        /// Retrieves avatar for specific seatname (Cropping to relevant part)
        /// </summary>
        /// <param name="screen">Tablescreenshot</param>
        /// <param name="seatname">Seatname in focus</param>
        /// <param name="numberOfSeats">Tablesize (pick correct positions)</param>
        /// <returns>Avatar Image</returns>
        public static Bitmap getSingleAvatar(Bitmap screen, string seatname, int numberOfSeats) // Return new avatar/Update avatar
        {
            GraphicPositions graphics = new GraphicPositions();
            Dictionary<string, Rectangle>[] positions = graphics.getGraphicPositions(numberOfSeats);
            Rectangle singleAvatar = positions[1][seatname];
            return Screenshot.CropImage(screen, singleAvatar);
        }


        /// <summary>
        /// Cuts main screenshot into pieces for avatar and nickname rectangles
        /// </summary>
        /// <param name="analyzeSeats">Seats which will get analyzed</param>
        /// <returns>Complete RawInfo (cropped images for every seat in focus) for the table</returns>
        private Dictionary<string,RawSeatInfo> constructRawSeatinfo(Stack<string> analyzeSeats)
        {
            Dictionary<string, RawSeatInfo> rawInfoDict = new Dictionary<string, RawSeatInfo>();

            foreach(string seat in analyzeSeats)
            {
                Bitmap nickScreen = Screenshot.CropImage(screenshot, nicknamePositions[seat]);
                Bitmap avatarScreen = Screenshot.CropImage(screenshot, avatarPositions[seat]);
                rawInfoDict.Add(seat, new RawSeatInfo(nickScreen, avatarScreen));
            }
            return rawInfoDict;
        }

        /// <summary>
        /// Runs OCR on nickname bitmaps
        /// </summary>
        /// <param name="tesseractEngine">Tesseract Engine</param>
        /// <param name="dict">RawInfo requested seats</param>
        /// <returns>Updated RawInfo - now with OCRd nickname</returns>
        private Dictionary<string, RawSeatInfo> runOpticalCharacterRecognition(TesseractEngine tesseractEngine, Dictionary<string,RawSeatInfo> dict)
        {
            // OCR
            // Place output in RawSeatinfoOCR and return
            foreach(KeyValuePair<string,RawSeatInfo> kvp in dict)
            {
                kvp.Value.nicknameSTRING = OpticalCharacterRecognition.ocr(tesseractEngine, kvp.Value.nicknameBMP); 
            }
            return dict;
        }

        // Return nickname (string & bitmap) and avatar bitmap
        public Dictionary<string, RawSeatInfo> getRawSeatInfo()
        {
            return rawInfo;
        }

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="dd"></param>
        private void printDictionary(Dictionary<string, RawSeatInfo> dd)
        {
            Console.WriteLine("PRINT");
            foreach (KeyValuePair<string, RawSeatInfo> kvp in dd)
            {
                 Console.WriteLine(kvp.Key + "\t" + kvp.Value.nicknameSTRING);
            }
        }

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="dd"></param>
        private void printDictionary(Dictionary<string, Rectangle> dd)
        {
            Console.WriteLine("PRINT");
            foreach (KeyValuePair<string, Rectangle> kvp in dd)
            {
                Console.WriteLine(kvp.Key + "\t" + kvp.Value.ToString());
            }
        }

    }
}
