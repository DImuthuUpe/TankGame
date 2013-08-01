using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNA2DCollisionDetection.Sprites
{
    class GenericTimeOut : DrawableGameComponent
    {
        private Game _game;
        protected Texture2D _texture;
        protected SpriteBatch _spriteBatch;
        protected Vector2 _position;
        private int Xco, Yco;
        private Point frameSize = new Point(36, 36);
        private Point currentFrame = new Point(0, 0);
        private Point sheetSize = new Point(8, 8);
        private long timeout;
        private int value;

        public GenericTimeOut(Game game, string SpriteTexture, int X, int Y, long time, int val)
            : base(game)
        {
            _game = game;
            _texture = _game.Content.Load<Texture2D>(SpriteTexture);
            _spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            Xco = X;
            Yco = Y;
            _position = new Vector2(40 + 36 * Xco, 41 + 36 * Yco);
            timeout = time;
            value = val;
        }
        public int Value
        {
            get { return value; }
        }

        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public long Time
        {
            get
            {
                return timeout;
            }
        }
        protected override void Dispose(bool disposing)
        {
            _texture.Dispose();
            base.Dispose(disposing);
        }
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
    }
}
