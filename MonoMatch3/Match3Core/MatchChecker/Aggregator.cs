using System.Collections.Generic;
using MonoMatch3.Match3Core.Tiles;

namespace MonoMatch3.Match3Core.MatchChecker;

public class Aggregator(Settings settings)
{
    private readonly CheckerBase[] checkers =
    [
        new ThreeInARow(settings, Direction.Right),
        new ThreeInARow(settings, Direction.Up),
    ];

    public bool TryProcessMatch(Dictionary<TilePosition, TileBase> tiles, TilePosition position)
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
