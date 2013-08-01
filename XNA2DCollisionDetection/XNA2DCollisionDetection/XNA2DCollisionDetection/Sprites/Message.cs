using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNA2DCollisionDetection.Sprites
{
    class MessageSprite : DrawableGameComponent
    {

        private SpriteBatch _spriteBatch;
        private Game _game;
        private SpriteFont _font;
        private string _message;
        private Vector2 _position;
        private Color _color;

        public MessageSprite(Game game, string Msg, Vector2 pos, Color col)
            : base(game)
        {
            _message = Msg;
            _font = game.Content.Load<SpriteFont>("Font");
            _spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            this._position = pos;
            this._color = col;
        }
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.DrawString(_font, _message, _position, _color);

            base.Draw(gameTime);
        }

        internal void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
