using System.Threading;

namespace Primitives3D
{
	public class AlternateGameLoop
	{
		private readonly Renderer _renderer;
		private readonly SemaphoreSlim _semaphore;
		private World _world;

		public AlternateGameLoop(Renderer renderer, SemaphoreSlim semaphore)
		{
			_renderer = renderer;
			_semaphore = semaphore;
			_world = new World(renderer);
		}

		public void Loop()
		{
			while (true)
			{
				if(_renderer.CanAcceptCommands() )
					Update();
				
			}
		}

		private void Update()
		{
			_world.Update(0);
		}
	}
}