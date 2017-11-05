using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Drawing;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Deals with everything Note-related
    /// # Loading notes from file
    /// # Saving to file
    /// # Refreshing loaded notes
    /// # Returning Note to other parts of the toolbox if needed
    /// </summary>
    class NoteHandler
    {
        /// <summary>
        /// Holds notes during runtime
        /// </summary>
        private static Dictionary<string, Note> loadedNotes;

        public NoteHandler()
        {
        }
        
        /// <summary>
        /// Loads JSON-Notefile and processes it into Dictionary loadedNotes
        /// </summary>
        public static void refreshNotesFromFile()
        {
            // Create new savefile if file does not exist
            if (!File.Exists(GlobalSettings.General.JSONpath)) loadedNotes = new Dictionary<string, Note>();
            else
            {
                // Load Notes from json file
                using (StreamReader file = File.OpenText(GlobalSettings.General.JSONpath))
                {
                    // Deserialize to Dictionary
                    JsonSerializer serializer = new JsonSerializer();
                    loadedNotes = (Dictionary<string, Note>)serializer.Deserialize(file, typeof(Dictionary<string, Note>));
                }
                Console.WriteLine(loadedNotes.Count + " Notes loaded"); // Debug
            }
        }

        /// <summary>
        /// Save Notes from loadedNotes-Dictionary to JSON-Notefile specific in GlobalSettings
        /// </summary>
        public static void saveNotesToFile()
        {
            using (StreamWriter file = File.CreateText(GlobalSettings.General.JSONpath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, loadedNotes);
            }
        }

        /// <summary>
        /// Handles and returns Note-Requests for specific playernames
        /// </summary>
        /// <param name="playername">Nickname requested</param>
        /// <returns>Either new/empty node or filled one if available in loadedNotes</returns>
        public static Note getNote(string playername)
        {
            if (loadedNotes.ContainsKey(playername))
            {
                return loadedNotes[playername];
            }
            else
            {
                return new Note();
            }
        }

        /// <summary>
        /// Saves note to Dictionary
        /// </summary>
        /// <param name="updatedNote"></param>
        public static void saveNote(Note updatedNote)
        {
            loadedNotes[updatedNote.Name] = updatedNote;
        }

        /// <summary>
        /// Deletes single notes from Dictionary (does not save!)
        /// </summary>
        /// <param name="playername"></param>
        public static void deleteNote(string playername)
        {
            loadedNotes.Remove(playername);
        }
         
        /// <summary>
        /// Search for players with certain combination of tags
        /// </summary>
        /// <param name="searchTags">Tags - can be multiple tags, separated by spaces</param>
        /// <returns>List of Notes with matching tags</returns>
        public static List<Note> searchNoteByTags(string searchTags)
        {
            // List which holds matches 
            List<Note> findings = new List<Note>();

            // check for empty tag-request
            if (searchTags.Trim() == "") return findings; // empty tags

            // split up tags by spaces to handle multiple tag requests
            string[] tags = searchTags.Split(' ');
            try
            {
                // for each Note present at this point
                foreach (Note note in loadedNotes.Values)
                {
                    // Check if note has a tag at all, skip otherwise
                    if (note.Tags == null) continue;
                    if (note.Tags.Trim() == "") continue;

                    // Split up tags retrieved from note
                    string[] savedTags = note.Tags.Split(' ');

                    // Keep track if all requested tags are matched
                    int matchCount = 0;

                    foreach (string search in tags)
                    {
                        foreach (string stored in savedTags)
                        {
                            // compare all tags from request to all tags from retrieved note
                            if (search.Equals(stored))
                            {
                                // if tags match, increase matchCount 
                                matchCount++; // needed to verify if ALL searchTags where found in a savedTag and not only one tag
                                break;
                            }
                        }
                    }

                    // After processing all loaded Notes, check if number of matches from matchCount is equal to number of initial requests
                    // Necessary to have the 'AND' combination of notes
                    if (matchCount.Equals(tags.Length)) // if matchCount equals the number of tags => display!
                    {
                        // Add to findings List as this note fullfills are necessary requested tags
                        findings.Add(note);
                    }
                }
            } catch (Exception ex) { Console.WriteLine(ex.ToString());  }

            // return all notes which match requested tags
            return findings;
        }

        /// <summary>
        /// Convert images to base64 format (gets stored in JSON)
        /// </summary>
        /// <param name="image">Avatar pictures</param>
        /// <returns>base64 encoded image</returns>
        public static String imageToBase64(Image image)
        {
            var myArray = (byte[])new ImageConverter().ConvertTo(image, typeof(byte[]));
            return Convert.ToBase64String(myArray);
        }
        
        /// <summary>
        /// Convert base64 strings to Images
        /// </summary>
        /// <param name="baseString">base64 string of encoded image</param>
        /// <returns>Avatars as image</returns>
        public static Image base64ToImage(String baseString)
        {
            if (baseString != null) return Image.FromStream(new MemoryStream(Convert.FromBase64String(baseString)));
            else return (Image)new Bitmap(65, 65);
        }

        /// <summary>
        /// Debug method for printing dictionaries
        /// </summary>
        /// <param name="dd"></param>
        private static void printDictionary(Dictionary<string, Note> dd)
        {
            Console.WriteLine("PRINT");
            string output = "PRINT\n";
            foreach (KeyValuePair<string,Note> kvp in dd)
            {
                output += String.Format("{0} \n\t\t{1}\n\t\t{2}\n", kvp.Value.Name,kvp.Value.getColor(), kvp.Value.Tags);
            }
            System.Windows.Forms.MessageBox.Show(output);
        }

        /// <summary>
        /// Check if player has a note text or only a color set!
        /// </summary>
        /// <param name="playername">nick of player</param>
        /// <returns>true/false</returns>
        public static string playerHasNote(string playername)
        {
            Note n = getNote(playername);
            return n.Text;
        }

        /// <summary>
        /// Since OCR on Chinese Nicknames can differ in quality, often the same nickname might get OCR'd slightly different (off by 1 or 2 signs f.e.)
        /// This method checks the Levenshtein Distance (Default is set to 2) from a playername vs. all playernames saved in the Note-File
        /// (This Info will later be needed when displaying similar nicknames on the table to prevent people taking couple notes on the same players)
        /// </summary>
        /// <param name="playername">Playername in question</param>
        /// <param name="maxDistance">Maximum acceptable Levenshtein Distance (default set to 2)</param>
        /// <returns>Levenshtein Distance</returns>
        public static List<Note> findSimilarNicknames(string playername, int maxDistance)
        {
            List<Note> similarNicks = new List<Note>();
            foreach(KeyValuePair<string,Note> kvp in loadedNotes)
            {
                Note n = kvp.Value;
                if (n.Name.Equals(playername)) continue;

                // Calculate Levenshtein Distance
                int distance = Helper.CalcLevenshteinDistance(playername, n.Name);

                // Check if distance is smaller than max Distance provided
                if (distance < maxDistance) similarNicks.Add(n);
            }
            return similarNicks;
        }
    }

}
