using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives3D
{
	public class Renderer
	{
		private readonly AutoResetEvent _autoResetEvent;
		private List<RenderCommand> _bufferedRenderCommandsA;
		private List<RenderCommand> _bufferedRenderCommandsB;
		private List<RenderCommand> _updatingRenderCommands;
		private List<RenderCommand> _drawingRenderCommands;
		private List<RenderCommand> _lastRenderCommands;
		private ConcurrentQueue<RenderCommand[]> _concurrentRenderCommandsThatRepresentAFrame;

		private CubePrimitive _cubePrimitive;
		private SemaphoreSlim _semaphoreSlim;
		static readonly object _locker = new object();
		public Renderer(AutoResetEvent autoResetEvent)
		{
			_autoResetEvent = autoResetEvent;
			_bufferedRenderCommandsA = _bufferedRenderCommandsB = new List<RenderCommand>();
			_lastRenderCommands = new List<RenderCommand>();
			_concurrentRenderCommandsThatRepresentAFrame = new ConcurrentQueue<RenderCommand[]>();
			_semaphoreSlim = new SemaphoreSlim(1);
		}

		//NOTE: Not very generic at all but it does what we need it to do
		public void AddCube(Cube primitive)
		{
			var translation = Matrix.CreateFromYawPitchRoll(primitive.Rotation.X, primitive.Rotation.Y, primitive.Rotation.Z) * Matrix.CreateTranslation(primitive.Position);
			_bufferedRenderCommandsA.Add(new RenderCommand { Color = primitive.Color, Radius = primitive.Radius, World = translation });
		}

		public void EndFrame()
		{
			lock (_locker)
			{
				_bufferedRenderCommandsB.AddRange(_bufferedRenderCommandsA);
				_bufferedRenderCommandsA.Clear();
			}
		}

		public void Draw(GraphicsDevice device, Matrix view, Matrix projection)
		{
			_cubePrimitive = _cubePrimitive ?? new CubePrimitive(device);
			_lastRenderCommands.Clear();
			lock (_locker)
			{
				_lastRenderCommands.AddRange(_bufferedRenderCommandsB);
				_bufferedRenderCommandsB.Clear();				
			}

			_autoResetEvent.Set();
			foreach (var renderingRenderCommand in _lastRenderCommands)
			{
				_cubePrimitive.Draw(renderingRenderCommand.World, view, projection, renderingRenderCommand.Color);
			}
		}

		//NOTE: not sure about this 
		public bool CanAcceptCommands()
		{
			return _bufferedRenderCommandsA.Count == 0 || _bufferedRenderCommandsA.Count == 0;
		}
	}
}