using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfYou
{
    public class HUD
    {
        Texture2D ClubOption;

        Rectangle[] sourceRectanglesClubs;
        public void loadHudContent(ContentManager Content)
        {
            ClubOption = Content.Load<Texture2D>("Sprites/ClubOptions");

            sourceRectanglesClubs = new Rectangle[2];
            for (int i = 0; i < 2; i++)
            {
                sourceRectanglesClubs[i] = new Rectangle(i * 15, 0, 15, 18);
            }
        }
        public void drawHudContent(SpriteBatch _spriteBatch, int hittingMode)
        {
            if (hittingMode == 0)
            {
                _spriteBatch.Draw(ClubOption, new Rectangle(730, 30, 30, 36), sourceRectanglesClubs[1], Color.White);
            }
            else if (hittingMode == 1)
            {
                _spriteBatch.Draw(ClubOption, new Rectangle(730,30, 30, 36), sourceRectanglesClubs[0], Color.White);
            }
        }
    }
}
