using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NewKillingStory.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewKillingStory.Model
{
    class ParticleGenerator
    {
        Texture2D texture;
        float spawnWidth;
        float density;

        Camera camera;//

        List<ParticleOverScreen> particles = new List<ParticleOverScreen>();

        float timer;
        Random random1, random2;

        public ParticleGenerator(Texture2D newTexture, float newSpawnWidth, float newDensity, Camera camera)
        {
            texture = newTexture;
            spawnWidth = newSpawnWidth;
            density = newDensity;
            random1 = new Random();
            random2 = new Random();
            this.camera = camera;
        }
        public void createParticle()
        {
            double anything = random1.Next();
            //sätter partiklarna -50 utanför skärmen
            particles.Add(new ParticleOverScreen(texture, new Vector2(-20 + (float)random1.NextDouble() * spawnWidth, 0), new Vector2(1, random2.Next(5, 8)), camera));
        }
        public void Update(GameTime gameTime, GraphicsDevice graphics)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while(timer > 0)
            {
                timer -= 1f / density;
                createParticle();
            }
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Update();
                if(particles[i].Position.Y > graphics.Viewport.Height)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ParticleOverScreen particle in particles)
            {
                particle.Draw(spriteBatch);
            }
        }
    }
}
