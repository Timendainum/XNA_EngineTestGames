using System;

namespace ThreeDCameraGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ThreeDCameraGame game = new ThreeDCameraGame())
            {
                game.Run();
            }
        }
    }
#endif
}

