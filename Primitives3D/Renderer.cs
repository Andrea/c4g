using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives3D
{
	public class Renderer
	{
		private readonly IList<RenderCommand> _renderCommandsA;
		private readonly IList<RenderCommand> _renderCommandsB;
		private IList<RenderCommand> _updatingRenderCommands;
		private IList<RenderCommand> _renderingRenderCommands;

		private CubePrimitive _cubePrimitive;
		private RenderCommand _lastRenderCommand;

		public Renderer()
		{
			_renderCommandsA = new List<RenderCommand>();
			_renderCommandsB = new List<RenderCommand>();
			_updatingRenderCommands = _renderCommandsA;
		}

		public void AddCube(Cube primitive)
		{
			//why the fuck this?
			var translation = Matrix.CreateTranslation(primitive.Position) * Matrix.CreateFromYawPitchRoll(primitive.Rotation.X, primitive.Rotation.Y, primitive.Rotation.Z);
			
			_updatingRenderCommands.Add(new RenderCommand{Color = primitive.Color, Radius = primitive.Radius, World = translation});
		}

		public void SwitchBuffers()
		{
			if(_updatingRenderCommands == _renderCommandsA)
			{
				_updatingRenderCommands = _renderCommandsB;
				_renderingRenderCommands = _renderCommandsA;
			}
			else
			{
				_updatingRenderCommands = _renderCommandsA;
				_renderingRenderCommands = _renderCommandsB;
			}
		}

		public void Draw(GraphicsDevice device, Matrix view, Matrix projection)
		{
			_cubePrimitive = _cubePrimitive ?? new CubePrimitive(device);
			foreach (var renderingRenderCommand in _renderingRenderCommands)
			{
				_cubePrimitive.Draw(renderingRenderCommand.World, view, projection, renderingRenderCommand.Color);
				_lastRenderCommand = renderingRenderCommand;
			}
			_renderingRenderCommands.Clear();
			
		}
	}
}