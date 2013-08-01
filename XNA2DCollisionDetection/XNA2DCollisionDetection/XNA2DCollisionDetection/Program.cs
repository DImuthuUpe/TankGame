using System;

namespace XNA2DCollisionDetection
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MainGameLoop game = new MainGameLoop())
            {
//                Client c = new Client();
                game.Run();
            }
        }


    }
}

