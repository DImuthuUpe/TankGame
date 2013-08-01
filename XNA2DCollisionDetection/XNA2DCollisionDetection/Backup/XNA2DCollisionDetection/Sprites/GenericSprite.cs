using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;

namespace XNA2DCollisionDetection.Sprites
{
    public class GenericSprite:DrawableGameComponent
    {
        private Game _game;

        private Texture2D _texture;
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

        private SpriteBatch _spriteBatch;

        #region Bounding Triangle

        public Vector2 TrianglePoint1
        {
            get { return _position; }
        }

        public Vector2 TriangleOffset1 { get; set; }
        public Vector2 TrianglePoint2
        {
            get { return _position+TriangleOffset1; }
        }

        public Vector2 TriangleOffset2 { get; set; }
        public Vector2 TrianglePoint3
        {
            get { return _position + TriangleOffset2; }
        }

        public List<Vector2> TrianglePoints
        {
            get
            {
                return new List<Vector2>()
                {
                    TrianglePoint1,TrianglePoint2,TrianglePoint3
                };
            }
        }
        #endregion
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
        #region Bounding Circle
        public Vector2 CircleCenter
        {
            get { return new Vector2(_position.X+_texture.Width/2,_position.Y+_texture.Height/2); }
        }
        public int CircleRadius
        {
            get { return Math.Max(_texture.Width / 2, _texture.Height / 2); }
        }
        #endregion

        public bool IsMovable { get; set; }

        public GenericSprite(Game game, string SpriteTexture, Vector2 InitialPosition)
            : base(game)
        {
            _position = InitialPosition;
            _game = game;
            _texture = _game.Content.Load<Texture2D>(SpriteTexture);
            _spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            IsMovable = false;
        }

        public void AddTriangleOffsets(Vector2 p1,Vector2 p2)
        {
            TriangleOffset1 = p1;
            TriangleOffset2 = p2;
        }

        protected override void Dispose(bool disposing)
        {
            _texture.Dispose();
            base.Dispose(disposing);
        }
        public override void Update(GameTime gameTime)
        {
            if (IsMovable)
            {
                MouseState MState = Mouse.GetState();
                Vector2 MouseVector = new Vector2(MState.X, MState.Y);
                _position = MouseVector;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_texture, _position, Color.White);

            if (CollisionDetection2D.CDPerformedWith==UseForCollisionDetection.Circles)
                Primitives2D.DrawCircle(CircleCenter, CircleRadius, _spriteBatch);
            if (CollisionDetection2D.CDPerformedWith == UseForCollisionDetection.Rectangles || CollisionDetection2D.CDPerformedWith==UseForCollisionDetection.PerPixel)
                Primitives2D.DrawRectangle(RectUpperLeftCorner,RectWidth,RectHeight, _spriteBatch);
            if (CollisionDetection2D.CDPerformedWith == UseForCollisionDetection.Triangles)
                Primitives2D.DrawTriangle(TrianglePoints, _spriteBatch);
            base.Draw(gameTime);
        }

        
    }
}
