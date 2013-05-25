using System.Diagnostics;

namespace Primitives3D
{
	public class AlternateGameLoop
	{
		private readonly World _world;
		private readonly Stopwatch _stopwatch;
		private long _lastElapsed;

		public AlternateGameLoop(Renderer renderer)
		{
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
			var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
			var elapsed = elapsedMilliseconds - _lastElapsed;
			_lastElapsed = elapsedMilliseconds;
			_world.Update(elapsed);
		}
	}
}