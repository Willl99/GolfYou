using Microsoft.Xna.Framework;
using System;
using TiledCS;

namespace GolfYou
{
    public class EnemyPhysics // Enemy physics class, much of this is lifted from my (Will's) Levels 1-3 implementations
    {
        private bool IsOnGround;
        private float timer = 0.0f;

        public Vector2 ApplyPhysics(GameTime gameTime, int windowHeight, int windowWidth, Rectangle enemy, TiledLayer collisionLayer, bool idle) // Simplified from PlayerPhysics
        {
            Vector2 enemyPosition = new Vector2(enemy.X, enemy.Y);
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = enemyPosition;

            // Apply position change if enemy is a "moving" enemy
            if (!idle)
            {
               enemyPosition += new Vector2(1, 1) * elapsed;
               enemyPosition = new Vector2((float)Math.Round(enemyPosition.X), (float)Math.Round(enemyPosition.Y));
            }

            // If the player is now colliding with the level, separate them.
            //playerPosition.X = MathHelper.Clamp(playerPosition.X, 0, windowWidth - 60);
            //playerPosition.Y = MathHelper.Clamp(playerPosition.Y, 0, windowHeight);
            enemy.X = (int)enemyPosition.X;
            enemy.Y = (int)enemyPosition.Y;
            enemyPosition = HandleCollisions(gameTime, enemy, collisionLayer);

            IsOnGround = (int)enemyPosition.Y-enemy.Y == 0;
            return enemyPosition;
        }

        public Vector2 HandleCollisions(GameTime gameTime, Rectangle enemy, TiledLayer collisionLayer)
        {

            foreach (var obj in collisionLayer.objects)
            {
                var objRect = new Rectangle((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height);
                bool intersectBottom = objRect.Bottom - enemy.Top < 30 && enemy.X >= objRect.Left && enemy.X <= objRect.Right;
                bool intersectLeft = enemy.Right - objRect.Left < 30 && enemy.Y <= objRect.Bottom && enemy.Y >= objRect.Top;
                bool intersectRight = objRect.Right - enemy.Left < 30 && enemy.Y <= objRect.Bottom && enemy.Y >= objRect.Top;

                if (intersectBottom) 
                {
                    enemy.Y = MathHelper.Clamp(enemy.Y, objRect.Bottom, 1000000);
                } 
                
                if (intersectLeft) 
                {
                    enemy.X = MathHelper.Clamp(enemy.X, 0, objRect.Left - 32);
                }
                if(intersectRight)
                {
                    enemy.X = MathHelper.Clamp(enemy.X, objRect.Right, 10000);
                }
            }
            return new Vector2(enemy.X, enemy.Y);
        }

    }

    }
