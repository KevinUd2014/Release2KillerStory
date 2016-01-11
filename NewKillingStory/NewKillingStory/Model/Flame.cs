using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NewKillingStory.View;

namespace NewKillingStory.Model
{
    class Flame : AnimatedSprites
    {
        private Vector2 velocity;
        private static Texture2D texture;
        private float age;
        private float range;
        private Map map;
        private Vector2 offset;

        public Flame(Vector2 position, Map map, Vector2 velocity, Camera camera, float range = 2f) : base(position, camera)
        {
            this.map = map;
            this.velocity = velocity;
            this.range = range;

            character = texture;

            offset = new Vector2(62 / 2, 76 / 2);

            FramesPerSecond = 9;
            AddAnimation(4, 0, 0, "fire", 62, 76);
            PlayAnimation("fire");
        }
        public override void Update(GameTime gameTime)
        {
            position += velocity;

            age += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (age > range)
            {
                Alive = false;
            }

            int x = (int)((position.X + offset.X) / map.Width * map.tilemap.GetLength(1));
            int y = (int)((position.Y + offset.Y) / map.Height * map.tilemap.GetLength(0));

            if (x >= 0
                && y >= 0
                && x < map.tilemap.GetLength(1)
                && y < map.tilemap.GetLength(0)
                && map.tilemap[y,x] % 2 == 0)
            {
                Alive = false;
            }

            base.Update(gameTime);
        }

        public static void SetTexture(Texture2D tex)
        {
            texture = tex;
        }
    }
}
