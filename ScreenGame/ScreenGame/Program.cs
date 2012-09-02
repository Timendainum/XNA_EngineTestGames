using System;

namespace ScreenGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ScreenGame game = new ScreenGame())
            {
                game.Run();
            }
        }
    }
#endif
}

