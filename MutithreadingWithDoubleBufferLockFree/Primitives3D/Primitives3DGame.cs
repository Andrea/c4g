using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Primitives3D
{
	public class Primitives3DGame : Game
	{
		private const int NumPrimitiveObjects = 100;
		GraphicsDeviceManager _graphics;

		SpriteBatch _spriteBatch;
		SpriteFont _spriteFont;

		KeyboardState _currentKeyboardState;
		KeyboardState _lastKeyboardState;
		GamePadState _currentGamePadState;
		GamePadState _lastGamePadState;
		MouseState _currentMouseState;
		MouseState _lastMouseState;

		// Store a list of primitive models, plus which one is currently selected.
		readonly List<GeometricPrimitive> _primitives = new List<GeometricPrimitive>();

		int _currentPrimitiveIndex = 0;

		// store a wireframe rasterize state
		RasterizerState _wireFrameState;

		// Store a list of tint colors, plus which one is currently selected.
		List<Color> colors = new List<Color>
        {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.White,
            Color.Black,
        };

		int _currentColorIndex = 0;

		// Are we rendering in wireframe mode?
		bool _isWireframe;
		
		private Renderer _renderer;
		

		public Primitives3DGame()
		{
			Content.RootDirectory = "Content";
			_graphics = new GraphicsDeviceManager(this) { SynchronizeWithVerticalRetrace = false, PreferMultiSampling = true};
			IsFixedTimeStep = false;
			IsMouseVisible = true;
		}
		protected override void Initialize()
		{
			base.Initialize();
			_renderer = new Renderer(GraphicsDevice);
			Task.Factory.StartNew(() =>
			{
				var gl = new UpdateLoop(_renderer);
				gl.Loop();
			});
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			_spriteFont = Content.Load<SpriteFont>("hudfont");

			var random = new Random(Environment.TickCount);
			for (int i = 0; i < NumPrimitiveObjects; i++)
			{
				var primitive = new CubePrimitive(GraphicsDevice)
					{
						Position = new Vector3(random.Next(100) - 50, random.Next(100) - 50, -random.Next(100))
					};
				_primitives.Add(primitive);
			}

			_wireFrameState = new RasterizerState
			{
				FillMode = FillMode.WireFrame,
				CullMode = CullMode.None,
			};
		}

		protected override void Update(GameTime gameTime)
		{
			HandleInput();
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

		void HandleInput()
		{
			_lastKeyboardState = _currentKeyboardState;
			_lastGamePadState = _currentGamePadState;
			_lastMouseState = _currentMouseState;

			_currentKeyboardState = Keyboard.GetState();
			_currentGamePadState = GamePad.GetState(PlayerIndex.One);
			_currentMouseState = Mouse.GetState();

			// Check for exit.
			if (IsPressed(Keys.Escape, Buttons.Back))
			{
				Exit();
			}

			// Change primitive?
			Viewport viewport = GraphicsDevice.Viewport;
			int halfWidth = viewport.Width / 2;
			int halfHeight = viewport.Height / 2;
			var topOfScreen = new Rectangle(0, 0, viewport.Width, halfHeight);
			if (IsPressed(Keys.A, Buttons.A) || LeftMouseIsPressed(topOfScreen))
			{
				_currentPrimitiveIndex = (_currentPrimitiveIndex + 1) % _primitives.Count;
			}
			
			// Change color?
			Rectangle botLeftOfScreen = new Rectangle(0, halfHeight, halfWidth, halfHeight);
			if (IsPressed(Keys.B, Buttons.B) || LeftMouseIsPressed(botLeftOfScreen))
			{
				_currentColorIndex = (_currentColorIndex + 1) % colors.Count;
			}


			// Toggle wireframe?
			Rectangle botRightOfScreen = new Rectangle(halfWidth, halfHeight, halfWidth, halfHeight);
			if (IsPressed(Keys.Y, Buttons.Y) || LeftMouseIsPressed(botRightOfScreen))
			{
				_isWireframe = !_isWireframe;
			}
		}

		bool IsPressed(Keys key, Buttons button)
		{
			return (_currentKeyboardState.IsKeyDown(key) && _lastKeyboardState.IsKeyUp(key)) ||
				   (_currentGamePadState.IsButtonDown(button) && _lastGamePadState.IsButtonUp(button));
		}

		bool LeftMouseIsPressed(Rectangle rect)
		{
			return (_currentMouseState.LeftButton == ButtonState.Pressed &&
					_lastMouseState.LeftButton != ButtonState.Pressed &&
					rect.Contains(_currentMouseState.X, _currentMouseState.Y));
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
