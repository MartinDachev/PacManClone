using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacMan
{
    public struct Tile
    {
        public int X;
        public int Y;
        public Vector2 position;

        public Tile(int numberX, int numberY, Vector2 posInSpace)
        {
            X = numberX;
            Y = numberY;
            position = posInSpace;
        }

        public void Set(int newX, int newY)
        {
            X = newX;
            Y = newY;
            UpdatePosition();
        }

        void UpdatePosition()
        {
            position.X = (X + 0.5f) * Game1.tileSize.X;
            position.Y = (Y + 0.5f) * Game1.tileSize.Y - 62;
        }
    }
}
