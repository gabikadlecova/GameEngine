using Casting.Environment.Interfaces;
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
using System.IO;
using Ray = Casting.RayCasting.Ray;

namespace Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //resolution
        private int _height = 800;
        private int _width = 600;

        /// <summary>
        /// Represents the pixel distance from the middle of the screen. In this area, no view rotation occurs
        /// </summary>
        private const float RotationTreshold = 30;
        /// <summary>
        /// If the distance of two objects is less than this constant, the objects touch.
        /// </summary>
        private const float MinEnemyDistance = 0.2F;
        /// <summary>
        /// Prevents enemies from spawning too close to the player
        /// </summary>
        private const float PlayerProtectDist = 2;
        /// <summary>
        /// Determines the view rotation speed
        /// </summary>
        private const float RotationSpeed = 0.03F;
        /// <summary>
        /// Limits animation of explosions and so like
        /// </summary>
        private const int AnimationMils = 300;

        //graphics
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private BackgroundPainter _painter;
        private readonly Random _random = new Random();

        //objects to be drawn
        private Texture2D _wallCanvas;
        private Texture2D _sky;
        private Texture2D _floor;
        private List<MovingObject> _currentSprites;


        private Player _player;
        private List<Enemy> _enemies;

        //singleton data
        private IMap _map;
        private IContainer<IWall> _walls;
        private Dictionary<int, EnemyData> _enemyData;
        private List<IWeapon> _weapons;

        //raycasting tools
        private RayCaster _caster;
        private ICastCondition _condition;
        private readonly HumanCastCondition _default = HumanCastCondition.Default();
        private List<Ray> _currentRays;



        //game related string drawing
        private SpriteFont arialFont;
        private SpriteFont bigFont;
        private double frameRate;
        private int frameCount;

        //in-game changing values
        private bool paused;

        private bool pausedBefore;
        private bool resetedBefore;
        private TimeSpan enemySpawnChange;
        private TimeSpan lastSpawnChange;
        private int kills;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 800,
                PreferredBackBufferWidth = 1024
            };
            IsFixedTimeStep = true;
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

            GameReader reader = new GameReader();

            EngineSettings settings = reader.LoadSettings(@"..\..\..\..\settings.txt");

            _condition = settings.Condition;
            _width = settings.Width;
            _height = settings.Height;


            _map = reader.ReadMap(settings.MapFilePath);
            _walls = reader.ReadWalls(settings.WallFilePath);
            _caster = new RayCaster(_map, _walls);
            _enemyData = reader.ReadEnemies(settings.EnemyFilePath, _caster);
            _weapons = reader.ReadWeapons(settings.WeaponFilePath, _caster);


            _player = reader.ReadPlayer(settings.PlayerFilePath, _caster);
            _player.Weapon = _weapons[0];


            _painter = new BackgroundPainter(_width, _height, settings.SkyFilePath, settings.FloorFilePath);
            _wallCanvas = new Texture2D(GraphicsDevice, _width, _height);



            _currentRays = new List<Ray>();
            _enemies = new List<Enemy>();
            _currentSprites = new List<MovingObject>();

            //initializing counters
            lastSpawnChange = TimeSpan.Zero;
            enemySpawnChange = new TimeSpan(0, 0, 0, settings.EnemySpawnSecs);


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //texture data needs to be loaded into memory in order to provide fast access to pixels
            #region Texture loading

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

            foreach (EnemyData data in _enemyData.Values)
            {
                using (FileStream stream = new FileStream(data.SpriteData.LivingPic.PicAddress, FileMode.Open))
                {
                    using (Texture2D tex = Texture2D.FromStream(GraphicsDevice, stream))
                    {
                        data.SpriteData.LivingPic.LoadTexture(tex);
                    }
                }

                using (FileStream stream = new FileStream(data.SpriteData.DeadPic.PicAddress, FileMode.Open))
                {
                    using (Texture2D tex = Texture2D.FromStream(GraphicsDevice, stream))
                    {
                        data.SpriteData.DeadPic.LoadTexture(tex);
                    }
                }
            }

            foreach (var weapon in _weapons)
            {
                using (FileStream stream = new FileStream(weapon.BulletData.LivingPic.PicAddress, FileMode.Open))
                {
                    using (Texture2D tex = Texture2D.FromStream(GraphicsDevice, stream))
                    {
                        weapon.BulletData.LivingPic.LoadTexture(tex);
                    }
                }

                using (FileStream stream = new FileStream(weapon.BulletData.DeadPic.PicAddress, FileMode.Open))
                {
                    using (Texture2D tex = Texture2D.FromStream(GraphicsDevice, stream))
                    {
                        weapon.BulletData.DeadPic.LoadTexture(tex);
                    }
                }
            }

            using (FileStream stream = new FileStream(_painter.Sky.PicAddress, FileMode.Open))
            {
                using (Texture2D tex = Texture2D.FromStream(GraphicsDevice, stream))
                {
                    _painter.Sky.LoadTexture(tex);
                }
            }

            using (FileStream stream = new FileStream(_painter.Floor.PicAddress, FileMode.Open))
            {
                using (Texture2D tex = Texture2D.FromStream(GraphicsDevice, stream))
                {
                    _painter.Floor.LoadTexture(tex);
                }
            }

            #endregion




            // TODO: use this.Content to load your game content here


            arialFont = Content.Load<SpriteFont>("font");
            bigFont = Content.Load<SpriteFont>("bigfont");

            _floor = Content.Load<Texture2D>("floor");
            _sky = Content.Load<Texture2D>("sky");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

            _wallCanvas.Dispose();

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            #region Key state

            KeyboardState state = Keyboard.GetState();

            /***** close the game *****/
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
                Exit();

            /***** soft reset *****/
            if (state.IsKeyDown(Keys.F3))
            {
                if (!resetedBefore)
                {
                    resetedBefore = true;
                    Initialize();

                    LoadContent();
                    GC.Collect();
                }
            }
            else
            {
                //prevents from reseting
                resetedBefore = false;
            }

            /***** pause *****/
            if (state.IsKeyDown(Keys.P))
            {
                if (!pausedBefore)
                {
                    pausedBefore = true;
                    paused = !paused;
                }
            }
            else
            {
                pausedBefore = false;
            }

            /***** hard reset *****/
            if (state.IsKeyDown(Keys.F12))
            {
                Program.RestartGame = true;
                Exit();
            }



            if (_player.IsKilled || paused)
            {
                return;
            }



            //movement input
            var keys = state.GetPressedKeys();

            Vector2 stepVector = Vector2.Zero;

            bool shooting = false;
            foreach (Keys keyse in keys)
            {
                switch (keyse)
                {
                    case Keys.W:
                        stepVector = _player.Direction;
                        break;
                    case Keys.S:
                        stepVector = -_player.Direction;
                        break;

                    case Keys.A:
                        stepVector = -_player.ScreenPlane;
                        break;

                    case Keys.D:
                        stepVector = _player.ScreenPlane;
                        break;

                    //for shooting
                    case Keys.Space:
                        shooting = true;
                        break;
                    case Keys.F5:
                        graphics.IsFullScreen = !graphics.IsFullScreen;
                        graphics.ApplyChanges();
                        break;


                    //weapons can be changed using numbered keys (all without other function)
                    default:

                        string keyValue = keyse.ToString();

                        char lastChar = keyValue[keyValue.Length - 1];

                        byte weaponNo;
                        if (byte.TryParse(lastChar.ToString(), out weaponNo))
                        {
                            weaponNo = (byte)(weaponNo == 0 ? 9 : weaponNo - 1);
                            if (weaponNo < _weapons.Count && _player.Weapon.Bullets.Count == 0)
                            {
                                _player.Weapon = _weapons[weaponNo];
                            }
                        }

                        break;
                }


                //player will 
                _player.Move(stepVector);

            }

            #endregion


            #region Mouse state

            Point currMouse = Mouse.GetState().Position;

            float screenWidth = GraphicsDevice.Viewport.Width;
            float difference = screenWidth / 2F - currMouse.X;

            //if the mouse position is close to the middle of the screen, the view is no rotated
            if (Math.Abs(difference) > RotationTreshold)
            {
                difference = 2 * difference / screenWidth * RotationSpeed;
                _player.Rotate(difference);

            }

            #endregion



            #region Enemy state

            //different enemy types have different spawn times
            foreach (var enemyData in _enemyData.Values)
            {
                TimeSpan currTime = gameTime.TotalGameTime;

                float time = enemyData.SpawnTime;
                TimeSpan offSet = new TimeSpan(0, 0, 0, (int)time, (int)((time - (int)time) * 1000));
                if (currTime > enemyData.LastSpawn + offSet)
                {
                    Vector2 enemyPos;
                    do
                    {
                        float posX = _random.Next(0, _map.Width);
                        float posY = _random.Next(0, _map.Height);
                        enemyPos = new Vector2(posX, posY);
                    }
                    while ((_player.Position - enemyPos).Length() < PlayerProtectDist);

                    Enemy nextEnemy = new Enemy(enemyPos, new Vector2(0.707F, 0.707F), _default, enemyData, _caster);

                    _enemies.Add(nextEnemy);
                    _currentSprites.Add(nextEnemy);

                    enemyData.LastSpawn = currTime;
                }

                //spawns are periodically changed
                if (currTime > lastSpawnChange + enemySpawnChange)
                {
                    enemyData.SpawnTime = enemyData.SpawnTime > 8 ? enemyData.SpawnTime / 2.0F : 8;
                    lastSpawnChange = currTime;
                }
            }


            //in order to remove objects from a list inside a for loop, we need to calculate new indices of object following
            //with each enemy removal, offset must be incremented
            int offset = 0;
            for (int i = 0; i < _enemies.Count; i++)
            {
                var enemy = _enemies[i - offset];

                Vector2 toPlayer = _player.Position - enemy.Position;

                //hits, movement and player damaging is exclusive to living enemies
                if (!enemy.IsKilled)
                {
                    //we check if the enemy is hit by bullets
                    foreach (var bullet in _player.Weapon.Bullets)
                    {

                        //exploded bullets are excluded
                        if (!bullet.HasHit)
                        {
                            Vector2 dist = bullet.Position - enemy.Position;
                            if (Math.Abs(dist.Length()) < enemy.HitBox)
                            {
                                enemy.HitPoints--;

                                //HitPoints must be 0, enemy could be killed by multiple bullets at one time, but kills should be increased once
                                if (enemy.IsKilled && enemy.HitPoints == 0)
                                {
                                    kills++;
                                    enemy.DeathSecs = gameTime.TotalGameTime.Seconds;
                                }

                                bullet.Hit(gameTime.TotalGameTime);
                            }
                        }
                    }

                    //if the enemy touches the player, they damage each other
                    if (toPlayer.Length() < MinEnemyDistance)
                    {
                        _player.HitPoints--;
                        enemy.HitPoints--;
                    }

                    //all enemies move towards the player
                    enemy.Move(toPlayer);

                    /***** random enemy teleportation *****/
                    //Enemies tend to make clusters while moving alongside the walls. This can result into easy HP gains, so the enemies are randomly teleported
                    int random = _random.Next();
                    if (random % 2000 == 0)
                    {
                        bool possible = false;
                        while (!possible)
                        {
                            enemy.Position = new Vector2(_random.Next(0, _map.Width - 1), _random.Next(0, _map.Height - 1));
                            toPlayer = enemy.Position - _player.Position;

                            //the enemy should not teleport on the player (instant damage)
                            //also, we need to avoid wall ports 
                            if (toPlayer.Length() >= PlayerProtectDist && _map[(int)enemy.Position.X, (int)enemy.Position.Y] == 0)
                            {
                                possible = true;
                            }
                        }
                    }
                }
                else //dead enemies can be collected or they disappear some time (better performance)
                {
                    bool close = toPlayer.Length() < MinEnemyDistance;
                    if (close || enemy.DeathSecs + 6 < gameTime.TotalGameTime.Seconds)
                    {
                        if (close)
                        {
                            _player.HitPoints++;
                        }
                        _currentSprites.Remove(enemy);
                        _enemies.Remove(enemy);
                        offset++;
                    }
                }
            }
            #endregion


            #region Shooting

            IWeapon currentWeapon = _player.Weapon;
            int count = currentWeapon.Bullets.Count;

            //shooting has been set above in Key state region
            //after a bullet hits, it can be shot again (so the ammo is actually infinite)
            shooting = currentWeapon.MaxAmmo > count && shooting;

            //next bullet cannot be shot immediately after the first one
            if (currentWeapon.Bullets.Count > 0)
            {
                Vector2 lastBullet = currentWeapon.Bullets[count - 1].Position;
                lastBullet = lastBullet - _player.Position;
                if (Math.Abs(lastBullet.Length()) < currentWeapon.MinBulletDist)
                {
                    shooting = false;
                }
            }

            //after all conditions are met, the bullet is shot straight forward
            if (shooting)
            {
                Bullet bullet = currentWeapon.Shoot(_player.Position, _player.Direction);
                if (bullet != null)
                {
                    _currentSprites.Add(bullet);
                }
            }


            offset = 0;
            for (int index = 0; index < currentWeapon.Bullets.Count; index++)
            {
                var bullet = currentWeapon.Bullets[index - offset];

                //after the bullets hits an obstacle or an enemy, it explodes
                if (bullet.HasHit)
                {
                    //the bullet explosion can be seen for a small period of a time (so the bullet should not be removed immediately)
                    TimeSpan shotDiff = gameTime.TotalGameTime - bullet.AnimationTime;
                    if (shotDiff.Milliseconds >= AnimationMils)
                    {
                        currentWeapon.Bullets.Remove(bullet);
                        _currentSprites.Remove(bullet);
                        offset++;
                    }
                }
                else
                {
                    //the bullet hasn't hit an enemy and hits a wall
                    if (!bullet.Move(bullet.Direction))
                    {
                        bullet.Hit(gameTime.TotalGameTime);
                    }

                }
            }

            #endregion

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GC.Collect();

            #region Walls

            //raycasting – gathering info for wall drawing
            _currentRays = _caster.FieldOfView(_width, _player.Position, _player.Direction, _player.ScreenPlane, _condition);

            #endregion

            #region Sprites

            //using a formula for calculating inverse matrix of rank 2
            Matrix invMatrix = new Matrix(_player.Direction.Y, -_player.Direction.X, 0, 0, -_player.ScreenPlane.Y, _player.ScreenPlane.X, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            invMatrix *= 1 / (_player.ScreenPlane.X * _player.Direction.Y - _player.Direction.X * _player.ScreenPlane.Y);

            //each sprite is projected on the screen
            foreach (var objectSprite in _currentSprites)
            {
                Vector2 playerDist = objectSprite.Position - _player.Position;

                //the X coordinate indicates the position on the screen, the Y coordinate determines the distance from the screen plane
                Vector2 result = new Vector2(playerDist.X * invMatrix.M11 + playerDist.Y * invMatrix.M12,
                                             playerDist.X * invMatrix.M21 + playerDist.Y * invMatrix.M22);

                //perpendicular distance from the screen plane to avoid fish eye
                int spriteXPos = (int)(_width / 2F * (1 + result.X / result.Y));

                //is sprite in front of the player?
                if (result.Y > 0)
                {
                    ICrossable crossableSprite = objectSprite as ICrossable;
                    if (crossableSprite == null)
                    {
                        throw new NullReferenceException("Moving object must be ICrossable in order to be drawn.");
                    }

                    //this determines which columns contain a part of the sprite
                    int spriteWidth = (int)(crossableSprite.Width / result.Y);

                    for (int i = -spriteWidth / 2; i < spriteWidth / 2; i++)
                    {
                        int currIndex = i + spriteXPos;

                        if (currIndex >= 0 && currIndex < _width)
                        {
                            double xPixel = (i + spriteWidth / 2) / (double)spriteWidth;
                            //as a part of the sprite could be behind the wall, we need to draw it by columns 
                            _currentRays[currIndex].Add(new DistanceWrapper<ICrossable>(result.Y, xPixel, Side.SideX, crossableSprite, objectSprite.Position, false));
                        }
                    }
                }
            }
            #endregion

            //creates an array of color data from ray information
            _painter.UpdateBuffer(_currentRays, _walls.MaxHeight, _player.Position, _player.Direction);
            _wallCanvas.SetData<Color>(_painter.Buffer);

            //smoother framerate update
            if ((frameCount = frameCount % 5) == 0)
            {
                frameRate = Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds);
            }
            frameCount++;


            spriteBatch.Begin();

            spriteBatch.Draw(_sky, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height / 2), Color.White);
            spriteBatch.Draw(_floor, new Rectangle(0, GraphicsDevice.Viewport.Height / 2, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height / 2), Color.White);
            spriteBatch.Draw(_wallCanvas, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.DrawString(arialFont, $"FPS: {frameRate}", new Vector2(0, 0), gameTime.IsRunningSlowly ? Color.Red : Color.Black);

            if (_player.IsKilled)
            {
                spriteBatch.DrawString(bigFont, $"YOU DIED!", new Vector2(GraphicsDevice.Viewport.Width / 2 - 30, GraphicsDevice.Viewport.Height / 2 - 30), Color.Red);
            }

            spriteBatch.DrawString(arialFont, $"HP: {_player.HitPoints}", new Vector2(GraphicsDevice.Viewport.Width - 100, 0), Color.Black);
            spriteBatch.DrawString(arialFont, $"Kills: {kills}", new Vector2(GraphicsDevice.Viewport.Width - 100, 25), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);

        }
    }
}



