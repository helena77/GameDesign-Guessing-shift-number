// Author: Helena Wang
// Filename: P1.cs
// Date: 4/28/2018
// Version: 2

// Description: 
// This class is responsible for letting the elementary school students guess
// the magic shift number which is used to encrypt the word which has been 
// gotten through the input file. 

// Functionality: 
// This class has a constructor of initialize the string encryptWord as empty, 
// and EncryptWord object. It print welcome message, get encryptword, ask shift 
// number from users and check guess number and ask if they want to play again
// or just get the infos, and finally it prints goobye message
// It uses getRandom to get one word in an word array, uses decode to decode word
// by using guessing number and generate number, uses ifshiftnumber is right to 
// check the shift number and two check methods to check it the validation of
// the word in the list

// Assumptions: 
// For the puzzle list, it is fixed and always by using what I put in the array 
// All input numbers are positive integers.
// For the statistic information: These information is depend on object. In 
// other word, if the user start to play the game, it will do the statistic of
// his/her play data until he/she quit the game. Once the game is quit, the 
// statistic information will reset to be zero.
// the object is always be off at the first time use it
// users need to turn on the state when they use this program 
// this object cannot compare, copy, add, minus
// the user only has one chance to guess the shift number, each time they guess 
// wrong, the right answer will be printed automatically and it means they query
// one time


using System;
using System.Collections;
using System.Text;
using System.IO;

namespace p1
{
    public class Driver
    {
        private const string MSG_WELCOME = "Welcome to the Animal " +   // welcome message
        "Guessing Game! ";
        private const string MSG_GOODBYE = "Thank you for playing " +   // goodbye message
        "Guessing game. \nHope you have a good fun! Goodbye!";
        private const string MSG_HOW_TO_PLAY = "To play this game, " +  // intro message
        "you will need to guess the animal I am thinking of. \n" +
        "The name of the animal has been scrambled. Each letter in " +
        "the word has been shifted over X times. \n\n" +
        "For example, if my shift number was 3, then cat would be " +
        "fdw. Because c + 3 -> f, a + 3 -> d and t + 3 -> w. \n\n" +
        "Your job is to guess the number that the letters have been " +
        "shifted. \n" +
        "Once you have done that, I will shift each letter over and we " +
        "can see if you get the animal I am thinking of. \n\n" +
        "If your guessing is wrong, you will get right answer after your " +
        "guessing. \n\n" +
        "At the end of the game, you can get the overall statistics for " +
        "your game. \n\n";
        private const string GAME_START = "The game is starting! \n";    // game start message
        private const int GUESS_NUM_LOW = 1;                             // the min guess number
        private const int GUESS_NUM_HIGH = 25;                           // the max guess number
        private string[] puzzle = {"giraffe", "flamingo", "chimpanzee",  // the list which stores word
                                         "penguin", "panda", "snake",
                                         "shark", "tiger", "skunk",
                                         "seal", "turtle", "porcupine",
                                         "panther", "ostrich" };

        private string encrytedWord;                // the encryptedWord
        EncryptedWord game;                         // game

        public Driver()
        {
            encrytedWord = "";
            game = new EncryptedWord();
        }

        public void PrintWelcomeMessage()
        {
            Console.WriteLine(MSG_WELCOME);
            Console.WriteLine();
            // game introduction
            Console.WriteLine(MSG_HOW_TO_PLAY);
            Console.WriteLine();
            // start game
            Console.WriteLine(GAME_START);
            Console.WriteLine();
        }

        public void GetEncryptWord()
        {
            
            // turn on encryptWord
            if (!game.IfEncryptIsOn())
            {
                game.TurnOnEncryptWord();
            }

            // get the encrypted word
            encrytedWord = Encrypt(game);
            Console.Write("The animal is: ");
            Console.Write(encrytedWord);
            Console.WriteLine();
        }

        public void AskShiftNumber()
        {
            Console.Write("What's magic shift number? \n (Type a number " +
                    "between 1 and 25 and then press enter or type \"0\" " +
                    "to get the right word): ");
        }

        public bool CheckGuessNumber(int guessNum)
        {
            bool flag = true;
            if (guessNum == 0)
            {
                GetRightShiftNum();
            } else if (guessNum < GUESS_NUM_LOW || guessNum >
                            GUESS_NUM_HIGH)
            {
                Console.Write("Number is out of the range, try again: ");
                flag = false;
            } else
            {
                GetAnswerByGuessNum(guessNum);
                IsShiftNumRight(guessNum);
            }
            return flag;
        }

        public void AskIfPlayAgain()
        {
            Console.Write("Do you want to play again? \n" +
                "(Type \"y\" to continue or \"end\" to quit the " +
                "game and get your statistics.): ");
        }

        public void GetStatisticInfo()
        {
            Console.WriteLine("number of queries: " + game.GetQueryCount());
            Console.WriteLine("high guesses: " + game.GetHighGuessCount());
            Console.WriteLine("low guesses: " + game.GetLowGuessCount());
            Console.WriteLine("average guess value: " + game.GetAveGuessVal());
            Console.WriteLine();
        }

        public void ResetAll()
        {
            game.Reset();
        }

        public void PrintGoodbyeMessage()
        {
            Console.WriteLine(MSG_GOODBYE);
            Console.WriteLine();
        }


        private bool IsShiftNumRight(int guessNum)
        {
            int initialGuessHigh;         // the initial guess high numbers
            int initialGuessLow;          // the initial guess low numbers
            bool flag = false;            // flag of true or false

            if (game.IfEncryptIsOn())
            {
                initialGuessHigh = game.GetHighGuessCount();
                initialGuessLow = game.GetLowGuessCount();
                flag = game.GuessShiftNum(guessNum);
                if (flag)
                {
                    Console.WriteLine("Bingo! Congratulations!");
                }
                else
                {
                    if (game.GetHighGuessCount() > initialGuessHigh)
                    {
                        Console.WriteLine("Sorry, your guess is too high.");
                        GetRightShiftNum();
                    }
                    else if (game.GetLowGuessCount() > initialGuessLow)
                    {
                        Console.WriteLine("Sorry, your guess is too low.");                  
                        GetRightShiftNum();
                    }
                }
            }
            
            return flag;
        }

       
        private string Encrypt(EncryptedWord game)
        {
            string target;                      // the word need to be encrypted
            string targetAfterEncrypted;        // the word after encrypted
            int invalidationWordCount;          // the number of invalid word

            invalidationWordCount = 0;
            targetAfterEncrypted = "";
            target = GetRandomWord();

            if (CheckInputLengthValidation(target))
            {
                bool flag = true;
                for (int i = 0; i < target.Length; i++)
                {
                    char letter = target[i];
                    int letterTransferToNum = (int)letter;
                    if (!CheckInputValidation(letterTransferToNum))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    targetAfterEncrypted = game.StartEncryptWord(target);
                }
                else
                {
                    if (invalidationWordCount < puzzle.Length)
                    {
                        invalidationWordCount++;
                        GetRandomWord();
                    }
                    else
                    {

                    }
                }
            }
            else
            {
                if (invalidationWordCount < puzzle.Length)
                {
                    invalidationWordCount++;
                    GetRandomWord();
                }
                else
                {
                    Console.Write("There is no valid word any more!");
                }
            }
            return targetAfterEncrypted;
        }


        private string DecodeInGuessShiftNum(int guessNum)
        {
            string targetAfterDecoded = "";      // the word which has been decoded


            if (CheckInputLengthValidation(encrytedWord))
            {
                bool flag = true;
                for (int i = 0; i < encrytedWord.Length; i++)
                {
                    char letter = encrytedWord[i];
                    int letterTransferToNum = (int)letter;
                    if (!CheckInputValidation(letterTransferToNum))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    targetAfterDecoded =
                        game.DecodeEncryptWordInGivenNum(encrytedWord, guessNum);
                }
            }
            Console.WriteLine();
            return targetAfterDecoded;
        }

        private string DecodeInGenerateShiftNum()
        {
            return game.DecodeEncryptWord(encrytedWord);
        }

        private string GetRandomWord()
        {
            string target;                      // the word
            int randomIndex;                    // the random index

            Random number = new Random();
            randomIndex = number.Next(0, puzzle.Length);
            target = puzzle[randomIndex];

            return target;
        }

        private bool CheckInputLengthValidation(string target)
        {
            const int LOW_LENGTH = 4;           // the min length of the word
            bool flag = true;                   // flag of true or false

            if (target.Length < LOW_LENGTH)
            {
                flag = false;
            }
            return flag;
        }

        private bool CheckInputValidation(int letterTransferToNum)
        {
            const char ENCRYPT_START_LETTER = 'a';        // encrypt rule start letter
            const char ENCRYPT_END_LETTER = 'z';          // encrypt rule end letter
            bool flag = true;                             // flag of true or false

            if (letterTransferToNum < (int)ENCRYPT_START_LETTER ||
                letterTransferToNum > (int)ENCRYPT_END_LETTER)
            {
                flag = false;
            }
            return flag;
        }

        private void GetRightShiftNum()
        {
            
            
            Console.Write("Let's figure out what this animal is: ");
            Console.Write(DecodeInGenerateShiftNum());
            Console.WriteLine();
            Console.WriteLine();
                     
        } 

        private void GetAnswerByGuessNum(int guessNum)
        {
            Console.WriteLine("Let's figure out what this animal is " +
                       "by using the number you guessed!");
            Console.Write("Your answer is: ");
            Console.Write(DecodeInGuessShiftNum(guessNum));
            Console.WriteLine();
            Console.WriteLine();
        }


    }
}
