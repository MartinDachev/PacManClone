using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacMan
{
    public abstract class Character
    {
        public Texture2D texture;
        public Vector2 position;
        public string tag;
        public Tile currentTile;
        public Tile targetTile;
        public Tile nextTile;
        public Vector2 velocity;
        public float offset;
        public float speed;
        public Vector2 center;
        public float width;
        public float height;

        public Character(Texture2D texture, Vector2 position, string tag)
        {
            this.texture = texture;
            this.position = position;
            this.tag = tag;
        }

        public Character()
        {
        }

        public void ChangeVelocity(Vector2 newVelocity)
        {
            velocity = newVelocity;
            offset = 0;
            float temp;

            if (velocity.X != 0)
            {
                temp = Game1.tileSize.X / velocity.X;
                offset = (float)(temp - Math.Floor(temp)) * velocity.X;
            }
            if (velocity.Y != 0)
            {
                temp = Game1.tileSize.Y / velocity.Y;
                offset = Math.Max((float)(temp - Math.Floor(temp)) * velocity.Y, offset);
            }

            offset += 0.001f;
        }

        void UpdateOffset()
        {
            offset = 0;
            float temp;

            if (velocity.X != 0)
            {
                temp = Game1.tileSize.X / velocity.X;
                offset = (float)(temp - Math.Floor(temp)) * velocity.X;
            }
            if (velocity.Y != 0)
            {
                temp = Game1.tileSize.Y / velocity.Y;
                offset = Math.Max((float)(temp - Math.Floor(temp)) * velocity.Y, offset);
            }

            offset += 0.001f;
        }

        public void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
            UpdateCenter();
        }

        public void UpdateCenter()
        {
            center.X = position.X + width / 2;
            center.Y = position.Y + height / 2;
        }
    }
}
