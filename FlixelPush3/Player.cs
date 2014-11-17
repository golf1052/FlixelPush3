using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLX;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlixelPush3
{
    public class Player : Sprite
    {
        Particle[] particles;
        public float speed;
        public bool died;

        public Player(GraphicsDeviceManager graphics) : base(graphics)
        {
            speed = 0.0f;
            died = false;
            drawRect.Width = 10;
            drawRect.Height = 10;
            pos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - drawRect.Width / 2,
                graphics.GraphicsDevice.Viewport.Height - drawRect.Height / 2 - 50);
            drawRect.X = (int)pos.X;
            drawRect.Y = (int)pos.Y;
            particles = new Particle[1000];

            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = new Particle(graphics);
            }
        }

        public override void Update(GameTimeWrapper gameTime, GraphicsDeviceManager graphics)
        {
            if (!died)
            {
                foreach (Particle particle in particles)
                {
                    particle.SpawnParticle(pos, Color.White,
                        new Tuple<int, int>(500, 1000),
                        new Tuple<int, int>(3, 3),
                        new Tuple<float, float>(speed, speed),
                        new Tuple<float, float>(0.95f, 0.99f),
                        new Tuple<float, float>(0.01f, 0.05f),
                        new Tuple<float, float>(0, 0),
                        90, 15, Color.White, false, 0.0f);
                    particle.Update(gameTime, graphics);
                }

                if (speed > 10)
                {
                    speed = 10;
                }

                base.Update(gameTime, graphics);
            }
            else
            {
                foreach (Particle particle in particles)
                {
                    particle.Update(gameTime, graphics);
                }
            }
        }

        public void Died(float particleSpeed)
        {
            foreach (Particle particle in particles)
            {
                particle.visible = false;
                particle.SpawnParticle(pos, Color.White,
                    new Tuple<int, int>(500, 1000),
                    new Tuple<int, int>(3, 3),
                    new Tuple<float, float>(particleSpeed, particleSpeed),
                    new Tuple<float, float>(0.95f, 0.99f),
                    new Tuple<float, float>(0.01f, 0.05f),
                    new Tuple<float, float>(0, 0),
                    0, 180, Color.White, false, 0.0f);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle particle in particles)
            {
                particle.Draw(spriteBatch);
            }
            if (!died)
            {
                DrawRect(spriteBatch);
            }
        }
    }
}
