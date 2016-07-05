using System;

namespace PacMan
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        public static Game1 game;

        static void Main(string[] args)
        {
            game = new Game1();
            using (game)
            {
                game.Run();
            }
        }
    }
#endif
}