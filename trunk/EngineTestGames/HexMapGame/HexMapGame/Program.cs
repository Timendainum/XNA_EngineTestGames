using System;

namespace HexMapGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (HexMapGame game = new HexMapGame())
            {
                game.Run();
            }
        }
    }
#endif
}

