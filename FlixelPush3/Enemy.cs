using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLX;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlixelPush3
{
    public class Enemy : Sprite
    {
        Particle[] particles;
        public float speed;
        public bool marked;

        public Enemy(GraphicsDeviceManager graphics) : base(graphics)
        {
            color = Color.Black;
            drawRect.Width = 20;
            drawRect.Height = 20;
            particles = new Particle[100];
            marked = false;
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = new Particle(graphics);
            }
        }

        public override void Update(GameTimeWrapper gameTime, GraphicsDeviceManager graphics)
        {
            vel.Y = speed;
            Vector2 startingPos = new Vector2(pos.X + drawRect.Width / 2,
                pos.Y + drawRect.Height / 2);
            foreach (Particle particle in particles)
            {
                particle.SpawnParticle(startingPos, Color.Black,
                    new Tuple<int, int>(250, 500),
                    new Tuple<int, int>(5, 5),
                    new Tuple<float, float>(2, 3),
                    new Tuple<float, float>(0.9f, 0.95f),
                    new Tuple<float, float>(0.01f, 0.05f),
                    new Tuple<float, float>(0, 0),
                    0, 180, Color.Black, false, 0.0f);
                particle.pos.Y += speed;
                particle.Update(gameTime, graphics);
            }
            base.Update(gameTime, graphics);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle particle in particles)
            {
                particle.Draw(spriteBatch);
            }
            DrawRect(spriteBatch);
        }
    }
}
