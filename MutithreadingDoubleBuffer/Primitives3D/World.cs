using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Primitives3D
{
	public class World
	{
		private readonly Renderer _renderer;
		private IList<Cube> _primitives;

		public World(Renderer renderer)
		{
			_renderer = renderer;
			Initialise();
		}

		public void Initialise()
		{
			_primitives = new List<Cube>();
			var random = new Random();
			for (int i = 0; i < 1000; i++)
			{
				_primitives.Add(new Cube
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
			foreach (var primitive in _primitives)
			{
				primitive.Rotation += new Vector3(1.510f, 1.5f, 1.513f) *(elapsedMilliseconds / 1000.0f);
				_renderer.AddCube(primitive);
			}
			_renderer.EndFrame();
		}
	}
}