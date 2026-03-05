using System;

namespace MonoGameLibrary.Timers
{
    public interface ITimer
    {
        ITimerPromise WaitOneFrame();
        ITimerPromise Wait(TimeSpan duration, Action<float> progressCallback = null);
        ITimerPromise WaitForTrue(Func<bool> condition);
        ITimerPromise WaitForMainThread();

        void StopTimer(long timerId, StopResult stopResult);
    }
}
