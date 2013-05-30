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
			for (int i = 0; i < 1000; i++)
			{
				_cubes.Add(new Cube
					{
						Color = Color.Red,
						Position = new Vector3(random.Next(100) - 50, random.Next(100) - 50, -random.Next(100)),
						Radius = random.Next(5),
						Rotation = Vector3.Zero
					});
			}
		}

		public void Update(long elapsedMilliseconds)
		{
			foreach (var cube in _cubes)
			{
				cube.Rotation += new Vector3(1.10f, 1.1f, 1.03f) *(elapsedMilliseconds / 1000.0f);
				_renderer.AddCube(cube);
			}
			_renderer.EndFrame();
		}
	}
}