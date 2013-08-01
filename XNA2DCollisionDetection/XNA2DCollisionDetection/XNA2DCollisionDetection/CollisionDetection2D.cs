using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNA2DCollisionDetection
{
    public enum UseForCollisionDetection { Triangles, Rectangles, Circles, PerPixel }

    public static class CollisionDetection2D
    {
        public static UseForCollisionDetection CDPerformedWith { get; set; }

        public static bool BoundingRectangle(int x1, int y1, int width1, int height1, int x2, int y2, int width2, int height2)
        {
            Rectangle rectangleA = new Rectangle((int)x1, (int)y1, width1, height1);
            Rectangle rectangleB = new Rectangle((int)x2, (int)y2, width2, height2);

            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            if (top >= bottom || left >= right)
                return false;

            return true;
        }        
        
        public static bool PerPixel(Texture2D Texture1, Texture2D Texture2, Vector2 Pos1, Vector2 Pos2)
        {
            Rectangle Rectangle1 = new Rectangle((int)Pos1.X, (int)Pos1.Y, Texture1.Width, Texture1.Height);
            Rectangle Rectangle2 = new Rectangle((int)Pos2.X, (int)Pos2.Y, Texture2.Width, Texture2.Height);

            if (!BoundingRectangle(Rectangle1.X, Rectangle1.Y, Rectangle1.Width, Rectangle1.Height,
                                  Rectangle2.X, Rectangle2.Y, Rectangle2.Width, Rectangle2.Height))
                return false;

            // Bounding rectangles collide beyond this point so we need to check
            // a per-pixel collision

            Color[] TextureData1 = new Color[Texture1.Width * Texture1.Height];
            Texture1.GetData(TextureData1);

            Color[] TextureData2 = new Color[Texture2.Width * Texture2.Height];
            Texture2.GetData(TextureData2);

            int top = Math.Max(Rectangle1.Top, Rectangle2.Top);
            int bottom = Math.Min(Rectangle1.Bottom, Rectangle2.Bottom);
            int left = Math.Max(Rectangle1.Left, Rectangle2.Left);
            int right = Math.Min(Rectangle1.Right, Rectangle2.Right);

            for (int y = top; y < bottom; y++)
                for (int x = left; x < right; x++)
                {
                    Color colorA = TextureData1[(x - Rectangle1.Left) + (y - Rectangle1.Top) * Rectangle1.Width];
                    Color colorB = TextureData2[(x - Rectangle2.Left) + (y - Rectangle2.Top) * Rectangle2.Width];
                    if (colorA.A != 0 && colorB.A != 0) return true;
                }
            return false;
        }
    }
}
