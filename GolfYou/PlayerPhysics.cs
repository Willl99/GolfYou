using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System;
using System.Linq;

namespace GolfYou
{
    public class PlayerPhysics
    {
        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.98f;
        private bool IsOnGround;
        private float prevYVelocity = 0.0f;
        private bool prevWasPutting = false;
        private float timer = 0.0f;
        Vector2 velocity;

        private const float MaxJumpTime = .35f;
        private const float JumpLaunchVelocity = -3500.0f;
        private const float GravityAcceleration = 3400.0f;
        private const float MaxFallSpeed = 550.0f;
        private const float JumpControlPower = 0.14f;



        public PlayerPhysics()
        {
            IsOnGround = false;
        }

        public Vector2 ApplyPhysics(GameTime gameTime, int windowHeight, int windowWidth, ref bool isRolling, Vector2 playerPosition, float movement, bool wasPutting)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = playerPosition;

            // Base velocity is a combination of horizontal movement control and
            // acceleration downward due to gravity.
            velocity.X += movement * MoveAcceleration * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);

            velocity = DoDrive(velocity, gameTime, ref isRolling, wasPutting);

            // Apply pseudo-drag horizontally.
            if (IsOnGround)
                velocity.X *= GroundDragFactor;
            else
                velocity.X *= AirDragFactor;

            // Prevent the player from running faster than his top speed.            
            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            // Apply velocity.
            playerPosition += velocity * elapsed;
            playerPosition = new Vector2((float)Math.Round(playerPosition.X), (float)Math.Round(playerPosition.Y));

            // If the player is now colliding with the level, separate them.
            playerPosition.X = MathHelper.Clamp(playerPosition.X, 0, windowWidth - 60);
            playerPosition.Y = MathHelper.Clamp(playerPosition.Y, 0, windowHeight - 100);
            //HandleCollisions();

            // If the collision stopped us from moving, reset the velocity to zero.
            if (playerPosition.X == previousPosition.X)
                velocity.X = 0;

            if (playerPosition.Y == previousPosition.Y)
                velocity.Y = 0;



            if (velocity.Y == 0)
            {
                IsOnGround = true;
            }
            else
            {
                IsOnGround = false;
            }
            prevYVelocity = velocity.Y;
            return playerPosition;
        }
        private Vector2 DoDrive(Vector2 velocity, GameTime gameTime, ref bool isRolling, bool wasPutting)
        {
            int threshold = 3;
            
            if (isRolling && timer > threshold)
            {
                GroundDragFactor = .96f;
                if (velocity.X <= 5) 
                { 
                    isRolling = false;
                    GroundDragFactor = .48f;
                    timer = 0;
                }
            }
            else if (isRolling && timer < threshold) 
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (prevWasPutting && !wasPutting)
            { 
                velocity.Y = -1000;
                velocity.X = 2000;
                prevWasPutting = wasPutting;
                return new Vector2(velocity.X, velocity.Y);
            }
            else if (prevWasPutting && wasPutting || !prevWasPutting && !wasPutting || !prevWasPutting && wasPutting) { }
            {
                prevWasPutting = wasPutting;
                return velocity;

            }




        }
    }
}
