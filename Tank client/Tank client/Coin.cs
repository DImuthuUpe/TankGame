﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tank_client
{
    class Coin
    {
        public int x;
        public int y;
        public long stTime = 0;
        public long endTime = 0;
        public int value = 0;

        public Coin(string data, long stTime) {
            data = data.Substring(0,data.Length-1);
            this.stTime = stTime;
            String[] temp = data.Split(':');
            x = Int32.Parse(temp[1].Split(',')[0]);
            y = Int32.Parse(temp[1].Split(',')[1]);
            endTime = Int64.Parse(temp[2])/1000+stTime;
            value = Int32.Parse(temp[3]);
            System.Console.WriteLine("End time "+endTime);
        }
        public Coin()
        {
        }
    }

}
