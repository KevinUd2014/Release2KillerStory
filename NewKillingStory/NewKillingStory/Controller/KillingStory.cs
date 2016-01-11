using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NewKillingStory.Controller;
using NewKillingStory.Model;
using NewKillingStory.View;

namespace NewKillingStory
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class KillingStory : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState lastMouseState;
        
        GameController gameController;
        MenuController menuController;
        Camera camera;
        ParticleGenerator rain;

        //pause funktionen
        Texture2D pauseTexture;

        Texture2D gameOverScreen;
        Rectangle gameOverRectangle;

        Texture2D FishishedScreen;
        Rectangle FishishedRectangle;

        Rectangle pausedRectangle;

        SoundEffectInstance soundEffectInstance;

        SoundEffect backgroundMusic;
        SoundEffect fireballSound;
        SpriteFont spritefont;
        SpriteFont menuFont;
        Texture2D pauseInstructions;
        Rectangle pausedRectangleInstruction;
        PauseButton buttonPlay, buttonQuit, buttonMainMenu, buttonInstruction, buttonBack;

        /// http://gamedev.stackexchange.com/questions/108518/monogame-screen-transition-with-fading
        public enum Gamestate//vet inte om denna! verkade vara det bästa sättet
        {
            Menu,//we have a menu
            Play,//and a play
            Pause,
            Instructions,
            GameOver,
            Finished,
        }


        public KillingStory()
        {
            graphics = new GraphicsDeviceManager(this);

            //Puts the size of the window!
            graphics.PreferredBackBufferHeight =840;//840
            graphics.PreferredBackBufferWidth = 840;
            
            Content.RootDirectory = "Content";
        }
        
        public Gamestate ScreenState = Gamestate.Menu;//menu will be the deffault!
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            
            //create all the necessary classes!
            menuController = new MenuController();
            gameController = new GameController(this);
            
            camera = new Camera(GraphicsDevice.Viewport);

            //snow = new ParticleGenerator(Content.Load<Texture2D>("Snowflake"), graphics.GraphicsDevice.Viewport.Width, 50, camera);
            rain = new ParticleGenerator(Content.Load<Texture2D>("Rain"), graphics.GraphicsDevice.Viewport.Width, 50, camera);

            backgroundMusic = Content.Load<SoundEffect>("sleeping_beast_4"); // ge cred ... //http://www.opsound.org/artist/dhalius/

            fireballSound = Content.Load<SoundEffect>("fireballShot");// när spelaren skjuter så kommer ljud!
            spritefont = Content.Load<SpriteFont>("NewSpriteFont");
            menuFont = Content.Load<SpriteFont>("SpriteFontMenu/NewSpriteFont");
            //Load all the textures here and sound as well!
            Texture2D enemyTexture = Content.Load<Texture2D>("Bat");
            Texture2D bossTexture = Content.Load<Texture2D>("Fox");
            Texture2D startMenuBackground = Content.Load<Texture2D>("KillerStory");
            Texture2D playButton = Content.Load<Texture2D>("playButton");
            Texture2D instructionButton = Content.Load<Texture2D>("InstructionsButton");
            //character = Content.Load<Texture2D>("Fox");

            soundEffectInstance = backgroundMusic.CreateInstance();

            //pause saker
            gameOverScreen = Content.Load<Texture2D>("GameOverScreen");
            gameOverRectangle = new Rectangle(0, 0, gameOverScreen.Width, gameOverScreen.Height);

            pauseTexture = Content.Load<Texture2D>("PauseMenu");
            pausedRectangle = new Rectangle(0, 0, pauseTexture.Width, pauseTexture.Height);

            FishishedScreen = Content.Load<Texture2D>("FinishedScreen");
            FishishedRectangle = new Rectangle(0, 0, FishishedScreen.Width, FishishedScreen.Height);

            pauseInstructions = Content.Load<Texture2D>("Instructions");
            pausedRectangleInstruction = new Rectangle(0, 0, pauseTexture.Width, pauseTexture.Height);

            buttonPlay = new PauseButton();
            buttonPlay.Load(Content.Load<Texture2D>("ResumeButton2"), new Vector2(400, 400));
            buttonQuit = new PauseButton();
            buttonQuit.Load(Content.Load<Texture2D>("QuitButton2"), new Vector2(400, 450));
            buttonMainMenu = new PauseButton();
            buttonMainMenu.Load(Content.Load<Texture2D>("MenuButton2"), new Vector2(400, 500));
            buttonInstruction = new PauseButton();
            buttonInstruction.Load(Content.Load<Texture2D>("InstructionsButton"), new Vector2(400, 550));
            buttonBack = new PauseButton();
            buttonBack.Load(Content.Load<Texture2D>("ButtonBack"), new Vector2(740, 780));
            
            //load all the classes and give them all the necessary parameters!
            menuController.LoadContent(spriteBatch, Content, GraphicsDevice.Viewport, startMenuBackground, playButton, instructionButton);
            gameController.LoadContent(spriteBatch, Content, GraphicsDevice.Viewport, camera, enemyTexture, graphics, backgroundMusic, gameController, fireballSound, spritefont, bossTexture);

            menuController.setPosition(new Vector2(400, 400));// sätter positionen för knappen!
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        public void Play()
        {
            ScreenState = Gamestate.Play;

            gameController.StartGame();
            gameController.Level1();

            IsMouseVisible = true;
            menuController.isClicked = false;  // har problem med vart jag ska sätta dessa för att dom ska bli bra menyerna buggar lite!
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            lastMouseState = mouse;

            KeyboardState Keystate = Keyboard.GetState();

            switch (ScreenState)//creats a Switch with all the diffrent screens!
            {
                case Gamestate.Menu:
                    IsMouseVisible = true;
                    menuController.Update((float)gameTime.ElapsedGameTime.TotalSeconds, mouse);

                    if (menuController.isClicked == true && lastMouseState.LeftButton == ButtonState.Released) 
                    {
                        Play();
                    }
                    if (Keystate.IsKeyDown(Keys.P))
                    {
                        Play();
                    }
                    if (menuController.isInstructionClicked == true && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        ScreenState = Gamestate.Instructions;
                        IsMouseVisible = true;
                        menuController.isInstructionClicked = true; // denna finns i instruction gamestate!
                        buttonBack.isClicked = false;
                    }
                    break;
                case Gamestate.Play:

                    rain.Update(gameTime, graphics.GraphicsDevice);
                    IsMouseVisible = false;

                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        ScreenState = Gamestate.Pause;
                        gameController.PauseSound();
                    }
                    gameController.Update(gameTime);

                    break;
                case Gamestate.Pause:
                    IsMouseVisible = true;
                    
                    if (buttonPlay.isClicked && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        ScreenState = Gamestate.Play;
                        gameController.ResumeSound();
                        buttonPlay.isClicked = false;
                    }
                    if (buttonQuit.isClicked && lastMouseState.LeftButton == ButtonState.Released)
                        Exit();
                    if (buttonMainMenu.isClicked && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        ScreenState = Gamestate.Menu;
                        buttonMainMenu.isClicked = false;
                    }
                    if(buttonInstruction.isClicked && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        ScreenState = Gamestate.Instructions;
                        buttonInstruction.isClicked = false;
                        buttonBack.isClicked = false;
                    }
                    buttonPlay.Update(mouse);
                    buttonQuit.Update(mouse);
                    buttonMainMenu.Update(mouse);
                    buttonInstruction.Update(mouse);

                    break;
                case Gamestate.Instructions:

                    IsMouseVisible = true;
                    if (buttonBack.isClicked && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        if (menuController.isInstructionClicked == true)
                        {
                            ScreenState = Gamestate.Menu;
                            menuController.isInstructionClicked = false;
                            buttonBack.isClicked = false;
                        }
                        else
                        {
                            ScreenState = Gamestate.Pause;
                        }
                    }
                    buttonBack.Update(mouse);

                    break;
                case Gamestate.GameOver:

                    if (Keystate.IsKeyDown(Keys.Enter))
                    {
                        ScreenState = Gamestate.Menu;
                    }

                    break;
                case Gamestate.Finished:
                    
                    if (Keystate.IsKeyDown(Keys.Enter))
                    {
                        ScreenState = Gamestate.Menu;
                    }
                    break;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            switch (ScreenState)//got this from a classmate! //really smart and inovative idéa
            {
                case Gamestate.Menu:
                    menuController.Draw(spriteBatch, (float)gameTime.ElapsedGameTime.TotalSeconds);
                    spriteBatch.DrawString(menuFont, "Press 'P' to play", new Vector2(350, 600), Color.White);
                    break;
                case Gamestate.Play:
                    gameController.Draw(spriteBatch);
                    spriteBatch.DrawString(menuFont, "Press 'ESC' to pause", new Vector2(10, 10), Color.Black);
                    //snow.Draw(spriteBatch);//snöpartiklarna!
                    rain.Draw(spriteBatch);//regnpartiklarna!
                    break;
                case Gamestate.Pause:

                    spriteBatch.Draw(pauseTexture, pausedRectangle, Color.White);
                    buttonPlay.Draw(spriteBatch);
                    buttonMainMenu.Draw(spriteBatch);
                    buttonQuit.Draw(spriteBatch);
                    buttonInstruction.Draw(spriteBatch);

                    break;
                case Gamestate.Instructions:
                    spriteBatch.Draw(pauseInstructions, pausedRectangle, Color.White);

                    buttonBack.Draw(spriteBatch);
                    break;
                case Gamestate.GameOver:
                    spriteBatch.Draw(gameOverScreen, gameOverRectangle, Color.White);

                    spriteBatch.DrawString(spritefont, "Press Enter for main menu", new Vector2(200, 400), Color.White);
                    break;
                case Gamestate.Finished:
                    spriteBatch.Draw(FishishedScreen, FishishedRectangle, Color.White);

                    spriteBatch.DrawString(spritefont, "Press Enter for main menu", new Vector2(200, 400), Color.White);
                    break;
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
