using System;
using MonoGameLibrary;
using MonoMatch3Core.Data;

namespace MonoMatch3Core.Specials;

internal class SquareBomb : TileDestroyer
{
    public SquareBomb(Settings settings, IGameEvents gameEvents, TilePosition center) : base(gameEvents)
    {
        const int squareSize = 1;

        BoardArea area = settings.GetBoardArea();
        TimeSpan explodeDelay = TimeSpan.FromSeconds(settings.board.timings.bombExplodeDelay);

        for (int x = center.X - squareSize; x <= center.X + squareSize; x++)
        {
            for (int y = center.Y - squareSize; y <= center.Y + squareSize; y++)
            {
                if (area.Contains(x, y) == true)
                {
                    TilePosition position = new TilePosition(true, (byte)x, (byte)y);
                    ScheduleDestroy(explodeDelay, position);
                }
            }
        }
    }
}
