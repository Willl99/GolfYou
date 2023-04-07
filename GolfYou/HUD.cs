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
        Texture2D VelBars;
        Rectangle[] sourceRectanglesClubs;
        Rectangle[] sourceRectanglesVelBars;
        
        float timer;
        
        int VelBarsIndex;
        int velModifier;
        bool barDirectionRight;
        public void loadHudContent(ContentManager Content)
        {
            ClubOption = Content.Load<Texture2D>("Sprites/ClubOptions");
            VelBars = Content.Load<Texture2D>("Sprites/Bars/BarAll");
            
            sourceRectanglesClubs = new Rectangle[2];
            
            timer = 0f;

            barDirectionRight = true;


            for (int i = 0; i < 2; i++)
            {
                sourceRectanglesClubs[i] = new Rectangle(i * 15, 0, 15, 18);
            }
            sourceRectanglesVelBars = new Rectangle[82];
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0 + j*41; i < 41 + j*41; i++)
                {
                    sourceRectanglesVelBars[i] = new Rectangle(i * 91 - j * 3731, 0 + j*17, 91, 17);
                }
            }
            
        }
        public void drawHudContent(SpriteBatch _spriteBatch, int hittingMode, bool isPutting, bool wasPutting, Vector2 playerPosition)
        {
            if (hittingMode == 0)
            {
                _spriteBatch.Draw(ClubOption, new Rectangle(730, 30, 30, 36), sourceRectanglesClubs[1], Color.White);
            }
            else if (hittingMode == 1)
            {
                _spriteBatch.Draw(ClubOption, new Rectangle(730,30, 30, 36), sourceRectanglesClubs[0], Color.White);
            }
            if (isPutting || wasPutting)
            {
                _spriteBatch.Draw(VelBars, new Rectangle((int)playerPosition.X - 28, (int)playerPosition.Y - 20, 91, 17), sourceRectanglesVelBars[VelBarsIndex], Color.White);
            }
        }

        public void playHudAnimations(GameTime gameTime, bool isPutting, bool isRolling)
        {
            if (isPutting)
            {
                float threshold = .01f;
                if (timer > threshold)
                {
                    if (VelBarsIndex < 81 && barDirectionRight)
                    {
                        VelBarsIndex+=3;
                    }
                    else if (VelBarsIndex >= 81 && barDirectionRight)
                    {
                        barDirectionRight = false;
                    }
                    else if (VelBarsIndex > 0 && !barDirectionRight)
                    {
                        VelBarsIndex-=3;
                    }
                    else if (VelBarsIndex == 0 && !barDirectionRight)
                    {
                        barDirectionRight= true;
                    }
                    timer = 0;
                }
                else
                {
                    timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            else if (isRolling)
            {
                velModifier = VelBarsIndex;
                Debug.WriteLine(VelBarsIndex);
                VelBarsIndex = 0;
            }
        }

        public int getVelModifier()
        {
            return velModifier;
        }

    }
}
