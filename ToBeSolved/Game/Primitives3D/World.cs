using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Primitives3D
{
	public class World
	{
		private readonly Renderer _renderer;
		private IList<Cube> _cubes;
		
		public World(Renderer renderer)
		{
			_renderer = renderer;
			Initialise();
		}

		public void Initialise()
		{
			_cubes = new List<Cube>();
			var random = new Random();
			for (int i = 0; i < Constants.CubeCount; i++)
			{
				_cubes.Add(new Cube
					{
						Color = Color.Red,
						Position = new Vector3(random.Next(100) - 50, random.Next(100) - 50, -random.Next(100)),
						Radius = random.Next(100),
						Rotation = Vector3.Zero
					});
			}
		}

		public void Update(long elapsedMilliseconds)
		{
            // TODO: Change the rotation of each of the cubes before adding them to the rendere so that the rotation depends on time.
            // to achieve this you generally multiply a Vector3 with a scalar that has some some or awareness 
            // of the elapsed time

			foreach (var cube in _cubes)
			{
                _renderer.AddCube(cube);
			}
		}
	}
}