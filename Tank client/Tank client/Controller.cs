using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tank_client
{
    class Controller {

        public Node[,] map;
        public int[,] data;
        public int startX = 2;
        public int starty = 2;
        public int mapSize = 20;
        public int stDir = 0;
        string init="I:P0:3,7;14,1;18,5;2,9;16,13;0,17;4,11:8,15;12,6;7,14;1,18;5,2;9,16;13,17;17,4;11,8:15,12;19,6;6,3;10,7;5,1;0,16;11,6;10,6;13,16;16,11#";
        
    public Controller(string init) {
        this.init= init;
        reset();

    }

    public void reset() {
        string[] obstacles= init.Substring(0, init.Length-1).Split(':');
        map = new Node[mapSize,mapSize];
        for (int i = 0; i < mapSize; i++) {
            for (int j = 0; j < mapSize; j++) {
                map[i,j] = new Node(i, j);
            }
        }
        data = new int[mapSize,mapSize];
        //System.out.println("Init "+init);
        string[] bricks= obstacles[2].Split(';');
        string[] stone = obstacles[3].Split(';');
        string[] water = obstacles[4].Split(';');
        //System.out.println("Brick ");
        for (int i = 0; i < bricks.Length; i++) {
            int x=Int32.Parse(bricks[i].Split(',')[0]);
            int y=Int32.Parse(bricks[i].Split(',')[1]);
            data[y,x]=2;
            //System.out.println(x+" "+y);
        }
        //System.out.println("Stone ");
        for (int i = 0; i < stone.Length; i++) {
            int x=Int32.Parse(stone[i].Split(',')[0]);
            int y=Int32.Parse(stone[i].Split(',')[1]);
            data[y,x]=3;
            //System.out.println(x+" "+y);
        }
        //System.out.println("Water ");
        for (int i = 0; i < water.Length; i++) {
            int x=Int32.Parse(water[i].Split(',')[0]);
            int y=Int32.Parse(water[i].Split(',')[1]);
            data[y,x]=4;
            //System.out.println(x+" "+y);
        }
    }

    public void createBFSTree() {
        reset();
        LinkedList<Int32[]> queue = new LinkedList<Int32[]>();
        Int32[] pos = new Int32[2];
        pos[0] = startX;
        pos[1] = starty;
        map[startX,starty].setDirectCount(0);
        map[startX,starty].setDisCount(0);
        map[startX,starty].setParent(null);
        map[startX,starty].setDirection(stDir);
        queue.AddLast(pos);
        while (queue.Count > 0) {
            int dX = queue.First.Value[0];
            int dY = queue.First.Value[1];
            queue.RemoveFirst();
            data[dX,dY] = 1;

            if ((dX + 1) < mapSize) {
                if (data[dX + 1,dY] == 0) {
                    int dirCost = 0;
                    if (map[dX,dY].getDirection() == 2) {
                        dirCost = 0;
                    } else {
                        dirCost = 1;
                    }
                    if ((map[dX + 1,dY].getDirectCount() + map[dX + 1,dY].getDisCount()) > dirCost + (map[dX,dY].getDirectCount() + map[dX,dY].getDisCount() + 1)) {
                        map[dX + 1,dY].setDirectCount(map[dX,dY].getDirectCount() + dirCost);
                        map[dX + 1,dY].setDirection(2);
                        map[dX + 1,dY].setDisCount(map[dX,dY].getDisCount() + 1);
                        map[dX + 1,dY].setParent(map[dX,dY]);
                        Int32[] dis = new Int32[2];
                        dis[0] = dX + 1;
                        dis[1] = dY;
                        queue.AddLast(dis);
                    }
                }
            }

            if ((dX - 1) >= 0) {
                if (data[dX - 1,dY] == 0) {
                    int dirCost = 0;
                    if (map[dX,dY].getDirection() == 0) {
                        dirCost = 0;
                    } else {
                        dirCost = 1;
                    }

                    if ((map[dX - 1,dY].getDirectCount() + map[dX - 1,dY].getDisCount()) > (dirCost + map[dX,dY].getDirectCount() + map[dX,dY].getDisCount() + 1)) {
                        map[dX - 1,dY].setDirectCount(map[dX,dY].getDirectCount() + dirCost);
                        map[dX - 1,dY].setDirection(0);
                        map[dX - 1,dY].setDisCount(map[dX,dY].getDisCount() + 1);
                        map[dX - 1,dY].setParent(map[dX,dY]);
                        Int32[] dis = new Int32[2];
                        dis[0] = dX - 1;
                        dis[1] = dY;
                        queue.AddLast(dis);
                    }
                }
            }

            if ((dY + 1) < mapSize) {
                if (data[dX,dY + 1] == 0) {
                    int dirCost = 0;
                    if (map[dX,dY].getDirection() == 1) {
                        dirCost = 0;
                    } else {
                        dirCost = 1;
                    }
                    if ((map[dX,dY + 1].getDirectCount() + map[dX,dY + 1].getDisCount()) > (dirCost + map[dX,dY].getDirectCount() + map[dX,dY].getDisCount() + 1)) {
                        map[dX,dY + 1].setDirectCount(map[dX,dY].getDirectCount() + dirCost);
                        map[dX,dY + 1].setDirection(1);
                        map[dX,dY + 1].setDisCount(map[dX,dY].getDisCount() + 1);
                        map[dX,dY + 1].setParent(map[dX,dY]);
                        Int32[] dis = new Int32[2];
                        dis[0] = dX;
                        dis[1] = dY + 1;
                        queue.AddLast(dis);
                    }
                }
            }

            if ((dY - 1) >= 0) {
                if (data[dX,dY - 1] == 0) {
                    int dirCost = 0;
                    if (map[dX,dY].getDirection() == 3) {
                        dirCost = 0;
                    } else {
                        dirCost = 1;
                    }
                    if ((map[dX,dY - 1].getDirectCount() + map[dX,dY - 1].getDisCount()) > (dirCost + map[dX,dY].getDirectCount() + map[dX,dY].getDisCount() + 1)) {
                        map[dX,dY - 1].setDirectCount(map[dX,dY].getDirectCount() + dirCost);
                        map[dX,dY - 1].setDirection(3);
                        map[dX,dY - 1].setDisCount(map[dX,dY].getDisCount() + 1);
                        map[dX,dY - 1].setParent(map[dX,dY]);
                        Int32[] dis = new Int32[2];
                        dis[0] = dX;
                        dis[1] = dY - 1;
                        queue.AddLast(dis);
                    }
                }
            }

        }


        Console.WriteLine("/////////////////////////////////////////");
        for (int i = 0; i < mapSize; i++) {
            for (int j = 0; j < mapSize; j++) {
                if (map[i,j].getDisCount() != 500) {
                    Console.Write((map[i,j].getDirectCount() + map[i,j].getDisCount()) + "," + map[i,j].getDirection() + "\t");
                } else {
                    Console.Write("0,0\t");
                }
            }
            Console.WriteLine("");
        }
        Console.WriteLine("//////////////////////////////////////////");



    }

    public void findPath(int x, int y) {
        createBFSTree();
        Node curr = map[x,y];
        while (curr != null) {
            data[curr.x,curr.y] = 5;
            curr = curr.getParent();
        }
        /*for (int i = 0; i < mapSize; i++) {
            for (int j = 0; j < mapSize; j++) {
                if (data[i,j] != 1) {
                    System.Console.Write(data[i,j] + "");
                } else {
                    System.Console.Write("0");
                }
            }
            System.Console.WriteLine("");
        }*/
    }

    
}

}
