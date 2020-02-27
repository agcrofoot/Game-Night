using System;

namespace MIS221_Starter_Code
{
    class Program
    {
        static void Main(string[] args)
        {
            //Starter Code
            int gilAmount = 50;
            WelcomeToTheGame();
            GetName();
            int gameChoice = MainMenu(ref gilAmount);
            GameSelection(gameChoice, ref gilAmount);
            Console.ReadKey();
        }



        //Introduce the Game
        public static void WelcomeToTheGame()
        {
            Console.WriteLine("Hello! Welcome to... MIS 221 GAME NIGHT!!");
        }


        //Get the user name
        public static void GetName()
        {
            Console.WriteLine("To begin, please enter your first name.");
            string userName = Console.ReadLine();
            userName = StringCheck(userName);
            Console.Clear();
            Console.WriteLine("Hello " + userName + "! Please select the game you want to play today!");

        }


        //Display the menu and get user selection
        public static int MainMenu(ref int gilAmount)
        {
            DisplayGil(ref gilAmount);
            Console.WriteLine("To play Card Shark, press '1'.");
            Console.WriteLine("To play Shut the Box, press '2'.");
            Console.WriteLine("To play Wheel of Fortune, press '3'.");
            int gameChoice = int.Parse(Console.ReadLine());
            gameChoice = IntCheck(gameChoice);
            Console.Clear();
            return gameChoice;
        }


        //Check that the user input for the name is valid
        public static string StringCheck(string userName)
        {
            if(string.IsNullOrEmpty(userName))
            {
                ErrorMessage();
                userName = Console.ReadLine();
                return userName;
            }
            else
            {
                return userName;
            }
        }


        //Check that the user input for the game choice is valid
        public static int IntCheck(int gameChoice)
        {
            if(gameChoice > 3)
            {
                ErrorMessage();
                gameChoice = int.Parse(Console.ReadLine());
                return gameChoice;
            }
            else
            {
                return gameChoice;
            }
        }


        //Direct to game based on user selection
        public static void GameSelection(int gameChoice, ref int gilAmount)
        {
            if (gameChoice == 1)
            {
                YouHaveChosen("Card Shark");
                CSRules();
                ClearScreen("continue");
                DisplayGil(ref gilAmount);
                int gilWager = GetWager();
                int roundsWon = 0;
                int timesPlayed = 0;
                PlayCardShark(ref gilAmount, gilWager, ref roundsWon, ref timesPlayed);
            }
            else if (gameChoice == 2)
            {
                YouHaveChosen("Shut the Box");
                STBRules();
                ClearScreen("continue");
                DisplayGil(ref gilAmount);
                int gilWager = GetWager();
                int[] tileBox = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                int tilesLeft = 12;
                int roundsWon = 0;
                int timesPlayed = 0;
                PlayShutTheBox(ref gilAmount, ref tilesLeft, tileBox, gilWager, ref roundsWon, ref timesPlayed);
            }
            else if (gameChoice == 3)
            {
                YouHaveChosen("Wheel of Fortune");
                WOFRules();
                ClearScreen("continue");
                DisplayGil(ref gilAmount);
                int gilWager = GetWager();
                int roundsWon = 0;
                int timesPlayed = 0;
                PlayWheelOfFortune(ref gilAmount, gilWager, ref roundsWon, ref timesPlayed);
            } 
        }


        //Play Card Shark
        public static void PlayCardShark(ref int gilAmount, int gilWager, ref int roundsWon, ref int timesPlayed)
        {
            Console.Clear();
            string value = DrawCard("a card");
            int numValue = StringToInt(value);
            int cardGuess = GuessValue();
            string newValue = DrawCard("another card");
            int newNumValue = StringToInt(newValue);
            if(numValue == newNumValue)
            {
                Console.WriteLine("Cards had the same value, please draw another card.");
                newValue = DrawCard("new");
                newNumValue = StringToInt(newValue);
            }
            CompareCards(cardGuess, numValue, newNumValue, ref roundsWon, gilWager, ref gilAmount, ref timesPlayed);
        }


        //Play Shut the Box
        public static void PlayShutTheBox(ref int gilAmount, ref int tilesLeft, int[] tileBox, int gilWager, ref int roundsWon, ref int timesPlayed)
        {
            Console.Clear();
            Random num = new Random();
            int dieOne = RollDie(num);
            int dieTwo = RollDie(num);
            TurnOver(dieOne, dieTwo, tilesLeft, ref gilAmount, gilWager, ref roundsWon, ref timesPlayed, tileBox);
        }


        //Play Wheel of Fortune
        public static void PlayWheelOfFortune(ref int gilAmount, int gilWager, ref int roundsWon, ref int timesPlayed)
        {
            Console.Clear();
            string category = GetCategory();
            string word = GetWord(category);
            char[] displayWord = WordSpaces(word);
            int wrong = 0;

            while(KeepGuessing(displayWord, wrong))
            {
                Console.WriteLine("The category is " + category + "!");
                ShowSpaces(displayWord, wrong);
                int wheelValue = SpinWheel();
                Console.WriteLine("You spun " + wheelValue + ".");
                Console.WriteLine();
                Console.WriteLine("Enter your guess.");
                char guess = Console.ReadLine().ToUpper()[0];
                CheckGuess(displayWord, word, ref wrong, guess, ref wheelValue, ref gilWager);
            }
            
            
            if(wrong == 10)
            {
                Console.WriteLine("Oh well, better luck next time!");
                timesPlayed++;
            }
            else
            {
                Console.WriteLine("Congrats! You won!!");
                roundsWon++;
                timesPlayed++;
                gilAmount = gilWager;
            }
            ClearScreen("continue");
            int gameChoice = MainMenu(ref gilAmount);
            GameSelection(gameChoice, ref gilAmount);
        }


        //Error Message
        public static void ErrorMessage()
        {
            Console.WriteLine("Input is not valid.");
        }


        //Tell the user what they have chosen
        public static void YouHaveChosen(string customText)
        {
            Console.WriteLine("You have chosen to play " + customText + "!");
        }


        //Display of gil amount
        public static void DisplayGil(ref int gilAmount)
        {
            Console.WriteLine("You have " + gilAmount + " gil.");
        }


        //Rules for Card Shark
        public static void CSRules()
        {
            Console.WriteLine("Here is how to play Card Shark!");
            Console.WriteLine("First, you will enter in the amount of gil you wish to wager for this game.");
            Console.WriteLine("Second, you will be shown a random card from the deck.");
            Console.WriteLine("Third, you must try to guess if the value of the next card in the deck is higher or lower than the one you just drew.");
            Console.WriteLine("For example: Say you draw a 7 from the deck and you guess that the value of the next card will be higher.");
            Console.WriteLine("If you are correct, then you get to guess again!");
            Console.WriteLine("However, if you guess incorrectly, you lose the game.");
            Console.WriteLine("If you guess correctly 10 times, then you triple the amount of gil you wagered!");
            Console.WriteLine("If you guess correctly at least 7 times, then you double your wager.");
            Console.WriteLine("If you guess correctly at least 5 times, then you break even.");
            Console.WriteLine("And if you cant get to 5 correct guesses, then you lose your wager.");
        }


        //Rules for Shut the Box
        public static void STBRules()
        {
            Console.WriteLine("Here is how to play Shut the Box!");
            Console.WriteLine("First, you will enter in the amount of gil you wish to wager for this game.");
            Console.WriteLine("Second, you will roll two dice.");
            Console.WriteLine("You will have the option of turning over the tile that represents the sum of the two die.");
            Console.WriteLine("Or you can turn over either one or both of the tiles that correspond to the numbers on the two die.");
            Console.WriteLine("For example: Say you roll a 3 and a 5. You then have the option to turn over the '8' tile, turn over the '3' tile, turn over the '5' tile, or turn over both the '3' and '5' tiles.");
            Console.WriteLine("After the first play, you roll again and continue.");
            Console.WriteLine("However, if a tile is already turned over, you cannot use it again, you must choose a different tile that corresponds to a die value that was just rolled.");
            Console.WriteLine("If you roll the dice, and cannot turn over any tiles, the game is over.");
            Console.WriteLine("The object is to turn over, or 'shut', all 12 tiles.");
            Console.WriteLine("If you have 2 or fewer tiles remaining, then you double the amount of gil you wagered.");
            Console.WriteLine("If you have 3 to 6 tiles remaining, then you break even.");
            Console.WriteLine("And if you have 7 or more tiles remaining, then you lose your wager.");
        }


        //Rules for Wheel of Fortune
        public static void WOFRules()
        {
            Console.WriteLine("Here is how to play Wheel of Fortune!");
            Console.WriteLine("First, you will enter in the amount of gil you wish to wager for this game.");
            Console.WriteLine("Second, you will be given the spaces of a word or phrase, and the category it relates to.");
            Console.WriteLine("Third, you will spin the 'wheel'.");
            Console.WriteLine("The number that the wheel lands on is the amount of gil you can either gain or lose.");
            Console.WriteLine("Fourth, you will input the letter you wish to guess for the word or phrase.");
            Console.WriteLine("If the letter you guessed is correct, then you win the gil you spun for!");
            Console.WriteLine("If you guess incorrectly, however, you forefeit the gil you spun for.");
            Console.WriteLine("If you guess incorrectly 10 times, you lose the game.");
            Console.WriteLine("If you guess all the letters correctly, you win all the gil you earned!");
        }


        //Card Shark

        //Decide whether the user wants to keep the card that was drawn
        public static string DrawCard(string customText)
        {
            ClearScreen("draw " + customText);
            string suit = RandomSuit();
            string value = RandomValue();
            Console.WriteLine("Your card is the " + value + " of " + suit + ".");
            return value;
        }


        //Convert the card names from string to ints
        public static int StringToInt(string value)
        {
            if (value == "Ace")
            {
                int numValue = 1;
                return numValue;
            }
            else if (value == "Two")
            {
                int numValue = 2;
                return numValue;
            }
            else if (value == "Three")
            {
                int numValue = 3;
                return numValue;
            }
            else if (value == "Four")
            {
                int numValue = 4;
                return numValue;
            }
            else if (value == "Five")
            {
                int numValue = 5;
                return numValue;
            }
            else if (value == "Six")
            {
                int numValue = 6;
                return numValue;
            }
            else if (value == "Seven")
            {
                int numValue = 7;
                return numValue;
            }
            else if (value == "Eight")
            {
                int numValue = 8;
                return numValue;
            }
            else if (value == "Nine")
            {
                int numValue = 9;
                return numValue;
            }
            else if (value == "Ten")
            {
                int numValue = 10;
                return numValue;
            }
            else if (value == "Jack")
            {
                int numValue = 11;
                return numValue;
            }
            else if (value == "Queen")
            {
                int numValue = 12;
                return numValue;
            }
            else
            {
                int numValue = 13;
                return numValue;
            }
        }


        //Get the user's git wager
        public static int GetWager()
        {
            Console.WriteLine("How many gil would you like to wager?");
            int gilWager = int.Parse(Console.ReadLine());
            return gilWager;
        }


        //Method that randomizes suits
        public static string RandomSuit()
        {
            string[] suits = { "Clubs", "Diamonds", "Spades", "Hearts" };
            Random rand = new Random();
            return suits[rand.Next(0, 4)];
        }


        //Method that randomizes values
        public static string RandomValue()
        {
            string[] values = { "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", 
                "Eight", "Nine", "Ten", "Jack", "Queen", "King" };
            Random rand = new Random();
            return values[rand.Next(0, 13)];
        }
        

        //Checks if card choice is valid
        public static int CardCheck(int cardGuess)
        {
            if(cardGuess > 2 || cardGuess < 1)
            {
                ErrorMessage();
                cardGuess = int.Parse(Console.ReadLine());
                return cardGuess;
            }
            else
            {
                return cardGuess;
            }
        }


        //Prompts user to guess value of next card drawn
        public static int GuessValue()
        {
            Console.WriteLine("Enter '1' if you guess that the value of the next card will be lower.");
            Console.WriteLine("Enter '2' if you guess that the value of the next card will be higher.");
            int cardGuess = int.Parse(Console.ReadLine());
            Console.Clear();
            cardGuess = CardCheck(cardGuess);
            return cardGuess;
        }


        //Compares the value of the card drawn to the user guess
        public static void CompareCards(int cardGuess, int numValue, int newNumValue, ref int roundsWon, int gilWager, ref int gilAmount, ref int timesPlayed)
        {
            Console.WriteLine("The first card's value was " + numValue + ".");
            Console.WriteLine("The second card's value was " + newNumValue + ".");
           if(newNumValue < numValue)
           {
                if(cardGuess == 1)
                {
                    Console.WriteLine("Correct!");
                    roundsWon++;
                    timesPlayed++;
                    Console.WriteLine("Round " + timesPlayed + " of 10.");
                    ClearScreen("continue");
                    NextRound(ref gilAmount, gilWager, ref roundsWon, ref timesPlayed);
                }
                else
                {
                    Console.WriteLine("Incorrect.");
                    timesPlayed++;
                    Console.WriteLine("Round " + timesPlayed + " of 10.");
                    ClearScreen("continue");
                    CSGameOver(ref roundsWon, gilWager, ref gilAmount, ref timesPlayed);
                    int gameChoice = MainMenu(ref gilAmount);
                    GameSelection(gameChoice, ref gilAmount);
                }
           }
           else
           {
                if(cardGuess == 1)
                {
                    Console.WriteLine("Incorrect.");
                    timesPlayed++;
                    Console.WriteLine("Round " + timesPlayed + " of 10.");
                    ClearScreen("continue");
                    CSGameOver(ref roundsWon, gilWager, ref gilAmount, ref timesPlayed);
                    int gameChoice = MainMenu(ref gilAmount);
                    GameSelection(gameChoice, ref gilAmount);
                }
                else
                {
                    Console.WriteLine("Correct!");
                    roundsWon++;
                    timesPlayed++;
                    Console.WriteLine("Round " + timesPlayed + " of 10.");
                    ClearScreen("continue");
                    NextRound(ref gilAmount, gilWager, ref roundsWon, ref timesPlayed);
                }
           }
        }


        //Keeps track of how many times the user has played
        public static void NextRound(ref int gilAmount, int gilWager, ref int roundsWon, ref int timesPlayed)
        {
            if(timesPlayed != 10)
            {
                PlayCardShark(ref gilAmount, gilWager, ref roundsWon, ref timesPlayed);
            }
            else
            {
                CSGameOver(ref roundsWon, gilWager, ref gilAmount, ref timesPlayed);
                int gameChoice = MainMenu(ref gilAmount);
                GameSelection(gameChoice, ref gilAmount);
            }
        }


        //Determines the user's gil winnings
        public static int CSGameOver(ref int roundsWon, int gilWager, ref int gilAmount, ref int timesPlayed)
        {
            if(roundsWon == 10)
            {
                Console.WriteLine("Congrats!! You beat the game and tripled your gil wager!");
                gilAmount = gilAmount + gilWager * 3;
                return gilAmount;
            }
            else if(roundsWon >= 7 && roundsWon < 10)
            {
                Console.WriteLine("Good job! You doubled your gil wager!");
                gilAmount = gilAmount + gilWager * 2;
                return gilAmount;
            }
            else if(roundsWon >= 5 && roundsWon < 7)
            {
                Console.WriteLine("Not bad. At least you didn't lose your gil wager.");
                return gilAmount;
            }
            else
            {
                Console.WriteLine("Too bad. Better luck next time.");
                gilAmount = gilAmount + 0 - gilWager;
                return gilAmount;
            }

        }


        //Shut the Box
        //Roll die
        public static int RollDie(Random num)
        {
            return num.Next(1,7);
        }


        //User decides what tiles to turn
        public static void TurnOver(int dieOne, int dieTwo, int tilesLeft, ref int gilAmount, int gilWager, ref int roundsWon, ref int timesPlayed, int[] tileBox)
        {
            Console.Clear();
            Console.WriteLine("You rolled a " + dieOne + " and a " + dieTwo + ".");
            Console.WriteLine("Enter '1' to flip the tile that corresponds to the sum of the dice.");
            Console.WriteLine("Enter '2' to flip the tile that corresponds to Die One.");
            Console.WriteLine("Enter '3' to flip the tile that corresponds to Die Two.");
            Console.WriteLine("Enter '4' to flip the tiles that correspond to both dice.");
            int userChoice = int.Parse(Console.ReadLine());
            userChoice = InputCheck(userChoice);
            if (userChoice == 1)
            {
                int tileValue = dieOne + dieTwo;
                FlipTiles(dieOne, dieTwo, tileValue, tilesLeft, ref gilAmount, gilWager, ref roundsWon, ref timesPlayed, tileBox);

            }
            else if (userChoice == 2)
            {
                int tileValue = dieOne;
                FlipTiles(dieOne, dieTwo, tileValue, tilesLeft, ref gilAmount, gilWager, ref roundsWon, ref timesPlayed, tileBox);

            }
            else if (userChoice == 3)
            {
                int tileValue = dieTwo;
                FlipTiles(dieOne, dieTwo, tileValue, tilesLeft, ref gilAmount, gilWager, ref roundsWon, ref timesPlayed, tileBox);
            }
            else
            {
                int tileValueOne = dieOne;
                int tileValueTwo = dieTwo;
                if (tileValueOne == tileValueTwo)
                {
                    FlipTiles(dieOne, dieTwo, tileValueOne, tilesLeft, ref gilAmount, gilWager, ref roundsWon, ref timesPlayed, tileBox);
                }
                else
                {
                    FlipBothTiles(dieOne, dieTwo, tileValueOne, tileValueTwo, tilesLeft, ref gilAmount, gilWager, ref roundsWon, ref timesPlayed, tileBox);
                }
            }
        }

        //Flips over Tile
        public static void FlipTiles(int dieOne, int dieTwo, int tileValue, int tilesLeft, ref int gilAmount, 
            int gilWager, ref int roundsWon, ref int timesPlayed, int[] tileBox )
        {
            Boolean open = true;
            for (int i = 0; i < 12; i++)
            {
                if (tileBox[i] == tileValue)
                {
                    tileBox[i] = -1;
                    open = false;
                }
            }
            if (open)
            {
                Console.WriteLine(tileValue + " is already flipped. If there are no more options, enter '1'.");
                Console.WriteLine("Otherwise, enter '2'.");
                int choice = int.Parse(Console.ReadLine());
                choice = CardCheck(choice);
                if(choice == 1)
                {
                    Console.Clear();
                    EndGame(tilesLeft, ref gilAmount, gilWager);
                    MainMenu(ref gilAmount);
                }
                else
                {
                    TurnOver(dieOne, dieTwo, tilesLeft, ref gilAmount, gilWager, ref roundsWon, ref timesPlayed, tileBox);
                }
            }
            else
            {
                Console.WriteLine("You have flipped the " + tileValue + " tile.");
                tilesLeft--;
                Console.WriteLine("There are " + tilesLeft + " tiles left.");
                Console.ReadKey();
                PlayShutTheBox(ref gilAmount, ref tilesLeft, tileBox, gilWager, ref roundsWon, ref timesPlayed);
            }
            
        }


        //Determines if two tiles can be flipped
        public static void FlipBothTiles(int dieOne, int dieTwo, int tileValueOne, int tileValueTwo, int tilesLeft, ref int gilAmount,
            int gilWager, ref int roundsWon, ref int timesPlayed, int[] tileBox)
        {
            Boolean open = true;
            for (int i = 0; i < 12; i++)
            {
                if (tileBox[i] == tileValueOne)
                {
                    tileBox[i] = -1;
                    open = false;
                }
            }

            if(open)
            {
                Console.WriteLine(tileValueOne + " is already flipped. If there are no more options, enter '1'.");
                Console.WriteLine("Otherwise, enter '2'.");
                int choice = int.Parse(Console.ReadLine());
                choice = CardCheck(choice);
                if (choice == 1)
                {
                    Console.Clear();
                    EndGame(tilesLeft, ref gilAmount, gilWager);
                    MainMenu(ref gilAmount);
                }
                else
                {
                    TurnOver(dieOne, dieTwo, tilesLeft, ref gilAmount, gilWager, ref roundsWon, ref timesPlayed, tileBox);
                }

            }
            else
            {
                Console.WriteLine("You have flipped the " + tileValueOne + " tile.");
                tilesLeft--;
                Console.WriteLine("There are " + tilesLeft + " tiles left.");
                Console.ReadKey();
            }

            for (int i = 0; i < 12; i++)
            {
                if (tileBox[i] == tileValueTwo)
                {
                    tileBox[i] = -1;
                    open = false;
                }
            }
            if (open)
            {
                Console.WriteLine(tileValueTwo + " is already flipped. If there are no more options, enter '1'.");
                Console.WriteLine("Otherwise, enter '2'.");
                int choice = int.Parse(Console.ReadLine());
                choice = CardCheck(choice);
                if (choice == 1)
                {
                    Console.Clear();
                    EndGame(tilesLeft, ref gilAmount, gilWager);
                    MainMenu(ref gilAmount);
                }
                else
                {
                    TurnOver(dieOne, dieTwo, tilesLeft, ref gilAmount, gilWager, ref roundsWon, ref timesPlayed, tileBox);
                }
            }
            else
            {
                Console.WriteLine("You have flipped the " + tileValueTwo + " tile.");
                tilesLeft --;
                Console.WriteLine("There are " + tilesLeft + " tiles left.");
                Console.ReadKey();
                PlayShutTheBox(ref gilAmount, ref tilesLeft, tileBox, gilWager, ref roundsWon, ref timesPlayed);
            }
           
        }


        //Determines winnings
        public static int EndGame(int tilesLeft, ref int gilAmount, int gilWager)
        {
            if (tilesLeft <= 2)
            {
                Console.WriteLine("Good job! You doubled your gil wager!");
                gilAmount = gilAmount + gilWager * 2;
                return gilAmount;
            }
            else if(tilesLeft >= 3 && tilesLeft <= 6)
            {
                Console.WriteLine("Not bad. At least you didn't lose your gil wager.");
                return gilAmount;
            }
            else
            {
                Console.WriteLine("Too bad. Better luck next time.");
                gilAmount = gilAmount + 0 - gilWager;
                return gilAmount;
            }

        }

        public static int InputCheck(int choice)
        {
            if (choice < 1 || choice > 4)
            {
                ErrorMessage();
                choice = int.Parse(Console.ReadLine());
                return choice;
            }
            else
            {
                return choice;
            }
        }

        //Wheel of Fortune
        //Get Category
        public static string GetCategory()
        {
            string[] categoryList = { "Songs", "Books", "Movies", "TV Shows" };
            Random num = new Random();
            string category = categoryList[num.Next(0, 3)];
            return category;
        }


        //Display Word
        public static string GetWord(string category)
        {
            if(category == "Songs")
            {
                string[] songNames = 
                    { 
                    "DONT STOP ME NOW", "HEY JUDE", 
                    "ROCKETMAN", "I WILL SURVIVE", 
                    "LAYLA", "LET IT BE",
                    "KILLER QUEEN", "AFRICA",
                    "AMERICAN PIE", "DREAM ON",
                    "PIANO MAN", "WE WILL ROCK YOU",
                    "COME ON EILEEN", "MR BRIGHTSIDE", 
                    "DIXIELAND DELIGHT", "SWEET HOME ALABAMA", 
                    "TOXIC", "VOGUE",
                    "WANNABE", "MAD WORLD",
                    "DANCING QUEEN", "BOHEMIAN RHAPSODY",
                    "THRILLER", "BILLIE JEAN",
                    "CALIFORNIA", "MISS YOU",
                    "SWEET CHILD O MINE", "IMMIGRANT SONG",
                    "RESPECT","YESTERDAY",
                    "PURPLE RAIN", "TAKE ON ME"
                };
                Random song = new Random();
                return songNames[song.Next(0, 33)];
            }
            else if(category == "Books")
            {
                string[] bookNames =
                {
                    "THE ART OF WAR", "JANE EYRE",
                    "THE GREAT GATSBY", "TO KILL A MOCKINGBIRD",
                    "ANNA KARENINA", "THE COLOR PURPLE",
                    "PRIDE AND PREJUDICE", "THE CALL OF THE WILD",
                    "MOBY DICK", "FRANKENSTEIN",
                    "ANIMAL FARM", "THE GRAPES OF WRATH",
                    "DRACULA", "THE LORD OF THE RINGS",
                    "HARRY POTTER", "GREAT EXPECTATIONS",
                    "THE SECRET GARDEN", "A TALE OF TWO CITIES",
                    "WUTHERING HEIGHTS", "WAR AND PEACE",
                    "LITTLE WOMEN", "LORD OF THE FLIES",
                    "HEART OF DARKNESS", "EMMA",
                    "THE SCARLET LETTER", "THE AGE OF INNOCENCE"
                };
                Random book = new Random();
                return bookNames[book.Next(0, 27)];
            }
            else if(category == "Movies")
            {
                string[] movieNames =
                {
                    "THE GODFATHER", "ROCKY",
                    "BRAVEHEART", "BEAUTY AND THE BEAST",
                    "INCEPTION", "DIE HARD",
                    "GHOSTBUSTERS", "GLADIATOR",
                    "AVATAR", "THE LION KING",
                    "MARY POPPINS", "GOOD WILL HUNTING",
                    "JURASSIC PARK", "SAVING PRIVATE RYAN",
                    "TITANIC", "THE MATRIX",
                    "ALIEN", "PSYCHO",
                    "THE SHINING", "STAR WARS",
                    "THE GRADUATE", "THE BREAKFAST CLUB",
                    "THE SOUND OF MUSIC", "JAWS",
                    "GONE WITH THE WIND", "FORREST GUMP",
                    "BACK TO THE FUTURE", "PULP FICTION"
                };
                Random movie = new Random();
                return movieNames[movie.Next(0, 29)];
            }
            else
            {
                string[] showNames =
                {
                    "HAPPY DAYS", "FRIENDS",
                    "GOLDEN GIRLS", "FRASIER",
                    "CHEERS", "MASH",
                    "DOWNTON ABBEY", "HOUSE OF CARDS",
                    "THE MANDALORIAN", "SEX AND THE CITY",
                    "DOCTOR WHO", "THE OFFICE",
                    "THE X FILES", "BAND OF BROTHERS",
                    "THE WEST WING", "PARKS AND RECREATION",
                    "GAME OF THRONES", "SEINFELD",
                    "LOST", "MAD MEN",
                    "I LOVE LUCY", "BREAKING BAD",
                    "THE TWILIGHT ZONE", "ROME",
                    "STRANGER THINGS", "THE WITCHER",
                    "THE WALKING DEAD", "STAR TREK",
                    "OUTLANDER", "THE TUDORS"
                };
                Random show = new Random();
                return showNames[show.Next(0, 31)];
            }
        }

        //Spins Wheel
        public static int SpinWheel()
        {
            Console.WriteLine("Press 'Enter' to spin the wheel!");
            Console.ReadKey();
            int[] wheelValue = { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50 }; 
            Random num = new Random();
            return wheelValue[num.Next(0, 10)];
        }


        //Show word spaces
        public static char[] WordSpaces(string word)
        {
            char[] displayWord = new char[word.Length];
            for(int i = 0; i < word.Length; i++)
            {
                if(word[i] == ' ')
                {
                    displayWord[i] = ' ';
                }
                else
                {
                    displayWord[i] = '_';
                }
            }
            return displayWord;
        }


        //Lets user guess again
        public static Boolean KeepGuessing(char[] displayWord, int wrong)
        {
            if(wrong == 10)
            {
                return false;
            }
            for(int i = 0; i < displayWord.Length; i++)
            {
                if(displayWord[i] == '_')
                {
                    return true;
                }
            }
            return false;
        }


        //Displays the word spaces
        public static void ShowSpaces(char[] displayWord, int wrong)
        {
            for(int i = 0; i < displayWord.Length; i++)
            {
                Console.Write(displayWord[i]);
            }

            Console.WriteLine();
            Console.WriteLine("You have missed " + wrong);
        }


        //Checks user guess
        public static void CheckGuess(char[] displayWord, string word, ref int wrong, char guess, ref int wheelValue, ref int gilWager)
        {
            Boolean incorrect = true;
            for(int i = 0; i < word.Length; i++)
            {
                if(word[i] == guess)
                {
                    displayWord[i] = guess;
                    incorrect = false;
                }
            }
            if(incorrect)
            {
                Console.WriteLine("There are no " + guess + "s.");
                Console.Clear();
                wrong++;
                gilWager -= wheelValue;
                Console.WriteLine("You have " + gilWager + ".");
            }
            else
            {
                Console.WriteLine("Yes, there are " + guess + "s.");
                Console.Clear();
                gilWager += wheelValue;
                Console.WriteLine("You have " + gilWager + ".");
            }
        }


        //Clears the screen
        public static void ClearScreen(string customText)
        {
            Console.WriteLine("Press 'Enter' to " + customText + ".");
            Console.ReadKey();
            Console.Clear();
        }
        
    }
}
