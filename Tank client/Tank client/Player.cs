using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tank_client
{
    class Player
    {
        public int x = 0;
        public int y = 0;
        public int dir = 0;
        public int shot = 0;
        public int health = 0;
        public int coins = 0;
        public int points = 0;
        public string name = "";

        public Player(string data) {

            string[] temp = data.Split(';');
            name=temp[0];
            x=Int32.Parse(temp[1].Split(',')[0]);
            y=Int32.Parse(temp[1].Split(',')[1]);
            dir = Int32.Parse(temp[2]);
            shot = Int32.Parse(temp[3]);
            health = Int32.Parse(temp[4]);
            coins = Int32.Parse(temp[5]);
            points = Int32.Parse(temp[6]);
        }

    }

}
