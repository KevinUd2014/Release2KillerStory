using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewKillingStory.View
{
    class Camera
    {
        private float scale;

        private float XCoord;
        private float YCoord;

        public Camera(Viewport port)//konstruktorn sköter skalningen på allt i fönstret!//
        {
            XCoord = port.Width;
            YCoord = port.Height;

            if (XCoord > YCoord)
            {
                scale = XCoord;
            }
            else
            {
                scale = YCoord;
            }
        }
        public Vector2 convertToVisualCoords(Vector2 position)//kopierade från uppgift 3 ungefär
        {
            float screenX = (XCoord * position.X);
            float screenY = (YCoord * position.Y);

            return new Vector2(screenX, screenY);
        }
        public Vector2 convertToLogicalCoords(Vector2 position)
        {
            float logicalX = (position.X / XCoord);
            float logicalY = (position.Y / YCoord);

            return new Vector2(logicalX, logicalY);
        }
        public float getScaleForView(float width)
        {
            return scale / width;
        }
    }
}
