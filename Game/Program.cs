using System;

namespace Game
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        public static bool RestartGame = false;

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
