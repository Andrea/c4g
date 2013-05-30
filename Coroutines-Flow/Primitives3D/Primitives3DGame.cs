using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives3D
{
	public class Primitives3DGame : Game
	{
		GraphicsDeviceManager _graphics;
		SpriteBatch _spriteBatch;
		SpriteFont _spriteFont;		
		private Renderer _renderer;
		private World _world;
		
		public Primitives3DGame()
		{
			Content.RootDirectory = "Content";
			_graphics = new GraphicsDeviceManager(this) { SynchronizeWithVerticalRetrace = false, PreferMultiSampling = true };
			IsFixedTimeStep = false;
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			base.Initialize();
			_renderer = new Renderer(GraphicsDevice);
			_world = new World(_renderer);
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			_spriteFont = Content.Load<SpriteFont>("hudfont");

			_wireFrameState = new RasterizerState
			{
				FillMode = FillMode.WireFrame,
				CullMode = CullMode.None,
			};
		}

		protected override void Update(GameTime gameTime)
		{
			_world.Update(gameTime.ElapsedGameTime.Milliseconds);
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			GraphicsDevice.RasterizerState = _isWireframe ? _wireFrameState : RasterizerState.CullCounterClockwise;

			var cameraPosition = new Vector3(0, 0, 2.5f);
			var aspect = GraphicsDevice.Viewport.AspectRatio;

			var view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
			var projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 1000);

			_renderer.Draw(view, projection);

			// Reset the fill mode renderstate.
			GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

			_spriteBatch.Begin();
			Profiler.DrawStats(_spriteBatch, _spriteFont);
			_spriteBatch.End();

			Profiler.Clear();

			base.Draw(gameTime);
		}
	}

	static class Program
	{
		static void Main()
		{
			using (var game = new Primitives3DGame())
			{
				game.Run();

			}
		}
	}
}
