using System;
using MonoGameLibrary;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;

namespace MonoMatch3Core.Specials;

public class LineDestroyer :TileDestroyer
{
    public LineDestroyer(Settings settings, IGameEvents gameEvents, TilePosition center, Direction direction) : base(gameEvents)
    {
        TilePosition cursor = center;
        int stepsCount = 0;
        float secondsPerStep = 1f / settings.board.timings.lineDestroyerFlySpeed;

        while (settings.TryShift(cursor, direction, out cursor) == true)
        {
            stepsCount++;
            TimeSpan destroyDelay = TimeSpan.FromSeconds(secondsPerStep * stepsCount);
            ScheduleDestroy(destroyDelay, cursor);
        }
    }
}
