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
        Texture2D driveRightSet;
        Texture2D driveLeftSet;

        Rectangle[] sourceRectanglesRun;
        Rectangle playerHitbox = new Rectangle(0,200,32,32);

        byte currentAnimationIndexRun;

        private int threshold;
        float timer;
        private float movement = 0.0f;
        private int facing = 1;
        private bool isAttacking;
        private bool rolling;

        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        private const float MaxJumpTime = .35f;
        private const float GravityAcceleration = 3400.0f;
        private const float JumpLaunchVelocity = -3500.0f;
        private const float MaxFallSpeed = 550.0f;
        private const float JumpControlPower = 0.14f;


        public void loadPlayerContent(ContentManager Content)
        {
            runRightSet = Content.Load<Texture2D>("Sprites/WalkanimFlipped");
            runLeftSet = Content.Load<Texture2D>("Sprites/Walkanim");

            currentAnimationIndexRun = 0;
            isAttacking= false;
            rolling = false;
            timer = 0;
            threshold = 10;


            sourceRectanglesRun = new Rectangle[4];
            for (int i = 0; i < 4; i++)
            {
                sourceRectanglesRun[i] = new Rectangle(i * 16, 0, 16, 16);
            }
        }

        public void drawPlayer(SpriteBatch _spriteBatch, GameTime gameTime)
        {
            //Debug.WriteLine(currentAnimationIndexRun);
            if (facing == 1 && movement > 0)
            {
                _spriteBatch.Draw(runRightSet, playerHitbox, sourceRectanglesRun[currentAnimationIndexRun], Color.White);
                
            }
            else if (facing == 1 && movement == 0)
            {
                _spriteBatch.Draw(runRightSet, playerHitbox, sourceRectanglesRun[1], Color.White);
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
            if (facing == 1)
            {
                threshold = 150;
                if (timer > threshold)
                {
                    if (currentAnimationIndexRun < 3)
                    {
                        currentAnimationIndexRun++;
                        Debug.WriteLine(currentAnimationIndexRun.ToString());
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
            if (facing == 0)
            {
                threshold = 150;
                if (timer > threshold)
                {
                    if (currentAnimationIndexRun > 0)
                    {
                        currentAnimationIndexRun--;
                        Debug.WriteLine(currentAnimationIndexRun.ToString());
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
        }

        public void handlePlayerInput(
                    KeyboardState keyboardState,
                    GamePadState gamePadState)
        {
            // Get analog horizontal movement.
            movement = gamePadState.ThumbSticks.Left.X;

            // Ignore small movements to prevent running in place.
            if (Math.Abs(movement) < 0.5f)
                movement = 0.0f;

            // If any digital horizontal movement input is found, override the analog movement.
            if (gamePadState.IsButtonDown(Buttons.DPadLeft) ||
                keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.A))
            {
                facing = 0;
                movement = -1.0f;
            }
            else if (gamePadState.IsButtonDown(Buttons.DPadRight) ||
                     keyboardState.IsKeyDown(Keys.Right) ||
                     keyboardState.IsKeyDown(Keys.D))
            {
                facing = 1;
                movement = 1.0f;
            }

            // Check if the player wants to jump.
            /*isJumping =
                gamePadState.IsButtonDown(JumpButton) ||
                keyboardState.IsKeyDown(Keys.Up) ||
                keyboardState.IsKeyDown(Keys.W);

            if (keyboardState.IsKeyDown(Keys.Space) && IsOnGround)
            {
                isAttacking = true;
            } */

            playerHitbox.X += (int)movement;


        }

    }
}