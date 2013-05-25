using Microsoft.Xna.Framework;

namespace Primitives3D
{
	public class RenderCommand
	{
		public float Radius { get; set; }
		public Color Color { get; set; }
		public Matrix World { get; set; }
	}

	public class Constants
	{
		public static int CubeCount = 1000;
	}
}