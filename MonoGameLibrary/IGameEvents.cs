using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary;

public interface IGameEvents
{
    event Action<GameTime> OnUpdate;
    event Action<GameTime> OnDraw;
}
