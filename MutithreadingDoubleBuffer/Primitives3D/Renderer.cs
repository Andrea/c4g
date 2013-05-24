using System;
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


		private CubePrimitive _cubePrimitive;

		static readonly object Locker = new object();
		public Renderer(AutoResetEvent autoResetEvent)
		{
			_autoResetEvent = autoResetEvent;
			_bufferedRenderCommandsA = new List<RenderCommand>();
			_bufferedRenderCommandsB = new List<RenderCommand>();
			_lastRenderCommands = new List<RenderCommand>();
			_updatingRenderCommands = _bufferedRenderCommandsA;
		}

		public void AddCube(Cube primitive)
		{
			var translation = Matrix.CreateFromYawPitchRoll(primitive.Rotation.X, primitive.Rotation.Y, primitive.Rotation.Z) * Matrix.CreateTranslation(primitive.Position);
			_updatingRenderCommands.Add(new RenderCommand { Color = primitive.Color, Radius = primitive.Radius, World = translation });
		}

		public void EndFrame()
		{
			lock (Locker)
			{
				if (_updatingRenderCommands == _bufferedRenderCommandsA)
				{
					_updatingRenderCommands = _bufferedRenderCommandsB;
					_drawingRenderCommands = _bufferedRenderCommandsA;
				}
				else if (_updatingRenderCommands == _bufferedRenderCommandsB)
				{
					_updatingRenderCommands = _bufferedRenderCommandsA;
					_drawingRenderCommands = _bufferedRenderCommandsB;
				}
			}
		}

		public void Draw(GraphicsDevice device, Matrix view, Matrix projection)
		{
			_cubePrimitive = _cubePrimitive ?? new CubePrimitive(device);
			
			Console.WriteLine("Draw call renderCommands count: {0}", _drawingRenderCommands.Count);
			lock (Locker)
			{
				_lastRenderCommands.Clear();
				_lastRenderCommands.AddRange(_drawingRenderCommands);
			}
			foreach (var renderingRenderCommand in _lastRenderCommands)
			{
				_cubePrimitive.Draw(renderingRenderCommand.World, view, projection, renderingRenderCommand.Color);
			}
			_autoResetEvent.Set();			
			_drawingRenderCommands.Clear();
		}

		//NOTE: not sure about this 
		public bool CanAcceptCommands()
		{
			return _bufferedRenderCommandsA.Count == 0 || _bufferedRenderCommandsA.Count == 0;
		}
	}
}