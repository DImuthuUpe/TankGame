using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using XNA2DCollisionDetection;

namespace Tank_client
{
    class Program
    {
        static void Main(string[] args)
        {
           // Game1 game = new Game1();
            //game.Run();
           
            MainGameLoop nn = new MainGameLoop();            
            Client c = new Client();
            Server server = new Server(c,nn);              
            Thread t = new Thread(new ThreadStart(server.run));
            t.Start();                       
            c.join();
            nn.Run();       

        }
    }
}
