using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Primitives3D
{
	public class World
	{
		private readonly Renderer _renderer;
		private IList<Cube> _primitives;
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
		private IEnumerator<int> _positionAndColourCoordinator;
		private IEnumerator<bool> _changePositionCoroutine;
		private IEnumerator<bool> _changeColourCoroutine;
		private long _lastColourChange;

		public World(Renderer renderer)
		{
			_renderer = renderer;
			Initialize();
		}

		public void Initialize()
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

			_positionAndColourCoordinator = CreatePositionAndColourCoordinator();
			_changeColourCoroutine = ChangeColour();
			_changePositionCoroutine = ChangePosition();
			_stopwatch = new Stopwatch();

			_stopwatch.Start();
			_colour = Color.Beige;
		}

		public void Update(long elapsedMilliseconds)
		{
			foreach (var primitive in _primitives)
			{
				primitive.Rotation += new Vector3(1.0f, 10.0f, 1.0f) * (elapsedMilliseconds / 1000.0f);
				primitive.Color = _colour;
				_renderer.AddCube(primitive);
			}
			_positionAndColourCoordinator.MoveNext();
		}
        
		private IEnumerator<int> CreatePositionAndColourCoordinator()
		{
            //TODO: Complete this method so that this coordinator coroutine works
            while (_changeColourCoroutine.MoveNext())


            while (_changePositionCoroutine.MoveNext())

                //TODO: remove return null below.
                    return null;
                    return null;
		}

		private IEnumerator<bool> ChangeColour()
		{
			// TODO: Complete this method so that it returns as expected :)
            // This code should make the cubes change colour every half second
            // and finish changing colour and start rotating after 5 seconds

				_colour = _colours[_currentColourIndex];
				if ((_stopwatch.ElapsedMilliseconds-_lastColourChange) > 500)
				{
					_currentColourIndex++;
					_lastColourChange = _stopwatch.ElapsedMilliseconds;
				}

				if (_currentColourIndex >= 4 )
				{
					_currentColourIndex = 0;
					if (_stopwatch.ElapsedMilliseconds > 5000)
						yield break; // TODO: Why are we doing this?
				}
		}

		private IEnumerator<bool> ChangePosition()
		{
            // TODO: Complete this method so that it returns as expected :)
			
				foreach (var primitive in _primitives)
				{
					var transform = Matrix.CreateRotationZ(0.001f);
					primitive.Position = Vector3.Transform(primitive.Position, transform);
				}
		    return null; // <- remove this. Temporary so that the code compiles
		}


	}
}