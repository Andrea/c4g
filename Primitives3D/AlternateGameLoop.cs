using System.Threading;

namespace Primitives3D
{
	public class AlternateGameLoop
	{
		private readonly SemaphoreSlim _semaphore;
		private World _world;

		public AlternateGameLoop(Renderer renderer, SemaphoreSlim semaphore)
		{
			_semaphore = semaphore;
			_world = new World(renderer);
		}

		public void Loop()
		{
			while (true)
			{
				// :)
				_semaphore.Wait();
				Update();
				_semaphore.Release();
			}
		}

		private void Update()
		{
			_world.Update(0);
		}
	}
}