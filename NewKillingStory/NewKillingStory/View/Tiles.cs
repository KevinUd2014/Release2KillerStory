using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NewKillingStory.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewKillingStory
{
    class Tiles
    {//
        Camera camera;

        public Tiles(Camera camera)
        {
            this.camera = camera;
        }
        protected Texture2D texture;

        private Rectangle rectangle;
        public Rectangle Rectangle
        {
            get { return rectangle; }
            protected set { rectangle = value; }
        }

        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }

        public void Draw(SpriteBatch spriteBatch)//, Camera camera)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);

            //spriteBatch.Draw(texture,//texture
            //    null,
            //    rectangle,//rectangle
            //    null,//rectangle source
            //    null,//origin
            //    0f,//rotation
            //    new Vector2(camera.getScaleForView(texture.Width)*100),//scale
            //    Color.White,//color 
            //    SpriteEffects.None,//spriteeffect
            //    0f);//depth of layer
        }
    }
    class CollisionTiles : Tiles
    {
        public CollisionTiles(int i, Rectangle newRectangle, Camera camera) : base(camera)
        {
            texture = Content.Load<Texture2D>("Tiles" + i);
            //trees = Content.Load<Texture2D>("Tree" + i);

            Rectangle = newRectangle;
        }
    }
}
