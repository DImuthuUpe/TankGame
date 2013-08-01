using System;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using XNA2DCollisionDetection;

namespace Tank_client
{
    class Server
    {
        TcpClient clientSocket;
        Client client;
        MainGameLoop gameLoop;
        public Server(Client c,MainGameLoop gameLoop)
        {
            this.client = c;
            this.gameLoop = gameLoop;
            
        }

        public void run()
        {
            System.Console.WriteLine("Step1");
            TcpListener serverSocket = new TcpListener(7000);
            System.Console.WriteLine("Step2");
            clientSocket = default(TcpClient);
            System.Console.WriteLine("Step3");
            serverSocket.Start();
            System.Console.WriteLine("Step4");
           
            System.Console.WriteLine("Step5");
            string init = "I:P0:18,14;18,5;9,16;13,8;4,11;11,8;15,3;19,6:3,18;14,1;13,9;16,13;0,8;8,4;8,15;2,6:3,14;1,18;5,12;13,4;6,3;10,17;12,9;0,7;15,2;7,4#";
        
            while (true)
            {
                clientSocket = serverSocket.AcceptTcpClient();
                Stream s = clientSocket.GetStream();
                StreamReader sr = new StreamReader(s);
                string inputLine = sr.ReadLine();
                if (inputLine != null)
                {
                    gameLoop.paramString = inputLine;
                    gameLoop.read = false;
                    Console.WriteLine(inputLine);
                    if (inputLine.ToCharArray()[0] == 'G' || (inputLine.ToCharArray()[0] == 'C' && !inputLine.Equals("CELL_OCCUPIED#")) || inputLine.ToCharArray()[0] == 'L')
                    {
                        client.move(inputLine);
                    }
                    else if (inputLine.ToCharArray()[0] == 'I')
                    {
                        init = inputLine;
                        client.init(init);
                        gameLoop.initString = init;
                    }
                
                }

            }
        }
    }
}
