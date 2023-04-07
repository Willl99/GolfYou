using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System;

namespace GolfYou
{
    public class Player
    {
        Texture2D runRightSet;
        Texture2D runLeftSet;
        Texture2D puttingRightSet;
        Texture2D puttingLeftSet;
        Texture2D head;

        Rectangle[] sourceRectanglesRun;
        Rectangle[] sourceRectanglesPutting;
        Rectangle playerHitbox = new Rectangle(0, 200, 32, 32);

        Vector2 origin;
        float rotation;

        byte currentAnimationIndexRun;
        byte currentAnimationIndexPuttingRight;
        byte currentAnimationIndexPuttingLeft;
        private int threshold;
        float timer;
        private float movement = 0.0f;
        private int facing = 1;
        public bool rolling;
        private bool isPutting;
        private bool wasPutting;
        private bool anglePutting;
        private int hittingMode; // 0 = drive, 1 = tap
        private int velChoice;

        private const Buttons puttButton = Buttons.A;

        public void loadPlayerContent(ContentManager Content)
        {
            runRightSet = Content.Load<Texture2D>("Sprites/WalkanimFlipped");
            runLeftSet = Content.Load<Texture2D>("Sprites/Walkanim");
            puttingRightSet = Content.Load<Texture2D>("Sprites/PlayerPuttingAll");
            puttingLeftSet = Content.Load<Texture2D>("Sprites/PlayerPuttingAllFlipped");
            head = Content.Load<Texture2D>("Sprites/Head");

            currentAnimationIndexRun = 0;
            currentAnimationIndexPuttingLeft = 5;
            currentAnimationIndexPuttingRight = 0;
            rotation = 0;
            timer = 0;
            threshold = 10;
            hittingMode = 0;

            rolling = false;
            isPutting = false;

            origin = new Vector2(head.Width / 2f, head.Height / 2f);

            sourceRectanglesRun = new Rectangle[4];
            for (int i = 0; i < 4; i++)
            {
                sourceRectanglesRun[i] = new Rectangle(i * 16, 0, 16, 16);
            }
            sourceRectanglesPutting = new Rectangle[6];
            for (int i = 0; i < 6; i++)
            {
                sourceRectanglesPutting[i] = new Rectangle(i * 64 + 23, 24, 18, 20);
            }
        }

        public void drawPlayer(SpriteBatch _spriteBatch, GameTime gameTime, Vector2 velocity)
        {
            if (facing == 1 && movement > 0 && !isPutting && !wasPutting && !rolling && !anglePutting)
            {
                _spriteBatch.Draw(runRightSet, playerHitbox, sourceRectanglesRun[currentAnimationIndexRun], Color.White);
            }
            else if (facing == 1 && movement == 0 && !isPutting && !wasPutting && !rolling && !anglePutting)
            {
                _spriteBatch.Draw(runRightSet, playerHitbox, sourceRectanglesRun[1], Color.White);
            }
            else if (facing == 1 && isPutting || facing == 1 && wasPutting || facing == 1 && anglePutting)
            {
                _spriteBatch.Draw(puttingRightSet, new Rectangle(playerHitbox.X, playerHitbox.Y, 34, 40), sourceRectanglesPutting[currentAnimationIndexPuttingRight], Color.White);
            }
            else if (facing == 0 && movement < 0 && !isPutting && !wasPutting && !rolling && !anglePutting)
            {
                _spriteBatch.Draw(runLeftSet, playerHitbox, sourceRectanglesRun[currentAnimationIndexRun], Color.White);
            }
            else if (facing == 0 && movement == 0 && !isPutting & !wasPutting && !rolling && !anglePutting)
            {
                _spriteBatch.Draw(runLeftSet, playerHitbox, sourceRectanglesRun[0], Color.White);
            }
            else if (facing == 0 && isPutting || facing == 0 && wasPutting || facing == 0 && anglePutting)
            {
                _spriteBatch.Draw(puttingLeftSet, new Rectangle(playerHitbox.X, playerHitbox.Y, 34, 40), sourceRectanglesPutting[currentAnimationIndexPuttingLeft], Color.White);
            }
            else if (rolling)
            {
                _spriteBatch.Draw(head, new Rectangle(playerHitbox.X + 12, playerHitbox.Y + 12, 26, 20), null, Color.White, rotation, origin, SpriteEffects.None, 0f);
                rotation = -.1f * velocity.X;
            }
        }

        public void playAnimation(GameTime gameTime)
        {
            if (facing == 1 && movement > 0 && !isPutting)
            {
                threshold = 150;
                if (timer > threshold)
                {
                    if (currentAnimationIndexRun < 3)
                    {
                        currentAnimationIndexRun++;
                    }
                    else
                    {
                        currentAnimationIndexRun = 0;
                    }
                    timer = 0;
                }
                else
                {
                    timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            else if (facing == 0 && movement < 0 && !isPutting)
            {
                threshold = 150;
                if (timer > threshold)
                {
                    if (currentAnimationIndexRun > 0)
                    {
                        currentAnimationIndexRun--;
                    }
                    else
                    {
                        currentAnimationIndexRun = 3;
                    }
                    timer = 0;
                }
                else
                {
                    timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            else if (facing == 1 && (isPutting || anglePutting))
            {
                threshold = 100;
                if (timer > threshold)
                {
                    if (currentAnimationIndexPuttingRight < 5)
                    {
                        currentAnimationIndexPuttingRight++;
                    }
                    timer = 0;
                }
                else
                {
                    timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            else if (facing == 1 && wasPutting)
            {
                threshold = 15;
                if (timer > threshold)
                {
                    if (currentAnimationIndexPuttingRight > 0)
                    {
                        currentAnimationIndexPuttingRight--;
                    }
                    else
                    {
                        wasPutting = false;
                        rolling = true;
                    }
                    timer = 0;
                }
                else
                {
                    timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }


            }
            else if (facing == 0 && (isPutting || anglePutting))
            {
                threshold = 100;
                if (timer > threshold)
                {
                    if (currentAnimationIndexPuttingLeft > 0 )
                    {
                        currentAnimationIndexPuttingLeft--;
                    }
                    timer = 0;
                }
                else
                {
                    timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            else if (facing == 0 && wasPutting)
            {
                threshold = 15;
                if (timer > threshold)
                {
                    if (currentAnimationIndexPuttingLeft < 5)
                    {
                        currentAnimationIndexPuttingLeft++;
                    }
                    else
                    {
                        wasPutting = false;
                        rolling = true;
                    }
                    timer = 0;
                }
                else
                {
                    timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }


            }
        }

        public void handlePlayerInput(
                    KeyboardState keyboardState,
                    GamePadState gamePadState,
                    GameTime gameTime)
        {
            // Get analog horizontal movement.
            movement = gamePadState.ThumbSticks.Left.X;

            // Ignore small movements to prevent running in place.
            if (Math.Abs(movement) < 0.5f)
                movement = 0.0f;

            // If any digital horizontal movement input is found, override the analog movement.
            if (gamePadState.IsButtonDown(Buttons.DPadLeft) ||
                keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.A) && !isPutting && !wasPutting && !rolling)
            {
                facing = 0;
                movement = -1.0f;
            }
            else if (gamePadState.IsButtonDown(Buttons.DPadRight) ||
                     keyboardState.IsKeyDown(Keys.Right) ||
                     keyboardState.IsKeyDown(Keys.D) && !isPutting && !wasPutting && !rolling)
            {
                facing = 1;
                movement = 1.0f;
            }

            // Check if the player wants to putt.
            myKeyboard.GetState();
            if (myKeyboard.HasBeenPressed(Keys.Space) && isPutting && !anglePutting)
            {
                isPutting = false;
                wasPutting = true;

            }
            if (myKeyboard.HasBeenPressed(Keys.Space) && anglePutting) 
            {
                anglePutting = false;
                isPutting = true;
                
            }

            if (myKeyboard.HasBeenPressed(Keys.Space) && !rolling && !isPutting && !anglePutting && !wasPutting)
            {
                anglePutting = true;

            }

            if (keyboardState.IsKeyDown(Keys.C))
            {
                isPutting = false;
                wasPutting = false;
                currentAnimationIndexPuttingRight = 0;
                currentAnimationIndexPuttingLeft = 5;
            }

            if (myKeyboard.HasBeenPressed(Keys.Q) && !isPutting && !wasPutting)
            {
                if (hittingMode == 0) { hittingMode = 1; }
                else { hittingMode = 0; }
            }

            playerHitbox.X += (int)movement;


        }

        public bool getIsPutting()
        {
            return isPutting; 
        }

        public Vector2 getPosition()
        {
            return new Vector2(playerHitbox.X, playerHitbox.Y);
        }

        public float getMovement()
        {
            return movement;
        }

        public void setPlayerPosition(Vector2 playerPosition)
        {
            playerHitbox.X = (int)playerPosition.X;
            playerHitbox.Y = (int)playerPosition.Y;
        }

        public bool getWasPutting()
        {
            return wasPutting;
        }

        public int getFacing()
        {
            return facing;
        }

        public int getHittingMode()
        {
            return hittingMode;
        }

        public bool getAnglePutting()
        {
            return anglePutting;
        }

        public class myKeyboard
        {
            static KeyboardState currentKeyState;
            static KeyboardState previousKeyState;

            public static KeyboardState GetState()
            {
                previousKeyState = currentKeyState;
                currentKeyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
                return currentKeyState;
            }

            public static bool HasBeenPressed(Keys key)
            {
                return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
            }
        }

    }

}