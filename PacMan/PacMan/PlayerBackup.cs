using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacMan
{
    public class PlayerBackup : Character
    {
        bool isCenteredX = true;
        bool isCenteredY = true;
        Vector2 pressedDirections;

        public PlayerBackup(Texture2D texture, Vector2 position, string tag)
            : base(texture, position, tag)
        {
            pressedDirections = new Vector2(0, 0);
        }

        void ClearPressedDirections()
        {
            pressedDirections.X = 0;
            pressedDirections.Y = 0;
        }

        void SetPressedDirections(int newX, int newY)
        {
            pressedDirections.X = newX;
            pressedDirections.Y = newY;
        }
        public void TargetingAI(KeyboardState kbState)
        {
            if (kbState.IsKeyDown(Keys.Left))
            {
                if (Game1.map[currentTile.Y, currentTile.X - 1] != 2)
                {
                    if (IsCenteredY())
                    {
                        nextTile.Set(currentTile.X - 1, currentTile.Y);
                        velocity.X = -1 * speed;
                        velocity.Y = 0;
                        ClearPressedDirections();
                    }
                    else
                    {
                        SetPressedDirections(-1, 0);
                    }
                }
            }
            else if (kbState.IsKeyDown(Keys.Right))
            {
                if (Game1.map[currentTile.Y, currentTile.X + 1] != 2)
                {
                    if (IsCenteredY())
                    {
                        nextTile.Set(currentTile.X + 1, currentTile.Y);
                        velocity.X = speed;
                        velocity.Y = 0;
                        ClearPressedDirections();
                    }
                    else
                    {
                        SetPressedDirections(1, 0);
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
                        ClearPressedDirections();
                    }
                    else
                    {
                        SetPressedDirections(0, 1);
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
                        ClearPressedDirections();
                    }
                    else
                    {
                        SetPressedDirections(0, -1);
                    }
                }
            }
        }

        public void NextTarget()
        {
            Tile temp = new Tile();
            temp.Set(nextTile.X + nextTile.X - currentTile.X, nextTile.Y + nextTile.Y - currentTile.Y);
            currentTile.Set(nextTile.X, nextTile.Y);
            if (temp.X == -1 && temp.Y == 17)
            {
                temp.Set(26, 17);
                currentTile.Set(27, 17);
                position.X = 220;
                position.Y = 136;
                UpdateCenter();
            }

            if (temp.X == 28 && temp.Y == 17)
            {
                temp.Set(1, 17);
                currentTile.Set(0, 17);
                position.X = 2;
                position.Y = 136;
                UpdateCenter();
            }

            if (Game1.map[temp.Y, temp.X] != 2)
            {
                nextTile.Set(temp.X, temp.Y);
            }
        }

        void UpdateCenter()
        {
            center.X = position.X + texture.Width / 2;
            center.Y = position.Y + texture.Height / 2;
        }

        public void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
            UpdateCenter();
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

            if (nextTile.X == currentTile.X && nextTile.Y == currentTile.Y)
            {
                if (Math.Abs(center.X - currentTile.position.X) >= offset || Math.Abs(center.Y - currentTile.position.Y) >= offset && velocity.Y != 0)
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
