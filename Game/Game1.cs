using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using Casting.Environment;
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
using Microsoft.Xna.Framework.Input.Touch;
using Rendering;

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

        private IContainer<IWall> _walls;
        private IContainer<IEnemy> _enemies;
        private IRayCaster _caster;
        private IRayCaster _humanCaster;

        private EngineSettings _settings;
        private BackgroundPainter _painter;

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

            _settings = new EngineSettings()
            {
                Condition = CastCondition.CastDistance(Double.MaxValue, 1),
                MapFilePath = @"..\..\..\..\Text\map.txt",
                WallFilePath = @"..\..\..\..\Text\wall.txt",
                ScreenPlane = new Vector2(0.707F, -0.707F),
                Player = new Player()
                {
                    Direction = new Vector2(0.707F, 0.707F),
                    HitPoints = 100,
                    Weapon = null,
                    Name = "Korela",
                    Position = new Vector2(0.1F, 0.1F),
                    MovementCondition = HumanCastCondition.Default()
                }
            };

            // TODO: Add your initialization logic here
            MapReader reader = new MapReader();

            IMap map = reader.ReadMap(_settings.MapFilePath);
            _walls = reader.ReadWalls(_settings.WallFilePath);
            _settings.Condition = CastCondition.WallCountInterval(3, 1);

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

            Vector2 nextPosition = _settings.Player.Position;
            Vector2 currPosition = nextPosition;
            foreach (Keys keyse in keys)
            {
                //todo should be in fact checked in each branch (function?)
                switch (keyse)
                {
                    case Keys.W:
                        nextPosition = Vector2.Add(_settings.Player.Position, movementSpeed * _settings.Player.Direction);
                        break;
                    case Keys.S:
                        nextPosition = Vector2.Add(_settings.Player.Position, -movementSpeed * _settings.Player.Direction);
                        break;

                    case Keys.A:
                        nextPosition = Vector2.Add(_settings.Player.Position, -movementSpeed * _settings.ScreenPlane);
                        break;

                    case Keys.D:
                        nextPosition = Vector2.Add(_settings.Player.Position, movementSpeed * _settings.ScreenPlane);
                        break;
                    default:

                        break;
                }
                
                Vector2 dir = nextPosition - currPosition;
                float distance = dir.Length();
                if (distance > Double.Epsilon)
                {
                    _settings.Player.MovementCondition.ResetDistance(distance);
                    dir.Normalize();
                    var resultRay = _humanCaster.Cast(_settings.Player.Position, dir, _settings.Player.MovementCondition);

                    bool canMove = true;
                    if (resultRay.ObjectsCrossed.Count > 0)
                    {
                        if (resultRay.ObjectsCrossed[0].Distance < distance)
                            canMove = false;
                    }
                    

                    if(canMove)
                        _settings.Player.Position = nextPosition;
                }
            }


            Point currMouse = Mouse.GetState().Position;
            Debug.WriteLine($"{currMouse}");
            float width = GraphicsDevice.Viewport.Width;
            float difference = width / 2F;
            difference = difference - currMouse.X;
            if (Math.Abs(difference) > RotationTreshold)
            {
                difference = 2 * difference / width * rotationSpeed;

                Debug.WriteLine($"{difference}");


                Matrix rotation = Matrix.CreateRotationZ(difference);
                _settings.Player.Direction = Vector2.Transform(_settings.Player.Direction, rotation );
                _settings.ScreenPlane = Vector2.Transform(_settings.ScreenPlane, rotation);
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

                float planeMultiplier = 2F * x / width - 1;
                Vector2 planePart = planeMultiplier * _settings.ScreenPlane;
                Vector2 direction = Vector2.Add(_settings.Player.Direction, planePart);
                IRay ray = _caster.Cast(_settings.Player.Position, direction, _settings.Condition);


                Stopwatch watch = Stopwatch.StartNew();
                _painter.UpdateBuffer(ray, x, _walls.MaxHeight);

                watch.Stop();

                // Debug.WriteLine("Watch: " + watch.ElapsedMilliseconds);
            }

            foreach (IEnemy enemy in _enemies)
            {
                Vector2 playerDist = _settings.Player.Position - enemy.Position;
                Matrix posMatrix = new Matrix(_settings.Player.Direction.Y, -_settings.Player.Direction.X,0,0, - _settings.ScreenPlane.Y, _settings.ScreenPlane.X,0,0,0,0,0,0,0,0,0,0);
                posMatrix = Matrix.Invert(posMatrix);
                posMatrix *= 1/(_settings.ScreenPlane.X* _settings.Player.Direction.Y - _settings.Player.Direction.X * _settings.ScreenPlane.Y);
                Vector2 result = new Vector2(playerDist.X * posMatrix.M11 + playerDist.Y * posMatrix.M21, playerDist.X * posMatrix.M12 + playerDist.Y * posMatrix.M22);



            }


            _wallCanvas.SetData<Color>(_painter.Buffer.BufferData);

            //Debug.WriteLine("First" + 1 / gameTime.ElapsedGameTime.TotalSeconds);
            spriteBatch.Begin();

            spriteBatch.Draw(_sky, new Rectangle(0, 0, _sky.Width, _sky.Height), Color.White);
            spriteBatch.Draw(_floor, new Rectangle(0, _sky.Height, _floor.Width, _floor.Height), Color.White);
            spriteBatch.Draw(_wallCanvas, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            spriteBatch.End();

            // Debug.WriteLine(1 / gameTime.ElapsedGameTime.TotalSeconds);
            base.Draw(gameTime);

        }
    }
}



