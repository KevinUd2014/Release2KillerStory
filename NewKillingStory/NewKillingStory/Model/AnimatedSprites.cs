using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NewKillingStory.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewKillingStory.Model
{
    abstract class AnimatedSprites
    {
        // The texture of the sprite
        protected Texture2D character;

        // The position of the SpriteObject
        protected Vector2 position;

        // Number of frames in the animation
        private int frameIndex;
        private Rectangle[] rectangle;

        public enum myDirection { none, left, right, up, down }
        protected myDirection currentDirection = myDirection.none;

        // Time that has passed since last frame change 
        private double timeElapsed;

        // Time it takes to update a frame
        private double timeToUpdate;
        
        // Keeps track of the current animation
        private string currentAnimation;
        
        protected Vector2 direction = Vector2.Zero;

        Camera camera;

        protected int width;
        protected int height;

        public bool Alive = true;

        public int life = 1;
        public int giveDamage = 0;

        // Our time per frame is equal to 1 divided by frames per second(we are deciding FPS)
        public int FramesPerSecond
        {
            set
            {
                timeToUpdate = (1f / value);//denna höjer farten på animationen eller sänker den m,ellan 1 och 2 är bra!
            }
        }

        // Dictionary that contains all animations
        private Dictionary<string, Rectangle[]> animations = new Dictionary<string, Rectangle[]>();

        public AnimatedSprites(Vector2 position, Camera camera)
        {
            this.position = position;
            this.camera = camera;
        }
        
        // Adds an animation to the AnimatedSprite
        public void AddAnimation(int frames, int yPosition, int xStartFrame, string name, int width, int height)//, Vector2 offset//om jag villl ha en attack sprite!   , int yPos, int xStartFrame, string name, int width, int height, Vector2 offset)
        {
            //int width = character.Width / frames;
            this.width = width;
            this.height = height;
            Rectangle[] newRectangle = new Rectangle[frames];// needs a dictionary

            for (int i = 0; i < frames; i++)////Fills up the array of rectangles
            {
                newRectangle[i] = new Rectangle((i + xStartFrame) * width, yPosition, width, height);
            }                                   // sätter start positionen på frame
            animations.Add(name, newRectangle);// lägger till allt i rektangel some n animation!
        }
        public Vector2 GetPosition()
        {
            return position;
        }
        // Determines when we have to change frames
        public virtual void Update(GameTime gameTime)
        {
            //Adds time that has elapsed since our last draw
            timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeElapsed > timeToUpdate)// if our timelapsed is greater than our timeToUpdate(calculated by our framerate) we need to change the image
            {
                //Resets the timer in a way, so that we keep our desired FPS
                timeElapsed -= timeToUpdate;

                //Adds one to our frameIndex
                if (frameIndex < animations[currentAnimation].Length - 1)//sAnimations[currentAnimation].Length - 1)
                {
                    frameIndex++;
                }
                else //Restarts the animation
                {
                    //AnimationDone(currentAnimation);
                    frameIndex = 0;
                }
            }

            if (life <= 0)
            {
                Alive = false;
            }
        }
        ///// Draws the sprite on the screen
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(character,
                position,//position,
                animations[currentAnimation][frameIndex],
                Color.White,
                0f,//rotation
                Vector2.Zero,
                camera.getScaleForView(character.Width * 4),
                SpriteEffects.None,
                0f);
        }
        // Plays an animation
        public void PlayAnimation(string name)
        {
                currentAnimation = name;
        }
    }
}
