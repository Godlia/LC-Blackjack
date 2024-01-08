using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TerminalApi;
using static TerminalApi.TerminalApi;
using TerminalApi.Events;
using BlackJack.Patches;
using TerminalApi.Classes;
using static TerminalApi.Events.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Patches
{
    internal class BlackJackGameManager
    {
        int[] playerCards;
        int[] dealerCards;



        public BlackJackGameManager()
        {
            BlackJackBase._instance.mls.LogInfo("Starting Game from patch");
        }

        public void StartGame()
        {
            BlackJackBase._instance.mls.LogInfo("Starting Game");
            Card card1 = new Card();
            Card card2 = new Card();
            BlackJackBase._instance.mls.LogInfo("Card 1: " + card1.ToString());
            NodeAppendLine("blackjack", card1.ToString());
            BlackJackBase._instance.mls.LogInfo("Card 2: " + card2.ToString());
            NodeAppendLine("blackjack", card2.ToString());
            BlackJackBase._instance.mls.LogInfo("Total: " + (card1.actualValue + card2.actualValue));

        }


        public void Hit()
        {
            Card newCard = new Card();

        }

        public void Stand()
        {

        }

        public void DoubleDown()
        {

        }

        public static void Stop(BlackJackGameManager game)
        {
            game = null;
        }

    }
}
