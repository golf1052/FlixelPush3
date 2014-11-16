using System;
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

        Particle[] particles;

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

            particles = new Particle[500];
            for (int i = 0; i < 500; i++)
            {
                particles[i] = new Particle(graphics);
            }
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
            world.Update(gameTime);
        }

        public void MainUpdate(GameTimeWrapper gameTime)
        {
            foreach (Particle particle in particles)
            {
                if (particle.SpawnParticle(new Vector2(graphics.GraphicsDevice.Viewport.Width / 2,
                    graphics.GraphicsDevice.Viewport.Height / 2),
                    Color.White,
                    new Tuple<int, int>(1000, 2000),
                    new Tuple<int, int>(3, 3),
                    new Tuple<float, float>(7, 10),
                    new Tuple<float, float>(0.95f, 0.99f),
                    new Tuple<float, float>(0.01f, 0.05f),
                    new Tuple<float, float>(0.01f, 0.01f),
                    0,
                    180,
                    Color.Yellow,
                    true,
                    0.8f,
                    0.5f))
                {
                    break;
                }
            }

            foreach (Particle particle in particles)
            {
                particle.Update(gameTime, graphics);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            foreach (Particle particle in particles)
            {
                particle.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
