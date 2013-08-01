using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tank_client
{
    class Node
    {
        private Node parent = null;
        private int disCount;
        private int directCount;
        private int direction;
        public int x, y;

        public Node(int x, int y)
        {
            disCount = 500;
            directCount = 500;
            direction = 5;
            this.x = x;
            this.y = y;
        }

        public int getDirection()
        {
            return direction;
        }

        public void setDirection(int direction)
        {
            this.direction = direction;
        }



        public Node getParent()
        {
            return parent;
        }

        public void setParent(Node parent)
        {
            this.parent = parent;
        }

        public int getDisCount()
        {
            return disCount;
        }

        public void setDisCount(int disCount)
        {
            this.disCount = disCount;
        }

        public int getDirectCount()
        {
            return directCount;
        }

        public void setDirectCount(int directCount)
        {
            this.directCount = directCount;
        }

    }

}
