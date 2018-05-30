using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MiningTeddies
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public const int WindowWidth = 800;
        public const int WindowHeight = 600;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<Mine> mines = new List<Mine>();
        List<TeddyBear> bears = new List<TeddyBear>();
        List<Explosion> explosions = new List<Explosion>();
        Mine mine;
        TeddyBear bear;
        Explosion explosion;

        Texture2D mineSprite0;
        Texture2D bearSprite0;
        Texture2D explosionSprite0;

        float timeElapsed = 0f;
        Random rnd = new Random();
        int randomSpawn = 0;
        Vector2 rndVelocity = new Vector2(0, 0);

        bool activated = false;
        int i = 0;
        int a = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
        }

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

            randomSpawn = rnd.Next(1, 3);

            mineSprite0 = Content.Load<Texture2D>(@"graphics\mine");
            bearSprite0 = Content.Load<Texture2D>(@"graphics\teddybear");
            explosionSprite0 = Content.Load<Texture2D>(@"graphics\explosion");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            rndVelocity.X = rnd.Next(-5, 5) / 10.0f;
            rndVelocity.Y = rnd.Next(-5, 5) / 10.0f;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                activated = true;
            }

            else if ((mouse.LeftButton == ButtonState.Released) && activated)
            {
                mine = new Mine(mineSprite0, mouse.X, mouse.Y);
                if (!mines.Contains(mine))
                    mines.Add(mine);

                activated = false;
            }

            if (timeElapsed >= randomSpawn)
            {
                bear = new TeddyBear(bearSprite0, rndVelocity, rnd.Next(0, 800), rnd.Next(0, 600));
                bears.Add(bear);
                timeElapsed = 0;
            }

            foreach (TeddyBear bear in bears)
            {
                bear.Update(gameTime);
            }

            if ((mines.Count != 0) && (bears.Count != 0))
            {
                for (i = bears.Count - 1; i >= 0; i--)
                {
                    for (a = mines.Count - 1; a >= 0; a--)
                    {
                        if (bears[i].CollisionRectangle.Intersects(mines[a].CollisionRectangle))
                        {
                            explosion = new Explosion(explosionSprite0, mines[a].CollisionRectangle.X, mines[a].CollisionRectangle.Y);
                            bears[i].Active = !bears[i].Active;
                            mines[a].Active = !mines[a].Active;
                            explosions.Add(explosion);
                            break;
                        }
                    }
                }
            }

            foreach (Explosion explosion in explosions)
            {
                explosion.Update(gameTime);
            }

            for (i = bears.Count - 1; i >= 0; i--)
            {
                if (!bears[i].Active)
                {
                    bears.Remove(bears[i]);
                }
            }

            for (i = mines.Count - 1; i >= 0; i--)
            {
                if (!mines[i].Active)
                {
                    mines.Remove(mines[i]);
                }
            }

            for (i = explosions.Count - 1; i >= 0; i--)
            {
                if (!explosions[i].Playing)
                {
                    explosions.Remove(explosions[i]);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (Mine mine in mines)
            {
                spriteBatch.Begin();
                mine.Draw(spriteBatch);
                spriteBatch.End();
            }

            if (bears.Count != 0)
            {
                foreach (TeddyBear bear in bears)
                {
                    spriteBatch.Begin();
                    bear.Draw(spriteBatch);
                    spriteBatch.End();
                }
            }
            
            foreach(Explosion explosion in explosions)
            {
                spriteBatch.Begin();
                explosion.Draw(spriteBatch);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
