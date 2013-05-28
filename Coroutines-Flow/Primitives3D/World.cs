using System;
using System.Collections.Generic;
using System.Diagnostics;
using Flow;
using Microsoft.Xna.Framework;

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
				primitive.Rotation += new Vector3(1.0f, 10.0f, 1.0f) *(elapsedMilliseconds / 1000.0f);
				primitive.Color = _colour;
				_renderer.AddCube(primitive);
			}
			_kernel.Step();
		}

		private IEnumerator<bool> ChangeColour(IGenerator t0)
		{

			while (true)
			{

				_colour = _colours[_currentColourIndex];

				_currentColourIndex++;
				if (_currentColourIndex >= 4)
					_currentColourIndex = 0;

				yield return t0.ResumeAfter(TimeSpan.FromSeconds(5));
			}
		}

		private IEnumerator<int> ChangePosition(IGenerator self)
		{
			while(true)
			{
				foreach (var primitive in _primitives)
				{
					Matrix transform = Matrix.CreateRotationY(0.001f);
					primitive.Position = Vector3.Transform(primitive.Position, transform);
				}
				yield return 0;
			}
		}
	}
}