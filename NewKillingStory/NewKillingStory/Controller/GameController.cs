using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NewKillingStory.Model;
using NewKillingStory.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NewKillingStory.Controller
{
    class GameController
    {
        Map map;
        private Player player;
        public Boss boss;
        GraphicsDeviceManager _graphics;
        Texture2D _enemyTexture;
        Texture2D bossTexture;
        Camera camera;
        public List<AnimatedSprites> AnimatedSprites;
        Texture2D character;
        ContentManager _content;

        SoundEffect fireballSound;
        SoundEffect backgroundMusic;
        SoundEffectInstance soundEffectInstance;
        List<Enemy> enemyList = new List<Enemy>();

        int tileSize = 64;

        SpriteFont spritefont;

        public bool onFirstLevel;
        public bool onSecondLevel;
        public bool onThirdLevel;
        public bool onFourthLevel;

        public int enemyCount = 10;
        private float enemySpawnTimer = .5f;
        private float enemyLastSpawned = 0;

        GameController gameController;

        KillingStory killingStory;

        public GameController(KillingStory killingStory)
        {
            this.killingStory = killingStory;
        }

        public void LoadContent(SpriteBatch spriteBatch, ContentManager Content, Viewport viewport, 
            Camera camera, Texture2D enemyTexture, GraphicsDeviceManager graphics, 
            SoundEffect _backgroundMusic, GameController _gameController, 
            SoundEffect _fireballSound, SpriteFont _spritefont, Texture2D _bossTexture)
        {
            _enemyTexture = enemyTexture;
            this.camera = camera;
            _graphics = graphics;

            spritefont = _spritefont;

            bossTexture = _bossTexture;

            backgroundMusic = _backgroundMusic;
            fireballSound = _fireballSound;

            _content = Content;
            gameController = _gameController;
            character = Content.Load<Texture2D>("imp");
            Flame.SetTexture(Content.Load<Texture2D>("Flames"));
            
            soundEffectInstance = backgroundMusic.CreateInstance();

            onFirstLevel = false;
            onSecondLevel = false;
            onThirdLevel = false;
            onFourthLevel = false;

        }
        public void StartGame()
        {
            enemyList.Clear();
            AnimatedSprites = new List<Model.AnimatedSprites>();
            map = new Map(camera);
            player = new Player(new Vector2(340, 220), map, AnimatedSprites, camera, gameController, fireballSound);// start positionen för player!
            AnimatedSprites.Add(player);

            player.LoadContent(character);
            
            Tiles.Content = _content;
        }

        public void GameOver()
        {
            killingStory.ScreenState = KillingStory.Gamestate.GameOver;
            StopSound();
        }
        public void Finished()
        {
            killingStory.ScreenState = KillingStory.Gamestate.Finished;
            StopSound();
        }
        public void StopSound()
        {
            soundEffectInstance.Stop();
        }
        public void StartSound()
        {
            soundEffectInstance.Play();

            soundEffectInstance.Volume = 0.05f;
            soundEffectInstance.Pan = -0.0f;
            soundEffectInstance.Pitch = 0.0f;
        }
        public void PauseSound()
        {
            soundEffectInstance.Pause();
        }
        public void ResumeSound()
        {
            soundEffectInstance.Resume();
        }

        public void Level1()
        {
            map.Generate(new int[,]{//denna sätter hur många tiles jag vill ha och vart jag vill ha dem på skärmen!
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,4,4,4,1,1,1,1,4,4,4,1,1},
                { 1,1,4,1,1,1,1,1,1,1,1,4,1,1},
                { 1,1,4,1,1,1,1,1,1,1,1,4,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,4,1,1,1,1,1,1,1,1,4,1,1},
                { 1,1,4,1,1,1,1,1,1,1,1,4,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,4,1,1,1,1,1,1,1,1,4,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,4,1,1,1,1,1,1,1,1,4,1,1},
                { 9,1,1,1,1,1,1,1,1,1,1,1,1,1},
            }, tileSize);//med tileSize så menar jag 64 pixlar! int tilesize = 64;
            onFirstLevel = true;
            onSecondLevel = false;
            onThirdLevel = false;
            onFourthLevel = false;
            enemySpawnTimer = 1f;
            enemyCount = 10;
            StartSound();
        }
        public void Level2()
        {
            map.Generate(new int[,]{//denna sätter hur många tiles jag vill ha och vart jag vill ha dem på skärmen!
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,4,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,4,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,4,1,4,4,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,4,1,1,4,1,1,1,1,1},
                { 1,1,1,1,1,4,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,4,1,1,1,1,1,1,1,1},
                { 1,1,4,1,1,4,1,1,1,1,1,1,1,1},
                { 1,4,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,4,1,1,1,1,1,1,1,1,1,1},
                { 1,4,4,4,4,1,1,1,4,1,1,1,1,1},
                { 1,1,1,1,4,1,1,1,1,1,4,1,1,1},
                { 1,1,9,1,4,1,1,1,1,1,1,1,1,1},
            }, tileSize);//med tileSize så menar jag 64 pixlar! int tilesize = 64;
            onSecondLevel = true;
            onFirstLevel = false;
            onThirdLevel = false;
            onFourthLevel = false;
            enemySpawnTimer = 1f;
            enemyCount = 20;
        }
        public void Level3()
        {
            map.Generate(new int[,]{//denna sätter hur många tiles jag vill ha och vart jag vill ha dem på skärmen!
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,6,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,6,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,6,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,6,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,6,1,1,1,1,1,6,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,6,9},
            }, tileSize);//med tileSize så menar jag 64 pixlar! int tilesize = 64;
            onThirdLevel = true;
            onSecondLevel = false;
            onFirstLevel = false;
            onFourthLevel = false;
            enemySpawnTimer = 0.6f;
            enemyCount =40;

            AnimatedSprites.Add(new Boss(new Vector2(500, 500), map, AnimatedSprites, camera, _graphics, bossTexture, player));
        }
        public void Level4()
        {
            map.Generate(new int[,]{//denna sätter hur många tiles jag vill ha och vart jag vill ha dem på skärmen!
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                { 1,1,9,1,1,1,1,1,1,1,1,1,1,1},
            }, tileSize);//med tileSize så menar jag 64 pixlar! int tilesize = 64;
            onSecondLevel = false;
            onFirstLevel = false;
            onThirdLevel = false;
            onFourthLevel = true;

            enemySpawnTimer = 1f;
            enemyCount = 30;
            AnimatedSprites.Add(new Boss(new Vector2(500, 500), map, AnimatedSprites, camera, _graphics, bossTexture, player));
            AnimatedSprites.Add(new Boss(new Vector2(100, 500), map, AnimatedSprites, camera, _graphics, bossTexture, player));
            AnimatedSprites.Add(new Boss(new Vector2(100, 200), map, AnimatedSprites, camera, _graphics, bossTexture, player));
            AnimatedSprites.Add(new Boss(new Vector2(500, 200), map, AnimatedSprites, camera, _graphics, bossTexture, player));
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalSeconds - enemyLastSpawned > enemySpawnTimer && enemyCount > 0)
            {
                Random r = new Random();
                int side = (int)(r.NextDouble() * 4);

                float length = (float)r.NextDouble();

                Vector2 pos = new Vector2(0, 0);
                switch (side)
                {
                    case 0:
                        pos.X = map.Width * length;
                        pos.Y = 0f;
                        break;
                    case 1:
                        pos.X = map.Width * length;
                        pos.Y = map.Height;
                        break;
                    case 2:
                        pos.Y = map.Height * length;
                        pos.X = 0f;
                        break;
                    case 3:
                        pos.Y = map.Height * length;
                        pos.X = map.Width;
                        break;
                }

                AnimatedSprites.Add(new Enemy(pos, map, AnimatedSprites, camera, _graphics, _enemyTexture, player));
                enemyLastSpawned = (float) gameTime.TotalGameTime.TotalSeconds;
                enemyCount--;
            }

            for (int i = AnimatedSprites.Count - 1; i >= 0; i--)
            {
                if (AnimatedSprites[i].Alive)
                    AnimatedSprites[i].Update(gameTime);
                else
                    AnimatedSprites.RemoveAt(i);
            }
        }
        public void Draw(SpriteBatch spriteBatch)//, Camera camera)
        {
            map.Draw(spriteBatch);

            if (onFirstLevel == true)
            {
                spriteBatch.DrawString(spritefont, "First Level", new Vector2(10, 790), Color.Black);
            }
            if (onSecondLevel == true)
            {
                spriteBatch.DrawString(spritefont, "Second Level", new Vector2(10, 790), Color.Black);
            }
            if (onThirdLevel == true)
            {
                spriteBatch.DrawString(spritefont, "Third Level", new Vector2(10, 790), Color.Black);
                spriteBatch.DrawString(spritefont, "Boss Fight", new Vector2(600, 10), Color.Red);
            }
            if (onFourthLevel == true)
            {
                spriteBatch.DrawString(spritefont, "Fourth Level", new Vector2(10, 790), Color.Black);
            }


            for (int i = AnimatedSprites.Count - 1; i >= 0; i--)
            {
                if (AnimatedSprites[i].Alive)
                    AnimatedSprites[i].Draw(spriteBatch);
            }
        }
    }
}
