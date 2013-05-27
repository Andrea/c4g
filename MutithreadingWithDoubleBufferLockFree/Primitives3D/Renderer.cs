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

		private CubePrimitive _cubePrimitive;
		
		private AutoResetEvent _renderActive;
		private AutoResetEvent _renderComandsReady;
		private AutoResetEvent _renderCompleted;

		public Renderer()
		{
			_bufferedRenderCommandsA = new List<RenderCommand>();
			_bufferedRenderCommandsB = new List<RenderCommand>();
			_updatingRenderCommands = _bufferedRenderCommandsA;

			_renderComandsReady = new AutoResetEvent(false);

			_renderActive = new AutoResetEvent(false);
			_renderCompleted = new AutoResetEvent(true);
		}

		public void AddCube(Cube primitive)
		{
			var translation = Matrix.CreateFromYawPitchRoll(primitive.Rotation.X, primitive.Rotation.Y, primitive.Rotation.Z) *
							  Matrix.CreateTranslation(primitive.Position);

			_updatingRenderCommands.Add(new RenderCommand { Color = primitive.Color, Radius = primitive.Radius, World = translation });
		}

		public void EndFrame()
		{
			_renderCompleted.WaitOne();
			_renderComandsReady.Set();
			_renderActive.WaitOne();
		}
		
		public void Draw(GraphicsDevice device, Matrix view, Matrix projection)
		{
			_renderCompleted.Reset();
			SwapBuffers();
			_renderActive.Set();

			_cubePrimitive = _cubePrimitive ?? new CubePrimitive(device);
			foreach (var renderingRenderCommand in _renderingRenderCommands)
			{
				_cubePrimitive.Draw(renderingRenderCommand.World, view, projection, renderingRenderCommand.Color);
			}
			_renderActive.Reset();
			_renderCompleted.Set();
			_renderComandsReady.WaitOne();
		}

		private void SwapBuffers()
		{
			_renderComandsReady.Reset();
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