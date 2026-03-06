using System.Collections.Generic;
using MonoMatch3Core.Data;
using MonoMatch3Core.Tiles;

namespace MonoMatch3Core.MatchChecker;

internal abstract class CheckerBase(Settings settings)
{
    internal abstract bool TryProcessMatch(Dictionary<TilePosition, TileBase> tiles, TilePosition position);
}
