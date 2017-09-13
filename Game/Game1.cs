using Casting.Environment.Interfaces;
using Casting.Environment.Tools;
using Casting.Player;
using Casting.Player.Interfaces;
using Casting.RayCasting;
using Casting.RayCasting.Interfaces;
using Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Ray = Casting.RayCasting.Ray;

namespace Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private const float RotationTreshold = 30;

        private float movementSpeed = 0.02F;
        private float rotationSpeed = 0.03F;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Texture2D _wallCanvas;
        private Texture2D _sky;
        private Texture2D _floor;

        private IMap _map;
        private IContainer<IWall> _walls;
        private IContainer<Enemy> _enemies;
        private IRayCaster _caster;

        private EngineSettings _settings;
        private Player _player;

        private BackgroundPainter _painter;

        private List<IRay> _currentRays;

        private SpriteFont _arialFont;

        private double frameRate;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 800,
                PreferredBackBufferWidth = 1024
                //IsFullScreen = true
            };
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _settings = new EngineSettings()
            {
                Condition = CastCondition.CastDistance(Double.MaxValue, 1),
                MapFilePath = @"..\..\..\..\Text\map.txt",
                WallFilePath = @"..\..\..\..\Text\wall.txt",
                EnemyFilePath = @"..\..\..\..\Text\enemies.txt"
            };

            _player = new Player(new Vector2(0.1F, 0.1F), Vector2.One, 100,
                HumanCastCondition.Default(), "Korela");

            MapReader reader = new MapReader();

            _map = reader.ReadMap(_settings.MapFilePath);
            _walls = reader.ReadWalls(_settings.WallFilePath);
            _settings.Condition = CastCondition.WallCountInterval(3, 1);
            _enemies = reader.ReadEnemies(_settings.EnemyFilePath);

            _caster = new RayCaster(_map, _walls);

            _painter = new BackgroundPainter(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _wallCanvas = new Texture2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            _currentRays = new List<IRay>();


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            int initialWidth = GraphicsDevice.Viewport.Width;
            int initialHeight = GraphicsDevice.Viewport.Height;

            _sky = new Texture2D(GraphicsDevice, initialWidth, (int)Math.Floor(initialHeight / 2.0));
            _floor = new Texture2D(GraphicsDevice, initialWidth, (int)Math.Ceiling(initialHeight / 2.0));


            #region Texture loading

            Color[] buff = new Color[_sky.Height * _sky.Width];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Color.LightSkyBlue;
            }
            _sky.SetData(buff);

            buff = new Color[_floor.Height * _floor.Width];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Color.LimeGreen;
            }
            _floor.SetData(buff);


            foreach (var wall in _walls)
            {
                using (FileStream stream = new FileStream(wall.TextureX.PicAddress, FileMode.Open))
                {
                    using (Texture2D texX = Texture2D.FromStream(GraphicsDevice, stream))
                    {
                        wall.TextureX.LoadTexture(texX);
                    }
                }

                using (FileStream stream = new FileStream(wall.TextureY.PicAddress, FileMode.Open))
                {
                    using (Texture2D texY = Texture2D.FromStream(GraphicsDevice, stream))
                    {
                        wall.TextureY.LoadTexture(texY);
                    }
                }
            }

            foreach (Enemy enemy in _enemies)
            {
                using (FileStream stream = new FileStream(enemy.Texture.PicAddress, FileMode.Open))
                {
                    using (Texture2D tex = Texture2D.FromStream(GraphicsDevice, stream))
                    {
                        enemy.Texture.LoadTexture(tex);
                    }
                }
            }

            #endregion




            // TODO: use this.Content to load your game content here

            _arialFont = Content.Load<SpriteFont>("font");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

            _wallCanvas.Dispose();
            _floor.Dispose();
            _sky.Dispose();

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keys = Keyboard.GetState().GetPressedKeys();
            
            Vector2 nextPosition = _player.Position;
            Vector2 currPosition = nextPosition;
            foreach (Keys keyse in keys)
            {
                //todo should be in fact checked in each branch (function?)
                switch (keyse)
                {
                    case Keys.W:
                        nextPosition = Vector2.Add(_player.Position, movementSpeed * _player.Direction);
                        break;
                    case Keys.S:
                        nextPosition = Vector2.Add(_player.Position, -movementSpeed * _player.Direction);
                        break;

                    case Keys.A:
                        nextPosition = Vector2.Add(_player.Position, -movementSpeed * _player.ScreenPlane);
                        break;

                    case Keys.D:
                        nextPosition = Vector2.Add(_player.Position, movementSpeed * _player.ScreenPlane);
                        break;
                    default:

                        break;
                }

                Vector2 dir = nextPosition - currPosition;
                float distance = dir.Length();
                if (distance > Double.Epsilon)
                {
                    _player.MovementCondition.ResetDistance(distance);
                    dir.Normalize();
                    var resultRay = _caster.Cast(_player.Position, dir, _player.MovementCondition);

                    bool canMove = true;
                    if (resultRay.ObjectsCrossed.Count > 0)
                    {
                        if (resultRay.ObjectsCrossed[0].Distance < distance)
                            canMove = false;
                    }


                    if (canMove)
                        _player.Position = nextPosition;
                }
            }


            Point currMouse = Mouse.GetState().Position;
            //Debug.WriteLine($"{currMouse}");

            float width = GraphicsDevice.Viewport.Width;
            float difference = width / 2F;
            difference = difference - currMouse.X;

            if (Math.Abs(difference) > RotationTreshold)
            {
                difference = 2 * difference / width * rotationSpeed;
                _player.Rotate(difference);

                //Debug.WriteLine($"{difference}");
            }

            // TODO: Add your update logic here


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _currentRays.Clear();
            // TODO: Add your drawing code here


            int width = GraphicsDevice.Viewport.Width;
            for (int x = 0; x < width; x++)
            {
                //todo check direction

                float planeMultiplier = 2F * x / width - 1;
                Vector2 planePart = planeMultiplier * _player.ScreenPlane;
                Vector2 direction = Vector2.Add(_player.Direction, planePart);
                IRay ray = _caster.Cast(_player.Position, direction, _settings.Condition);

                _currentRays.Add(ray);

                Stopwatch watch = Stopwatch.StartNew();

                watch.Stop();

                // Debug.WriteLine("Watch: " + watch.ElapsedMilliseconds);
            }

            foreach (Enemy enemy in _enemies)
            {
                Vector2 playerDist = enemy.Position - _player.Position;
                Matrix posMatrix = new Matrix(_player.Direction.Y, -_player.Direction.X, 0, 0, -_player.ScreenPlane.Y, _player.ScreenPlane.X, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                posMatrix *= 1 / (_player.ScreenPlane.X * _player.Direction.Y - _player.Direction.X * _player.ScreenPlane.Y);
                Vector2 result = new Vector2(playerDist.X * posMatrix.M11 + playerDist.Y * posMatrix.M12, playerDist.X * posMatrix.M21 + playerDist.Y * posMatrix.M22);

                int spriteXPos = (int)((width / 2) * (1 + result.X / result.Y));

                if (result.Y > 0)
                {

                    int spriteWidth = (int)(enemy.Width / result.Y);

                    for (int i = -spriteWidth / 2; i <= spriteWidth / 2; i++)
                    {
                        if (i + spriteXPos >= 0 && i + spriteXPos < _currentRays.Count)
                        {
                            double xPixel = (i + spriteWidth / 2) / (double)spriteWidth;
                            xPixel = xPixel == 1 ? 0 : xPixel;
                            //(int)((i + spriteWidth / 2) * (double)enemy.Width)
                            ((Ray)_currentRays[i + spriteXPos]).Add(new DistanceWrapper<ICrossable>(result.Y, xPixel, Side.SideX, enemy));
                        }
                    }
                }
            }


            for (int columnNr = 0; columnNr < _currentRays.Count; columnNr++)
            {
                _painter.UpdateBuffer(_currentRays[columnNr], columnNr, _walls.MaxHeight);
            }


            frameRate = 1 / gameTime.ElapsedGameTime.TotalSeconds;


            _wallCanvas.SetData<Color>(_painter.Buffer.BufferData);

            //Debug.WriteLine("First" + 1 / gameTime.ElapsedGameTime.TotalSeconds);
            spriteBatch.Begin();


            spriteBatch.Draw(_sky, new Rectangle(0, 0, _sky.Width, _sky.Height), Color.White);
            spriteBatch.Draw(_floor, new Rectangle(0, _sky.Height, _floor.Width, _floor.Height), Color.White);
            spriteBatch.Draw(_wallCanvas, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            if(gameTime.IsRunningSlowly)
                spriteBatch.DrawString(_arialFont, $"FPS: {frameRate}", new Vector2(0, 0), Color.Black);
            spriteBatch.End();

            // Debug.WriteLine(1 / gameTime.ElapsedGameTime.TotalSeconds);
            base.Draw(gameTime);

        }
    }
}



