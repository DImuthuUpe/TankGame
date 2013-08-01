using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNA2DCollisionDetection.Sprites
{
    class Bullet : DrawableGameComponent
    {
        private Vector2 _speed = new Vector2(108, 108);
        private Vector2 _position;
        private Vector2 _newPos;        
        private Texture2D _texture;

        private Game _game;
        private float _angle;
        private SpriteBatch _spriteBatch;
        private long time;

        public Bullet(Game game)
            : base(game)
        {
            _game = game;
            _texture = _game.Content.Load<Texture2D>("bullet");
            _spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
        }
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        protected override void Dispose(bool disposing)
        {
            _texture.Dispose();
            base.Dispose(disposing);
        }

        public long Time
        {
            set { time = value; }
            get { return time; }
        }
        public Vector2 Station
        {
            get { return _newPos; }
        }
        public void SetPos(Vector2 nwPos)
        {
            _newPos = _position + nwPos;
            Console.WriteLine("PASSED VALUE " + nwPos + "NEW POSITION " + _newPos + "INTIAL POSITION " + _position);
        }
        public float Angle {
            get { return _angle; }
        }
        public Vector2 Speed
        {
            set { _speed = value; }
            get { return _speed; }
        }
        public void Position(int d, int X, int Y, long t)
        {
            if (d == 0)
            {
                _angle = 3.0f * (float)Math.PI / 2.0f;
            }
            else if (d == 1)
            {
                _angle = 0;
            }
            else if (d == 2)
            {
                _angle = (float)Math.PI / 2.0f;
            }
            else if (d == 3)
            {
                _angle = (float)Math.PI;
            }
            _position.X = 40 + 36 * X;
            _position.Y = 40 + 36 * Y;
            _speed = (new Vector2((float)Math.Cos(_angle), (float)Math.Sin(_angle)));//speed of bullet is the value of 
            _newPos.X = 40 + 36 * X;
            _newPos.Y = 40 + 36 * Y;
            time = t;
        }


        public override void Update(GameTime gameTime)
        {
            //_position += _speed;
            if (_newPos.X <= 22 || _newPos.Y <= 22 || _newPos.X >= 742 || _newPos.Y >= 742)
                this.Visible = false;
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_texture, _newPos, null, Color.White, _angle, new Vector2(8, 8), 1.0f, SpriteEffects.None, 0);
            base.Draw(gameTime);
        }

    }
}
