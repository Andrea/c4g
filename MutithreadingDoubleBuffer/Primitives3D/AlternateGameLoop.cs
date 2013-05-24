using System.Diagnostics;
using System.Threading;

namespace Primitives3D
{
	public class AlternateGameLoop
	{
		private readonly Renderer _renderer;
		
		private readonly World _world;
		private Stopwatch _stopwatch;
		private long _lastElapsed;
		

		public AlternateGameLoop(Renderer renderer)
		{
			_renderer = renderer;
			_world = new World(renderer);
			_stopwatch = new Stopwatch();
		}

		public void Loop()
		{
			_stopwatch.Start();
			while (true)
			{
				Update();
			}
		}

		private void Update()
		{
			if (!_renderer.CanAcceptCommands())
				return;

			var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
			var elapsed = elapsedMilliseconds - _lastElapsed;
			_lastElapsed = elapsedMilliseconds;
			_world.Update(elapsed);
			
			
		}
	}
}