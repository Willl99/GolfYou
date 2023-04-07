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
        Texture2D AngleArrow;
        Rectangle[] sourceRectanglesClubs;
        Rectangle[] sourceRectanglesVelBars;
        
        private float timer;
        private float rotation;
        private float angle;

        private int VelBarsIndex;
        private int velModifier;
        private const float MaxAngle = 1.5f; // 45 degrees
        private const float MinAngle = 0f; // 90 degrees, essentially take this float and multiply by 30 to get the angle, as to why its in reverse I don't know


        private bool barDirectionRight;
        private bool prevAnglePutting;
        private bool angleDirection;
        public void loadHudContent(ContentManager Content)
        {
            ClubOption = Content.Load<Texture2D>("Sprites/ClubOptions");
            VelBars = Content.Load<Texture2D>("Sprites/Bars/BarAll");
            AngleArrow = Content.Load<Texture2D>("Sprites/Arrow");
            
            sourceRectanglesClubs = new Rectangle[2];
            
            timer = 0f;

            barDirectionRight = true;
            angleDirection = true;
            prevAnglePutting = false;

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
        public void drawHudContent(SpriteBatch _spriteBatch, int hittingMode, bool isPutting, bool wasPutting, Vector2 playerPosition, bool anglePutting, int facing)
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
            else if (anglePutting)
            {

                
                Vector2 origin = new Vector2(AngleArrow.Width / 2, AngleArrow.Height);
                if (facing == 1) { _spriteBatch.Draw(AngleArrow, new Rectangle((int)playerPosition.X + 36, (int)playerPosition.Y, 24, 36), null, Color.White, rotation, origin, SpriteEffects.None, 0f); }
                else if (facing == 0) { _spriteBatch.Draw(AngleArrow, new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 24, 36), null, Color.White, -rotation, origin, SpriteEffects.None, 0f); }

            }
        }

        public void playHudAnimations(GameTime gameTime, bool isPutting, bool isRolling, bool anglePutting, int facing)
        {
            calcRotation(anglePutting, facing);
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
                VelBarsIndex = 0;
            }
        }

        public int getVelModifier()
        {
            return velModifier;
        }

        public float getAngle()
        {
            return angle;
        }

        private float calcRotation(bool anglePutting, int facing)
        {
            if (anglePutting && !prevAnglePutting)
            {
                rotation = MaxAngle;
            }
            else if (anglePutting && prevAnglePutting) 
            {
                rotation = MathHelper.Clamp(rotation, MinAngle, MaxAngle);
                if (rotation < MaxAngle && angleDirection) 
                {   
                    rotation += .01f;
                    angle = 90 - (rotation * 30);
                }
                else if (rotation == MaxAngle && angleDirection)
                {
                    angleDirection = false;
                }
                if (rotation > MinAngle && !angleDirection) 
                { 
                    rotation -= .01f;
                    angle = 90 - (rotation * 30);
                }
                else if (rotation == MinAngle && !angleDirection) {angleDirection= true; }

            }
            prevAnglePutting = anglePutting;
            return rotation;
        }
    }
}
