using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NewKillingStory.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewKillingStory.Controller
{
    class PauseButton
    {

        Texture2D texture;

        Vector2 position;
        Rectangle rectangle;

        Color color = new Color(255, 255, 255, 255);

        bool down;
        public bool isClicked;

        public void Load(Texture2D newTexture, Vector2 newPosition)
        {
            position = newPosition;
            texture = newTexture;
        }

        public void Update(MouseState mousePosition)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, 80, 50);//, (int)size.X, (int)size.Y //storleken på playknappen avgörs här!
            Rectangle mouseRectangle = new Rectangle(mousePosition.X, mousePosition.Y, 1, 1);//en rektangel på musen!

            if (mouseRectangle.Intersects(rectangle))
            {
                if (color.A == 255)
                    down = false;
                if (color.A == 0)
                    down = true;
                if (down)
                    color.A += 3;
                else color.A -= 3;

                if (mousePosition.LeftButton == ButtonState.Pressed)
                    isClicked = true;
            }
            else if (color.A < 255)
            {
                color.A += 3;
                isClicked = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }
    }
}
