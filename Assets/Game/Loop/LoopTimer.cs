using System;

namespace ShopGame.Loop
{
    public sealed class LoopTimer
    {
        private readonly float timeLimitSeconds;

        public float ElapsedSeconds { get; private set; }

        public bool IsRunning { get; private set; }

        public bool IsTimeLimitReached =>
            ElapsedSeconds >= timeLimitSeconds;

        public LoopTimer(float timeLimitSeconds)
        {
            if (timeLimitSeconds <= 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(timeLimitSeconds),
                    "Time limit must be greater than zero.");
            }

            this.timeLimitSeconds = timeLimitSeconds;
        }

        public void Start()
        {
            ElapsedSeconds = 0f;
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void Update(float deltaTime)
        {
            if (!IsRunning)
            {
                return;
            }

            if (deltaTime < 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(deltaTime),
                    "Delta time must not be negative.");
            }

            if (IsTimeLimitReached)
            {
                return;
            }

            ElapsedSeconds += deltaTime;

            if (ElapsedSeconds > timeLimitSeconds)
            {
                ElapsedSeconds = timeLimitSeconds;
            }
        }

        public void Reset()
        {
            ElapsedSeconds = 0f;
            IsRunning = false;
        }
    }
}