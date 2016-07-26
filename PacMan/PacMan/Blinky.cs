using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PacMan;

namespace PacMan
{

    public enum MovementMode
    {
        Scatter, Chase, Frightened
    }

    class Blinky : Character
    {

        bool nextTileAvailable;
        public bool velocityReversePending;
        public MovementMode movementMode;
        public Tile scatterTargetTile;
        private Texture2D frightenedTexture;

        public Blinky(Texture2D texture, Texture2D frightenedTexture, Vector2 position, string tag)
            : base(texture, position, tag)
        {
            this.frightenedTexture = frightenedTexture;
            UpdateCenter();
        }

        public Texture2D Texture
        {
            get
            {
                if(movementMode == MovementMode.Frightened)
                {
                    return frightenedTexture;
                }

                return texture;
            }
        }


        public void SetMovementMode(MovementMode movementMode)
        {
            // If changing mode to Scatter from other
            if(this.movementMode != movementMode && (movementMode == MovementMode.Scatter || movementMode == MovementMode.Frightened))
            {
                velocityReversePending = true;
            }
            this.movementMode = movementMode;
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
            if(movementMode == MovementMode.Chase)
            { 
                targetTile = character.currentTile; 
            }
            else if(movementMode == MovementMode.Scatter)
            {
                targetTile = this.scatterTargetTile;
            }

            if(velocityReversePending)
            {
                velocity = -velocity;
                velocityReversePending = false;
            }
            else
            {
                currentTile.Set(nextTile.X, nextTile.Y);
            }
            
            
            float dist = float.MaxValue, distMin = float.MaxValue;
            Tile tempTile = new Tile();
       

            if ((velocity.Y == 0 || velocity.Y > 0) &&
                (Game1.IsTileAvailible(currentTile.X, currentTile.Y + 1) || Game1.IsTileTunnel(currentTile.X, currentTile.Y + 1)))
            {
                tempTile.Set(currentTile.X, currentTile.Y + 1);
                distMin = Vector2.DistanceSquared(tempTile.position, targetTile.position);
                if (distMin < dist)
                {
                    dist = distMin;
                    nextTile.Set(tempTile.X, tempTile.Y);
                }
            }

            if ((velocity.Y == 0 || velocity.Y < 0) &&
                (Game1.IsTileAvailible(currentTile.X, currentTile.Y - 1) || Game1.IsTileTunnel(currentTile.X, currentTile.Y - 1)))
            {
                tempTile.Set(currentTile.X, currentTile.Y - 1);
                distMin = Vector2.DistanceSquared(tempTile.position, targetTile.position);
                if (distMin < dist)
                {
                    dist = distMin;
                    nextTile.Set(tempTile.X, tempTile.Y);
                }
            }

            if ((velocity.X == 0 || velocity.X > 0) && 
                (Game1.IsTileAvailible(currentTile.X + 1, currentTile.Y) || Game1.IsTileTunnel(currentTile.X + 1, currentTile.Y)))
            {
                tempTile.Set(currentTile.X + 1, currentTile.Y);
                distMin = Vector2.DistanceSquared(tempTile.position, targetTile.position);
                if (distMin < dist)
                {
                    dist = distMin;
                    nextTile.Set(tempTile.X, tempTile.Y);
                }
            }

            if ((velocity.X == 0 || velocity.X < 0) &&
                (Game1.IsTileAvailible(currentTile.X - 1, currentTile.Y) || Game1.IsTileTunnel(currentTile.X - 1, currentTile.Y)))
            {
                tempTile.Set(currentTile.X - 1, currentTile.Y);
                distMin = Vector2.DistanceSquared(tempTile.position, targetTile.position);
                if (distMin < dist)
                {
                    dist = distMin;
                    nextTile.Set(tempTile.X, tempTile.Y);
                }
            }

            //If the candidate tile is the left side of the "tunnel", then set it to the other end
            if (nextTile.X == -1 && nextTile.Y == 17)
            {
                nextTile.Set(26, 17);
                currentTile.Set(27, 17);
                position.X = 486;
                position.Y = 244;
                UpdateCenter();
            }


            //If the candidate tile is the right side of the "tunnel", then set it to the other end
            if (nextTile.X == 28 && nextTile.Y == 17)
            {
                nextTile.Set(1, 17);
                currentTile.Set(0, 17);
                position.X = 0;
                position.Y = 244;
                UpdateCenter();
            }

        }

        public void Move()
        {
            SetPosition(position + velocity);
        }
    }
}
