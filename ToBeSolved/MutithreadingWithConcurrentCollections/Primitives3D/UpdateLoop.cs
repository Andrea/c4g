using System.Diagnostics;

namespace Primitives3D
{
    public class UpdateLoop
    {
        private readonly Renderer _renderer;
        private readonly World _world;
        private readonly Stopwatch _stopwatch;
        private long _lastElapsed;

        public UpdateLoop(Renderer renderer)
        {
            _renderer = renderer;
            _world = new World(renderer);
            _stopwatch = new Stopwatch();
        }

        public void Loop()
        {
            _stopwatch.Start();
            while (true)
                Update();
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