using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacMan
{
    public class Player : Character
    {
        bool isCenteredX = true;
        bool isCenteredY = true;
        Vector2 pressedDirections;
        public int score;
        public Texture2D pacmanDownTexture, pacmanLeftTexture, pacmanRightTexture, pacmanUpTexture;

        public Player(Texture2D texture, Vector2 position, string tag)
            : base(texture, position, tag)
        {
        }

        public Texture2D GetTexture()
        {
            if(velocity.X < 0)
            {
                texture = pacmanLeftTexture;
                return pacmanLeftTexture;
            }
            else if(velocity.X > 0)
            {
                texture = pacmanRightTexture;
                return pacmanRightTexture;
            }
            else if(velocity.Y > 0)
            {
                texture = pacmanDownTexture;
                return pacmanDownTexture;
            }
            else if (velocity.Y < 0)
            {
                texture = pacmanUpTexture;
                return pacmanUpTexture;
            }
            else
            {
                return texture;
            }
        }

        public void HoldInput(KeyboardState kbState)
        {
            if (kbState.IsKeyDown(Keys.Left))
            {
                if (Game1.IsTileAvailible(currentTile.X - 1, currentTile.Y) && Game1.map[currentTile.Y, currentTile.X - 1] != 2)
                {
                    if (IsCenteredY())
                    {
                        nextTile.Set(currentTile.X - 1, currentTile.Y);
                        velocity.X = -1 * speed;
                        velocity.Y = 0;
                    }
                }
            }
            else if (kbState.IsKeyDown(Keys.Right))
            {
                if (Game1.IsTileAvailible(currentTile.X + 1, currentTile.Y) && Game1.map[currentTile.Y, currentTile.X + 1] != 2)
                {
                    if (IsCenteredY())
                    {
                        nextTile.Set(currentTile.X + 1, currentTile.Y);
                        velocity.X = speed;
                        velocity.Y = 0;
                    }
                }
            }
            else if (kbState.IsKeyDown(Keys.Up))
            {
                if (Game1.map[currentTile.Y - 1, currentTile.X] != 2)
                {
                    if (IsCenteredX())
                    {
                        nextTile.Set(currentTile.X, currentTile.Y - 1);
                        velocity.X = 0;
                        velocity.Y = -1 * speed;
                    }
                }
            }
            else if (kbState.IsKeyDown(Keys.Down))
            {
                if (Game1.map[currentTile.Y + 1, currentTile.X] != 2)
                {
                    if (IsCenteredX())
                    {
                        nextTile.Set(currentTile.X, currentTile.Y + 1);
                        velocity.X = 0;
                        velocity.Y = speed;
                    }
                }
            }
        }



        public void NextTarget()
        {

            // The candidate for next tile
            Tile temp = new Tile();

            // Get the direction in which we are moving by substracting the next tile from the current
            // This way we choose the next tile to be in the same line of direction
            temp.Set(nextTile.X + nextTile.X - currentTile.X, nextTile.Y + nextTile.Y - currentTile.Y);

            //Set the current tile to the next tile, because we are in it now (reason: this method is called)
            currentTile.Set(nextTile.X, nextTile.Y);

            //If the candidate tile is the left side of the "tunnel", then set it to the other end
            if (temp.X == -1 && temp.Y == 17)
            {
                temp.Set(26, 17);
                currentTile.Set(27, 17);
                nextTile.Set(26, 17);
                position.X = 486;
                position.Y = 244;
                UpdateCenter();
            }

            //If the candidate tile is the right side of the "tunnel", then set it to the other end
            if (temp.X == 28 && temp.Y == 17)
            {
                temp.Set(1, 17);
                currentTile.Set(0, 17);
                nextTile.Set(0, 17);
                position.X = 0;
                position.Y = 244;
                UpdateCenter();
            }

            // If the candidate tile is not a wall, set it to next tile
            if (Game1.map[temp.Y, temp.X] != 2)
            {
                nextTile.Set(temp.X, temp.Y);
            }
        }

        bool IsCenteredX()
        {
            return Math.Abs(center.X - currentTile.position.X) <= offset;
        }

        bool IsCenteredY()
        {
            return Math.Abs(center.Y - currentTile.position.Y) <= offset;
        }

        public void Move()
        {
            SetPosition(position + velocity);
            if(Game1.map[currentTile.Y, currentTile.X] == 1)
            {
                Game1.map[currentTile.Y, currentTile.X] = 0;
                score += 10;
            }

            if (nextTile.X == currentTile.X && nextTile.Y == currentTile.Y)
            {
                if ((Math.Abs(center.X - currentTile.position.X) <= offset && velocity.X != 0) || (Math.Abs(center.Y - currentTile.position.Y) <= offset && velocity.Y != 0))
                {
                    SetPosition(new Vector2(currentTile.position.X - Game1.tileSize.X / 2, currentTile.position.Y - Game1.tileSize.Y / 2));
                    velocity.X = 0f;
                    velocity.Y = 0f;
                }
            }

            if (Math.Abs(center.X - currentTile.position.X) <= offset && velocity.X != 0)
            {
                SetPosition(new Vector2(position.X, currentTile.position.Y - Game1.tileSize.Y / 2));
            }

            if (Math.Abs(center.Y - currentTile.position.Y) <= offset && velocity.Y != 0)
            {
                SetPosition(new Vector2(currentTile.position.X - Game1.tileSize.X / 2, position.Y));
            }

            if (Math.Abs(center.X - currentTile.position.X) > Game1.tileSize.X / 2 || Math.Abs(center.Y - currentTile.position.Y) > Game1.tileSize.Y / 2)
            {
                NextTarget();
            }
        }
    }
}
