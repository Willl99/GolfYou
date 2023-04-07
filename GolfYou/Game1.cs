using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GolfYou
{
	public class Game1 : Game //Main game loop, really its just a container to call functions from other classes.
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		//Class instances are declared here
		Player myPlayer = new Player();
		PlayerPhysics myPhysics = new PlayerPhysics();
		HUD myHUD = new HUD();

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{

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
			myPlayer.playAnimation(gameTime);
			myPlayer.handlePlayerInput(Keyboard.GetState(), GamePad.GetState(PlayerIndex.One), gameTime);
            myHUD.playHudAnimations(gameTime, myPlayer.getIsPutting(), myPlayer.rolling, myPlayer.getAnglePutting(), myPlayer.getFacing()); //HUD MUST be drawn before physics as the physics relies on calculations done in the HUD class,
																																			//weird I know, but it was an easy solution
            myPlayer.setPlayerPosition(myPhysics.ApplyPhysics(gameTime, Window.ClientBounds.Height, Window.ClientBounds.Width, ref myPlayer.rolling, myPlayer.getPosition(),
				myPlayer.getMovement(), myPlayer.getWasPutting(), myPlayer.getFacing(), myPlayer.getHittingMode(), myHUD.getVelModifier(), myHUD.getAngle()));



			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

			myPlayer.drawPlayer(_spriteBatch, gameTime, myPhysics.getVelocity());
			myHUD.drawHudContent(_spriteBatch, myPlayer.getHittingMode(), myPlayer.getIsPutting(), myPlayer.getWasPutting(), myPlayer.getPosition(), myPlayer.getAnglePutting(), myPlayer.getFacing());
			_spriteBatch.End();
			base.Draw(gameTime);
		}

	}
}