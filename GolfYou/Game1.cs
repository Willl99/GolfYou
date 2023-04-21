using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace GolfYou
{
	public class Game1 : Game //Main game loop, really its just a container to call functions from other classes.
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		//Class instances are declared here
		private Player myPlayer = new Player();
		private PlayerPhysics myPhysics;
		private HUD myHUD = new HUD();
		private Level levelManager = new Level();
		private Camera myCamera = new Camera();
		private string[] levels = {"GolfYouScrollingTest.tmx", "LevelTwoTest.tmx"};
		int levelCounter = 0;

		private Texture2D startMenuSprites;
        public static int ScreenHeight;
		public static int ScreenWidth;
		public static bool levelEnd = true;
		public static bool loadMainMenu;
		public static bool startMenu;
		public static bool startButtonPressed;
		private Microsoft.Xna.Framework.Rectangle startMenuHitbox = new Microsoft.Xna.Framework.Rectangle(360, 200, 27, 16);
        private Microsoft.Xna.Framework.Rectangle exitToStartMenuHitbox = new Microsoft.Xna.Framework.Rectangle(370, 250, 22, 11);
        private Texture2D pixel;
		private Texture2D font;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			ScreenHeight = _graphics.PreferredBackBufferHeight;
			ScreenWidth = _graphics.PreferredBackBufferWidth;
			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
            startMenuSprites = Content.Load<Texture2D>("Levels/LevelMaterials/GUI");
			myPlayer.loadPlayerContent(this.Content);
			myHUD.loadHudContent(this.Content);
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Microsoft.Xna.Framework.Color>(new Microsoft.Xna.Framework.Color[] { Microsoft.Xna.Framework.Color.White });
			startButtonPressed = false;
			loadMainMenu = false;
			startMenu = false;

    }

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

            // TODO: Add your update logic here
			
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
				// Check for intersection
                if (startMenuHitbox.Contains(mouseState.X, mouseState.Y))
                {
					startButtonPressed = true;

                }
            }

			// if start pressed, start loading level and the game
			if (startButtonPressed)
			{
				startMenu = false;
				LoadLevel();

                myPlayer.playAnimation(gameTime);
                myPlayer.handlePlayerInput(Keyboard.GetState(), GamePad.GetState(PlayerIndex.One), gameTime);
                myCamera.Follow(myPlayer.getPlayerHitbox(), levelManager.getMapBounds());
                myHUD.playHudAnimations(gameTime, myPlayer.getIsPutting(), myPlayer.rolling, myPlayer.getAnglePutting(), myPlayer.getFacing()); //HUD MUST be drawn before physics as the physics relies on calculations done in the HUD class,
                                                                                                                                                //weird I know, but it was an easy solution
                myPlayer.setPlayerPosition(myPhysics.ApplyPhysics(gameTime, Window.ClientBounds.Height, Window.ClientBounds.Width, ref myPlayer.rolling, myPlayer.getPlayerHitbox(),
                myPlayer.getMovement(), myPlayer.getWasPutting(), myPlayer.getFacing(), myPlayer.getHittingMode(), myHUD.getVelModifier(), myHUD.getAngle(), levelManager.getCollisionLayer()));
                levelManager.endCurLevel(myPlayer.getPlayerHitbox());
            }

			// if level end, press exit to main menu, go back and load main

			if (levelEnd)
			{
				if (mouseState.LeftButton == ButtonState.Pressed)
				{
					if (exitToStartMenuHitbox.Contains(mouseState.X, mouseState.Y))
					{
						levelEnd = false;
						startMenu = true;
					}
				}
			}
			/*
			if (levelEnd)
			{
				LoadLevel();
                
            }

			myPlayer.playAnimation(gameTime);
			myPlayer.handlePlayerInput(Keyboard.GetState(), GamePad.GetState(PlayerIndex.One), gameTime);
			myCamera.Follow(myPlayer.getPlayerHitbox(), levelManager.getMapBounds());
            myHUD.playHudAnimations(gameTime, myPlayer.getIsPutting(), myPlayer.rolling, myPlayer.getAnglePutting(), myPlayer.getFacing()); //HUD MUST be drawn before physics as the physics relies on calculations done in the HUD class,
																																			//weird I know, but it was an easy solution
            myPlayer.setPlayerPosition(myPhysics.ApplyPhysics(gameTime, Window.ClientBounds.Height, Window.ClientBounds.Width, ref myPlayer.rolling, myPlayer.getPlayerHitbox(),
			myPlayer.getMovement(), myPlayer.getWasPutting(), myPlayer.getFacing(), myPlayer.getHittingMode(), myHUD.getVelModifier(), myHUD.getAngle(), levelManager.getCollisionLayer()));
			levelManager.endCurLevel(myPlayer.getPlayerHitbox());
//			Debug.WriteLine(myPhysics.getVelocity());
			*/

            base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);
			_spriteBatch.Begin();
            if(startMenu)
			{
                // Draw the text to the screen
                _spriteBatch.Draw(startMenuSprites, new Vector2(350, 150), new Microsoft.Xna.Framework.Rectangle(146, 66, 46, 13), Microsoft.Xna.Framework.Color.White);
                _spriteBatch.Draw(startMenuSprites, new Vector2(360, 200), new Microsoft.Xna.Framework.Rectangle(67, 148, 27, 16), Microsoft.Xna.Framework.Color.White);
				
            }
            if (startButtonPressed)
            {
                levelManager.drawLevel(_spriteBatch);
                myPlayer.drawPlayer(_spriteBatch, gameTime, myPhysics.getVelocity());
                myHUD.drawHudContent(_spriteBatch, myPlayer.getHittingMode(), myPlayer.getIsPutting(), myPlayer.getWasPutting(), myPlayer.getPosition(), myPlayer.getAnglePutting(), myPlayer.getFacing());

            }

			if(levelEnd)
			{
                _spriteBatch.Draw(startMenuSprites, new Vector2(360, 150), new Microsoft.Xna.Framework.Rectangle(145, 113, 47, 15), Microsoft.Xna.Framework.Color.White);
                _spriteBatch.Draw(startMenuSprites, new Vector2(380, 200), new Microsoft.Xna.Framework.Rectangle(114, 114, 31, 15), Microsoft.Xna.Framework.Color.White);
				_spriteBatch.Draw(startMenuSprites, new Vector2(370, 250), new Microsoft.Xna.Framework.Rectangle(7, 164, 22, 11), Microsoft.Xna.Framework.Color.White);
            }



            /*
            levelManager.drawLevel(_spriteBatch);
            myPlayer.drawPlayer(_spriteBatch, gameTime, myPhysics.getVelocity());
            myHUD.drawHudContent(_spriteBatch, myPlayer.getHittingMode(), myPlayer.getIsPutting(), myPlayer.getWasPutting(), myPlayer.getPosition(), myPlayer.getAnglePutting(), myPlayer.getFacing());
			*/

            _spriteBatch.End();
			
			base.Draw(gameTime);
		}

		private void LoadLevel()
		{
			myPhysics = new PlayerPhysics();
            levelManager.loadLevel(this.Content, levels[levelCounter]);
            myPlayer.setSpawnLocation(levelManager.getPlayerSpawnLocation());
            levelEnd = false;
            levelCounter++;
        }
	}
}