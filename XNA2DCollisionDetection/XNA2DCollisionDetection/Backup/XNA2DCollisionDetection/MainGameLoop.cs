using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using XNA2DCollisionDetection.Sprites;

namespace XNA2DCollisionDetection
{
    public class MainGameLoop : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GenericSprite _sprite1;
        private GenericSprite _sprite2;

        private GenericSprite _movableSprite;
        private MessageSprite _messageSprite;

        
        public MainGameLoop()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();
            CollisionDetection2D.CDPerformedWith = UseForCollisionDetection.PerPixel;
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), _spriteBatch);

            _sprite1=new GenericSprite(this,"Goody",new Vector2(500,400));
            _sprite1.AddTriangleOffsets(new Vector2(40, 10), new Vector2(3, 40));
            
            _sprite2=new GenericSprite(this,"Life",new Vector2(300,300));
            _sprite2.AddTriangleOffsets(new Vector2(40, 10), new Vector2(3, 40));

            _movableSprite=new GenericSprite(this,"Ivan",new Vector2(400,300));
            _movableSprite.AddTriangleOffsets(new Vector2(40, 10), new Vector2(3, 40));
            _movableSprite.IsMovable=true;

            _messageSprite = new MessageSprite(this, "================== COLLISION DETECTED ==================");
            _messageSprite.Visible = false;

            Primitives2D.dotTexture =Content.Load<Texture2D>("Dot");

            Components.Add(_sprite1);
            Components.Add(_sprite2);
            Components.Add(_movableSprite);
            Components.Add(_messageSprite);

        }
        protected override void UnloadContent()
        {
            _sprite1.Dispose();
            _sprite2.Dispose();
            _movableSprite.Dispose();
            _messageSprite.Dispose();
            Primitives2D.dotTexture.Dispose();
        }
        protected override void Update(GameTime gameTime)
        {
            _messageSprite.Visible = false;

            #region Collision Detection with Bounding Rectangles
            if (CollisionDetection2D.CDPerformedWith==UseForCollisionDetection.Rectangles)
            {
                if (CollisionDetection2D.BoundingRectangle((int)_movableSprite.RectUpperLeftCorner.X, (int)_movableSprite.RectUpperLeftCorner.Y,
                                                           _movableSprite.RectWidth, _movableSprite.RectHeight,
                                                           (int)_sprite1.RectUpperLeftCorner.X, (int)_sprite1.RectUpperLeftCorner.Y,
                                                           _sprite1.RectWidth, _sprite1.RectHeight))
                    _messageSprite.Visible = true;
                if (CollisionDetection2D.BoundingRectangle((int)_movableSprite.RectUpperLeftCorner.X, (int)_movableSprite.RectUpperLeftCorner.Y,
                                                           _movableSprite.RectWidth, _movableSprite.RectHeight,
                                                           (int)_sprite2.RectUpperLeftCorner.X, (int)_sprite2.RectUpperLeftCorner.Y,
                                                           _sprite2.RectWidth, _sprite2.RectHeight))
                    _messageSprite.Visible = true;
            }
            #endregion
            
            #region Collision Detection with Bounding Circles
            if (CollisionDetection2D.CDPerformedWith == UseForCollisionDetection.Circles)
            {
                if (CollisionDetection2D.BoundingCircle((int)_movableSprite.CircleCenter.X, (int)_movableSprite.CircleCenter.Y,
                                                           _movableSprite.CircleRadius,
                                                           (int)_sprite1.CircleCenter.X, (int)_sprite1.CircleCenter.Y,
                                                           _sprite1.CircleRadius))
                    _messageSprite.Visible = true;
                if (CollisionDetection2D.BoundingCircle((int)_movableSprite.CircleCenter.X, (int)_movableSprite.CircleCenter.Y,
                                                           _movableSprite.CircleRadius,
                                                           (int)_sprite2.CircleCenter.X, (int)_sprite2.CircleCenter.Y,
                                                           _sprite2.CircleRadius))
                    _messageSprite.Visible = true;
            }
            #endregion

            #region Collision Detection with Bounding Triangle
            if (CollisionDetection2D.CDPerformedWith == UseForCollisionDetection.Triangles)
            {
                if (CollisionDetection2D.BoundingTriangles(_movableSprite.TrianglePoints, _sprite1.TrianglePoints))
                    _messageSprite.Visible = true;

                if (CollisionDetection2D.BoundingTriangles(_movableSprite.TrianglePoints, _sprite2.TrianglePoints))
                    _messageSprite.Visible = true;
            }
            #endregion

            #region Collision Detection with PerPixel
            if (CollisionDetection2D.CDPerformedWith == UseForCollisionDetection.PerPixel)
            {
                if (CollisionDetection2D.PerPixel(_movableSprite.Texture,_sprite1.Texture,_movableSprite.Position,_sprite1.Position))
                    _messageSprite.Visible = true;

                if (CollisionDetection2D.PerPixel(_movableSprite.Texture, _sprite2.Texture, _movableSprite.Position, _sprite2.Position))
                    _messageSprite.Visible = true;
            }
            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            
            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}
