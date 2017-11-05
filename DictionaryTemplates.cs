using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Initialize Dictionary templates for all supported table-sizes
    /// </summary>
    class DictionaryTemplates
    {
        Dictionary<string, SeatData> template = new Dictionary<string, SeatData>();
        public DictionaryTemplates() {}

        public Dictionary<string, SeatData> getDictionaryTemplate(int numberOfSeats)
        {
            switch(numberOfSeats)
            {
                case 6:
                    {
                        initSixHanded();
                        break;
                    }
                case 7:
                    {
                        initSevenHanded();
                        break;
                    }
                case 8:
                    {
                        initEightHanded();
                        break;
                    }
                case 9:
                    {
                        initNineHanded();
                        break;
                    }
                default: throw new Exception("No valid Dictionary Template selected");
            }
            return template;
        }

        // 6handed template
        private void initSixHanded()
        {
            foreach (string single in TableSize.SixHanded.getAll())
            {
                template.Add(single, new SeatData());
            }
        }

        // 7handed template
        private void initSevenHanded()
        {
            foreach (string single in TableSize.SevenHanded.getAll())
            {
                template.Add(single, new SeatData());
            }
        }

        // 8handed template
        private void initEightHanded()
        {
            foreach (string single in TableSize.EightHanded.getAll())
            {
                template.Add(single, new SeatData());
            }
        }

        // 9handed template
        private void initNineHanded()
        {
            foreach (string single in TableSize.NineHanded.getAll())
            {
                template.Add(single, new SeatData());
            }
        }
    }
}
