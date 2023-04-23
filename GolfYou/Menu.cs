using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;

namespace GolfYou
{
	public class Menu
	{
        private Texture2D startMenuSprites;
        private Texture2D mainMenu;
        private SpriteFont font;
        private Microsoft.Xna.Framework.Rectangle startMenuHitbox = new Microsoft.Xna.Framework.Rectangle(350, 150, 100, 50);
        private Microsoft.Xna.Framework.Rectangle controlMenuHitbox = new Microsoft.Xna.Framework.Rectangle(350, 200, 100, 50);
        private Microsoft.Xna.Framework.Rectangle controlsExitToStartMenuHitbox = new Microsoft.Xna.Framework.Rectangle(350, 250, 100, 50);
        private Microsoft.Xna.Framework.Rectangle exitToStartMenuHitbox = new Microsoft.Xna.Framework.Rectangle(350, 200, 100, 50);

        public void loadMenus(ContentManager Content)
        {
            startMenuSprites = Content.Load<Texture2D>("Levels/LevelMaterials/GUI");
            font = Content.Load<SpriteFont>("MenuText");
        }

        public bool didPressStart(MouseState mouseState)
        {
            if(startMenuHitbox.Contains(mouseState.X, mouseState.Y))
            {
                return true;
            }
            return false;
        }

        public bool didPressControls(MouseState mouseState)
        {
            if (controlMenuHitbox.Contains(mouseState.X, mouseState.Y))
            {
                return true;
            }
            return false;
        }

        public bool controlDidPressExitToStart(MouseState mouseState)
        {
            if (controlsExitToStartMenuHitbox.Contains(mouseState.X, mouseState.Y))
            {
                return true;
            }
            return false;
        }

        public bool didPressExitToStart(MouseState mouseState)
        {
            if (exitToStartMenuHitbox.Contains(mouseState.X, mouseState.Y))
            {
                return true;
            }
            return false;
        }

        public void drawStartMenu(SpriteBatch _spriteBatch)
        {
            _spriteBatch.DrawString(font, "Golf You!", new Vector2(350, 100), Microsoft.Xna.Framework.Color.Black);
            _spriteBatch.DrawString(font, "Start", new Vector2(350, 150), Microsoft.Xna.Framework.Color.White);
            _spriteBatch.DrawString(font, "Controls", new Vector2(350, 200), Microsoft.Xna.Framework.Color.White);
            /*
            _spriteBatch.Draw(startMenuSprites, new Vector2(350, 150), new Microsoft.Xna.Framework.Rectangle(146, 66, 46, 13), Microsoft.Xna.Framework.Color.White);
            _spriteBatch.Draw(startMenuSprites, new Vector2(360, 200), new Microsoft.Xna.Framework.Rectangle(67, 148, 27, 16), Microsoft.Xna.Framework.Color.White);
            */
        }

        public void drawControlMenu(SpriteBatch _spriteBatch)
        {
            _spriteBatch.DrawString(font, "Controls", new Vector2(350, 100), Microsoft.Xna.Framework.Color.Black);
            _spriteBatch.DrawString(font, "A: Move Left", new Vector2(200, 150), Microsoft.Xna.Framework.Color.Black);
            _spriteBatch.DrawString(font, "D: Move Right", new Vector2(200, 175), Microsoft.Xna.Framework.Color.Black);
            _spriteBatch.DrawString(font, "Space to enter putting mode, Space again to choose angle, Space again to choose velocity", new Vector2(100, 200), Microsoft.Xna.Framework.Color.Black);
            _spriteBatch.DrawString(font, "C to cancel out of putting mode, Q to change putting mode", new Vector2(150, 225), Microsoft.Xna.Framework.Color.Black);
            _spriteBatch.DrawString(font, "Exit to Main Menu", new Vector2(350, 250), Microsoft.Xna.Framework.Color.White);
            
        }

        public void drawLevelEndMenu(SpriteBatch _spriteBatch)
        {
            _spriteBatch.DrawString(font, "Level Over", new Vector2(350, 100), Microsoft.Xna.Framework.Color.Black);
            _spriteBatch.DrawString(font, "Next Level", new Vector2(350, 150), Microsoft.Xna.Framework.Color.White);
            _spriteBatch.DrawString(font, "Main Menu", new Vector2(350, 200), Microsoft.Xna.Framework.Color.White);
            /*
             * _spriteBatch.Draw(startMenuSprites, new Vector2(360, 150), new Microsoft.Xna.Framework.Rectangle(145, 113, 47, 15), Microsoft.Xna.Framework.Color.White);
            _spriteBatch.Draw(startMenuSprites, new Vector2(380, 200), new Microsoft.Xna.Framework.Rectangle(114, 114, 31, 15), Microsoft.Xna.Framework.Color.White);
            _spriteBatch.Draw(startMenuSprites, new Vector2(370, 250), new Microsoft.Xna.Framework.Rectangle(7, 164, 22, 11), Microsoft.Xna.Framework.Color.White);
            */
        }
    }
}

