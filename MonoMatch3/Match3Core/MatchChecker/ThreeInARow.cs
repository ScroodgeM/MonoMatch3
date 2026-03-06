using System;

namespace MonoMatch3.Match3Core.MatchChecker;

public class ThreeInARow(Board board) : CheckerBase(board)
{
    internal override bool TryProcessMatch(TilePosition position)
    {
        Console.WriteLine(position);
        return false;
    }
}
