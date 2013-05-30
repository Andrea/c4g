using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Flow;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Primitives3D
{
	public class World
	{
		private readonly Renderer _renderer;
		private IList<Cube> _primitives;
		private IKernel _kernel;
		private Stopwatch _stopwatch;

		readonly List<Color> _colours = new List<Color>
        {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.White,
            Color.Black,
        };

		private Color _colour;
		private int _currentColourIndex;
		private IFuture<bool> _aPressed;
		private KeyboardState _lastState;
	    private IChannel<bool> _channel;

	    public World(Renderer renderer)
		{
			_renderer = renderer;
			Initialise();
		}

		public void Initialise()
		{
			_primitives = new List<Cube>();
			var random = new Random();
			for (int i = 0; i < Constants.CubeCount; i++)
			{
				_primitives.Add(new Cube
					{
						Color = Color.Red,
						Position = new Vector3(random.Next(100) - 50, random.Next(100) - 50, -random.Next(100)),
						Radius = random.Next(100),
						Rotation = Vector3.Zero
					});
			}
			_kernel = Create.NewKernel();
		    _channel = _kernel.Factory.NewChannel<bool>();
			_stopwatch = new Stopwatch();
			_stopwatch.Start();
			_colour = Color.Beige;
			
			_kernel.Factory.NewCoroutine(ChangePosition);
			_kernel.Factory.NewCoroutine(ChangeColour);
		    
		}

		public void Update(long elapsedMilliseconds)
		{
			foreach (var primitive in _primitives)
			{
				primitive.Rotation += new Vector3(1.0f, 10.0f, 1.0f) * (elapsedMilliseconds / 1000.0f);
				primitive.Color = _colour;
				_renderer.AddCube(primitive);
			}
			
			var keyboardState = Keyboard.GetState();
			
			if (keyboardState.IsKeyDown(Keys.A) && _lastState.IsKeyUp(Keys.A))
			{
                _channel.Insert(true);
			}
			_lastState = keyboardState;

			_kernel.Step();
		}

		private IEnumerator<bool> ChangeColour(IGenerator self)
		{
		    IFuture<bool> extract = _channel.Extract();
		    yield return self.ResumeAfter(extract);

		    while (true)
			{
				_colour = _colours[_currentColourIndex];

				_currentColourIndex++;
				if (_currentColourIndex >= 4)
					_currentColourIndex = 0;

				yield return self.ResumeAfter(TimeSpan.FromSeconds(5));
			}
		}        

		private IEnumerator<bool> ChangePosition(IGenerator self)
		{
            
			while (true)
			{
				foreach (var primitive in _primitives)
				{
					Matrix transform = Matrix.CreateRotationY(0.001f);
					primitive.Position = Vector3.Transform(primitive.Position, transform);
				}
				yield return true;
			}
		}
	}
}