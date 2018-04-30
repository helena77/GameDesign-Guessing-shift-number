// Author: Helena Wang
// Filename: P1.cs
// Date: 4/29/2018
// Version: 2

// Description: 
// This program is responsible for getting input and 
// calling driver


using System;


namespace p1
{
    class P1
    {

        static void Main(string[] args)
        {
            string answer;
            int guessNum;
            Driver driver = new Driver();

            // print welcome message and introduction message and start game
            driver.PrintWelcomeMessage();

            // start game
            do
            {
                driver.GetEncryptWord();
                
                // guess the shift number
                driver.AskShiftNumber();
                guessNum = int.Parse(Console.ReadLine());
                Console.WriteLine();
                    
                // check guess number
                while (!driver.CheckGuessNumber(guessNum))
                {
                    guessNum = int.Parse(Console.ReadLine());
                }

                driver.AskIfPlayAgain();
                answer = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine();

            } while (answer[0] == 'y' || answer[0] == 'Y');

            driver.GetStatisticInfo();
            driver.ResetAll();
            driver.PrintGoodbyeMessage();

        }
            
    }
}
