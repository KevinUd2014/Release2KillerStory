using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NewKillingStory.View;
using NewKillingStory.Controller;
using System.Threading;
using Microsoft.Xna.Framework.Audio;

namespace NewKillingStory.Model
{
    class Player : AnimatedSprites
    {
        float mySpeed = 100;
        Map map;

        Vector4 hitbox;
        private Vector2 offset;

        private float lastShot = 0;
        private float fireRate = 0.3f;
        private float spread = 5f;
        private Random spreadRandom;
        Camera camera;
        private List<AnimatedSprites> animatedSprites;
        SoundEffect fireballSound;

        GameController gameController;

        // The constructor of the Player class//
        public Player(Vector2 position, Map map, List<AnimatedSprites> animatedSprites, Camera camera, GameController _gameController, SoundEffect _fireballSound) : base(position, camera)//this position is handled through the base class
        {
            spreadRandom = new Random();
            life = 5;
            giveDamage = 1;
            this.position = position;
            this.camera = camera;
            this.map = map;
            this.animatedSprites = animatedSprites;

            fireballSound = _fireballSound;

            hitbox = new Vector4(15, 40, 49, 66); // bästa raden gällande karaktären!   (15, 40, 49, 66)
            offset = new Vector2(hitbox.X + hitbox.Z, hitbox.Y + hitbox.W) / 2f;
            gameController = _gameController;
            FramesPerSecond = 6;

            //Adds all the players animations
            AddAnimation(3, 1, 0, "Down", 64, 64);
            AddAnimation(3, 65, 0, "Left", 64, 64);
            AddAnimation(3, 129, 0, "Right", 64, 64);
            AddAnimation(3, 193, 0, "Up", 64, 64);
            PlayAnimation("Down");
        }

        public Vector2 GetCenterPositionForPlayer()
        {
            return position + offset;
        }
        // Loads content specific to the player class
        public void LoadContent(Texture2D character)
        {
            this.character = character;//laddar in charactern!
        }
        public override void Update(GameTime gameTime)
        {
            direction = Vector2.Zero; //Makes the player stop moving when no key is pressed

            HandleKeyboardInput(Keyboard.GetState(), gameTime);//Handles the users keyboard input

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;//Calculates how many seconds since last Update//Is not based on fps!
            
            direction *= mySpeed;//Applies the speed speed
            
            position += (direction * deltaTime);//Makes the movement framerate independent by multiplying with deltaTime
            
            foreach (AnimatedSprites sprite in animatedSprites)
            {
                if(sprite is Enemy && sprite.Alive)
                {
                    Enemy enemy = sprite as Enemy;
                    if((enemy.GetCenterPositionForEnemy() - GetCenterPositionForPlayer()).Length() <= 22 + enemy.Radius)
                    {
                        life -= enemy.giveDamage;
                    }
                }
            }

            base.Update(gameTime);

            if (!Alive)
            {
                gameController.GameOver();
            }
        }
        public void HandleKeyboardInput(KeyboardState keyState, GameTime gameTime)
        {
            if (keyState.IsKeyDown(Keys.W) && !checkForCollision(position + new Vector2(0, -2.5f)))
            {
                //Move char Up
                direction += new Vector2(0, -1.5f);
                PlayAnimation("Up");
                currentDirection = myDirection.up;
            }
            if (keyState.IsKeyDown(Keys.A) && !checkForCollision(position + new Vector2(-2.5f, 0)))
            {
                //Move char Left
                direction += new Vector2(-1.5f, 0);
                PlayAnimation("Left");
                currentDirection = myDirection.left;
            }
            if (keyState.IsKeyDown(Keys.S) && !checkForCollision(position + new Vector2(0, 2.5f)))
            {
                //Move char Down
                direction += new Vector2(0, 1.5f);
                PlayAnimation("Down");
                currentDirection = myDirection.down;
            }
            if (keyState.IsKeyDown(Keys.D) && !checkForCollision(position + new Vector2(2.5f, 0)))
            {
                //Move char Right
                direction += new Vector2(1.5f, 0);
                PlayAnimation("Right");
                currentDirection = myDirection.right;
            }
            //denna är för flame!
            if (keyState.IsKeyDown(Keys.Up)) //&& !checkForCollision(position + new Vector2(0, 0)))
            {
                PlayAnimation("Up");
                currentDirection = myDirection.up;
                shoot(gameTime, new Vector2(0, -5));
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                PlayAnimation("Down");
                currentDirection = myDirection.down;
                shoot(gameTime, new Vector2(0, 5));
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                PlayAnimation("Right");
                currentDirection = myDirection.right;
                shoot(gameTime, new Vector2(5, 0));
            }
            if (keyState.IsKeyDown(Keys.Left))
            {
                PlayAnimation("Left");
                currentDirection = myDirection.left;
                shoot(gameTime, new Vector2(-5, 0));
            }
            currentDirection = myDirection.none;
        }

        private void shoot(GameTime gameTime, Vector2 bulletSpeed)
        {
            if (gameTime.TotalGameTime.TotalSeconds - lastShot > fireRate)
            {
                fireballSound.Play(0.03f, 0, 0);//(float)spreadRandom.NextDouble() * 2f - 1f, (float)spreadRandom.NextDouble() * 2f - 1f);
                attack(bulletSpeed);
                lastShot = (float)gameTime.TotalGameTime.TotalSeconds;
            }
        }

        private void attack(Vector2 V)
        {
            float s = (spread * (float)spreadRandom.NextDouble() - spread/2f) * (float)Math.PI / 180f;
            Flame flame = new Flame(position,map, Vector2.Transform(V,Matrix.CreateRotationZ(s)), camera);
            flame.giveDamage = giveDamage;
            animatedSprites.Add(flame);
        }
      

        public bool checkForCollision(Vector2 pos)//denna funktion fick jag hjälp med då den är ganska komplex och avancerad!
        {
            pos.X += hitbox.X;
            pos.Y += hitbox.Y;

            Vector2 size = new Vector2( hitbox.Z - hitbox.X , hitbox.W - hitbox.Y);

            bool tileNW = map.tilemap[(int)(pos.Y / map.Height * map.tilemap.GetLength(0)), (int)(pos.X / map.Width * map.tilemap.GetLength(1))] % 2 == 0;
            bool tileNE = map.tilemap[(int)(pos.Y / map.Height * map.tilemap.GetLength(0)), (int)((pos.X + size.X) / map.Width * map.tilemap.GetLength(1))] % 2 == 0;
            bool tileSW = map.tilemap[(int)((pos.Y + size.Y) / map.Height * map.tilemap.GetLength(0)), (int)(pos.X / map.Width * map.tilemap.GetLength(1))] % 2 == 0;
            bool tileSE = map.tilemap[(int)((pos.Y + size.Y) / map.Height * map.tilemap.GetLength(0)), (int)((pos.X + size.X) / map.Width * map.tilemap.GetLength(1))] % 2 == 0;

            if (gameController.enemyCount <= 0)
            {
                bool tileChangeWorld = map.tilemap[(int)((pos.Y + size.Y) / map.Height * map.tilemap.GetLength(0)), (int)(pos.X / map.Width * map.tilemap.GetLength(1))] == 9;

                if (tileChangeWorld && gameController.onFirstLevel == true)//&& gameController.onSecondLevel == false && gameController.onThirdLevel == false)
                {
                    gameController.StartGame();
                    gameController.Level2();
                    gameController.onFirstLevel = false;
                    gameController.onThirdLevel = false;
                    tileChangeWorld = false;
                }
                if (tileChangeWorld && gameController.onSecondLevel == true && gameController.onFirstLevel == false && gameController.onThirdLevel == false && gameController.onFourthLevel == false)
                {
                    gameController.StartGame();
                    gameController.Level3();
                    gameController.onSecondLevel = false;
                    tileChangeWorld = false;
                }
                if (tileChangeWorld && gameController.onThirdLevel == true && gameController.onFirstLevel == false && gameController.onSecondLevel == false && gameController.onFourthLevel == false)
                {
                    gameController.StartGame();
                    gameController.Level4();
                    gameController.onThirdLevel = false;
                    gameController.onSecondLevel = false;
                    tileChangeWorld = false;
                }
                if (tileChangeWorld && gameController.onFourthLevel == true && gameController.onThirdLevel == false && gameController.onFirstLevel == false && gameController.onSecondLevel == false)
                {
                    gameController.Finished();
                    gameController.onThirdLevel = false;
                    gameController.onFourthLevel = false;
                    gameController.onSecondLevel = false;
                    tileChangeWorld = false;
                }
            }
            bool outside = pos.X < 0 || pos.X + hitbox.Z > map.Width || pos.Y < 0 || pos.Y + hitbox.W > map.Height;

            if (tileNW || tileNE || tileSW || tileSE || outside)
            {
                return true;
            }
            return false;
        }
    }
}