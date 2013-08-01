using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNA2DCollisionDetection.Sprites
{
    class Health : GenericTimeOut
    {
        
         public Health(Game game, string SpriteTexture, int X, int Y, long time, int val)
            : base(game, SpriteTexture, X, Y, time, val)
        {


        }
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(18, 19), 1, SpriteEffects.None,0);
            base.Draw(gameTime);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
