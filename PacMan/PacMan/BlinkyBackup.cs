using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacMan
{
    class BlinkyBackup : Character
    {
        bool nextTileAvailable;

        public BlinkyBackup(Texture2D texture, Vector2 position, string tag)
            : base(texture, position, tag)
        {
            UpdateCenter();
        }

        public void TargetingAI(Character character)
        {
            if (Math.Abs(center.X - currentTile.position.X) > Game1.tileSize.X / 2 || Math.Abs(center.Y - currentTile.position.Y) > Game1.tileSize.Y / 2)
            {
                NextTarget(character);
            }

            if ((Math.Abs(center.X - currentTile.position.X) <= offset && velocity.X != 0) || (Math.Abs(center.Y - currentTile.position.Y) <= offset && velocity.Y != 0))
            {
                SetPosition(new Vector2(currentTile.position.X - Game1.tileSize.X / 2, currentTile.position.Y - Game1.tileSize.Y / 2));
                velocity.X = (nextTile.X - currentTile.X) * speed;
                velocity.Y = (nextTile.Y - currentTile.Y) * speed;
            }

            Move();
        }

        public void NextTarget(Character character)
        {
            targetTile = character.currentTile;
            float dist = float.MaxValue, dist2 = float.MaxValue;
            Tile tempTile = new Tile();
            currentTile.Set(nextTile.X, nextTile.Y);

            if ((velocity.Y == 0 || velocity.Y > 0) && Game1.IsTileAvailible(currentTile.X, currentTile.Y + 1))
            {
                tempTile.Set(currentTile.X, currentTile.Y + 1);
                dist2 = Vector2.DistanceSquared(tempTile.position, targetTile.position);
                if (dist2 < dist)
                {
                    dist = dist2;
                    nextTile.Set(tempTile.X, tempTile.Y);
                }
            }

            if ((velocity.Y == 0 || velocity.Y < 0) && Game1.IsTileAvailible(currentTile.X, currentTile.Y - 1))
            {
                tempTile.Set(currentTile.X, currentTile.Y - 1);
                dist2 = Vector2.DistanceSquared(tempTile.position, targetTile.position);
                if (dist2 < dist)
                {
                    dist = dist2;
                    nextTile.Set(tempTile.X, tempTile.Y);
                }
            }

            if ((velocity.X == 0 || velocity.X > 0) && Game1.IsTileAvailible(currentTile.X + 1, currentTile.Y))
            {
                tempTile.Set(currentTile.X + 1, currentTile.Y);
                dist2 = Vector2.DistanceSquared(tempTile.position, targetTile.position);
                if (dist2 < dist)
                {
                    dist = dist2;
                    nextTile.Set(tempTile.X, tempTile.Y);
                }
            }

            if ((velocity.X == 0 || velocity.X < 0) && Game1.IsTileAvailible(currentTile.X - 1, currentTile.Y))
            {
                tempTile.Set(currentTile.X - 1, currentTile.Y);
                dist2 = Vector2.DistanceSquared(tempTile.position, targetTile.position);
                if (dist2 < dist)
                {
                    dist = dist2;
                    nextTile.Set(tempTile.X, tempTile.Y);
                }
            }
        }

        public void Move()
        {
            SetPosition(position + velocity);
        }
    }
}
