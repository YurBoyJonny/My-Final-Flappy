using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace My_Final_Flappy
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        enum Screen
        {
            Intro,
            Main,
            Dead
        }
        Screen screen;

        List<Rectangle> barriers;

        KeyboardState keyboardState;
        MouseState mouseState;

        Texture2D patrickTexture;

        Texture2D wallTexture;

        SpriteFont titleText;

        Texture2D houseTexture;
        Rectangle houseRect;

        Texture2D coolPatrickTexture;
        Rectangle coolPatrickRect;

        Texture2D groundTexture;
        Rectangle groundRect;
        Vector2 groundSpeed;

        Player patrick;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 500; // Sets the width of the window
            _graphics.PreferredBackBufferHeight = 800; // Sets the height of the window
            _graphics.ApplyChanges(); // Applies the new dimensions
            // TODO: Add your initialization logic here
            screen = Screen.Intro;
            barriers = new List<Rectangle>();
            barriers.Add(new Rectangle(300, 450, 65, 250)); // First Barrier
            barriers.Add(new Rectangle(300, 0, 65, 250)); // First Barrier

            barriers.Add(new Rectangle(450, 0, 65, 250));

            barriers.Add(new Rectangle(0, 0, 500, 0)); // Roof

            base.Initialize();
            patrick = new Player(patrickTexture, 50, 350);
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            patrickTexture = Content.Load<Texture2D>("Patrick");

            wallTexture = Content.Load<Texture2D>("rectangle");

            houseTexture = Content.Load<Texture2D>("house");
            houseRect = new Rectangle(0, 0, 1600, 1000);

            coolPatrickTexture = Content.Load<Texture2D>("coolpatrick");
            coolPatrickRect = new Rectangle(0, 0, 500, 800);

            titleText = Content.Load<SpriteFont>("Title");

            groundTexture = Content.Load<Texture2D>("flappyGround");
            groundRect = new Rectangle(0, 700, 1000, 100);
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (screen == Screen.Main)
            {
                groundRect.X += (int)groundSpeed.X;
                groundSpeed.X = -5;

                patrick.VSpeed = 1;

            }
            else if (screen != Screen.Main)
            {
                patrick.VSpeed = 0;

                patrick = new Player(patrickTexture, 50, 350);
            }

            if (groundRect.X == -500) // Moving Ground
            {
                groundRect = new Rectangle(0, 700, 1000, 100);
            }

            if (screen == Screen.Intro)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                    screen = Screen.Main;
            }
            patrick.HSpeed = 0;

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                patrick.VSpeed = -1;
            }

            patrick.Update();

            foreach (Rectangle barrier in barriers)
                if (patrick.Collide(barrier) || patrick.Collide(groundRect))
                {
                    patrick.UndoMove();
                    screen = Screen.Dead;
                }

            if (screen == Screen.Dead && keyboardState.IsKeyDown(Keys.Space))
            {
                screen = Screen.Intro;
            }

            if (keyboardState.IsKeyDown(Keys.Space))
                patrick.VSpeed = -15;
            patrick.Update();


            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            if (screen == Screen.Dead)
            {
                _spriteBatch.Draw(houseTexture, houseRect, Color.White);
                _spriteBatch.DrawString(titleText, "YOU DIED", new Vector2(20, 125), Color.Red);
                _spriteBatch.DrawString(titleText, "Space to replay", new Vector2(0, 250), Color.Red);
            }
            else if (screen == Screen.Intro)
            {
                _spriteBatch.Draw(houseTexture, houseRect, Color.White);

                _spriteBatch.Draw(coolPatrickTexture, coolPatrickRect, Color.White);
                _spriteBatch.DrawString(titleText, "Flappy Patrick", new Vector2(20, 25), Color.Gray);
            }
            else if (screen == Screen.Main)
            {
                _spriteBatch.Draw(groundTexture, groundRect, Color.White);
                patrick.Draw(_spriteBatch);

                foreach (Rectangle barrier in barriers)
                    _spriteBatch.Draw(wallTexture, barrier, Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
