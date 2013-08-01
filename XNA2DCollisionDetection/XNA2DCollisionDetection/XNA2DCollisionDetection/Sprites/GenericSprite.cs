using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;

namespace XNA2DCollisionDetection.Sprites
{
    public class GenericSprite : DrawableGameComponent
    {
        private Game _game;
        private float _angle = 0;
        Vector2 inputDirection = Vector2.Zero;
        private SpriteFont _font;
        private Vector2 scoreboard;
        private List<Bullet> bullets = new List<Bullet>();
        private int shooting;
        private Texture2D _texture;        
        private SpriteBatch _spriteBatch;
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public int Health
        {
            set;
            get;
        }

        


        #region Bounding Rectangle
        public Vector2 RectUpperLeftCorner
        {
            get { return _position; }
        }
        public int RectWidth
        {
            get { return _texture.Width; }
        }
        public int RectHeight
        {
            get { return _texture.Height; }
        }
        #endregion

        public bool IsMovable { get; set; }
       

        public GenericSprite(Game game, string SpriteTexture, Vector2 InitialPosition, Vector2 borad)
            : base(game)
        {
            _position = InitialPosition;
            _game = game;
            _font = game.Content.Load<SpriteFont>("Font");
            _texture = _game.Content.Load<Texture2D>(SpriteTexture);
            _spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            IsMovable = false;            
            scoreboard = borad;
        }
        
        protected override void Dispose(bool disposing)
        {
            _texture.Dispose();
            base.Dispose(disposing);
        }
        public int Score
        {
            get;
            set;
        }
        protected override void LoadContent()
        {
            base.LoadContent();
        }
        public void setVal(int d, int X, int Y)
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
            if (_position.X <= 40)
                _position.X = 40;
            if (_position.Y <= 40)
                _position.Y = 40;
            if (_position.X >= 724)
                _position.X = 724;
            if (_position.Y >= 724)
                _position.Y = 724;
        }       
        public override void Update(GameTime gameTime)
        {          

        }
        public float Angle
        {
            get { return _angle; }
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_texture, _position, null, Color.White, _angle, new Vector2(18, 19), 1.0f, SpriteEffects.None, 1);
            _spriteBatch.DrawString(_font, "    " + Score + "   " + Health, scoreboard, Color.Red);
            _spriteBatch.Draw(_texture, scoreboard, null, Color.White, 3.0f * (float)Math.PI / 2.0f, new Vector2(18, 19), 1.0f, SpriteEffects.None, 1);
            //new Vector2(750f, 600f)
            base.Draw(gameTime);
        }
    }
}
