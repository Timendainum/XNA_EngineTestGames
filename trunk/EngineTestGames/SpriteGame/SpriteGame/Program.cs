using System;

namespace SpriteGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SpriteGame game = new SpriteGame())
            {
                game.Run();
            }
        }
    }
#endif
}

