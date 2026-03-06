using System.Collections.Generic;
using MonoMatch3.Match3Core.Tiles;
using MonoMatch3Core.Data;

namespace MonoMatch3.Match3Core.MatchChecker;

public abstract class CheckerBase(Settings settings)
{
    internal abstract bool TryProcessMatch(Dictionary<TilePosition, TileBase> tiles, TilePosition position);
}
