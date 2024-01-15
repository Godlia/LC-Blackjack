using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BlackJack.Patches
{
    public class BlackJackGameManager
    {
        BlackJackGameManager gamePassthrough;

        public List<Card> playerHand = new List<Card>();
        public List<Card> dealerHand = new List<Card>();

        public bool playerLost = false;
        public bool playerStand = false;
        public bool playerWon = false;
        public bool insured = false;


        public BlackJackGameManager()
        {
            BlackJackBase._instance.mls.LogInfo("Starting Game from patch");
        }

        public void StartGame(BlackJackGameManager game)
        {
            gamePassthrough = game;
            BlackJackBase._instance.mls.LogInfo("Object.Startgame HAS BEEN RUN");
            for (int i = 0; i < 2; i++)
            {
                playerHand.Add(new Card());
                dealerHand.Add(new Card());
                //BlackJackBase._instance.mls.LogInfo("Players " + i + ". Card is " + playerHand[i].ToString());
            }

            BlackJackBase._instance.mls.LogInfo("Player Hand: " + playerHand[0].ToString() + " " + playerHand[1].ToString());

            BlackJackBase._instance.mls.LogInfo("Dealer Hand: " + dealerHand[0].ToString() + " " + dealerHand[1].ToString());
            Evaluate();



        }



        public string eEvaluate()
        {
            int playersum = 0;
            int dealersum = 0;

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
            for (int i = 0; i < playerHand.Count; i++)
            {
                if (playerHand[i] != null)
                {
                    playersum += playerHand[i].actualValue;
                }
            }
            for (int i = 0; i < dealerHand.Count; i++)
            {
                if (dealerHand[i] != null)
                {
                    dealersum += dealerHand[i].actualValue;
                }
            }


            //check for bust
            if (playersum > 21)
            {
                BlackJackBase._instance.mls.LogInfo("You busted");
                GameToString("You busted, better luck next time.");
                playerLost = true;
                return loseCondition();
            }
            if (dealersum > 21)
            {
                BlackJackBase._instance.mls.LogInfo("Dealer busted");
                Payout();
                return WinCondition();
            }


            //check for blackjack
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


            //check for stand
            if (playerStand)
            {
                if (dealersum <= 17)
                {
                    Card newCard = new Card();
                    dealerHand.Add(newCard);
                    return Evaluate();
                }
                if (playersum > dealersum)
                {
                    BlackJackBase._instance.mls.LogInfo("You won");
                    GameToString("You won!");
                    Payout();
                    return WinCondition();
                }
                else
                {
                    BlackJackBase._instance.mls.LogInfo("You lost");
                    GameToString("You lost!");
                    playerLost = true;
                    return loseCondition();
                }
            }
            return GameToString();
        }

        public string Evaluate()
        {
            int playerSum = 0;
            int dealerSum = 0;

            bool playerLost = false;
            bool dealerLost = false;
            
            bool playerblackjack = false;
            bool dealerBlackjack = false;

            //sum total value of hands
            for(int i = 0; i < playerHand.Count; i++)
            {
                if (playerHand[i] != null)
                {
                    playerSum += playerHand[i].actualValue;
                }
            }
            for(int i = 0; i < dealerHand.Count; i++)
            {
                if (dealerHand[i] != null)
                {
                    dealerSum += dealerHand[i].actualValue;
                }
            }

            //check if there are aces and if they go above 21 to make them worth 1
            for(int i = 0; i < playerHand.Count; i++)
            {
                if (playerHand[i].actualValue == 11)
                {
                    if(21 < playerSum)
                    {
                        playerHand[i].actualValue = 1;
                    }
                }
            }
            for(int i = 0; i < dealerHand.Count; i++)
            {
                if (dealerHand[i].actualValue == 11)
                {
                    if (21 < dealerSum)
                    {
                        dealerHand[i].actualValue = 1;
                    }
                }
            }

            //busted?
            if (21 < dealerSum) playerLost = true;
            if (21 < playerSum) dealerLost = true;

            //blackjack?
            if(playerSum == 21 && playerHand.Count == 2) playerblackjack = true;
            if(dealerSum == 21 && dealerHand.Count == 2) dealerBlackjack = true;

            //ask for insurance?
            if (dealerHand[0].actualValue == 11)
            {
                return GameToString("Dealer has an ace, Do you want to buy insurance by typing insure?");
            }


            BlackJackBase._instance.mls.LogInfo("\nPSum: " + playerSum + "\nDSum:" + dealerSum + "\n" +
                "PLost: " + playerLost + "\nDLost: " + dealerLost + "\nPBJ: " + playerblackjack + "\nDBJ: " + dealerBlackjack +"" +
                "\nPstand: " + playerStand);

            if(playerStand)
            {
                if(dealerSum < 17 && dealerSum > playerSum)
                {
                    playerLost = true;
                } else {
                    dealerHand.Add(new Card());

                }

            }


            if (dealerLost && playerLost)
            {
                return GameToString("Push, no losses");
            }
            if(dealerLost && !playerLost)
            {
                return WinCondition();
            }
            if(playerLost)
            {
                return loseCondition();
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

        public string Stand()
        {
            if(!playerLost)
            {
                playerStand = true;
                return Evaluate();
            } else
            {
                BlackJackBase._instance.mls.LogInfo("You already lost, start a new game by typing blackjack");
                return "You already lost, start a new game by typing blackjack";
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
            int dealerSum = 0;

            for(int i = 0; i < playerHand.Count; i++)
            {
                if (playerHand[i] != null)
                {
                    returnString += playerHand[i].ToString() + "\n";
                    playersum += playerHand[i].actualValue;
                }
            }

            for (int i = 0; i < dealerHand.Count; i++)
            {
                if (dealerHand[i] != null)
                {
                    dealerSum += dealerHand[i].actualValue;
                }
            }

            returnString += "Total: " + playersum + "\n";



            returnString += "\n\nDealer's Hand: \n";
            returnString += dealerHand[0].ToString() + "\n";
            if (playerStand && condition != "")
            {
                for (int i = 0; i < dealerHand.Count; i++)
                {
                    if (dealerHand[i] != null)
                    {
                        returnString += dealerHand[i].ToString() + "\n";
                    }
                }

            }
            else if(playerWon)
            {
                returnString += dealerHand[1].ToString() + "\n";
                returnString += "Total: " + dealerSum + "\n";
            }
            else
            {
                returnString += "Hidden Card\n";
                returnString += "Total: " + dealerHand[0].actualValue + "\n";
            }




            if(condition != "")
            {
                returnString += "\n" + condition + "";
            }
            returnString += "\n\nYou may: hit, stand, double. \n";
            return returnString;
        }

        public string WinCondition()
        {
            playerWon = true;
            BlackJackBase._instance.mls.LogInfo("Win Condition");
            return GameToString("You won!");
        }
        public string loseCondition()
        {
            playerLost = true;
            BlackJackBase._instance.mls.LogInfo("Lose Condition");
            return GameToString("You won!");
        }

        public static void StopGame()
        {
            BlackJackBase._instance.mls.LogInfo("Stopping Game");
            BlackJackBase._instance.currentGame = null;
        }
    }
}
