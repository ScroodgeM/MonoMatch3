using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary.StatefulEvent;

namespace MonoGameLibrary;

public interface IGameEvents
{
    IStatefulEvent<TimeSpan> CurrentTime { get; }
    event Action<GameTime> OnUpdate;
    event Action<GameTime> OnDraw;
}
