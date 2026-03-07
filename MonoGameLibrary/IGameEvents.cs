using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary.StatefulEvent;
using MonoGameLibrary.Timers;

namespace MonoGameLibrary;

public interface IGameEvents
{
    event Action<GameTime> OnUpdate;
    event Action<GameTime> OnDraw;

    IStatefulEvent<TimeSpan> CurrentTime { get; }
    ITimer Timer { get; }
}
