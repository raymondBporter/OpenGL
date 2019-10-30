using System;

namespace TkRenderer
{
    public class Timer
    {
        private DateTime _startTime;
        private DateTime _lastTick;

        public void Start()
        {
            _startTime = DateTime.Now;
            _lastTick = DateTime.Now;
        }

        public void Tick()
        {
            DateTime now = DateTime.Now;

            Time = (now - _startTime).TotalSeconds;
            DeltaTime = (now - _lastTick).TotalSeconds;
            _lastTick = now;
        }

        public double Time { get; private set; }

        public double DeltaTime { get; private set; }
    }
}
