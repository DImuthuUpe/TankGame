using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNA2DCollisionDetection.Sprites
{
    class Coins : GenericTimeOut
    {
      
        private Point frameSize = new Point(36, 36);
        private Point currentFrame = new Point(0, 0);
        private Point sheetSize = new Point(8, 8);        

        public Coins(Game game, string SpriteTexture, int X, int Y,long time, int val)
            : base(game, SpriteTexture, X,Y, time, val)
        {            

        }      
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_texture, _position, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, new Vector2(18, 19), 1, SpriteEffects.None, 0);
            base.Draw(gameTime);
        }
        public override void Update(GameTime gameTime)
        {
            ++currentFrame.X;
            if (currentFrame.X >= sheetSize.X)
            {
                currentFrame.X = 0;
                ++currentFrame.Y;
                if (currentFrame.Y >= sheetSize.Y)
                    currentFrame.Y = 0;
            }
            base.Update(gameTime);
        }
    }
}
