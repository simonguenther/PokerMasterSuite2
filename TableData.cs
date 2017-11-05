using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Key class of information management (updating status of seats, change playernames,...)
    /// </summary>
    class TableData
    {
        public string tablename { set; get; }           // Emulator name
        public IntPtr tablePointer { set; get; }        //
        int tableSize = 0;                              // Default tablesize set to zero
        public ScanButton scanButton { set; get; }      // Contains scan (and therefore emulator instance) where the data belongs to
        Dictionary<string, SeatData> seatList = new Dictionary<string,SeatData>(); // Handles playername, color and stuff for each respective seat

        /// <summary>
        /// Initialize TableData for different sizes of tables (2,6,7,8,9)
        /// </summary>
        /// <param name="numberOfSeats">TableSize (6max, 7max,..)</param>
        public TableData(int numberOfSeats)
        {
            // Retrieve templates for tablesize
            DictionaryTemplates dictTemplates = new DictionaryTemplates();
            seatList = dictTemplates.getDictionaryTemplate(numberOfSeats);
            tableSize = numberOfSeats;
        }

        /// <summary>
        /// Returns plain SeatList
        /// </summary>
        /// <returns>SeatList</returns>
        public Dictionary<string,SeatData> getSeatList()
        {
            return seatList;
        }

        /// <summary>
        /// Checks if seat is locked
        /// </summary>
        /// <param name="seat"></param>
        /// <returns>true/false</returns>
        public bool isSeatLocked(string seat)
        {
            if (seatList[seat].locked) return true;
            else return false;
         }

        /// <summary>
        /// Locks single seat
        /// </summary>
        /// <param name="seat"></param>
        public void lockSeat(string seat)
        {
            if (seatList[seat].nickname != null)
            {
                seatList[seat].locked = true;
            }
        }

        /// <summary>
        /// Unlocks single seat
        /// </summary>
        /// <param name="seat"></param>
        public void unlockSeat(string seat)
        {
            seatList[seat].locked = false;
        }

        /// <summary>
        /// Unlocks all Seats
        /// </summary>
        public void unlockAllSeats()
        {
            foreach(string seat in seatList.Keys)
            {
                unlockSeat(seat);
            }
        }

        /// <summary>
        /// Locks all Seats
        /// </summary>
        public void lockAllSeats()
        {
            foreach (String seatname in seatList.Keys)
            {
                lockSeat(seatname);
            }
        }

        /// <summary>
        /// Request a list of all unlocked seats from the data representation
        /// </summary>
        /// <returns>Stack of strings of seatnames</returns>
        public Stack<string> getUnlockedSeats()
        {
            Stack<string> unlockedSeats = new Stack<string>();
            foreach(string seatName in seatList.Keys)
            {
                if (!seatList[seatName].locked) unlockedSeats.Push(seatName);
            }
            return unlockedSeats;
        }

        /// <summary>
        /// Retrieves table size (6max, 7max)...
        /// </summary>
        /// <returns>int - 6,7,8,9...</returns>
        public int getTableSize()
        {
            return tableSize;
        }
       
        /// <summary>
        /// Set Nickname for specific seat
        /// </summary>
        /// <param name="seat">Seatname</param>
        /// <param name="nickname">Nickname</param>
        public void setNickname(string seat, string nickname)
        {
            seatList[seat].nickname = nickname;
            //else throw new Exception("Can't set nickname, Seat is locked!");
        }

        /// <summary>
        /// Requests seatname from/by playername
        /// </summary>
        /// <param name="playername"></param>
        /// <returns>String - seatname</returns>
        public string getSeatname(string playername)
        {
            foreach (KeyValuePair<string, SeatData> kvp in seatList)
            {
                SeatData sd = kvp.Value;
                if (sd.nickname == null)
                {
                    continue;
                }
                else if (kvp.Value.nickname.Equals(playername))
                {
                    return kvp.Key;
                }
            }
            throw new Exception("Player was not found in SeatList");
        }

        /// <summary>
        /// Gets Nickname from Seatname
        /// </summary>
        /// <param name="seat"></param>
        /// <returns></returns>
        public string getNickname(string seat)
        {
            return seatList[seat].nickname;
        }

        /// <summary>
        /// Delete Nickname in specific seat
        /// </summary>
        /// <param name="seat"></param>
        public void deleteNickname(string seat)
        {
            seatList[seat].nickname = "";
        }

        /// <summary>
        /// Set Color for Seat
        /// </summary>
        /// <param name="seat"></param>
        /// <param name="color"></param>
        public void setColor(string seat, Color color)
        {
            seatList[seat].color = color;
        }

        /// <summary>
        /// Set Avatar-Image for specific seat
        /// </summary>
        /// <param name="seat"></param>
        /// <param name="avatar"></param>
        public void setAvatar(string seat, Bitmap avatar)
        {
            seatList[seat].avatar = avatar;
        }

        /// <summary>
        /// Request playercolor for specific seatname
        /// </summary>
        /// <param name="seat"></param>
        /// <returns></returns>
        public Color getColor(string seat)
        {
            return seatList[seat].color;
        }

        /// <summary>
        /// Delete Avatar for a specific seat
        /// </summary>
        /// <param name="seat"></param>
        public void deleteAvatar(string seat)
        {
            seatList[seat].avatar = new Bitmap(62,62);
        }

        /// <summary>
        /// Request Avatar for specific seat
        /// </summary>
        /// <param name="seat"></param>
        /// <returns></returns>
        public Bitmap getAvatar(string seat)
        {
            return seatList[seat].avatar;
        }

        /// <summary>
        /// Remove specific seat from data representation 
        /// (Doesn't really remove. Just resets to new/empty seatData instasnce)
        /// </summary>
        /// <param name="seatName"></param>
        public void removeSeat(string seatName)
        {
            for(int i = 0; i < seatList.Count; i++) 
            {
                string key = seatList.ElementAt(i).Key;
                if(key.Equals(seatName))
                {
                    seatList[seatName] = new SeatData();
                    break;
                }
            }
        }

        /// <summary>
        /// Remove (Reset) all Seats from data representation
        /// </summary>
        public void removeAllSeats()
        {
            for (int i = 0; i < seatList.Count; i++)
            {
                string key = seatList.ElementAt(i).Key;
                seatList[key] = new SeatData();
            }
        }

        /// <summary>
        /// Debug Output
        /// </summary>
        public void printTableData()
        {
            foreach(KeyValuePair<string,SeatData> kvp in seatList)
            {
                Console.WriteLine(String.Format("{0} \t {1}", kvp.Key,kvp.Value.nickname));
            }
        }

        /// <summary>
        /// Check if players is actually seating at that table
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool playerIsSeated(string name)
        {
            foreach(SeatData seat in seatList.Values)
            {
                if(seat.nickname != null &&
                    seat.nickname.Equals(name)) return true;
            }
            return false;
        }


    }
}
