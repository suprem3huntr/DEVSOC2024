using System.Collections.Generic;
using System.Linq;

namespace DEVSOC2024
{
    public class DefaultPlayerCardData
    {

        public List<int> defaultCards = new List<int>();
        public List<int> defaultDeck = new List<int>();

        public DefaultPlayerCardData()
        {
            for(int i = 0; i < 10; i++)
            {
                this.defaultCards.Add(i);
                this.defaultDeck.Add(i);
            }
        }

        public override string ToString()
        {
            string str = "";
            for(int i = 0; i < defaultCards.Count; i++)
            {
                str += defaultCards.ElementAt(i).ToString() + " ";
                str += "\n";
            }
            for(int i = 0; i < defaultDeck.Count; i++)
            {
                str += defaultDeck.ElementAt(i).ToString() + " ";
            }

            return str;
        }
        
        
    }

    
}
