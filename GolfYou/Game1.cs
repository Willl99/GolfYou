using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

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

        public static int ScreenHeight;
		public static int ScreenWidth;
		public static bool levelEnd = true;

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
			myPlayer.loadPlayerContent(this.Content);
			myHUD.loadHudContent(this.Content);
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// TODO: Add your update logic here
			
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



			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			//GraphicsDevice.Clear(Color.CornflowerBlue);
			_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: myCamera.Transform);
            levelManager.drawLevel(_spriteBatch);
            myPlayer.drawPlayer(_spriteBatch, gameTime, myPhysics.getVelocity());
			myHUD.drawHudContent(_spriteBatch, myPlayer.getHittingMode(), myPlayer.getIsPutting(), myPlayer.getWasPutting(), myPlayer.getPosition(), myPlayer.getAnglePutting(), myPlayer.getFacing());
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