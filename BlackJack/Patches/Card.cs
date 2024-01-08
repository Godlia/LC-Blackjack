using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Patches
{
    public class Card
    {
        public int actualValue;
        public string suit;
        public int number;
        public string name; 
        
        public Card()
        {
            this.suit = BlackJackConstants.CardSuits[randomInt(1, 4)];
            this.number = randomInt(1, 13);
            this.name = BlackJackConstants.CardNames[this.number];
            this.actualValue = BlackJackConstants.CardValues[this.number];
        }

        public override string ToString()
        {
            return "[" + this.actualValue + "] - " + this.name + " of " + this.suit;
        }

        public static int randomInt(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
