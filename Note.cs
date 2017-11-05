using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Note Class Definition
    /// </summary>
    public class Note
    {
        /// <summary>
        /// Playername
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Colorcode for playername as ToString()
        /// USE getColor INSTEAD of just COLOR! only public b/c otherwise saving would not work
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Note text
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// Avatar Picture encoded in base64
        /// </summary>
        public string Avatar { get; set; }
        
        /// <summary>
        /// Playertags as string
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Constructor when opened from table
        /// </summary>
        /// <param name="name">Playername</param>
        /// <param name="color">Playercolor</param>
        /// <param name="text">Notetext</param>
        public Note(string name, string color, string text)
        {
            this.Name = name;
            this.Color = color;
            this.Text = text;
        }

        /// <summary>
        /// Dummy constructor when Note fails to load
        /// </summary>
        public Note() {
            this.Name = null;
            this.Color = "Controls";
            this.Text = null;
        }

        /// <summary>
        /// Checks for "null" and empty string in Color
        /// </summary>
        /// <returns>"Cotrols" (Windows default color) on empty/null-string or Color as string</returns>
        public string getColor()
        {
            if (Color == null || Color.Equals(""))
            {
                return "Controls";
                //throw new Exception("Damaged note-file! No color assigned for player " + Name);
            }
            return Color;
        }
    }
}
