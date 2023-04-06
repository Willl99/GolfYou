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
        Texture2D driveRightSet;
        Texture2D driveLeftSet;

        Rectangle[] sourceRectanglesRun;
        Rectangle[] sourceRectanglesPutting;
        Rectangle playerHitbox = new Rectangle(0, 200, 32, 32);

        byte currentAnimationIndexRun;
        byte currentAnimationIndexPutting;
        private int threshold;
        float timer;
        float inputTimer;
        private float movement = 0.0f;
        private int facing = 1;
        private bool rolling;
        private bool isPutting;
        private bool wasPutting;
        private bool spaceWasPressed;
        private int hittingMode; // 0 = drive, 1 = putt

        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        private const float MaxJumpTime = .35f;
        private const float GravityAcceleration = 3400.0f;
        private const float JumpLaunchVelocity = -3500.0f;
        private const float MaxFallSpeed = 550.0f;
        private const float JumpControlPower = 0.14f;

        private const Buttons puttButton = Buttons.A;

        public void loadPlayerContent(ContentManager Content)
        {
            runRightSet = Content.Load<Texture2D>("Sprites/WalkanimFlipped");
            runLeftSet = Content.Load<Texture2D>("Sprites/Walkanim");
            puttingRightSet = Content.Load<Texture2D>("Sprites/PlayerPuttingAll");

            currentAnimationIndexRun = 0;
            rolling = false;
            timer = 0;
            inputTimer = 0;
            threshold = 10;
            isPutting = false;
            spaceWasPressed = false;

            sourceRectanglesRun = new Rectangle[4];
            for (int i = 0; i < 4; i++)
            {
                sourceRectanglesRun[i] = new Rectangle(i * 16, 0, 16, 16);
            }
            sourceRectanglesPutting = new Rectangle[6];
            for (int i = 0; i < 6; i++)
            {
                sourceRectanglesPutting[i] = new Rectangle(i * 64 + 23, 24, 16, 20);
            }
        }

        public void drawPlayer(SpriteBatch _spriteBatch, GameTime gameTime)
        {
            if (facing == 1 && movement > 0 && !isPutting && !wasPutting)
            {
                _spriteBatch.Draw(runRightSet, playerHitbox, sourceRectanglesRun[currentAnimationIndexRun], Color.White);
            }
            else if (facing == 1 && movement == 0 && !isPutting && !wasPutting)
            {
                _spriteBatch.Draw(runRightSet, playerHitbox, sourceRectanglesRun[1], Color.White);
            }
            else if (facing == 1 && isPutting || facing == 1 && wasPutting)
            {
                _spriteBatch.Draw(puttingRightSet, new Rectangle(playerHitbox.X, playerHitbox.Y, 32, 40), sourceRectanglesPutting[currentAnimationIndexPutting], Color.White);
            }
            else if (facing == 0 && movement < 0)
            {
                _spriteBatch.Draw(runLeftSet, playerHitbox, sourceRectanglesRun[currentAnimationIndexRun], Color.White);
            }
            else if (facing == 0 && movement == 0)
            {
                _spriteBatch.Draw(runLeftSet, playerHitbox, sourceRectanglesRun[0], Color.White);
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
            else if (facing == 1 && isPutting)
            {
                threshold = 100;
                if (timer > threshold)
                {
                    if (currentAnimationIndexPutting < 5)
                    {
                        currentAnimationIndexPutting++;
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
                    if (currentAnimationIndexPutting > 0)
                    {
                        currentAnimationIndexPutting--;
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
                keyboardState.IsKeyDown(Keys.A) && !isPutting && !wasPutting)
            {
                facing = 0;
                movement = -1.0f;
            }
            else if (gamePadState.IsButtonDown(Buttons.DPadRight) ||
                     keyboardState.IsKeyDown(Keys.Right) ||
                     keyboardState.IsKeyDown(Keys.D) && !isPutting && !wasPutting)
            {
                facing = 1;
                movement = 1.0f;
            }

            // Check if the player wants to putt.
            myKeyboard.GetState();
            if (myKeyboard.HasBeenPressed(Keys.Space) && isPutting)
            {
                isPutting = false;
                wasPutting = true;

            }
            else if (myKeyboard.HasBeenPressed(Keys.Space) && rolling == false && !isPutting)
            {
                isPutting = true;

            }

            if (keyboardState.IsKeyDown(Keys.C))
            {
                isPutting = false;
                wasPutting = false;
                currentAnimationIndexPutting = 0;
            }

            playerHitbox.X += (int)movement;


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