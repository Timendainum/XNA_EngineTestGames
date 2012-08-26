using System;

namespace TileMapGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (TileMapGame game = new TileMapGame())
            {
                game.Run();
            }
        }
    }
#endif
}

