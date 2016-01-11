using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewKillingStory.View
{//
    class ParticleOverScreen
    {
        Texture2D texture;
        Vector2 position;
        Vector2 velocity;
        Camera camera;

        public Vector2 Position
        {
            get { return position; }
        }
        public ParticleOverScreen(Texture2D newTexture, Vector2 newPosition, Vector2 newVelocity, Camera camera)
        {
            texture = newTexture;
            position = newPosition;
            velocity = newVelocity;
            this.camera = camera;
        }
        public void  Update()
        {
            position += velocity;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture,
                position,
                null,
                Color.White,
                0f,//rotation
                Vector2.Zero,
                camera.getScaleForView(texture.Width*80),
                SpriteEffects.None,
                0f);
        }
    }
}
