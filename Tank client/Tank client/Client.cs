using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Tank_client
{
    class Client
    {
        TcpClient clientSocket = new TcpClient();
        Controller con;
        List<Coin> coinList = new List<Coin>();
        string player = "P0";
        public Client()
        {
            clientSocket.Connect("127.0.0.1", 6000);
        }
        public void join()
        {
            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("JOIN#");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
            serverStream.Close();
            clientSocket.Close();
        }
        int direction = 1;
        long timer = 0;
        int curX = 0;
        int curY = 0;
        int dir = 0;
        int coins = 0;
        long timeout = 0;
        List<Player> playerList = new List<Player>();
        int mode = 0;
        int max = 0;

        public void move(string command)
        {

            direction = 4;

            if (command.ToCharArray()[0] == 'G')
            {

                long time = System.Environment.TickCount;
                playerList.Clear();

                string[] splitter = command.Split(':');
                for (int i = 1; i < splitter.Length; i++)
                {
                    //System.out.println( splitter[i].substring(0, 3));
                    if (splitter[i].Substring(0, 2).Equals(player))
                    {
                        curX = Int32.Parse(splitter[i].Split(';')[1].Split(',')[0]);
                        curY = Int32.Parse(splitter[i].Split(';')[1].Split(',')[1]);
                        dir = Int32.Parse(splitter[i].Split(';')[2]);
                        coins = Int32.Parse(splitter[i].Split(';')[5]);
                    }
                    if (splitter[i].ToCharArray()[0] == 'P')
                    {
                        playerList.Add(new Player(splitter[i]));
                    }

                }


                if (mode == 1)
                {
                    //  System.Console.WriteLine("target " + playerList[max].name);
                    if (playerList[max].health == 0)
                    {
                        mode = 0;
                        timeout = 0;
                        Coin c = new Coin();
                        c.x = playerList[max].x;
                        c.y = playerList[max].y;
                        c.endTime = timer + 20000;
                        c.value = playerList[max].coins;
                        coinList.Add(c);
                    }
                }
                //System.out.println(curX+" "+curY+" "+dir);
                con.startX = curY;
                con.starty = curX;
                con.stDir = dir;
                timeout++;
                //System.Console.WriteLine("Time out "+timeout);

                int[] nextpos = getNextPos();
                if (mode == 0)
                {

                    for (int i = 0; i < playerList.Count; i++)
                    {
                        if (((playerList[i].coins > (coins + 2000)) && timeout > 20) || timeout > 100)
                        {
                            mode = 1;

                            int maxCoins = 0;
                            for (int j = 0; j < playerList.Count; j++)
                            {
                                Player p = playerList[j];
                                if (p.coins > maxCoins && p.health != 0 && (!p.name.Equals(player)))
                                {
                                    maxCoins = p.coins;
                                    nextpos[0] = p.x;
                                    nextpos[1] = p.y;
                                    max = j;
                                }
                            }
                        }
                    }


                    //System.Console.WriteLine("Next " + nextpos[0] + " " + nextpos[1]);

                    con.findPath(nextpos[1], nextpos[0]);

                    if (curY < con.mapSize - 1)
                    {
                        if (con.data[curY + 1, curX] == 5)
                        {
                            direction = con.map[curY + 1, curX].getDirection();
                        }
                    }
                    if (curX < con.mapSize - 1)
                    {
                        if (con.data[curY, curX + 1] == 5)
                        {
                            direction = con.map[curY, curX + 1].getDirection();
                        }
                    }

                    if (curX > 0)
                    {
                        if (con.data[curY, curX - 1] == 5)
                        {
                            direction = con.map[curY, curX - 1].getDirection();
                        }
                    }

                    if (curY > 0)
                    {
                        if (con.data[curY - 1, curX] == 5)
                        {
                            direction = con.map[curY - 1, curX].getDirection();
                        }
                    }
                }
                else if (mode == 1)
                {

                    nextpos[0] = playerList[max].x;
                    nextpos[1] = playerList[max].y;

                    con.findPath(nextpos[1], nextpos[0]);

                    if (((curX - playerList[max].x) == 0) || ((curY - playerList[max].y) == 0))
                    {
                        if ((curX - playerList[max].x) == 0)
                        {
                            if (curY < playerList[max].y)
                            {
                                if (dir != 2)
                                {
                                    direction = 2;
                                }
                            }
                            else
                            {
                                if (dir != 0)
                                {
                                    direction = 0;
                                }
                            }
                        }
                        else
                        {
                            if (curX < playerList[max].x)
                            {
                                if (dir != 1)
                                {
                                    direction = 1;
                                }
                            }
                            else
                            {
                                if (dir != 3)
                                {
                                    direction = 3;
                                }
                            }
                        }
                    }
                    else
                    {

                        if (curY < con.mapSize - 1)
                        {
                            if (con.data[curY + 1, curX] == 5)
                            {
                                direction = con.map[curY + 1, curX].getDirection();
                            }
                        }
                        if (curX < con.mapSize - 1)
                        {
                            if (con.data[curY, curX + 1] == 5)
                            {
                                direction = con.map[curY, curX + 1].getDirection();
                            }
                        }

                        if (curX > 0)
                        {
                            if (con.data[curY, curX - 1] == 5)
                            {
                                direction = con.map[curY, curX - 1].getDirection();
                            }
                        }

                        if (curY > 0)
                        {
                            if (con.data[curY - 1, curX] == 5)
                            {
                                direction = con.map[curY - 1, curX].getDirection();
                            }
                        }
                    }
                }

                //                for (int i = 0; i < con.mapSize; i++) {
                //                    for (int j = 0; j < con.mapSize; j++) {
                //                        if (con.data[i,j] != 1) {
                //                            System.out.print(con.data[i,j] + "\t");
                //                        } else {
                //                            System.out.print("0\t");
                //                        }
                //                    }
                //                    System.out.println("");
                //                }

                while (System.Environment.TickCount - time < 900) ;
                clientSocket = new TcpClient();
                clientSocket.Connect("127.0.0.1", 6000);
                NetworkStream serverStream = clientSocket.GetStream();
                byte[] outStream;

                if (direction == 0)
                {
                    outStream = System.Text.Encoding.ASCII.GetBytes("UP#");
                }
                else if (direction == 1)
                {
                    outStream = System.Text.Encoding.ASCII.GetBytes("RIGHT#");
                }
                else if (direction == 2)
                {
                    outStream = System.Text.Encoding.ASCII.GetBytes("DOWN#");
                }
                else if (direction == 3)
                {
                    outStream = System.Text.Encoding.ASCII.GetBytes("LEFT#");
                }
                else
                {
                    outStream = System.Text.Encoding.ASCII.GetBytes("SHOOT#");
                }
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                serverStream.Close();
                clientSocket.Close();
                timer++;

            }
            else if (command.ToCharArray()[0] == 'C')
            {
                coinList.Add(new Coin(command, timer));               
            }

        }


        int[] getNextPos()
        {

            Boolean finished = false;
            while (!finished)
            {
                finished = true;
                for (int i = 0; i < coinList.Count; i++)
                {
                    Boolean captured = false;
                    foreach (Player p in playerList)
                    {
                        if (((coinList[i].x == p.x) && (coinList[i].y == p.y)) && p.health != 0)
                        {
                            captured = true;
                        }
                    }
                    if ((coinList[i].endTime <= timer) || captured)
                    {
                        //System.out.println("Coin removed " + coinList.get(i).x + " , " + coinList.get(i).y);
                        coinList.RemoveAt(i);
                        finished = false;
                        break;
                    }
                }
            }



            int[] next = { curX, curY };
            int min = 5000;
            con.createBFSTree();
            foreach (Coin c in coinList)
            {
                if ((con.map[c.y, c.x].getDirectCount() + con.map[c.y, c.x].getDisCount()) < min)
                {
                    next[0] = c.x;
                    next[1] = c.y;
                    min = (con.map[c.y, c.x].getDirectCount() + con.map[c.y, c.x].getDisCount());
                }
            }


            return next;
        }

        public void init(String init)
        {
            con = new Controller(init);
            player = init.Split(':')[1];       

        }

    }
}
