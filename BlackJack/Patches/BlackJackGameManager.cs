using System.Collections.Generic;

namespace BlackJack.Patches
{
    public class BlackJackGameManager
    {
        BlackJackGameManager gamePassthrough;

        public List<Card> playerHand = new List<Card>();
        public List<Card> dealerHand = new List<Card>();

        public bool playerLost = false;
        public bool playerStand = false;


        public BlackJackGameManager()
        {
            BlackJackBase._instance.mls.LogInfo("Starting Game from patch");
        }

        public void StartGame(BlackJackGameManager game)
        {
            gamePassthrough = game;
            BlackJackBase._instance.mls.LogInfo("Object.Startgame HAS BEEN RUN");
            for(int i = 0; i < 2; i++)
            {
                playerHand.Add(new Card());
                dealerHand.Add(new Card());
                //BlackJackBase._instance.mls.LogInfo("Players " + i + ". Card is " + playerHand[i].ToString());
            }

            BlackJackBase._instance.mls.LogInfo("Player Hand: " + playerHand[0].ToString() + " " + playerHand[1].ToString());

            BlackJackBase._instance.mls.LogInfo("Dealer Hand: " + dealerHand[0].ToString() + " " + dealerHand[1].ToString());
            Evaluate();


            
        }



        public string Evaluate()
        {
            int playersum = 0;
            int dealersum = 0;

            BlackJackBase._instance.mls.LogInfo("1");
            //check for aces
            for (int i = 0; i < playerHand.Count; i++)
            {
                if (playerHand[i] != null)
                {
                    if (playerHand[i].actualValue == 11)
                    {
                        if (playerHand[i].actualValue + playerHand[i + 1].actualValue > 21)
                        {
                            playerHand[i].actualValue = 1;
                        }
                    }
                }
            }
            //check for aces
            BlackJackBase._instance.mls.LogInfo("2");
            for (int i = 0; i < dealerHand.Count; i++)
            {
                if (dealerHand[i] != null)
                { 
                    if (dealerHand[i].actualValue == 11 && i == 0)
                    {
                        if (dealerHand[i].actualValue + dealerHand[i + 1].actualValue > 21)
                        {
                            dealerHand[i].actualValue = 1;
                        }
                    }
                }
            }
            //sum up
            BlackJackBase._instance.mls.LogInfo("3");
            for (int i = 0; i < playerHand.Count; i++)
            {
                if (playerHand[i] != null)
                {
                    playersum += playerHand[i].actualValue;
                }
            }
            BlackJackBase._instance.mls.LogInfo("4");
            for (int i = 0; i < dealerHand.Count; i++)
            {
                if (dealerHand[i] != null)
                {
                    dealersum += dealerHand[i].actualValue;
                }
            }
            BlackJackBase._instance.mls.LogInfo("5");

            //check for bust
            if (playersum > 21)
            {
                BlackJackBase._instance.mls.LogInfo("You busted");
                GameToString("You busted, better luck next time.");
                playerLost = true;
                return loseCondition();
            }
            BlackJackBase._instance.mls.LogInfo("6");
            if (dealersum > 21)
            {
                BlackJackBase._instance.mls.LogInfo("Dealer busted");
                Payout();
                return WinCondition();
            }
            BlackJackBase._instance.mls.LogInfo("7");

            //check for blackjack
            BlackJackBase._instance.mls.LogInfo("8");
            if (playersum == 21)
            {
                if (playerHand.Count == 2)
                {
                    BlackJackBase._instance.mls.LogInfo("You got blackjack");
                    GameToString("You got Blackjack, well played!");
                    Payout();
                    return WinCondition();
                }
            }
            BlackJackBase._instance.mls.LogInfo("9");
            if (dealersum == 21)
            {
                if (dealerHand.Count == 2)
                {
                    BlackJackBase._instance.mls.LogInfo("Dealer got blackjack");
                    GameToString("Dealer got Blackjack, terribly sorry.");
                    playerLost = true;
                    return loseCondition();
                }
            }
            return GameToString();
        }

        public string Hit()
        {
            if (!playerLost)
            {
                Card newCard = new Card();
                playerHand.Add(newCard);
                return Evaluate();
            } else
            {
                return "You already lost, start a new game by typing blackjack";
            }
        }

        public void Stand()
        {
            if(!playerLost)
            {
                playerStand = true;
                Evaluate();
            } else
            {
                BlackJackBase._instance.mls.LogInfo("You already lost, start a new game by typing blackjack");
            }
        }

        public void DoubleDown()
        {
            if(!playerLost)
            {
                Card newCard = new Card();
                playerHand.Add(newCard);
                Stand();
            } else
            {
                BlackJackBase._instance.mls.LogInfo("You already lost, start a new game by typing blackjack");
            }
        }

        public void Payout()
        {
            //TODO: Payout
        }

        public string GameToString(string condition = "")
        {
            string returnString = "Your Hand: \n";
            int playersum = 0;

            for(int i = 0; i < playerHand.Count; i++)
            {
                if (playerHand[i] != null)
                {
                    returnString += playerHand[i].ToString() + "\n";
                    playersum += playerHand[i].actualValue;
                }
            }
            returnString += "Total: " + playersum + "\n";



            returnString += "\n\nDealer's Hand: \n";
            returnString += dealerHand[0].ToString() + "\n";
            if (playerStand && condition != "")
            {
                returnString += dealerHand[1].ToString() + "\n";

            }
            else
            {
                returnString += "Hidden Card\n";
            }
                returnString += "Total: " + dealerHand[0].actualValue + "\n";




            if(condition != "")
            {
                returnString += "\n" + condition + "";
            }
            returnString += "\n\nYou may: hit, stand, double. \n";
            return returnString;
        }

        public string WinCondition()
        {
            return "You won!";
        }
        public string loseCondition()
        {
            return "You lost!";
        }

        public static void StopGame()
        {
            BlackJackBase._instance.mls.LogInfo("Stopping Game");
            BlackJackBase._instance.currentGame = null;
        }
    }
}
