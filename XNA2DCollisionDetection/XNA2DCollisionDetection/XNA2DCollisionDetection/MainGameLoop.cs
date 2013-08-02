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
using System.Collections;

using XNA2DCollisionDetection.Sprites;

namespace XNA2DCollisionDetection
{
    public class MainGameLoop : Microsoft.Xna.Framework.Game
    {
        #region Variables
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private List<Texture2D> tiles = new List<Texture2D>();
        private List<Bullet> _bullets = new List<Bullet>();
        private List<Health> _healths = new List<Health>();
        private List<Coins> _coinslist = new List<Coins>();


        private GenericSprite _movableSprite;
        private GenericSprite _tank2;
        private List<GenericSprite> tanks = new List<GenericSprite>();

        private Texture2D _grid;
        private Texture2D _jj;
        private Texture2D background;
        private Texture2D _log;

        private Bullet _bullet;
        private MessageSprite _messageSprite;

        private List<Point> _tiles = new List<Point>();
        private List<Point> _tiles2 = new List<Point>();
        private List<Point> _tiles3 = new List<Point>();

        public bool read = true;
        public string paramString = "";
        public String initString = null;


        //Map format
        static int tileWidht = 36;
        static int tileHeight = 36;
        int[,] map;

        #endregion

        //Adding points
        private void PointAdd()
        {
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 1)
                        _tiles.Add(new Point(i, j));
                    if (map[i, j] == 2)
                        _tiles2.Add(new Point(i, j));
                    if (map[i, j] == 3)
                        _tiles3.Add(new Point(i, j));

                }
        }
        //Mapp
        public void reset()
        {
            string[] obstacles = initString.Substring(0, initString.Length - 1).Split(':');

            map = new int[20, 20];
            //System.out.println("Init "+init);
            string[] bricks = obstacles[2].Split(';');
            string[] stone = obstacles[3].Split(';');
            string[] water = obstacles[4].Split(';');
            //System.out.println("Brick ");
            for (int i = 0; i < bricks.Length; i++)
            {
                int x = Int32.Parse(bricks[i].Split(',')[0]);
                int y = Int32.Parse(bricks[i].Split(',')[1]);
                map[y, x] = 3;
                //System.out.println(x+" "+y);
            }
            //System.out.println("Stone ");
            for (int i = 0; i < stone.Length; i++)
            {
                int x = Int32.Parse(stone[i].Split(',')[0]);
                int y = Int32.Parse(stone[i].Split(',')[1]);
                map[y, x] = 2;
                //System.out.println(x+" "+y);
            }
            //System.out.println("Water ");
            for (int i = 0; i < water.Length; i++)
            {
                int x = Int32.Parse(water[i].Split(',')[0]);
                int y = Int32.Parse(water[i].Split(',')[1]);
                map[y, x] = 1;
                //System.out.println(x+" "+y);
            }
        }
        //Scoring      
        public int score
        {
            get;
            set;
        }
        public void AddScore(int val)
        {
            _movableSprite.Score += val;
        }

        public MainGameLoop()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 740;
            _graphics.PreferredBackBufferWidth = 1000;
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();
            CollisionDetection2D.CDPerformedWith = UseForCollisionDetection.PerPixel;
        }

        #region Graphic
        protected override void LoadContent()
        {
            score = 10;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), _spriteBatch);
            _grid = Content.Load<Texture2D>("Bitmap1");
            // _coins = new Coins(this, "32952", 19, 19, 200);
            //_health = new Health(this, "cross", 3, 2, 200);
            tiles.Add(_grid);
            tiles.Add(Content.Load<Texture2D>("Life"));
            tiles.Add(Content.Load<Texture2D>("Goody"));
            tiles.Add(Content.Load<Texture2D>("Stone"));
            _jj = Content.Load<Texture2D>("P02");
            background = Content.Load<Texture2D>("CC");
            _log = Content.Load<Texture2D>("log");
            _tank2 = new GenericSprite(this, "P02", new Vector2(40 + 36 * 2, 40 + 36 * 3), new Vector2(20, 20));

            //Initial pixle gain by 26 26          
            _movableSprite = new GenericSprite(this, "P01", new Vector2(40, 40), new Vector2(20, 20));

            _movableSprite.IsMovable = true;
            _tank2.IsMovable = true;
            while (initString == null) { }
            reset();
            PointAdd();
            _messageSprite = new MessageSprite(this, "  Coins    Health", new Vector2(780f, 285f), Color.Red);
            _messageSprite.Visible = false;
            Components.Add(_messageSprite);

        }
        #endregion

        protected override void UnloadContent()
        {
            _movableSprite.Dispose();
            _messageSprite.Dispose();
        }
        //
        int j = 1;
        private List<string> players = new List<string>();
        private Hashtable playerTable = new Hashtable();
        //
        protected override void Update(GameTime gameTime)
        {

            //////Coins disapper//////////////////////////////////////////
            for (int i = 0; i < _coinslist.Count; i++)
            {

                if (_coinslist[i].Time < System.Environment.TickCount)
                {
                    _coinslist[i].Visible = false;
                    _coinslist.RemoveAt(i);
                }

            }
            //health disapper
            for (int i = 0; i < _healths.Count; i++)
            {

                if (_healths[i].Time < System.Environment.TickCount)
                {
                    _healths[i].Visible = false;
                    _healths.RemoveAt(i);
                }

            }
            ////////////////////////////////////////////
            if (paramString.ToCharArray()[0] == 'G' && !read)
            {
                string[] splitter = paramString.Split(':');
                for (int i = 1; i < splitter.Length; i++)
                {
                    string player = splitter[i].Substring(0, 2);
                    if (player.ToCharArray()[0] != 'P')
                    {
                        continue;
                    }
                    bool playerAvailable = false;
                    foreach (string p in players)
                    {
                        if (p.Equals(player))
                        {
                            playerAvailable = true;
                            break;
                        }
                    }
                    if (!playerAvailable)
                    {
                        players.Add(player);
                        GenericSprite ii = new GenericSprite(this, "P0" + j, new Vector2(40, 40), new Vector2(780f, 300 + 40 * j));
                        ii.Health = 100;
                        playerTable.Add(player, ii);
                        j++;
                        Components.Add(ii);
                    }
                    int X = Int32.Parse(splitter[i].Split(';')[1].Split(',')[0]);
                    int Y = Int32.Parse(splitter[i].Split(';')[1].Split(',')[1]);
                    int direction = Int32.Parse(splitter[i].Split(';')[2]);
                    int shoot = Int32.Parse(splitter[i].Split(';')[3]);
                    GenericSprite playerObject = (GenericSprite)playerTable[player];
                    playerObject.setVal(direction, X, Y);
                    if (shoot == 1)
                    {
                        _bullet = new Bullet(this);
                        _bullet.Position(direction, X, Y, System.Environment.TickCount);
                        _bullet.Visible = true;
                        Components.Add(_bullet);
                        _bullets.Add(_bullet);
                    }
                }
                read = true;
            }
            for (int i = 0; i < _bullets.Count; i++)
            {
                long cTime = System.Environment.TickCount;
                int val = (int)(((cTime - _bullets[i].Time) * 36) / 333);
                _bullets[i].Speed = val * (new Vector2((float)Math.Cos(_bullets[i].Angle), (float)Math.Sin(_bullets[i].Angle)));
                _bullets[i].SetPos(_bullets[i].Speed);
                //_bullets[i].Time = System.Environment.TickCount;                

            }
            //////Adding coins//////
            if ((paramString.ToCharArray()[0] == 'C' && paramString.ToCharArray()[1] == ':'))
            {
                if (!read)
                {
                    String data = paramString.Substring(0, paramString.Length - 1);
                    String[] temp = data.Split(':');
                    int x = Int32.Parse(temp[1].Split(',')[0]);
                    int y = Int32.Parse(temp[1].Split(',')[1]);
                    long endTime = Int64.Parse(temp[2]) + System.Environment.TickCount;
                    int val = Int32.Parse(temp[3]);
                    Coins coins = new Coins(this, "32952", x, y, endTime, val);
                    Components.Add(coins);
                    _coinslist.Add(coins);
                    read = true;
                }
            }
            //Health packs//////////
            if ((paramString.ToCharArray()[0] == 'L' && paramString.ToCharArray()[1] == ':'))
            {
                if (!read)
                {
                    String data = paramString.Substring(0, paramString.Length - 1);
                    String[] temp = data.Split(':');
                    int x = Int32.Parse(temp[1].Split(',')[0]);
                    int y = Int32.Parse(temp[1].Split(',')[1]);
                    long endTime = Int64.Parse(temp[2]) + System.Environment.TickCount;
                    Health health = new Health(this, "cross", x, y, endTime, 20);
                    Components.Add(health);
                    _healths.Add(health);
                    read = true;
                }
            }

            /////////////////////////////////////

            _messageSprite.Visible = false;
            //if (_bullets.Count > 0)
            //    if (_bullets[_bullets.Count - 1].Visible == false)
            //        if (keypress.IsKeyDown(Keys.Space))
            //        {
            //            _bullets[_bullets.Count - 1].Position(_movableSprite.Position, _movableSprite.Angle);
            //            _bullets[_bullets.Count - 1].Visible = true;
            //        }
            //perpixel
            #region Collision Detection
            if (CollisionDetection2D.CDPerformedWith == UseForCollisionDetection.PerPixel)
            {
                ///new vector(add 4 to x value, take to the middle of tile with 22 adding because of top margin)
                ///
                //vector2(x,y)
                //new Vector2(36 * i + 4, 40 + j * 36)
                for (int j = 0; j < playerTable.Count; j++)
                {
                    GenericSprite player = (GenericSprite)playerTable[players[j]];
                    for (int i = 0; i < _tiles.Count; i++)
                        if (CollisionDetection2D.PerPixel(_movableSprite.Texture, tiles[1], player.Position, new Vector2(36 * (_tiles[i].Y + 1) + 4, 40 + _tiles[i].X * 36)))
                            _messageSprite.Visible = true;
                    for (int i = 0; i < _tiles2.Count; i++)
                        if (CollisionDetection2D.PerPixel(_movableSprite.Texture, tiles[2], player.Position, new Vector2(36 * (_tiles2[i].Y + 1) + 4, 40 + _tiles2[i].X * 36)))
                            _messageSprite.Visible = true;
                    for (int i = 0; i < _tiles3.Count; i++)
                        if (CollisionDetection2D.PerPixel(_movableSprite.Texture, tiles[3], player.Position, new Vector2(36 * (_tiles3[i].Y + 1) + 4, 40 + _tiles3[i].X * 36)))
                            _messageSprite.Visible = true;
                    // Brick detection is over
                }
                //Bullet hit for any box or tank with box and other empty box (x,y) =(10+36*X,10+36*Y)
                //changes in system
                for (int j = 0; j < playerTable.Count; j++)
                {
                    GenericSprite player = (GenericSprite)playerTable[players[j]];
                    for (int i = 0; i < _bullets.Count; i++)
                    {
                        if (CollisionDetection2D.PerPixel(_bullet.Texture, _movableSprite.Texture, _bullets[i].Station, player.Position))
                        {
                            if (player.Position != _bullets[i].Intial)
                            {
                                player.Health -= 10;
                                _bullets[i].Visible = false;
                                _bullets.RemoveAt(i);
                                if (player.Health == 0)
                                    player.Visible = false;
                            }

                        }
                    }
                    if (player.Health == 0)
                        playerTable.Remove(player);
                }
                //Coins and Health pack detection
                if (_coinslist.Count > 0)
                    for (int i = 0; i < playerTable.Count; i++)
                    {

                        GenericSprite player = (GenericSprite)playerTable[players[i]];
                        for (int j = 0; j < _coinslist.Count; j++)
                            if (CollisionDetection2D.PerPixel(_movableSprite.Texture, tiles[1], player.Position, _coinslist[j].Position))
                            {
                                _coinslist[j].Visible = false;
                                player.Score += _coinslist[j].Value;
                                _coinslist.RemoveAt(j);
                            }
                    }
                if (_healths.Count > 0)
                    for (int i = 0; i < playerTable.Count; i++)
                    {
                        GenericSprite player = (GenericSprite)playerTable[players[i]];
                        for (int j = 0; j < _healths.Count; j++)
                            if (CollisionDetection2D.PerPixel(_movableSprite.Texture, tiles[1], player.Position, _healths[j].Position))
                            {
                                _healths[j].Visible = false;
                                player.Health += _healths[j].Value;
                                _healths.RemoveAt(j);
                            }
                    }
                ////

                //Bullet hit for any box or tank with box and other empty box (x,y) =(10+36*X,10+36*Y)
                //if (CollisionDetection2D.PerPixel(_bullets[0].Texture, _jj, _bullets[0].Station, new Vector2(36 * 3, 36 * 7)))
                //{
                //    _messageSprite.Visible = true;
                //}
            }
            #endregion

            //time out 

            base.Update(gameTime);
        }

        #region Drawing methos
        protected override void Draw(GameTime gameTime)
        {

            //GraphicsDevice.Clear(Color.FloralWhite);
            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            _spriteBatch.Draw(_log, new Vector2(730, 190),Color.Wheat);
            _messageSprite.Visible = true;

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    _spriteBatch.Draw(tiles[map[i, j]], new Rectangle(j * tileWidht + 22, i * tileHeight + 22, tileWidht, tileHeight), Color.White);
                }
            }
            base.Draw(gameTime);
            _spriteBatch.End();
        }
        #endregion
    }
}
