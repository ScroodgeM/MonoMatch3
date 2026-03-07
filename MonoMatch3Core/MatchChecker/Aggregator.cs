using System.Collections.Generic;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.Tiles;

namespace MonoMatch3Core.MatchChecker;

internal class Aggregator(Settings settings)
{
    private readonly CheckerBase[] checkers =
    [
        new CrossLines(settings, TileType.DestroyerSquare),
        new FivePlusInARow(settings, Direction.Right, TileType.DestroyerSquare),
        new FivePlusInARow(settings, Direction.Up, TileType.DestroyerSquare),
        new FourInARow(settings, Direction.Right, TileType.DestroyerHorizontalLine),
        new FourInARow(settings, Direction.Up, TileType.DestroyerVerticalLine),
        new ThreeInARow(settings, Direction.Right),
        new ThreeInARow(settings, Direction.Up),
    ];

    internal bool TryProcessMatch(Dictionary<TilePosition, TileBase> tiles, TilePosition position, ProcessMatchMode mode)
    {
        foreach (CheckerBase checker in checkers)
        {
            if (checker.TryProcessMatch(tiles, position, mode) == true)
            {
                return true;
            }
        }

        return false;
    }
}
