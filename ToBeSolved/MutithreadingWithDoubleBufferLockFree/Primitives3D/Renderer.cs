using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives3D
{
	public class Renderer 
	{
		private List<RenderCommand> _updatingRenderCommands;
		private List<RenderCommand> _renderingRenderCommands;
		private readonly List<RenderCommand> _bufferedRenderCommandsA;
		private readonly List<RenderCommand> _bufferedRenderCommandsB;

		private readonly CubePrimitive _cubePrimitive;
		
		private ManualResetEvent _renderActive;
		private ManualResetEvent _renderComandsReady;
		private ManualResetEvent _renderCompleted;

		public Renderer(GraphicsDevice device)
		{
			_bufferedRenderCommandsA = new List<RenderCommand>();
			_bufferedRenderCommandsB = new List<RenderCommand>();
			_updatingRenderCommands = _bufferedRenderCommandsA;

			_renderComandsReady = new ManualResetEvent(false);

			_renderActive = new ManualResetEvent(false);
			_renderCompleted = new ManualResetEvent(true);
			_cubePrimitive = _cubePrimitive ?? new CubePrimitive(device);
		}

		public void AddCube(Cube primitive)
		{
			var translation = Matrix.CreateFromYawPitchRoll(
									primitive.Rotation.X, 
									primitive.Rotation.Y, 
									primitive.Rotation.Z) *
							  Matrix.CreateTranslation(primitive.Position);

			_updatingRenderCommands.Add(new RenderCommand
				                            {
					                            Color = primitive.Color, 
												Radius = primitive.Radius, 
												World = translation
				                            });
		}

		public void EndFrame()
		{
            // TODO:  Stuff :|
            /* 
             *  
             */
			
		}
		
		public void Draw(Matrix view, Matrix projection)
		{
            // TODO: More stuff :D
            
			
			SwapBuffers();
			
			
			foreach (var renderingRenderCommand in _renderingRenderCommands)
			{
				_cubePrimitive.Draw(renderingRenderCommand.World, view, projection, renderingRenderCommand.Color);
			}
		}

		private void SwapBuffers()
		{
			if (_updatingRenderCommands == _bufferedRenderCommandsA)
			{
				_updatingRenderCommands = _bufferedRenderCommandsB;
				_renderingRenderCommands = _bufferedRenderCommandsA;

			}
			else if (_updatingRenderCommands == _bufferedRenderCommandsB)
			{
				_updatingRenderCommands = _bufferedRenderCommandsA;
				_renderingRenderCommands = _bufferedRenderCommandsB;
			}
			_updatingRenderCommands.Clear();
		}
	}
}