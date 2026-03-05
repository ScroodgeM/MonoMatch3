using System;
using MonoGameLibrary.Promises;

namespace MonoGameLibrary.Timers
{
    internal class Awaiter : ITimerPromise
    {
        internal long id => startData.id;
        internal TimeSpan duration => startData.duration;
        internal TimeSpan finishTime => startData.finishTime;
        internal Func<bool> additionalSuccessCondition => startData.additionalSuccessCondition;
        internal Action<float> progressCallback => startData.progressCallback;

        internal Deferred resolver { get; private set; }

        private AwaiterStartData startData;

        internal void PrepareNew(AwaiterStartData startData)
        {
            this.startData = startData;
            this.resolver = Deferred.GetFromPool();
        }

        internal void ClearReferences()
        {
            startData = default;
            resolver = null;
        }

        public IPromise Done(Action callback) => resolver.Done(callback);

        public IPromise Fail(Action<Exception> callback) => resolver.Fail(callback);

        public IPromise Always(Action callback) => resolver.Always(callback);

        public IPromise Then(Func<IPromise> next) => resolver.Then(next);

        public IPromise<TNext> Then<TNext>(Func<IPromise<TNext>> next) => resolver.Then(next);

        public ITimerPromise GetTimerId(out long timerId)
        {
            timerId = id;
            return this;
        }
    }
}
