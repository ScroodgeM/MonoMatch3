using System.Collections.Generic;
using MonoMatch3.Match3Core.Tiles;

namespace MonoMatch3.Match3Core.MatchChecker;

public abstract class CheckerBase(Settings settings)
{
    internal abstract bool TryProcessMatch(Dictionary<TilePosition, TileBase> tiles, TilePosition position);
}
