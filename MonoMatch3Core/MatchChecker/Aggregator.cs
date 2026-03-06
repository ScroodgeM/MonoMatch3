using System.Collections.Generic;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.Tiles;

namespace MonoMatch3Core.MatchChecker;

internal class Aggregator(Settings settings)
{
    private readonly CheckerBase[] checkers =
    [
        new FourInARow(settings, Direction.Right, TileType.DestroyerHorizontalLine),
        new FourInARow(settings, Direction.Up, TileType.DestroyerVerticalLine),
        new ThreeInARow(settings, Direction.Right),
        new ThreeInARow(settings, Direction.Up),
    ];

    internal bool TryProcessMatch(Dictionary<TilePosition, TileBase> tiles, TilePosition position)
    {
        foreach (CheckerBase checker in checkers)
        {
            if (checker.TryProcessMatch(tiles, position) == true)
            {
                return true;
            }
        }

        return false;
    }
}
