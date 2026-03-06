namespace MonoMatch3.Match3Core.MatchChecker;

public class Aggregator(Board board)
{
    private readonly CheckerBase[] checkers = [new ThreeInARow(board)];

    public bool TryProcessMatch(TilePosition position)
    {
        foreach (CheckerBase checker in checkers)
        {
            if (checker.TryProcessMatch(position) == true)
            {
                return true;
            }
        }

        return false;
    }
}
