using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNA2DCollisionDetection.Sprites
{
    class MessageSprite:DrawableGameComponent
    {
       
        private SpriteBatch _spriteBatch;
        private Game _game;
        private SpriteFont _font;
        private string _message;

        public MessageSprite(Game game, string Msg)
            : base(game)
        {
            _message = Msg;
            _font = game.Content.Load<SpriteFont>("Font");
            _spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.DrawString(_font, _message, new Vector2(100f, 5f), Color.Yellow);
           
            base.Draw(gameTime);
        }
    }
}
