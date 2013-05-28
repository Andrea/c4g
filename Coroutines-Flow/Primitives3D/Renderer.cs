using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives3D
{
	public class Renderer
	{
		private CubePrimitive _cubePrimitive;
		private readonly List<RenderCommand> _renderCommands;

		public Renderer(GraphicsDevice device)
		{
			_renderCommands = new List<RenderCommand>();
			_cubePrimitive = new CubePrimitive(device);
		}

		public List<RenderCommand> RenderCommands
		{
			get { return _renderCommands; }
		}

		public void AddCube(Cube primitive)
		{
			var translation = Matrix.CreateFromYawPitchRoll(primitive.Rotation.X, primitive.Rotation.Y, primitive.Rotation.Z) * Matrix.CreateTranslation(primitive.Position);
			RenderCommands.Add(new RenderCommand { Color = primitive.Color, Radius = primitive.Radius, World = translation });
		}
		
		public void Draw(Matrix view, Matrix projection)
		{
			foreach (var renderingRenderCommand in RenderCommands)
			{
				_cubePrimitive.Draw(renderingRenderCommand.World, view, projection, renderingRenderCommand.Color);
			}
			_renderCommands.Clear();
		}
	}
}