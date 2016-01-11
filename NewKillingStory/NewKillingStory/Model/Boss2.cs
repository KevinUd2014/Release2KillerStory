using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NewKillingStory.View;

namespace NewKillingStory.Model
{
    class Boss : Enemy
    {
        public Boss(Vector2 position, Map map, List<AnimatedSprites> animatedSprites, Camera camera, GraphicsDeviceManager graphics, Texture2D texture, Player _player) : base(position, map, animatedSprites, camera, graphics, texture, _player)
        {
            enemySpeed = 1.5f;
            life = 40;
            Radius = 30f;
        }

        public override void LoadContent()
        {
            FramesPerSecond = 14;
            AddAnimation(3, 0, 0, "Enemy", 43, 43);
            PlayAnimation("Enemy");
        }
    }
}
