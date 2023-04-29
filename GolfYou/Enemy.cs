using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TiledCS;

namespace GolfYou
{
    public class Enemy //Enemy class
    {
        bool idle;
        Rectangle sourceRectangle;
        Rectangle enemyHitBox = new Rectangle(0, 200, 32, 32);
        int[] walkingAnimation = {6, 7, 8, 9, 10, 11}; // Frames when enemy is moving
        int[] idleAnimation = {0, 1, 2, 3, 4, 5}; // Frames when enemy is idle
        Texture2D left;
        Texture2D right;
        Vector2 position;
        bool forward; // Determines whether enemy is facing left or right
        int frame;
        bool incFrame;
        Rectangle hitBox;
        int half = 0;
        int halfcap = 7; // Slows down animations with larger numbers (non-fixable fps, might want to change later to account for gameTime)

         public Enemy(ContentManager Content, bool stationary, Vector2 pos)
        {
            idle = stationary;
            forward = true;
            frame = 0;
            incFrame = true;

            left = Content.Load<Texture2D>("Sprites/EnemyLeft");
            right = Content.Load<Texture2D>("Sprites/EnemyRight");
            position = pos;
            hitBox = new Rectangle((int)pos.X, (int)pos.Y, 12, 12);
        }

        public void updateEnemy()
        {
            half++;
            if (half==halfcap)
            {
                half = 0;
                if (incFrame)
                {
                    frame++;
                    if ((idle & frame==idleAnimation.Length-1) | (!idle & frame==walkingAnimation.Length-1)) incFrame = false;
                }
                else
                {
                    frame--;
                    if (frame==0) incFrame = true;
                }
            }
        }

        public void drawEnemy(SpriteBatch _spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 24*2, 24*2);
            if (idle) sourceRectangle = getAnimFrame(idleAnimation[frame]);
            else sourceRectangle = getAnimFrame(walkingAnimation[frame]);
            if (idle || forward) _spriteBatch.Draw(right, destinationRectangle, sourceRectangle, Color.White);
            else _spriteBatch.Draw(left, destinationRectangle, sourceRectangle, Color.White);
        }

        public void drawHitBoxes(SpriteBatch _spriteBatch)
        {
            sourceRectangle = getAnimFrame(frame);
            DrawRectangle(_spriteBatch, hitBox);
        }

        private Rectangle getAnimFrame(int i) // Determines what section of the image source to take from
        {
            return new Rectangle(10, 48*(i+1)-40, 24, 24);
        }

        private void DrawRectangle(SpriteBatch sb, Rectangle Rec)
        {
            Vector2 pos = new Vector2(Rec.X, Rec.Y);
            sb.Draw(left, pos, Rec,
                Color.Green * 1.0f,
                0, Vector2.Zero, 1.0f,
                SpriteEffects.None, 0.00001f);
        }

        public Rectangle getHitBox()
        {
            return hitBox;
        }

        public int getFrame()
        {
            return frame;
        }

        public bool getIdle()
        {
            return idle;
        }

    }
}