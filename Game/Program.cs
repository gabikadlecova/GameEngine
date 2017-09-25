using System;

namespace Game
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Determines whether the game should restart after exiting
        /// </summary>
        public static bool RestartGame;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            do
            {
                RestartGame = false;
                using (var game = new Game1())
                    game.Run();

            } while (RestartGame);
        }
    }
#endif
}
