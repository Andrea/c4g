using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives3D
{
	public class Renderer
	{
		private readonly IList<RenderCommand> _updatingRenderCommands;
		private readonly ConcurrentQueue<RenderCommand[]> _concurrentRenderCommandsThatRepresentAFrame;
		private readonly CubePrimitive _cubePrimitive;
		

		public Renderer(GraphicsDevice device)
		{
			_updatingRenderCommands = new List<RenderCommand>();
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
			// TODO: Copy the render commands you added via Addcube into renderCommands . Then use the concurrent collection
            // so that the info there is stored.
            // Finally there will be some cleaning up to do, but where?
            
		}
		
		public void Draw(Matrix view, Matrix projection)
		{
			RenderCommand[] renderCommands = null;
            
            // TODO populate renderCommands with data so that it can be renderd
		    

		    foreach (var renderingRenderCommand in renderCommands)
			{
				_cubePrimitive.Draw(renderingRenderCommand.World, view, projection, renderingRenderCommand.Color);
			}
		}

		public bool CanAcceptCommands()
		{
			// TODO: Why is this necesary?
            return _concurrentRenderCommandsThatRepresentAFrame.Count < 2;
		}
	}
}