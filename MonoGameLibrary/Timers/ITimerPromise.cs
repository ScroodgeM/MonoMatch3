using MonoGameLibrary.Promises;

namespace MonoGameLibrary.Timers
{
    public interface ITimerPromise : IPromise
    {
        ITimerPromise GetTimerId(out long timerId);
    }
}
