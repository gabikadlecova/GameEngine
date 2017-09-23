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
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Ray = Casting.RayCasting.Ray;

namespace Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private int _height = 800;
        private int _width = 600;

        private const float RotationTreshold = 30;
        private const float MinEnemyDistance = 0.2F;
        private const float MinBulletDistance = 1;
        private const float PlayerProtectDist = 2;
        private const float RotationSpeed = 0.03F;
        private const int AnimationMils = 300;


        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private readonly Random _random = new Random();

        private Texture2D _wallCanvas;
        private Texture2D _sky;
        private Texture2D _floor;
        private List<Ray> _currentRays;
        private List<MovingObject> _currentSprites;
        private List<Enemy> _enemies;

        private IMap _map;
        private IContainer<IWall> _walls;
        private Dictionary<int, EnemyData> _enemyData;
        private List<IWeapon> _weapons;


        private RayCaster _caster;
        private ICastCondition _condition;
        private readonly HumanCastCondition _default = HumanCastCondition.Default();

        private Player _player;


        private BackgroundPainter _painter;

        private SpriteFont arialFont;
        private double frameRate;

        private bool resetedBefore;
        private TimeSpan enemySpawnChange; // = new TimeSpan(0,0,0,10);
        private TimeSpan LastSpawnChange; // = TimeSpan.Zero;


        private int kills; // = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 800,
                PreferredBackBufferWidth = 1024
                //,IsFullScreen = true
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

            EngineSettings settings = reader.LoadSettings("settings.txt");

            _player = new Player(new Vector2(0.1F, 0.1F), Vector2.One, 10,
                    _default, "Korela", 0.020F, _caster);



            _map = reader.ReadMap(settings.MapFilePath);
            _walls = reader.ReadWalls(settings.WallFilePath);
            _enemyData = reader.ReadEnemies(settings.EnemyFilePath);
            _weapons = reader.ReadWeapons(settings.WeaponFilePath);

            
            _player.Weapon = _weapons[0];

            _condition = settings.Condition;
            _caster = new RayCaster(_map, _walls);
            
            

            foreach (IWeapon weapon in _weapons)
            {
                weapon.Caster = _caster;
            }

            foreach (EnemyData enemyData in _enemyData.Values)
            {
                enemyData.Caster = _caster;
            }

            _painter = new BackgroundPainter(_width, _height, settings.SkyFilePath, settings.FloorFilePath);
            _wallCanvas = new Texture2D(GraphicsDevice, _width, _height);



            _currentRays = new List<Ray>();
            _enemies = new List<Enemy>();
            _currentSprites = new List<MovingObject>();

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

            bool shooting = false;

            KeyboardState state = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
                Exit();

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
                resetedBefore = false;
            }

            if (state.IsKeyDown(Keys.F12))
            {
                Program.RestartGame = true;
                Exit(); 
            }



            if (_player.IsKilled)
            {
                return;
            }

            


            var keys = state.GetPressedKeys();

            Vector2 stepVector = Vector2.Zero;

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

                   
                    default:

                        string keyValue = keyse.ToString();

                        char lastChar = keyValue[keyValue.Length - 1];

                        byte weaponNo;
                        if (byte.TryParse(lastChar.ToString(), out weaponNo))
                        {
                            weaponNo = (byte) (weaponNo == 0 ? 9 : weaponNo - 1);
                            if (weaponNo < _weapons.Count && _player.Weapon.Bullets.Count == 0)
                            {
                                _player.Weapon = _weapons[weaponNo];
                            }
                        }

                        break;
                }



                _player.Move(stepVector);

            }
            
            

            #endregion

            #region Mouse state

            Point currMouse = Mouse.GetState().Position;
            //Debug.WriteLine($"{currMouse}");

            float width = GraphicsDevice.Viewport.Width;
            float difference = width / 2F;
            difference = difference - currMouse.X;

            if (Math.Abs(difference) > RotationTreshold)
            {
                difference = 2 * difference / width * RotationSpeed;
                _player.Rotate(difference);

                //Debug.WriteLine($"{difference}");
            }

            #endregion



            #region Enemy state

            foreach (var enemyData in _enemyData.Values)
            {
                TimeSpan currTime = gameTime.TotalGameTime;

                float time = enemyData.SpawnTime;
                TimeSpan offSet = new TimeSpan(0,0,0,(int)time, (int) ((time - (int) time) * 1000));
                if (currTime  > enemyData.LastSpawn + offSet)
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
                if (currTime > LastSpawnChange + enemySpawnChange)
                {
                    enemyData.SpawnTime =  enemyData.SpawnTime > 8 ? enemyData.SpawnTime / 2.0F : 8;
                    LastSpawnChange = currTime;
                }
            }



            int offset = 0;
            for (int i = 0; i < _enemies.Count; i ++)
            {

                var enemy = _enemies[i - offset];
                Vector2 toPlayer = _player.Position - enemy.Position;
                if (!enemy.IsKilled)
                {
                    foreach (var bullet in _player.Weapon.Bullets)
                    {
                        if (!bullet.HasHit)
                        {
                            Vector2 dist = bullet.Position - enemy.Position;
                            if (Math.Abs(dist.Length()) < enemy.HitBox)
                            {
                                enemy.HitPoints--;
                                if (enemy.IsKilled)
                                {
                                    kills++;
                                    enemy.DeathSecs = gameTime.TotalGameTime.Seconds;
                                }

                                bullet.Hit(gameTime.TotalGameTime);
                            }
                        }
                    }
                    

                    if (toPlayer.Length() < MinEnemyDistance)
                    {
                        _player.HitPoints--;
                        enemy.HitPoints--;
                    }


                    enemy.Move(toPlayer);

                    int random = _random.Next();
                    if (random % 1000 == 0)
                    {
                        bool possible = false;
                        while (!possible)
                        {
                            enemy.Position = new Vector2(_random.Next(0, _map.Width - 1), _random.Next(0, _map.Height - 1));
                            toPlayer = enemy.Position - _player.Position;
                            if (toPlayer.Length() >= PlayerProtectDist * 1.5 && _map[(int)enemy.Position.X, (int)enemy.Position.Y] == 0)
                            {
                                possible = true;
                            }
                        }
                    }
                }
                else
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


            var currentWeapon = _player.Weapon;

            int count = currentWeapon.Bullets.Count;


            shooting = currentWeapon.MaxAmmo > count && shooting;
            if (currentWeapon.Bullets.Count > 0)
            {
                Vector2 lastBullet = currentWeapon.Bullets[count - 1].Position;
                lastBullet = lastBullet - _player.Position;
                if (Math.Abs(lastBullet.Length()) < MinBulletDistance)
                {
                    shooting = false;
                }
            }

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
                if (bullet.HasHit)
                {
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
            // TODO: Add your drawing code here

            

            //raycasting
            _currentRays = _caster.FieldOfView(_width, _player.Position, _player.Direction, _player.ScreenPlane, _condition);

            #region Enemies

            Matrix invMatrix = new Matrix(_player.Direction.Y, -_player.Direction.X, 0, 0, -_player.ScreenPlane.Y, _player.ScreenPlane.X, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            invMatrix *= 1 / (_player.ScreenPlane.X * _player.Direction.Y - _player.Direction.X * _player.ScreenPlane.Y);

            foreach (var objectSprite in _currentSprites)
            {
                Vector2 playerDist = objectSprite.Position - _player.Position;



                /***********************/
                /*                     */
                /* (dir Y   ,  -dir X) */
                /* (-plane Y, plane X) */
                /*                     */
                /***********************/

                Vector2 result = new Vector2(playerDist.X * invMatrix.M11 + playerDist.Y * invMatrix.M12,
                                             playerDist.X * invMatrix.M21 + playerDist.Y * invMatrix.M22);

                int spriteXPos = (int)(_width / 2F * (1 + result.X / result.Y));

                //is sprite in front of the player?
                if (result.Y > 0)
                {
                    ICrossable crossableSprite = objectSprite as ICrossable;
                    if (crossableSprite == null)
                    {
                        throw new NullReferenceException("Moving object must be ICrossable in order to be drawn.");
                    }


                    int spriteWidth = (int)(crossableSprite.Width / result.Y);

                    for (int i = -spriteWidth / 2; i < spriteWidth / 2; i++)
                    {
                        int currIndex = i + spriteXPos;

                        if (currIndex >= 0 && currIndex < _width)
                        {
                            double xPixel = (i + spriteWidth / 2) / (double)spriteWidth;
                            _currentRays[currIndex].Add(new DistanceWrapper<ICrossable>(result.Y, xPixel, Side.SideX, crossableSprite, objectSprite.Position, false));
                        }
                    }
                }
            }
            #endregion


            _painter.UpdateBuffer(_currentRays, _walls.MaxHeight, _player.Position, _player.Direction);

            _wallCanvas.SetData<Color>(_painter.Buffer);

            frameRate = 1 / gameTime.ElapsedGameTime.TotalSeconds;

            
            spriteBatch.Begin();

            spriteBatch.Draw(_sky, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height / 2), Color.White);
            spriteBatch.Draw(_floor, new Rectangle(0, GraphicsDevice.Viewport.Height / 2, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height / 2), Color.White);
            spriteBatch.Draw(_wallCanvas, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.DrawString(arialFont, $"FPS: {frameRate}", new Vector2(0, 0),
                gameTime.IsRunningSlowly ? Color.Red : Color.Black);

            if (_player.IsKilled)
            {
                spriteBatch.DrawString(arialFont, $"YOU DIED!", new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.Red);
            }

            spriteBatch.DrawString(arialFont, $"HP: {_player.HitPoints}", new Vector2(GraphicsDevice.Viewport.Width - 100, 0), Color.Black );
            spriteBatch.DrawString(arialFont, $"Kills: {kills}", new Vector2(GraphicsDevice.Viewport.Width - 100, 25), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);

        }
    }
}



