using System;

namespace JitterGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (JitterGame game = new JitterGame())
            {
                game.Run();
            }
        }
    }
#endif
}

