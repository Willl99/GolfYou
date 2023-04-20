using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System;
using System.Linq;

namespace GolfYou
{
    internal class Camera
    {
        public Matrix Transform { get; private set; }
        public void Follow(Rectangle target, Vector2 mapBounds)
        {
            var camPositionX = -target.X - (target.Width / 2);
            var camPositionY = -target.Y - (target.Height / 2);
            camPositionY = MathHelper.Clamp(camPositionY, (int)(-mapBounds.Y) + 240 /*-304*/, -Game1.ScreenHeight / 2);
            camPositionX = MathHelper.Clamp(camPositionX, (int)(-mapBounds.X) + 400, -Game1.ScreenWidth / 2);
            var position = Matrix.CreateTranslation(
                camPositionX,
                camPositionY,
                0);

            var offset = Matrix.CreateTranslation(
                    Game1.ScreenWidth / 2,
                    Game1.ScreenHeight /2,
                    0);

            Transform = position * offset;


        }
    }
}
