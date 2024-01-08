using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Patches
{
    internal class BlackJackConstants
    {
        public static Dictionary<int, string> CardNames = new Dictionary<int, string>()
        {
            {1, "Ace"},
            {2, "Two"},
            {3, "Three"},
            {4, "Four"},
            {5, "Five"},
            {6, "Six"},
            {7, "Seven"},
            {8, "Eight"},
            {9, "Nine"},
            {10, "Ten"},
            {11, "Jack"},
            {12, "Queen"},
            {13, "King"}
        };
        public static Dictionary<int, string> CardSuits = new Dictionary<int, string>()
        {
            {1, "Hearts"},
            {2, "Diamonds"},
            {3, "Clubs"},
            {4, "Spades"}
        };
        public static Dictionary<int, int> CardValues = new Dictionary<int, int>()
        {
            {1, 11},
            {2, 2},
            {3, 3},
            {4, 4},
            {5, 5},
            {6, 6},
            {7, 7},
            {8, 8},
            {9, 9},
            {10, 10},
            {11, 10},
            {12, 10},
            {13, 10}
        };

    }
}
