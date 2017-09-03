using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using Casting.Environment;
using Casting.Environment.Interfaces;
using Casting.Environment.Tools;
using Casting.Player;
using Casting.RayCasting;
using Casting.RayCasting.Interfaces;
using Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Rendering;

namespace Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private const double RotationTreshold = 30;

        private double movementSpeed = 0.02;
        private double rotationSpeed = 0.03;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Texture2D _wallCanvas;
        private Texture2D _sky;
        private Texture2D _floor;

        private IWallContainer _walls;
        private IRayCaster _caster;
        private IRayCaster _humanCaster;

        private EngineSettings _settings;
        private BackgroundPainter _painter;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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

            _settings = new EngineSettings()
            {
                Condition = CastCondition.LimitWalls(1),
                MapFilePath = @"..\..\..\..\Text\map.txt",
                WallFilePath = @"..\..\..\..\Text\wall.txt",
                ScreenPlane = new Vector(0.707, -0.707),
                Player = new Player() { Direction = new Vector(0.707, 0.707), HitPoints = 100, Weapon = null, Name = "Korela", Position = new Vector(0.1, 0.1),
                                      MovementCondition = HumanCastCondition.Default()  }
            };

            // TODO: Add your initialization logic here
            MapReader reader = new MapReader();

            IMap map = reader.ReadMap(_settings.MapFilePath);
            _walls = reader.ReadWalls(_settings.WallFilePath);

            _caster = new RayCaster(map, _walls);
            _humanCaster = new RayCaster(map, _walls);

            _painter = new BackgroundPainter(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _wallCanvas = new Texture2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);



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


            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;

            _sky = new Texture2D(GraphicsDevice, width, (int)Math.Floor(height / 2.0));

            _floor = new Texture2D(GraphicsDevice, width, (int)Math.Ceiling(height / 2.0));

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
            // TODO: use this.Content to load your game content here
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

            IVector nextPosition = _settings.Player.Position;
            foreach (Keys keyse in keys)
            {
                //todo should be in fact checked in each branch (function?)
                switch (keyse)
                {
                    case Keys.W:
                        nextPosition = Vector.Add(_settings.Player.Position, Vector.Multiply(movementSpeed, _settings.Player.Direction));
                        break;
                    case Keys.S:
                        nextPosition = Vector.Add(_settings.Player.Position, Vector.Multiply(-movementSpeed, _settings.Player.Direction));
                        break;

                    case Keys.A:
                        nextPosition = Vector.Add(_settings.Player.Position, Vector.Multiply(-movementSpeed, _settings.ScreenPlane));
                        break;

                    case Keys.D:
                        nextPosition = Vector.Add(_settings.Player.Position, Vector.Multiply(movementSpeed, _settings.ScreenPlane));
                        break;

                    default:

                        break;
                }
            }


            _settings.Player.Position = nextPosition;

            Point currMouse = Mouse.GetState().Position;
            Debug.WriteLine($"{currMouse.ToString()}");
            double width = GraphicsDevice.Viewport.Width;
            double difference = width / 2;
            difference = currMouse.X - difference;
            if (Math.Abs(difference) > RotationTreshold)
            {
                difference = 2 * difference / width * rotationSpeed;

                Debug.WriteLine($"{difference}");


                //left
                if (difference > 0)
                {
                    _settings.Player.Direction.Rotate(difference);
                    _settings.ScreenPlane.Rotate(difference);
                }
                else //right
                {

                    _settings.Player.Direction.Rotate(difference);
                    _settings.ScreenPlane.Rotate(difference);
                }
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

            // TODO: Add your drawing code here


            int width = GraphicsDevice.Viewport.Width;
            for (int x = 0; x < width; x++)
            {
                //todo check direction

                double planeMultiplier = 2.0 * x / width - 1;
                IVector planePart = Vector.Multiply(planeMultiplier, _settings.ScreenPlane);
                IVector direction = Vector.Add(_settings.Player.Direction, planePart);
                IRay ray = _caster.Cast(_settings.Player.Position, direction, _settings.Condition);


                Stopwatch watch = Stopwatch.StartNew();
                _painter.UpdateBuffer(ray, x);

                watch.Stop();

                // Debug.WriteLine("Watch: " + watch.ElapsedMilliseconds);
            }
            
            _wallCanvas.SetData<Color>(_painter.Buffer.BufferData);

            //Debug.WriteLine("First" + 1 / gameTime.ElapsedGameTime.TotalSeconds);
            spriteBatch.Begin();

            spriteBatch.Draw(_sky, new Rectangle(0,0, _sky.Width, _sky.Height), Color.White);
            spriteBatch.Draw(_floor, new Rectangle(0, _sky.Height, _floor.Width, _floor.Height), Color.White);
            spriteBatch.Draw(_wallCanvas, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            spriteBatch.End();

            // Debug.WriteLine(1 / gameTime.ElapsedGameTime.TotalSeconds);
            base.Draw(gameTime);

        }
    }
}



