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
		private Menu myMenu = new Menu();

		private string[] levels = {"LevelOne.tmx", "LevelTwo.tmx"};
		int levelCounter = 0;

		private Texture2D startMenuSprites;
        public static int ScreenHeight;
		public static int ScreenWidth;
		public static bool levelEnd;
		public static bool loadMainMenu;
		public static bool startMenu = true;
		public static bool startButtonPressed;
		public static bool loadControlMenu;
		public static bool controlButtonPressed;


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
			myMenu.loadMenus(this.Content);
			myPlayer.loadPlayerContent(this.Content);
			myHUD.loadHudContent(this.Content);
			startButtonPressed = false;
			loadMainMenu = false;
			controlButtonPressed = false;
			levelEnd = false;

    }

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

            // TODO: Add your update logic here
			
            MouseState mouseState = Mouse.GetState();
			if(startMenu)
			{
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    // Check for intersection
                    if (myMenu.didPressStart(mouseState))
                    {
                        startButtonPressed = true;

                    }
                    if (myMenu.didPressControls(mouseState))
                    {
                        controlButtonPressed = true;
                        startMenu = false;
                    }
                }
            }
            

			if(controlButtonPressed)
			{
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (myMenu.controlDidPressExitToStart(mouseState))
                    {
						controlButtonPressed = false;
                        startMenu = true;
                    }
                }
            }

			// if start pressed, start loading level and the game
			if (startButtonPressed && !levelEnd)
			{
				if (startMenu) { LoadLevel(); }
				startMenu = false;
				

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
					if (myMenu.didPressExitToStart(mouseState))
					{
						levelEnd = false;
						controlButtonPressed = false;
						startButtonPressed= false;
						startMenu = true;
						levelCounter = 0;
					}
					else { LoadLevel(); }
				}
			}

            base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);
			
            if(startMenu)
			{
                _spriteBatch.Begin();
				myMenu.drawStartMenu(_spriteBatch);
            }
            else if (startButtonPressed && !levelEnd)
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: myCamera.Transform);
                levelManager.drawLevel(_spriteBatch);
                myPlayer.drawPlayer(_spriteBatch, gameTime, myPhysics.getVelocity());
                myHUD.drawHudContent(_spriteBatch, myPlayer.getHittingMode(), myPlayer.getIsPutting(), myPlayer.getWasPutting(), myPlayer.getPosition(), myPlayer.getAnglePutting(), myPlayer.getFacing());


            }
			else if (controlButtonPressed)
			{
				_spriteBatch.Begin();
				myMenu.drawControlMenu(_spriteBatch);
			}

			else if(levelEnd)
			{
                _spriteBatch.Begin();
				myMenu.drawLevelEndMenu(_spriteBatch);
            }

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