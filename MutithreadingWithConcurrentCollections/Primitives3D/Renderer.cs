using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives3D
{
	public class Renderer
	{
		private IList<RenderCommand> _updatingRenderCommands;
		
		private ConcurrentQueue<RenderCommand[]> _concurrentRenderCommandsThatRepresentAFrame;

		private CubePrimitive _cubePrimitive;
		private RenderCommand[] _lastRenderCommands;

		public Renderer(GraphicsDevice device)
		{
			_updatingRenderCommands = new List<RenderCommand>();
			_lastRenderCommands = new RenderCommand[]{};
			_concurrentRenderCommandsThatRepresentAFrame = new ConcurrentQueue<RenderCommand[]>();
            _cubePrimitive = _cubePrimitive ?? new CubePrimitive(device);

		}

		public void AddCube(Cube primitive)
		{
			var translation = Matrix.CreateFromYawPitchRoll(primitive.Rotation.X, primitive.Rotation.Y, primitive.Rotation.Z) * Matrix.CreateTranslation(primitive.Position);
			_updatingRenderCommands.Add(new RenderCommand
			                                {
			                                    Color = primitive.Color, 
                                                Radius = primitive.Radius, 
                                                World = translation
			                                });
		}

		public void EndFrame()
		{
			var renderCommands = new RenderCommand[_updatingRenderCommands.Count];
			_updatingRenderCommands.CopyTo(renderCommands, 0);
			_concurrentRenderCommandsThatRepresentAFrame.Enqueue(renderCommands);
			_updatingRenderCommands.Clear();
		}
		
		public void Draw(Matrix view, Matrix projection)
		{
			RenderCommand[] renderCommands;
			if (_concurrentRenderCommandsThatRepresentAFrame.TryDequeue(out renderCommands))
			{
				_lastRenderCommands = renderCommands;
			}

			foreach (var renderingRenderCommand in _lastRenderCommands)
			{
				_cubePrimitive.Draw(renderingRenderCommand.World, view, projection, renderingRenderCommand.Color);
			}
		}

		public bool CanAcceptCommands()
		{
			return _concurrentRenderCommandsThatRepresentAFrame.Count < 2;
		}
	}
}