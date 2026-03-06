namespace MonoMatch3.Match3Core.MatchChecker;

public abstract class CheckerBase(Board board)
{
    internal abstract bool TryProcessMatch(TilePosition position);
}
