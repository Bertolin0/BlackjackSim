using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace WindowsApplication1
{
    public partial class frmBlackJackSim : Form
    {
        public double TCAdjustment = 1;
        public int shoes = 0;
        public int currentCount = 0;
        public double decksLeft = 0;
        public double trueCount = 0;
        public double highestBet = 0;
        public double highestCount = 0;
        public double lowestCount = 0;
        public double highestBankroll = 0;
        public double lowestBankroll = 1000000000;
        public int numberOfErrors = 0;
        public bool playerCounts = false;
        public bool errorOn = false;
        public int errorRate = 0;
        double PlayerMoneyTotal = 0;

        public frmBlackJackSim()
        {
            InitializeComponent();
        }

        private int TakeCard(int CurrentCardPostion, ref String[] FullShoe, out String CardType)
        {
            int CardValue = Convert.ToInt32(FullShoe[CurrentCardPostion].Substring(0,FullShoe[CurrentCardPostion].Length-3));
            CardType = Convert.ToString(FullShoe[CurrentCardPostion].Substring(FullShoe[CurrentCardPostion].Length - 1,1));
            
            //Keep a running count of the cards
            if ((CardValue > 1) && (CardValue < 7))
            {
                currentCount++;
            }
            else if ((CardValue > 9) || (CardValue == 1))
            {
                currentCount--;
            }

            return CardValue; 
        }
        
        private String TheRightMove(int DealerUpCard, int PlayerTotal, int PlayerAceCount, bool CanSplit, int NumberOfPlayerCards)
        {
            if ((NumberOfPlayerCards == 2) && (CanSplit) && (PlayerTotal != 10)) //If the player has 5, 5 we treat it is a hard, non-splittable 10.
            {
                //**************************************
                //*              SPLITTING             *
                //**************************************
                if (PlayerTotal == 4) //Player has 2, 2
                {
                    if ((DealerUpCard > 1) && (DealerUpCard < 8))
                    {
                        return("Split");
                    }
                    else
                    {
                        return("Hit");
                    }
                }
                if (PlayerTotal == 6) //Player has 3, 3
                {
                    if ((DealerUpCard > 1) && (DealerUpCard < 8))
                    {
                        return("Split");
                    }
                    else
                    {
                        return("Hit");
                    }
                }
                if (PlayerTotal == 8) //Player has 4, 4
                {
                    if ((DealerUpCard == 5) || (DealerUpCard == 6))
                    {
                        return("Split");
                    }
                    else
                    {
                        return("Hit");
                    }
                }
                if (PlayerTotal == 12) //Player has 6, 6
                {
                    if ((DealerUpCard > 1) && (DealerUpCard < 7))
                    {
                        return("Split");
                    }
                    else
                    {
                        return("Hit");
                    }
                }
                if (PlayerTotal == 14) //Player has 7, 7
                {
                    if ((DealerUpCard > 1) && (DealerUpCard < 8))
                    {
                        return("Split");
                    }
                    else
                    {
                        return("Hit");
                    }
                }
                if (PlayerTotal == 16)//Player has 8, 8
                {
                    return("Split");
                }
                if (PlayerTotal == 18) //Player has 9, 9
                {
                    if ((DealerUpCard == 1) || (DealerUpCard == 7) || (DealerUpCard == 10))
                    {
                        return("Stand");
                    }
                    else
                    {
                        return("Split");
                    }
                }
                if (PlayerTotal == 20) //Player has 10, 10 or J, J or Q, Q or K, K
                {
                    return ("Stand");
                }
                if (PlayerTotal == 22) //Player has A, A
                {
                    return("Split");
                }
            }
            else if (PlayerAceCount > 0)
            {
                //**************************************
                //*              ACE RULES             *
                //**************************************
                if (PlayerTotal == 13) //Player has A, 2
                {
                    if (((DealerUpCard == 5) || (DealerUpCard == 6)) && (NumberOfPlayerCards == 2))
                    {
                        return("Double");
                    }
                    else
                    {
                        return("Hit");
                    }
                }
                if (PlayerTotal == 14) //Player has A, 3
                {
                    if (((DealerUpCard == 5) || (DealerUpCard == 6)) && (NumberOfPlayerCards == 2))
                    {
                        return("Double");
                    }
                    else
                    {
                        return("Hit");
                    }
                }
                if (PlayerTotal == 15) //Player has A, 4
                {
                    if (((DealerUpCard == 4) || (DealerUpCard == 5) || (DealerUpCard == 6)) && (NumberOfPlayerCards == 2))
                    {
                        return("Double");
                    }
                    else
                    {
                        return("Hit");
                    }
                }
                if (PlayerTotal == 16) //Player has A, 5
                {
                    if (((DealerUpCard == 4) || (DealerUpCard == 5) || (DealerUpCard == 6)) && (NumberOfPlayerCards == 2))
                    {
                        return("Double");
                    }
                    else
                    {
                        return("Hit");
                    }
                }
                if (PlayerTotal == 17) //Player has A, 6
                {
                    if (((DealerUpCard == 3) || (DealerUpCard == 4) || (DealerUpCard == 5) || (DealerUpCard == 6)) && (NumberOfPlayerCards == 2))
                    {
                        return("Double");
                    }
                    else
                    {
                        return("Hit");
                    }
                }
                if (PlayerTotal == 18) //Player has A, 7
                {
                    if ((DealerUpCard == 3) || (DealerUpCard == 4) || (DealerUpCard == 5) || (DealerUpCard == 6))
                    {
                        if (NumberOfPlayerCards == 2)
                        {
                            return ("Double");
                        }
                        else
                        {
                            return ("Hit");
                        }
                    }
                    else if ((DealerUpCard == 2) || (DealerUpCard == 7) || (DealerUpCard == 8))
                    {
                        return("Stand");
                    }
                    else
                    {
                        return("Hit");
                    }
                }
                if (PlayerTotal > 18)
                {
                    return("Stand");
                }
            }
            else
            {
                //******************************************
                //*              STANDARD RULES            *
                //******************************************
                if (PlayerTotal < 9)
                {
                    return ("Hit");
                }
                else
                {
                    if (PlayerTotal == 9)
                    {
                        if (((DealerUpCard > 2) && (DealerUpCard < 7)) && (NumberOfPlayerCards == 2))
                        {
                            return ("Double");
                        }
                        else
                        {
                            return ("Hit");
                        }
                    }
                    if (PlayerTotal == 10)
                    {
                        if (((DealerUpCard > 1) && (DealerUpCard < 10)) && (NumberOfPlayerCards == 2))
                        {
                            return ("Double");
                        }
                        else
                        {
                            return ("Hit");
                        }
                    }
                    if (PlayerTotal == 11)
                    {
                        if ((DealerUpCard > 1) && (NumberOfPlayerCards == 2))
                        {
                            return ("Double");
                        }
                        else
                        {
                            return ("Hit");
                        }
                    }
                    if (PlayerTotal == 12)
                    {
                        if ((DealerUpCard == 4) || (DealerUpCard == 5) || (DealerUpCard == 6))
                        {
                            return ("Stand");
                        }
                        else
                        {
                            return ("Hit");
                        }
                    }
                    if ((PlayerTotal >= 13) && (PlayerTotal <= 16))
                    {
                        if ((DealerUpCard > 1) && (DealerUpCard < 7))
                        {
                            return ("Stand");
                        }
                        else
                        {
                            return ("Hit");
                        }
                    }
                    if (PlayerTotal >= 17)
                    {
                        return ("Stand");
                    }
                }
            }
            MessageBox.Show("ERROR IN NEXT PLAYER RIGHT-MOVE CALCULATIONS");
            return ("ERROR");
        }

        private int PlayerHand(ref String[] FullShoe, ArrayList Player1, String PlayerCard1Type, String PlayerCard2Type, int DealerUpCard, int CurrentCardPosition, int NumberOfPreviousSplitsThisRound, out int NewCardPosition, out bool DoubleDown)
        {
            //CURRENT CARD POSITION
            NewCardPosition = CurrentCardPosition;

            //WINNER TRACKING
            int PlayerTotal = 0;

            //ACE TRACKING
            int PlayerAceCount = 0;

            //SPLIT TRACKING
            bool PlayerCanSplit = false;
            String NotRelevant = null;

            //DOUBLE TRACKING
            DoubleDown = false;

            //REMOVE DOUBLE & SPLIT OPTION AFTER INITIAL HIT
            int NumberOfPlayerCards = 2;

            //NEXT MOVE ACCORDING TO BASIC STRATEGY
            String PlayerMove = null;

            
            if(true)
            {
                //Count Aces
                if ((int)Player1[0] == 1)
                {
                    PlayerAceCount++;
                }
                if ((int)Player1[1] == 1)
                {
                    PlayerAceCount++;
                }
                        
                //Does player have option to split? (Set right now to prohibit splits after a split)
                if ((PlayerCard1Type == PlayerCard2Type) && (NumberOfPreviousSplitsThisRound < 1))
                {
                    PlayerCanSplit = true;
                }

                //Total player's initial hand (counting all Aces as 11's)
                PlayerTotal = ((int)Player1[0] + (int)Player1[1] + (PlayerAceCount * 10));
                
                //Find best move according to basic strategy
                PlayerMove = TheRightMove(DealerUpCard, PlayerTotal, PlayerAceCount, PlayerCanSplit, NumberOfPlayerCards);

                //Execute correct move
                while (PlayerMove != "Stand")
                {
                    if (PlayerMove == "Hit")
                    {
                        Player1.Add(TakeCard(NewCardPosition, ref FullShoe, out NotRelevant));
                        NewCardPosition++;
                        NumberOfPlayerCards++;
                        PlayerTotal = PlayerTotal + (int)Player1[Player1.Count - 1];
                        if ((int)Player1[Player1.Count - 1] == 1)
                        {
                            PlayerAceCount++;
                            PlayerTotal = PlayerTotal + 10;
                        }
                    }
                    else if (PlayerMove == "Double")
                    {
                        //Take one more card only
                        Player1.Add(TakeCard(NewCardPosition, ref FullShoe, out NotRelevant));
                        NewCardPosition++;
                        NumberOfPlayerCards++;
                        PlayerTotal = PlayerTotal + (int)Player1[Player1.Count - 1];
                        if ((int)Player1[Player1.Count - 1] == 1)
                        {
                            PlayerAceCount++;
                            PlayerTotal = PlayerTotal + 10;
                        }
                        DoubleDown = true;
                    }
                    else
                    {
                        //Split Hand
                        return (-99);
                    }  

                    //Adjust for soft aces if applicable
                    if (PlayerTotal > 21)
                    {
                        if (PlayerAceCount == 0)
                        {
                            //Player Busts
                            CurrentCardPosition = NewCardPosition;
                            return (PlayerTotal);
                        }
                        else
                        {
                            PlayerAceCount--;
                            PlayerTotal = PlayerTotal - 10;
                        }
                    }
                    
                    //Only allow one card to be drawn on double
                    if (PlayerMove == "Double")
                    {
                        break;
                    }

                    PlayerMove = TheRightMove(DealerUpCard, PlayerTotal, PlayerAceCount, PlayerCanSplit, NumberOfPlayerCards);
                }
                //CurrentCardPosition = NewCardPosition;
                return(PlayerTotal);
            }
        }

        private int DealerHand(ref String[] FullShoe, ArrayList Dealer, bool DealerHitsSoft17, int currentCardPosition, out int cardPositionOUT)
        {
            String NotRelevant = null;

            //WINNER TRACKING
            int DealerTotal = 0;
            
            //ACE TRACKING
            int DealerAceCount = 0;

            //Check for Aces       
            if ((int)Dealer[0] == 1)
            {
                DealerAceCount++;
            }
            if ((int)Dealer[1] == 1)
            {
                DealerAceCount++;
            }

            //Calculate Initial Total
            DealerTotal = ((int)Dealer[0] + (int)Dealer[1] + (DealerAceCount * 10));
            bool done = false;

            while (done == false)
            {
                if (DealerTotal > 21)
                {
                    if (DealerAceCount == 0)
                    {
                        done = true;
                    }
                    else
                    {
                        DealerAceCount--;
                        DealerTotal = DealerTotal - 10;
                    }
                }
                else if ((((DealerTotal > 16) && ((DealerAceCount == 0) || (DealerHitsSoft17 == false))) || (DealerTotal > 17)) && (DealerTotal <= 21))
                {
                    done = true;
                }
                else
                {
                    if ((DealerTotal < 17) || ((DealerTotal == 17) && (DealerAceCount > 0) && (DealerHitsSoft17)))
                    {
                        Dealer.Add(TakeCard(currentCardPosition, ref FullShoe, out NotRelevant));
                        currentCardPosition++;
                        DealerTotal = DealerTotal + (int)Dealer[Dealer.Count - 1];
                        if ((int)Dealer[Dealer.Count - 1] == 1)
                        {
                            DealerAceCount++;
                            DealerTotal += 10;
                        }
                        if (DealerTotal > 21)
                        {
                            if (DealerAceCount > 0)
                            {
                                DealerAceCount--;
                                DealerTotal = DealerTotal - 10;
                            }
                        }
                    }
                }
            }
            cardPositionOUT = currentCardPosition;
            return (DealerTotal);
        }

        private void cmdDeal_Click(object sender, EventArgs e)
        {
            TCAdjustment = Convert.ToDouble(lblTCAdjustment.Text);
            currentCount = 0;
            trueCount = 0;
            shoes = 0;
            lowestBankroll = 1000000000;
            highestBankroll = 0;
            highestBet = 0;
            lowestCount = 0;
            highestCount = 0;
            numberOfErrors = 0;
            playerCounts = false;
            errorOn = false;
            errorRate = 0;
            numberOfErrors = 0;

            int InsuranceBetsWon = 0;
            int InsuranceBetsLost = 0;
            bool updateGUIContinuously = false;
            

            if (rbCountTrue.Checked)
            {
                playerCounts = true;
            }
            if (chkGUIUpdate.Checked)
            {
                updateGUIContinuously = true;
            }
            if (rbErrorsOn.Checked) //Check to see if we are playing with human error
            {
                errorOn = true;
                errorRate = tbErrorRate.Value;
            }

            //Initial Setup
            rtbHandOutcome.Text = "";
            int NumOfRoundsToSim = 0;
            try
            {
                NumOfRoundsToSim = Convert.ToInt32(txtHandsToSim.Text);
            }
            catch
            {
                txtHandsToSim.Text = "100";
                NumOfRoundsToSim = 100;
            }
            if ((NumOfRoundsToSim < 1) || (NumOfRoundsToSim > 1000000000))
            {
                txtHandsToSim.Text = "100";
                NumOfRoundsToSim = 100;
            }
            Application.DoEvents();
            this.Update();

            int pushes = 0;
            int dealerwins = 0;
            int playerwins = 0;
            int splits = 0;
            int playerblackjacks = 0;
            int winningDoubles = 0;
            int losingDoubles = 0;
            int pushingDoubles = 0;

            bool DealerHitsSoft17 = false;
            if (rbTrue.Checked == true)
            {
                DealerHitsSoft17 = true;
            }
            double DecksAfterCut = 2;
            double CardsAfterCut = DecksAfterCut * 52;
            shoes = 0;
            rtbHandOutcome.Text = "";
            
            //Create First Shoe
            String[] FullShoe = createShoe(8);
            FullShoe = shuffleDeck(FullShoe);
            int TotalNumberOfCards = FullShoe.Length;
            int CutPosition = TotalNumberOfCards - (int)Math.Round(CardsAfterCut,0); //Finds how many cards remain after the cut card
            int CurrentCardPostion = 1; //Always discard the first card after shuffle
            
            //Funds Tracking
            PlayerMoneyTotal = Convert.ToInt32(txtStartingBankRoll.Text);
            double DealerMoneyTotal = 0;
            int TableMinimum = 10;
            int TableMaximum = 1000;
            int BettingUnit = 100;
            int TotalBankroll = 50000;
            //************
            
            try
            {
                TotalBankroll = Convert.ToInt32(txtStartingBankRoll.Text);
                BettingUnit = Convert.ToInt32(lblBettingUnit.Text);
                TableMinimum = Convert.ToInt32(txtTableMin.Text);
                TableMaximum = Convert.ToInt32(txtTableMax.Text);
            }
            catch
            {
                TotalBankroll = 50000;
                BettingUnit = 100;
                TableMinimum = 10;
                TableMaximum = 1000;
                txtStartingBankRoll.Text = "50000";
                txtTableMin.Text = "10";
                lblBettingUnit.Text = "100"; 
                txtTableMax.Text = "1000";
            }
            if ((TotalBankroll < 100) || (TotalBankroll > 10000000))
            {
                BettingUnit = 50000;
            }

            //Current Hand Totals
            int PlayerTotal = 0;
            int DealerTotal = 0;

            int hand = 0;
            for (hand = 0; hand < NumOfRoundsToSim; hand++)
            {
                double PercentDone = (((double)hand / (double)NumOfRoundsToSim)*100);
                progressBar1.Value = (int)PercentDone;
                this.Update();

                if (CurrentCardPostion < CutPosition)
                {

                    int PlayerBet = 0;
                    //Bet is based on count if player is a counter
                    if (playerCounts)
                    {
                        decksLeft = Math.Round(((double)(TotalNumberOfCards - CurrentCardPostion) / 52), 1);
                        trueCount = Math.Round(((double)currentCount / (double)decksLeft),1);
                        trueCount = trueCount - TCAdjustment; //Subtract 1 form true if the rules are good, subtract 1.5 from ture if rules are bad.
                        if (trueCount <= 1)
                        {
                            PlayerBet = (TableMinimum);
                            PlayerMoneyTotal -= PlayerBet;
                        }
                        else
                        {
                            if (trueCount > 1)
                            {
                                PlayerBet = (int)Math.Round((BettingUnit * trueCount), 1);
                            }
                            if (PlayerBet > TableMaximum)
                            {
                                PlayerBet = TableMaximum;
                            }
                            PlayerMoneyTotal -= PlayerBet;
                        }

                        if (highestBet < PlayerBet)
                        {
                            highestBet = PlayerBet;
                        }
                        if (highestCount < trueCount)
                        {
                            highestCount = trueCount;
                        }
                        if (lowestCount > trueCount)
                        {
                            lowestCount = trueCount;
                        }
                        if (PlayerMoneyTotal < lowestBankroll)
                        {
                            lowestBankroll = PlayerMoneyTotal;
                        }
                        if (PlayerMoneyTotal > highestBankroll)
                        {
                            highestBankroll = PlayerMoneyTotal;
                        }
                    }
                    else
                    {
                        //Place Flat Bet Normally
                        PlayerBet = (TableMinimum);
                        PlayerMoneyTotal -= PlayerBet;
                    }

                    //Clear Flags
                    bool PlayerDouble = false;
                    bool PlayerBlackJack = false;
                    bool DealerBlackJack = false;
                    bool PlayerSplitHand = false;
                    bool SplitHand1Double = false;
                    bool SplitHand2Double = false;

                    //Split Tracking
                    int SplitHand1Total = 0;
                    int SplitHand2Total = 0;

                    //Draw Initial Cards
                    ArrayList Player1 = new ArrayList();
                    ArrayList Player1_Split1 = new ArrayList();
                    ArrayList Player1_Split2 = new ArrayList();
                    ArrayList Dealer = new ArrayList();
                    String NotRelevant = null;
                    String PlayerCard1Type = null;
                    String PlayerCard2Type = null;
                    Player1.Add(TakeCard(CurrentCardPostion, ref FullShoe, out PlayerCard1Type));
                    CurrentCardPostion++;
                    Dealer.Add(TakeCard(CurrentCardPostion, ref FullShoe, out NotRelevant));
                    CurrentCardPostion++;
                    Player1.Add(TakeCard(CurrentCardPostion, ref FullShoe, out PlayerCard2Type));
                    CurrentCardPostion++;
                    Dealer.Add(TakeCard(CurrentCardPostion, ref FullShoe, out NotRelevant));
                    CurrentCardPostion++;

                    //Check for initial blackjacks
                    if ((((int)Player1[0] == 1) && ((int)Player1[1] >= 10)) || (((int)Player1[0] >= 10) && ((int)Player1[1] == 1)))
                    {
                        PlayerBlackJack = true;
                    }
                    
                    if ((((int)Dealer[0] == 1) && ((int)Dealer[1] >= 10)) || (((int)Dealer[0] >= 10) && ((int)Dealer[1] == 1)))
                    {
                        DealerBlackJack = true;
                    }

                    if ((int)Dealer[0] == 1) //If the dealer's up card is an Ace, Offer insurance
                    {
                        //Only Consider Insurance when we are Counting
                        if ((playerCounts) && (trueCount > 20))
                        {
                            if (DealerBlackJack)
                            {
                                InsuranceBetsWon++;
                                PlayerMoneyTotal = PlayerMoneyTotal + PlayerBet; //This adds the bet that is taken away below, therefore causing the player to break even on the hand.
                            }
                            else
                            {
                                InsuranceBetsLost++;
                                PlayerMoneyTotal = PlayerMoneyTotal - (PlayerBet / 2); //No blackjack so the player loses their insurance bet (half of their normal bet), then they play out the hand as normal.
                            }
                        }
                    }

                    //If we are playing with error do the calculation here
                    if (errorOn)
                    {
                        //See if player makes error based on applying user entered error percentage rate
                        RNGCryptoServiceProvider rnd = new RNGCryptoServiceProvider();
                        int PRand = (GetNextInt32(rnd) % 100);
                        if (PRand <= errorRate)
                        {
                            //If an error occurs, randomly select wether the count is going to be higher or lower than actual
                            int PRand2 = (GetNextInt32(rnd) % 2);
                            int PRand3 = (GetNextInt32(rnd) % 4);
                            PRand3++; //Creates a random number between 1 and 4 to be off by;
                            if (PRand2 == 0)
                            {
                                //Error High
                                currentCount = currentCount + PRand3;
                                numberOfErrors++;
                            }
                            else
                            {
                                //Error Low
                                currentCount = currentCount - PRand3;
                                numberOfErrors++;
                            }
                        }
                    }

                    //If neighther the dealer nor the player has a blackjack then proceed...
                    if ((DealerBlackJack == false) && (PlayerBlackJack == false))
                    {
                        int NewCardPosition = 0;
                        int NumberOfPreviousSplitsThisRound = 0;
                        PlayerTotal = PlayerHand(ref FullShoe, Player1, PlayerCard1Type, PlayerCard2Type, Convert.ToInt32(Dealer[0]), CurrentCardPostion, NumberOfPreviousSplitsThisRound, out NewCardPosition, out PlayerDouble);
                        CurrentCardPostion = NewCardPosition;

                        if (PlayerTotal == -99)//We split the hands
                        {
                            splits++;
                            PlayerSplitHand = true;

                            //Draw 1 replacement card...
                            Player1_Split1.Add(Player1[0]);
                            Player1_Split1.Add(TakeCard(CurrentCardPostion, ref FullShoe, out PlayerCard2Type));
                            CurrentCardPostion++;

                            if ((int)Player1_Split1[0] > 1) //If we split aces then the player only gets one card on each ace, no need to go through basic strategy rules...
                            {
                                //Play out new first hand (Blackjack does not pay 2 to 1 so we dont treat it individually)
                                NewCardPosition = 0;
                                NumberOfPreviousSplitsThisRound = 1;
                                SplitHand1Total = PlayerHand(ref FullShoe, Player1_Split1, PlayerCard1Type, PlayerCard2Type, Convert.ToInt32(Dealer[0]), CurrentCardPostion, NumberOfPreviousSplitsThisRound, out NewCardPosition, out SplitHand1Double);
                                CurrentCardPostion = NewCardPosition;
                            }
                            else
                            {
                                SplitHand1Total = (int)Player1_Split1[0] + 10 + (int)Player1_Split1[1];
                                if ((int)Player1_Split1[1] == 1)
                                {
                                    SplitHand1Total = 2;
                                }
                            }

                            //Draw 1 replacement card (For hand 2) and Play out new second hand...
                            Player1_Split2.Add(Player1[0]);
                            Player1_Split2.Add(TakeCard(CurrentCardPostion, ref FullShoe, out PlayerCard2Type));
                            CurrentCardPostion++;

                            if ((int)Player1_Split2[0] > 1) //If we did not split aces...
                            {
                                //Play out new second hand
                                NewCardPosition = 0;
                                NumberOfPreviousSplitsThisRound = 1;
                                SplitHand2Total = PlayerHand(ref FullShoe, Player1_Split2, PlayerCard1Type, PlayerCard2Type, Convert.ToInt32(Dealer[0]), CurrentCardPostion, NumberOfPreviousSplitsThisRound, out NewCardPosition, out SplitHand2Double);
                                CurrentCardPostion = NewCardPosition;
                            }
                            else
                            {
                                SplitHand2Total = (int)Player1_Split2[0] + 10 + (int)Player1_Split2[1];
                                if ((int)Player1_Split2[1] == 1)
                                {
                                    SplitHand2Total = 2;
                                }
                            }
                        }
                        if ((PlayerTotal <= 21) && (SplitHand1Total <= 21) && (SplitHand2Total <= 21)) //If player did not already bust and player does not have blackjack, play out dealer hand. (if no split occured, SplitHandTotals will be zero)
                        {
                            NewCardPosition = 0;
                            DealerTotal = DealerHand(ref FullShoe, Dealer, DealerHitsSoft17, CurrentCardPostion, out NewCardPosition);
                            CurrentCardPostion = NewCardPosition;
                        }
                    }
                    
                    //Find winner and settle bets
                    bool PlayerWon = false;
                    bool Push = false;
                    bool PlayerWonSplit1 = false;
                    bool PlayerWonSplit2 = false;
                    bool PlayerPushSplit1 = false;
                    bool PlayerPushSplit2 = false;

                    if (PlayerSplitHand)
                    {
                        PlayerMoneyTotal -= PlayerBet; //Subtract base bet again because now we have two hands

                        //Settle Hand 1
                        int TempOriginalBet = PlayerBet;
                        if (SplitHand1Double)
                        {
                            PlayerMoneyTotal -= PlayerBet; //Remove another bettingUnit from stack for Double
                            PlayerBet = (PlayerBet * 2); //Adjust for new bet
                        }
                        if ((SplitHand1Total > 21) || ((SplitHand1Total < DealerTotal) && (DealerTotal <= 21)))
                        {
                            if (SplitHand1Double)
                            {
                                losingDoubles++;
                            }
                            dealerwins++;
                            DealerMoneyTotal += PlayerBet;
                        }
                        else if ((DealerTotal > 21) || ((DealerTotal < SplitHand1Total) && (SplitHand1Total <= 21)))
                        {
                            if (SplitHand1Double)
                            {
                                winningDoubles++;
                            }
                            playerwins++;
                            PlayerMoneyTotal += (PlayerBet * 2);
                            DealerMoneyTotal -= PlayerBet;
                            PlayerWon = true;
                            PlayerWonSplit1 = true;
                        }
                        else if (DealerTotal == SplitHand1Total)
                        {
                            if (SplitHand1Double)
                            {
                                pushingDoubles++;
                            }
                            pushes++;
                            PlayerMoneyTotal += PlayerBet;
                            PlayerPushSplit1 = true;
                        }
                        //Settle Hand 2
                        PlayerBet = TempOriginalBet;
                        if (SplitHand2Double)
                        {
                            PlayerMoneyTotal -= PlayerBet; //Remove another bettingUnit from stack for Double
                            PlayerBet = (PlayerBet * 2); //Adjust for new bet
                        }
                        if ((SplitHand2Total > 21) || ((SplitHand2Total < DealerTotal) && (DealerTotal <= 21)))
                        {
                            if (SplitHand2Double)
                            {
                                losingDoubles++;
                            }
                            dealerwins++;
                            DealerMoneyTotal += PlayerBet;
                        }
                        else if ((DealerTotal > 21) || ((DealerTotal < SplitHand2Total) && (SplitHand2Total <= 21)))
                        {
                            if (SplitHand2Double)
                            {
                                winningDoubles++;
                            }
                            playerwins++;
                            PlayerMoneyTotal += (PlayerBet * 2);
                            DealerMoneyTotal -= PlayerBet;
                            PlayerWonSplit2 = true;
                        }
                        else if (DealerTotal == SplitHand2Total)
                        {
                            if (SplitHand2Double)
                            {
                                pushingDoubles++;
                            }
                            pushes++;
                            PlayerMoneyTotal += PlayerBet;
                            PlayerPushSplit2 = true;
                        }

                    }
                    else
                    {
                        if (DealerBlackJack || PlayerBlackJack) //See if we need to answer blackjacks first
                        {
                            if (DealerBlackJack && PlayerBlackJack)
                            {
                                pushes++;
                                PlayerMoneyTotal += PlayerBet;
                                Push = true;
                            }
                            else if (DealerBlackJack && !PlayerBlackJack)
                            {
                                dealerwins++;
                                DealerMoneyTotal += PlayerBet;
                            }
                            else if (PlayerBlackJack && !DealerBlackJack)
                            {
                                playerwins++;
                                PlayerMoneyTotal += (PlayerBet + (PlayerBet * 1.5));
                                DealerMoneyTotal -= (PlayerBet * 1.5);
                                PlayerWon = true;
                                playerblackjacks++;
                            }
                        }
                        else //Check to see if player busted first, then dealer
                        {
                            if (PlayerDouble)
                            {
                                PlayerMoneyTotal -= PlayerBet; //Remove another bettingUnit from stack for Double
                                PlayerBet = (PlayerBet * 2); //Adjust for new bet
                            }
                            if ((PlayerTotal > 21) || ((PlayerTotal < DealerTotal) && (DealerTotal <= 21)))
                            {
                                if (PlayerDouble)
                                {
                                    losingDoubles++;
                                }
                                dealerwins++;
                                DealerMoneyTotal += PlayerBet;
                            }
                            else if ((DealerTotal > 21) || ((DealerTotal < PlayerTotal) && (PlayerTotal <= 21)))
                            {
                                if (PlayerDouble)
                                {
                                    winningDoubles++;
                                }
                                playerwins++;
                                PlayerMoneyTotal += (PlayerBet * 2);
                                DealerMoneyTotal -= PlayerBet;
                                PlayerWon = true;
                            }
                            else if (DealerTotal == PlayerTotal)
                            {
                                if (PlayerDouble)
                                {
                                    pushingDoubles++;
                                }
                                pushes++;
                                PlayerMoneyTotal += PlayerBet;
                                Push = true;
                            }
                        }
                    }

                    //OUTPUT (SHOW FIRST 100 HANDS ONLY)
                    if (hand < 10)
                    //if (PlayerBet > BettingUnit * 2)
                    //if (PlayerSplitHand)
                    {
                        rtbHandOutcome.Text += "Dealer: ";
                        for (int d = 0; d < Dealer.Count; d++)
                        {
                            rtbHandOutcome.Text += Dealer[d] + ", ";
                        }
                        rtbHandOutcome.Text += " \n";

                        if (PlayerSplitHand)
                        {
                            rtbHandOutcome.Text += "Player: " + Player1[0] + ", " + Player1[0] + " SPLIT\n";
                            rtbHandOutcome.Text += "Hand 1: ";
                            for (int p1 = 0; p1 < Player1_Split1.Count; p1++)
                            {
                                rtbHandOutcome.Text += Player1_Split1[p1] + ", ";
                            }
                            if (SplitHand1Double)
                            {
                                rtbHandOutcome.Text += " DOUBLE ";
                            }
                            rtbHandOutcome.Text += " \n";
                            if (PlayerWonSplit1)
                            {
                                rtbHandOutcome.Text += "PLAYER WINS!";
                            }
                            else if (PlayerPushSplit1)
                            {
                                rtbHandOutcome.Text += "PUSH!";
                            }
                            else
                            {
                                rtbHandOutcome.Text += "DEALER WINS!";
                            }
                            rtbHandOutcome.Text += " \n";
                            rtbHandOutcome.Text += "Hand 2: ";
                            for (int p2 = 0; p2 < Player1_Split2.Count; p2++)
                            {
                                rtbHandOutcome.Text += Player1_Split2[p2] + ", ";
                            }
                            if (SplitHand2Double)
                            {
                                rtbHandOutcome.Text += " DOUBLE ";
                            }
                            rtbHandOutcome.Text += " \n";
                            if (PlayerWonSplit2)
                            {
                                rtbHandOutcome.Text += "PLAYER WINS!";
                            }
                            else if (PlayerPushSplit2)
                            {
                                rtbHandOutcome.Text += "PUSH!";
                            }
                            else
                            {
                                rtbHandOutcome.Text += "DEALER WINS!";
                            }
                            rtbHandOutcome.Text += " \n";
                        }
                        else
                        {
                            rtbHandOutcome.Text += "Player: ";
                            for (int p = 0; p < Player1.Count; p++)
                            {
                                rtbHandOutcome.Text += Player1[p] + ", ";
                            }
                            if (PlayerDouble)
                            {
                                rtbHandOutcome.Text += " DOUBLE ";
                            }
                            rtbHandOutcome.Text += " \n";
                            if (PlayerWon)
                            {
                                rtbHandOutcome.Text += "PLAYER WINS!";
                            }
                            else if (Push)
                            {
                                rtbHandOutcome.Text += "PUSH!";
                            }
                            else
                            {
                                rtbHandOutcome.Text += "DEALER WINS!";
                            }
                        }

                        rtbHandOutcome.Text += " \n";
                        rtbHandOutcome.Text += "Bet: " + PlayerBet + "\n";
                        if (playerCounts)
                        {
                            rtbHandOutcome.Text += "True Count:" + trueCount + "\n";
                        }
                        rtbHandOutcome.Text += "Player Money Total: " + PlayerMoneyTotal + "\n";
                        rtbHandOutcome.Text += "Dealer Money Total: " + DealerMoneyTotal + "\n";
                        rtbHandOutcome.Text += " \n";
                    }
                    if (updateGUIContinuously)
                    {
                        //PLACEHOLDER
                    }
                }
                else
                {
                    currentCount = 0;
                    FullShoe = shuffleDeck(FullShoe);
                    CurrentCardPostion = 1;
                    hand -= 1;
                }
            }
            if (updateGUIContinuously == false)
            {
                //Straight Stat Display
                int TotalHands = dealerwins + playerwins + pushes;
                lblTotalHands.Text = Convert.ToString(TotalHands);
                int TotalNonPushingHands = TotalHands - pushes;
                lblNonPushingTotal.Text = Convert.ToString(TotalNonPushingHands);
                lblDealerWins.Text = Convert.ToString(dealerwins);
                lblPlayerWins.Text = Convert.ToString(playerwins);
                lblPushes.Text = Convert.ToString(pushes);
                lblShoes.Text = Convert.ToString(shoes);
                lblSplits.Text = Convert.ToString(splits);
                lblWinningDoubles.Text = Convert.ToString(winningDoubles);
                lblLosingDoubles.Text = Convert.ToString(losingDoubles);
                lblPushingDoubles.Text = Convert.ToString(pushingDoubles);
                lblPlayerBankroll.Text = "$" + Convert.ToString(Math.Round(PlayerMoneyTotal - Convert.ToInt32(txtStartingBankRoll.Text), 2));
                lblTotalRounds.Text = Convert.ToString(hand);
                lblPlayerBlackjacks.Text = Convert.ToString(playerblackjacks);
                lblErrors.Text = Convert.ToString(numberOfErrors);

                //Player Probability Calculations
                double PlayerWinOnDouble = (double)winningDoubles / TotalNonPushingHands;
                lblWinOnDouble.Text = Convert.ToString(Math.Round((PlayerWinOnDouble * 100), 4)) + "%";
                double PlayerWinOnBJ = (double)playerblackjacks / TotalNonPushingHands;
                lblWinOnBJ.Text = Convert.ToString(Math.Round((PlayerWinOnBJ * 100), 4)) + "%";
                double PlayerWinStraight = (double)(playerwins - (winningDoubles + playerblackjacks)) / TotalNonPushingHands;
                lblPlayerStraightWin.Text = Convert.ToString(Math.Round((PlayerWinStraight * 100), 4)) + "%";
                double PlayerProb = (double)playerwins / TotalNonPushingHands;
                lblPlayerWinTotal.Text = Convert.ToString(Math.Round((PlayerProb * 100), 4)) + "%";
                double costWeightedPlayerWin = ((PlayerWinStraight * 1) + (PlayerWinOnDouble * 2) + (PlayerWinOnBJ * 1.5));
                lblPlayerCostWin.Text = Convert.ToString(Math.Round((costWeightedPlayerWin * 100), 4)) + "%";

                //DealerProbability Calculations
                double DealerWinOnDouble = (double)losingDoubles / TotalNonPushingHands;
                lblDealerWinOnDouble.Text = Convert.ToString(Math.Round((DealerWinOnDouble * 100), 4)) + "%";
                double DealerWinStraight = (double)(dealerwins - losingDoubles) / TotalNonPushingHands;
                lblDealerStraightWin.Text = Convert.ToString(Math.Round((DealerWinStraight * 100), 4)) + "%";
                double DealerProb = (double)dealerwins / TotalNonPushingHands;
                lblDealerWinTotal.Text = Convert.ToString(Math.Round((DealerProb * 100), 4)) + "%";
                double costWeightedDealerWin = ((DealerWinStraight * 1) + (DealerWinOnDouble * 2));
                lblDealerCostWin.Text = Convert.ToString(Math.Round((costWeightedDealerWin * 100), 4)) + "%";

                //Totals
                double UnweightedHouseAdv = DealerProb - PlayerProb;
                lblUnweightedAdv.Text = Convert.ToString(Math.Round((UnweightedHouseAdv * 100), 4)) + "%";
                double WeightedHouseAdv = costWeightedDealerWin - costWeightedPlayerWin;
                lblHouseAdvantage.Text = Convert.ToString(Math.Round((WeightedHouseAdv * 100), 2)) + "%";

                //Counting Stats
                lblHighestBet.Text = "$" + Convert.ToString(highestBet);
                lblHighestCount.Text = Convert.ToString(highestCount);
                lblLowestCount.Text = Convert.ToString(lowestCount);
                lblLowestBankroll.Text = Convert.ToString(lowestBankroll);
                lblHighestBankroll.Text = Convert.ToString(highestBankroll);
                lblInsuranceBetsLost.Text = Convert.ToString(InsuranceBetsLost);
                lblInsuranceBetsWon.Text = Convert.ToString(InsuranceBetsWon);
            }

            this.Update();
            Application.DoEvents();
        }

        private String[] createShoe(int NumOfDecks)
        {
            int CardNumber = 0;
            int value = 0;
            String cardType = "";
            String[] FullShoe = new String[NumOfDecks*52];
            for (int d = 0; d < NumOfDecks; d++)
            {
                for (int c = 1; c < 14; c++)
                {
                    cardType = "";
                    if (c == 1)
                    {
                        value = 1;
                        cardType = "-A";
                    }
                    else if (c == 10)
                    {
                        value = 10;
                        cardType = "-T";
                    }
                    else if (c == 11)
                    {
                        value = 10;
                        cardType = "-J";
                    }
                    else if (c == 12)
                    {
                        value = 10;
                        cardType = "-Q";
                    }
                    else if (c == 13)
                    {
                        value = 10;
                        cardType = "-K";
                    }
                    else
                    {
                        value = c;
                        cardType = "-" + value;
                    }
                    FullShoe[CardNumber] = Convert.ToString(value) + "H" + cardType;
                    CardNumber++;
                    FullShoe[CardNumber] = Convert.ToString(value) + "D" + cardType;
                    CardNumber++;
                    FullShoe[CardNumber] = Convert.ToString(value) + "C" + cardType;
                    CardNumber++;
                    FullShoe[CardNumber] = Convert.ToString(value) + "S" + cardType;
                    CardNumber++;
                }
            }
            return FullShoe;
        }

        private String[] shuffleDeck(string[] FullShoe)
        {
            shoes++;
            int NumberOfCards = FullShoe.Length - 1;
            String temp = null;
            RNGCryptoServiceProvider rnd = new RNGCryptoServiceProvider();
            int PRand = 0;
            
            for (int x = 1; x <= NumberOfCards; x++)
            {
                //Swap x with random position...
                PRand = (GetNextInt32(rnd) % NumberOfCards);
                temp = FullShoe[x];
                FullShoe[x] = FullShoe[PRand];
                FullShoe[PRand] = temp;
            }
            return FullShoe;
        }

        static int GetNextInt32(RNGCryptoServiceProvider rnd)
        { 
            byte[] randomInt = new byte[4]; 
            rnd.GetBytes(randomInt); 
            return Convert.ToInt32(randomInt[0]); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Width = 567;
            this.Height = 640;
            double Agression = (tbAgression.Value * 0.1);
            double ConservitiveUnit = (int)Math.Round((Convert.ToDouble(txtStartingBankRoll.Text) / 500), 0);
            int bettingUnitCalc = (int)Math.Round((ConservitiveUnit + (ConservitiveUnit * Agression)), 0);
            lblBettingUnit.Text = Convert.ToString(bettingUnitCalc);
        }

        private void rbCountFalse_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void tbAgression_Scroll(object sender, EventArgs e)
        {
            int i = 0;
            try
            {
                i = Convert.ToInt32(txtStartingBankRoll.Text);
                if ((i < 100) && (i > 10000000))
                {
                    txtStartingBankRoll.Text = "50000";
                    i = 50000;
                }
            }
            catch
            {
                txtStartingBankRoll.Text = "50000";
                i = 50000;
            }
            double Agression = (tbAgression.Value * 0.1);
            double ConservitiveUnit = (int)Math.Round(((double)i / 500), 0);
            int bettingUnitCalc = (int)Math.Round((ConservitiveUnit + (ConservitiveUnit * Agression)), 0);
            lblBettingUnit.Text = Convert.ToString(bettingUnitCalc);
        }

        private void txtStartingBankRoll_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            try
            {
                i = Convert.ToInt32(txtStartingBankRoll.Text);
                if ((i < 100) && (i > 10000000))
                {
                    txtStartingBankRoll.Text = "50000";
                    i = 50000;
                }
            }
            catch
            {
                txtStartingBankRoll.Text = "50000";
                i = 50000;
            }
            double Agression = (tbAgression.Value * 0.1);
            double ConservitiveUnit = (int)Math.Round(((double)i / 500), 0);
            int bettingUnitCalc = (int)Math.Round((ConservitiveUnit + (ConservitiveUnit * Agression)), 0);
            lblBettingUnit.Text = Convert.ToString(bettingUnitCalc);
        }

        private void rbFalse_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFalse.Checked == true)
            {
                lblTCAdjustment.Text = "1";
            }
            else
            {
                lblTCAdjustment.Text = "1.5";
            }
        }

        private void cmdDetails_Click(object sender, EventArgs e)
        {
            if (this.Width == 885)
            {
                this.Width = 567;
                this.Height = 640;
            }
            else
            {
                this.Width = 885;
                this.Height = 640;
            }
        }

        private void rbErrorsOn_CheckedChanged(object sender, EventArgs e)
        {
            if (rbErrorsOn.Checked)
            {
                gbError.Enabled = true;
            }
            else
            {
                gbError.Enabled = false;
            }
        }

        private void rbErrorsOff_CheckedChanged(object sender, EventArgs e)
        {
            if (rbErrorsOn.Checked)
            {
                gbError.Enabled = true;
            }
            else
            {
                gbError.Enabled = false;
            }
        }

        private void gbError_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void tbErrorRate_Scroll(object sender, EventArgs e)
        {
            lblErrorsPer100Hands.Text = Convert.ToString(tbErrorRate.Value) + " errors per every 100 hands.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                String outputPath = txtPath.Text;
                outputPath = outputPath.ToLower();
                if (!(outputPath.EndsWith(".txt")))
                {
                    outputPath = outputPath + ".txt";
                }
                StreamWriter sw = new StreamWriter(outputPath);
                sw.WriteLine("Highest Bankroll, Ending Bankroll, Lowest Bankroll");
                int numberOfIterations = Convert.ToInt16(numericUpDown1.Value);
                for (int i = 0; i < numberOfIterations; i++)
                {
                    cmdDeal_Click(null, null);
                    sw.WriteLine(highestBankroll + "," + PlayerMoneyTotal + "," + lowestBankroll);
                }
                sw.Close();
                MessageBox.Show("Simulation Complete!");
            }
            catch
            {
                MessageBox.Show("You must enter a valid path, and number of iterations to run multiple simulations.");
            }
        }
    }
}