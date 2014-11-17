using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GLX;

namespace FlixelPush3
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        World world;
        GameTimeWrapper mainGameTime;

        KeyboardState previousKeyboardState;

        Camera camera;

        Player player;
        Enemy[] enemies;
        float enemySpeed;
        float originalEnemySpeed;
        int enemySpawnValue;
        TimeSpan enemySpawnTime;
        bool marked;
        int score;
        TextItem scoreTextItem;

        public Game1()
            : base()
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
            world = new World(graphics);
            mainGameTime = new GameTimeWrapper(MainUpdate, this, 1.0m);
            world.AddTime(mainGameTime);
            previousKeyboardState = Keyboard.GetState();
            camera = new Camera(graphics.GraphicsDevice.Viewport, Camera.Focus.TopLeft);
            //camera.smoothZoom = true;
            enemySpeed = 0.0f;
            enemySpawnValue = 500;
            enemySpawnTime = TimeSpan.FromMilliseconds(enemySpawnValue);
            marked = false;
            DebugText.Initialize(Vector2.Zero, DebugText.Corner.TopLeft, 0);
            score = 0;

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
            player = new Player(graphics);
            enemies = new Enemy[50];
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new Enemy(graphics);
                enemies[i].visible = false;
            }
            scoreTextItem = new TextItem(Content.Load<SpriteFont>("DebugFont"), "Score: " + score.ToString());
            DebugText.debugTexts.Add(scoreTextItem);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            camera.Update();
            world.Update(gameTime);
        }

        public void MainUpdate(GameTimeWrapper gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            float moveSpeed = 0.1f;
            if (keyboardState.IsKeyDown(Keys.Q) && previousKeyboardState.IsKeyUp(Keys.Q))
            {
                if (!player.died)
                {
                    player.vel.X = -5.0f * (float)gameTime.GameSpeed;
                    enemySpawnValue--;
                    player.speed += moveSpeed;
                    player.vel.Y -= moveSpeed;
                    enemySpeed += moveSpeed;
                }
            }
            else if (keyboardState.IsKeyDown(Keys.P) && previousKeyboardState.IsKeyUp(Keys.P))
            {
                if (!player.died)
                {
                    player.vel.X = 5.0f * (float)gameTime.GameSpeed;
                    enemySpawnValue--;
                    player.speed += moveSpeed;
                    player.vel.Y -= moveSpeed;
                    enemySpeed += moveSpeed;
                }
            }
            if (!marked)
            {
                originalEnemySpeed = enemySpeed;
            }

            if (enemySpawnValue < 1)
            {
                enemySpawnValue = 1;
            }

            if (!player.died)
            {
                enemySpawnTime -= gameTime.ElapsedGameTime;
            }

            if (enemySpawnTime <= TimeSpan.Zero)
            {
                enemySpawnTime = TimeSpan.FromMilliseconds(enemySpawnValue);
                SpawnEnemy();
            }
            player.Update(gameTime, graphics);
            foreach (Enemy enemy in enemies)
            {
                if (enemy.pos.Y > graphics.GraphicsDevice.Viewport.Height)
                {
                    enemy.visible = false;
                }

                if (enemy.visible)
                {
                    enemy.speed = enemySpeed;
                    enemy.Update(gameTime, graphics);
                    if (enemy.drawRect.Intersects(player.drawRect) && !player.died)
                    {
                        player.died = true;
                        gameTime.GameSpeed = 1;
                        player.Died(originalEnemySpeed);
                        player.speed = 0;
                        player.vel = Vector2.Zero;
                        enemySpeed = 0;
                        originalEnemySpeed = 0;
                        camera.zoom = 1;
                        camera.focalPoint = Vector2.Zero;
                        camera.focus = Camera.Focus.TopLeft;
                    }
                    if (player.pos.X > enemy.pos.X - 10 &&
                        player.pos.X < enemy.pos.X + enemy.drawRect.Width + 10 && !player.died)
                    {
                        if (enemy.pos.Y > 0 && enemy.pos.Y < 50)
                        {
                            gameTime.GameSpeed = 0.9m;
                            enemy.marked = true;
                            marked = true;
                            camera.focus = Camera.Focus.Center;
                            camera.focalPoint = new Vector2(enemy.pos.X + enemy.drawRect.Width / 2,
                                player.pos.Y);
                            camera.zoom += 0.01f;
                        }
                        else if (enemy.pos.Y >= 50 && enemy.pos.Y < 100)
                        {
                            gameTime.GameSpeed = 0.7m;
                            enemy.marked = true;
                            marked = true;
                            camera.focus = Camera.Focus.Center;
                            camera.focalPoint = new Vector2(enemy.pos.X + enemy.drawRect.Width / 2,
                                player.pos.Y);
                            camera.zoom += 0.01f;
                        }
                        else if (enemy.pos.Y >= 100 && enemy.pos.Y < 150)
                        {
                            gameTime.GameSpeed = 0.5m;
                            enemy.marked = true;
                            marked = true;
                            camera.focus = Camera.Focus.Center;
                            camera.focalPoint = new Vector2(enemy.pos.X + enemy.drawRect.Width / 2,
                                player.pos.Y);
                            camera.zoom += 0.01f;
                        }
                        else if (enemy.pos.Y >= 150 && enemy.pos.Y < 200)
                        {
                            gameTime.GameSpeed = 0.3m;
                            enemy.marked = true;
                            marked = true;
                            camera.focus = Camera.Focus.Center;
                            camera.focalPoint = new Vector2(enemy.pos.X + enemy.drawRect.Width / 2,
                                player.pos.Y);
                            camera.zoom += 0.01f;
                        }
                        else if (enemy.pos.Y >= 200 && enemy.pos.Y < 240)
                        {
                            gameTime.GameSpeed = 0.1m;
                            enemy.marked = true;
                            marked = true;
                            camera.focus = Camera.Focus.Center;
                            camera.focalPoint = new Vector2(enemy.pos.X + enemy.drawRect.Width / 2,
                                player.pos.Y);
                            camera.zoom += 0.01f;
                        }
                        enemySpeed = originalEnemySpeed * (float)gameTime.GameSpeed;
                    }
                    else if (enemy.marked)
                    {
                        gameTime.GameSpeed = 1m;
                        camera.zoom = 1;
                        camera.focalPoint = Vector2.Zero;
                        camera.focus = Camera.Focus.TopLeft;
                        player.color = Color.White;
                        enemySpeed = originalEnemySpeed;
                        enemy.marked = false;
                        marked = false;
                    }
                }
            }

            if (player.pos.Y < graphics.GraphicsDevice.Viewport.Height / 2)
            {
                player.pos.Y = graphics.GraphicsDevice.Viewport.Height / 2;
            }
            if (player.pos.X < 0 + player.drawRect.Width / 2)
            {
                player.pos.X = 0 + player.drawRect.Width / 2;
            }
            if (player.pos.X > graphics.GraphicsDevice.Viewport.Width - player.drawRect.Width * 1.5f)
            {
                player.pos.X = graphics.GraphicsDevice.Viewport.Width - player.drawRect.Width * 1.5f;
            }
            score += (int)originalEnemySpeed;
            scoreTextItem.text = "Score: " + score.ToString();
            previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        void SpawnEnemy()
        {
            foreach (Enemy enemy in enemies)
            {
                if (!enemy.visible)
                {
                    enemy.visible = true;
                    enemy.pos = new Vector2(World.random.Next(0, graphics.GraphicsDevice.Viewport.Width),
                        World.random.Next(-1000, -900));
                    break;
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            camera.CameraBeginSpriteBatch(spriteBatch);
            foreach (Enemy enemy in enemies)
            {
                if (enemy.visible)
                {
                    enemy.Draw(spriteBatch);
                }
            }
            player.Draw(spriteBatch);
            DebugText.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
