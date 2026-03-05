using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Promises;

namespace MonoGameLibrary.Timers
{
    public class Timer : ITimer
    {
        private static readonly Exception stopException = new OperationCanceledException();

        private IGameEvents gameEvents;
        private readonly ConcurrentQueue<Awaiter> newAwaiters = new ConcurrentQueue<Awaiter>();
        private readonly AwaitersPool awaitersPool = new AwaitersPool();

        public Timer(IGameEvents gameEvents)
        {
            this.gameEvents = gameEvents;
            this.gameEvents.OnUpdate += OnUpdate;
        }

        ~Timer()
        {
            this.gameEvents.OnUpdate -= OnUpdate;
        }

        [ThreadStatic] private static uint nextTimerId;

        private void OnUpdate(GameTime gameTime)
        {
            while (newAwaiters.TryDequeue(out Awaiter awaiter) == true)
            {
                awaitersPool.AddToRotation(awaiter);
            }

            for (int i = 0; i < awaitersPool.Length; i++)
            {
                Awaiter candidate = awaitersPool[i];

                if (gameTime.TotalGameTime >= candidate.finishTime)
                {
                    if (candidate.additionalSuccessCondition == null || candidate.additionalSuccessCondition() == true)
                    {
                        Deferred resolver = candidate.resolver;
                        Action<float> progressCallback = candidate.progressCallback;

                        awaitersPool.DisableAt(i);
                        i--;

                        if (progressCallback != null)
                        {
                            progressCallback(1f);
                        }

                        resolver.Resolve();
                        continue;
                    }
                }
                else
                {
                    if (candidate.progressCallback != null)
                    {
                        TimeSpan startTime = candidate.finishTime - candidate.duration;
                        double progress = (gameTime.TotalGameTime - startTime) / candidate.duration;
                        switch (progress)
                        {
                            case >= 1.0:
                                candidate.progressCallback(1f);
                                break;
                            case <= 0.0:
                                candidate.progressCallback(0f);
                                break;
                            default:
                                candidate.progressCallback((float)progress);
                                break;
                        }
                    }
                }
            }
        }

        public ITimerPromise WaitOneFrame() => Wait(TimeSpan.FromMilliseconds(1), null, null);

        public ITimerPromise Wait(TimeSpan duration, Action<float> progressCallback = null) => Wait(duration, progressCallback, null);

        public ITimerPromise WaitForTrue(Func<bool> condition) => Wait(TimeSpan.Zero, null, condition);

        public ITimerPromise WaitForMainThread() => Wait(TimeSpan.Zero, null, null);

        public void StopTimer(long timerId, StopResult stopResult)
        {
            for (int i = 0; i < awaitersPool.Length; i++)
            {
                Awaiter candidate = awaitersPool[i];

                if (candidate.id == timerId)
                {
                    Deferred resolver = candidate.resolver;

                    awaitersPool.DisableAt(i);
                    StopWithResult(resolver, stopResult);
                    break;
                }
            }
        }

        private Awaiter Wait(TimeSpan duration, Action<float> progressCallback, Func<bool> additionalSuccessCondition)
        {
            AwaiterStartData startData;

            startData.id = nextTimerId | ((long)Thread.CurrentThread.ManagedThreadId << 32);
            startData.duration = duration;
            startData.finishTime = gameEvents.CurrentTime.Value + duration;
            startData.additionalSuccessCondition = additionalSuccessCondition;
            startData.progressCallback = progressCallback;

            unchecked
            {
                nextTimerId++;
            }

            Awaiter awaiter = awaitersPool.GetOrCreateAwaiter(startData);
            newAwaiters.Enqueue(awaiter);
            return awaiter;
        }

        private static void StopWithResult(Deferred deferred, StopResult result)
        {
            switch (result)
            {
                case StopResult.WithRejection:
                    deferred.Reject(stopException);
                    break;

                case StopResult.WithResolving:
                    deferred.Resolve();
                    break;

                case StopResult.Silently:
                    break;
            }
        }
    }
}
