using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoMatch3Core.Data;

namespace MonoMatch3Core.Specials;

public abstract class TileDestroyer(IGameEvents gameEvents) : SpecialBase(gameEvents)
{
    private List<(TimeSpan, TilePosition)> scheduledDestroys = new List<(TimeSpan, TilePosition)>();

    protected void ScheduleDestroy(TimeSpan delay, TilePosition position)
    {
        scheduledDestroys.Add((gameEvents.CurrentTime.Value + delay, position));
        gameEvents.OnUpdate -= OnUpdate;
        gameEvents.OnUpdate += OnUpdate;
    }

    private void OnUpdate(GameTime gameTime)
    {
        for (int i = scheduledDestroys.Count - 1; i >= 0; i--)
        {
            (TimeSpan, TilePosition) scheduledDestroy = scheduledDestroys[i];
            if (gameTime.TotalGameTime >= scheduledDestroy.Item1)
            {
                TriggerTileDestroyAttempt(scheduledDestroy.Item2);
                scheduledDestroys.RemoveAt(i);
            }
        }

        if (scheduledDestroys.Count == 0)
        {
            TriggerCompleted();
            gameEvents.OnUpdate -= OnUpdate;
        }
    }
}
