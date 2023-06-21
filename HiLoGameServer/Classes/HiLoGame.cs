using HiLoGameServer.Enums;

namespace HiLoGameServer.Classes
{
    /// <Summary>
    /// This class contains the game logic and structure
    /// </Summary>
    public class HiLoGame
    {
        private int MysteryNumber { get; set; }

        /// <summary>Generates a random number between to given boundaries.</summary>
        /// <returns></returns>
        /// <param name="min">The lower bound.</param>
        /// <param name="max">The upper bound.</param>
        public void GenerateMysteryNumber(int min, int max)
        {
            Random random = new Random();
            MysteryNumber = random.Next(min, max + 1);
        }

        /// <summary>Checks if the guess is either correct, higher or lower than the mystery number.</summary>
        /// <returns><see cref="EGuessResult.Correct"/> if the guess is equal to the mystery number; <see cref="EGuessResult.Higher"/> if the guess is lower than the mystery number; Otherwise <see cref="EGuessResult.Lower"/>.</returns>
        /// <param name="guess">The guess made by the player.</param>
        public EGuessResult ProcessGuess(int guess)
        {
            if (guess == MysteryNumber)
                return EGuessResult.Correct;
            
            return guess < MysteryNumber ? EGuessResult.Higher : EGuessResult.Lower;
        }
    }
}