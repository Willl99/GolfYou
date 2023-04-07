using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GolfYou
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
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
			// TODO: Add your initialization logic here

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			myPlayer.loadPlayerContent(this.Content);
			myHUD.loadHudContent(this.Content);
			// TODO: use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// TODO: Add your update logic here
			myPlayer.playAnimation(gameTime);
			myPlayer.handlePlayerInput(Keyboard.GetState(), GamePad.GetState(PlayerIndex.One), gameTime);
            myHUD.playHudAnimations(gameTime, myPlayer.getIsPutting(), myPlayer.rolling, myPlayer.getAnglePutting(), myPlayer.getFacing());
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
			// TODO: Add your drawing code here
			_spriteBatch.End();
			base.Draw(gameTime);
		}

	}
}