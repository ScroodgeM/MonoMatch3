using System;
using MonoGameLibrary;
using MonoMatch3Core.Data;

namespace MonoMatch3Core.Specials;

public abstract class SpecialBase(IGameEvents gameEvents)
{
    public event Action<TilePosition> OnTileDestroyAttempt = position => { };
    public event Action<SpecialBase> OnCompleted = special => { };

    protected void TriggerTileDestroyAttempt(TilePosition tilePosition) => OnTileDestroyAttempt(tilePosition);
    protected void TriggerCompleted() => OnCompleted(this);
}
