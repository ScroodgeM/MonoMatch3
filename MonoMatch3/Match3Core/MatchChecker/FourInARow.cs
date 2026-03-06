using System;
using System.Collections.Generic;
using MonoMatch3.Match3Core.Tiles;

namespace MonoMatch3.Match3Core.MatchChecker;

public class FourInARow(Settings settings, Direction lineDirection, TileType lineSpecialBonus) : ThreeInARow(settings, lineDirection)
{
    protected override bool ProcessFoundTiles(Dictionary<TilePosition, TileBase> tiles, TilePosition position)
    {
        if (foundTilesCache.Count == 4)
        {
            foundTilesCache.Remove(position);

            if (base.ProcessFoundTiles(tiles, position) == false)
            {
                throw new InvalidOperationException("something went wrong");
            }

            tiles[position].ChangeTypeTo(lineSpecialBonus);

            return true;
        }

        return false;
    }
}
