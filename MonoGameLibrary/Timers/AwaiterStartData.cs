using System;

namespace MonoGameLibrary.Timers
{
    internal struct AwaiterStartData
    {
        internal long id;
        internal TimeSpan duration;
        internal TimeSpan finishTime;
        internal Func<bool> additionalSuccessCondition;
        internal Action<float> progressCallback;
    }
}
