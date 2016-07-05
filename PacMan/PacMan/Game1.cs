using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PacMan
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public enum GameState
        {
            Playing, Menu, Paused
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameState gameState = GameState.Menu;
        MovementMode ghostsMovementMode = MovementMode.Scatter;
        Texture2D[] tiles;
        Texture2D mapTexture;
        Texture2D pelletTexture;
        SpriteFont font, fontSmall;
        public static Player player;
        Blinky blinky;
        KeyboardState kbState;

        GameTimer ghostsModeTimer;
        double[] scatterStart = new double[4];
        double[] chaseStart = new double[3];
        bool chasePermanently;

        public static Vector2 tileSize = new Vector2(18, 18);

        Tile blinkyScatterTile = new Tile();

        public static int[,] map = 
        {
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
            { 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 },
            { 2, 1, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 1, 2, 2, 1, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 1, 2 },
            { 2, 1, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 1, 2, 2, 1, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 1, 2 },
            { 2, 1, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 1, 2, 2, 1, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 1, 2 },
            { 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 },
            { 2, 1, 2, 2, 2, 2, 1, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 2, 2, 2, 2, 1, 2 },
            { 2, 1, 2, 2, 2, 2, 1, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 2, 2, 2, 2, 1, 2 },
            { 2, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 2 },
            { 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 0, 2, 2, 0, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 0, 2, 2, 0, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 2, 1, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 1, 2, 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 2, 1, 2, 2, 0, 2, 2, 2, 0, 0, 2, 2, 2, 0, 2, 2, 1, 2, 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 2, 1, 2, 2, 0, 2, 0, 0, 0, 0, 0, 0, 2, 0, 2, 2, 1, 2, 2, 2, 2, 2, 2 },
            { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
            { 2, 2, 2, 2, 2, 2, 1, 2, 2, 0, 2, 0, 0, 0, 0, 0, 0, 2, 0, 2, 2, 1, 2, 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 2, 1, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 1, 2, 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 2, 1, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 1, 2, 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 2, 1, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 1, 2, 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 2, 1, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 1, 2, 2, 2, 2, 2, 2 },
            { 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 },
            { 2, 1, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 1, 2, 2, 1, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 1, 2 },
            { 2, 1, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 1, 2, 2, 1, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 1, 2 },
            { 2, 4, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 4, 2 },
            { 2, 2, 2, 1, 2, 2, 1, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 2, 2, 1, 2, 2, 2 },
            { 2, 2, 2, 1, 2, 2, 1, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 2, 2, 1, 2, 2, 2 },
            { 2, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 2 },
            { 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2 },
            { 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2 },
            { 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 }

        };

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 508;
        }

        public static bool IsTileTunnel(int x, int y)
        {
            bool result = (x == 28 && y == 17) || (x == -1 && y == 17);
            return result;
        }

        public static bool IsTileAvailible(int x, int y)
        {
            bool result = x >= 0 && y >= 0 && y < map.GetLength(0) && x < map.GetLength(1) && map[y, x] != 2;
            return result;
        }

        protected override void Initialize()
        {
            tiles = new Texture2D[5];
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tiles[0] = Content.Load<Texture2D>("zeroTile");
            tiles[1] = Content.Load<Texture2D>("foodTile");
            tiles[2] = Content.Load<Texture2D>("forbbTile");
            tiles[3] = Content.Load<Texture2D>("enterTile");
            tiles[4] = Content.Load<Texture2D>("powerTile");
            font = Content.Load<SpriteFont>("Broadway");
            fontSmall = Content.Load<SpriteFont>("BroadwaySmall");
            mapTexture = Content.Load<Texture2D>("map");
            pelletTexture = Content.Load<Texture2D>("pellet");
            player = new Player(Content.Load<Texture2D>("player"), new Vector2(tileSize.X, 4 * tileSize.Y - 62), "player");
            player.currentTile = new Tile(1, 4, new Vector2(1.5f * tileSize.X, 4.5f * tileSize.Y));
            player.currentTile.Set(1, 4);
            player.nextTile = player.currentTile;
            player.speed = 3f;
            player.width = tileSize.X;
            player.height = tileSize.Y;
            player.ChangeVelocity(new Vector2(0f, 0f));
            player.pacmanDownTexture = Content.Load<Texture2D>("pacmanDown");
            player.pacmanLeftTexture = Content.Load<Texture2D>("pacmanLeft");
            player.pacmanRightTexture = Content.Load<Texture2D>("pacmanRight");
            player.pacmanUpTexture = Content.Load<Texture2D>("pacmanUp");
            blinky = new Blinky(Content.Load<Texture2D>("blinky"), new Vector2(11 * tileSize.X, 17 * tileSize.Y - 62), "enemy");
            blinky.currentTile = new Tile(11, 17, new Vector2(11.5f * tileSize.X, 17.5f * tileSize.Y));
            blinky.currentTile.Set(11, 17);
            blinkyScatterTile.Set(25, 0);
            blinky.scatterTargetTile = blinkyScatterTile;
            blinky.nextTile = blinky.currentTile;
            blinky.nextTile.Set(blinky.currentTile.X + 1, blinky.currentTile.Y + 1);
            blinky.width = tileSize.X;
            blinky.height = tileSize.Y;
            blinky.speed = 3f;
            blinky.ChangeVelocity(new Vector2(3f, 0f));
            blinky.UpdateCenter();
            //blinky.TargetingAI(player);
            ghostsModeTimer = new GameTimer();
            ghostsModeTimer.Start();
            scatterStart = new double[] { 0, 27000, 54000, 79000 };
            chaseStart = new double[] { 7000, 34000, 59000, 84000 };
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState kbState = Keyboard.GetState();

            if(gameState == GameState.Playing)
            {
                if(kbState.IsKeyDown(Keys.Escape))
                {
                    gameState = GameState.Paused;
                }

                UpdateGame(kbState, gameTime);
            }

            if(gameState == GameState.Menu || gameState == GameState.Paused)
            {
                if(kbState.IsKeyDown(Keys.Space))
                {
                    gameState = GameState.Playing;
                }
            }

            base.Update(gameTime);
        }

        private void UpdateGame(KeyboardState kbState, GameTime gameTime)
        {
            IncrementTimers(gameTime);
            HandleGhostModesTimer();
            player.HoldInput(kbState);
            player.Move();
            blinky.TargetingAI(player);
        }

        private void IncrementTimers(GameTime gameTime)
        {
            ghostsModeTimer.Tick(gameTime.ElapsedGameTime.Milliseconds);
        }
        //bool changedToScatter = false;
        private void HandleGhostModesTimer()
        {
            if(!chasePermanently && ghostsModeTimer.Enabled)
            {
                int length = Math.Min(scatterStart.Length, chaseStart.Length);
                bool  changedToScatter = false;
                for (int i = 0; i < length; i++)
                {
                    if (ghostsModeTimer.Time >= scatterStart[i] && ghostsModeTimer.Time <= chaseStart[i])
                    {
                        SetGhostsMode(MovementMode.Scatter);
                        changedToScatter = true;
                        break;
                    }
                }

                if(!changedToScatter)
                {
                    //changedToScatter = true;
                    SetGhostsMode(MovementMode.Chase);
                }

                if(ghostsModeTimer.Time > chaseStart[chaseStart.Length - 1])
                {
                    chasePermanently = true;
                }
            }
        }

        private void SetGhostsMode(MovementMode movementMode)
        {
            ghostsMovementMode = movementMode;
            blinky.SetMovementMode(movementMode);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            spriteBatch.Begin();
            Vector2 pos = new Vector2(0, -62);
            Vector2 offset = new Vector2(8, 16);
            spriteBatch.Draw(mapTexture, new Vector2(0, 0), Color.White);
            for(int i = 0; i < 36; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    if (map[i, j] == 1)
                    spriteBatch.Draw(pelletTexture, pos + offset, Color.White);
                    pos.X += tileSize.X;
                }
                pos.X = 0;
                pos.Y += tileSize.Y;
            }

            /*for(int i = 0; i < 36; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    if (map[i, j] == 1)
                    {
                        spriteBatch.Draw(pelletTexture, new Vector2(((i + 0.5f) * tileSize.X - pelletTexture.Width), (j + 0.5f) * tileSize.X - pelletTexture.Width), Color.White);
                    }
                }
            }*/

            spriteBatch.Draw(blinky.texture, new Vector2(blinky.position.X - 4, blinky.position.Y), Color.White);
            spriteBatch.Draw(player.GetTexture(), new Vector2(player.position.X - 4, player.position.Y), Color.White);
            //spriteBatch.DrawString(fontSmall, "Score: " + player.score, new Vector2(320, 560), Color.White);
            spriteBatch.DrawString(fontSmall, "Score: " + blinky.movementMode.ToString(), new Vector2(320, 560), Color.White);
            if (gameState == GameState.Menu || gameState == GameState.Paused)
            {
                spriteBatch.DrawString(font, "Press Space to play!", new Vector2(90, 245), Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
