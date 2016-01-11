using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewKillingStory.View
{
    class MenuView
    {
        int alphaValue = 1;
        int fadeIncrement = 3;
        double fadeDelay = 0.010;
        
        Camera camera;

        public MenuView(Camera camera)
        {
            this.camera = camera;
        }

        public void Update(float elapsedSeconds)
        {
            //fade effekt på meny namnet!
            fadeDelay -= elapsedSeconds;

            if (fadeDelay <= 0)//denna if sats kommer att fixa fade på titel texten!//
            {
                fadeDelay = 0.010;
                alphaValue += fadeIncrement;

                if (alphaValue >= 255 || alphaValue <= 2)
                {
                    fadeIncrement *= -1;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, float elapsedSeconds, Texture2D menuBackground, Texture2D Playbutton, Rectangle rectangle, Color color, Texture2D InstructionButton, Rectangle instructionRectangle)
        {


            float scale = camera.getScaleForView(menuBackground.Width);
            float scaleButton = camera.getScaleForView(Playbutton.Width);

            spriteBatch.Draw(menuBackground,
                    Vector2.Zero,
                    menuBackground.Bounds, 
                    new Color((byte)MathHelper.Clamp(alphaValue, 22, 255), 255, 255, (byte)MathHelper.Clamp(alphaValue, 22, 255)),
                    0f,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0f);

            spriteBatch.Draw(Playbutton, rectangle, color);
            spriteBatch.Draw(InstructionButton, instructionRectangle, color);
            //spriteBatch.Draw(Playbutton,//texture
            //   null,
            //   rectangle,//rectangle
            //   null,//rectangle source
            //   null,//origin
            //   0f,//rotation
            //   new Vector2(camera.getScaleForView(Playbutton.Width)),
            //   Color.White,//color 
            //   SpriteEffects.None,//spriteeffect
            //   0f);//depth of layer
        }

    }
}
