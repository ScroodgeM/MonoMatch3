using System.Collections.Generic;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.Tiles;

namespace MonoMatch3Core.MatchChecker;

internal class CrossLines(Settings settings) : CheckerBase(settings)
{
    protected static readonly HashSet<TilePosition> foundTilesCache = new HashSet<TilePosition>();

    internal override bool TryProcessMatch(Dictionary<TilePosition, TileBase> tiles, TilePosition position, ProcessMatchMode mode)
    {
        if (tiles.TryGetValue(position, out TileBase mainTile) == false)
        {
            return false;
        }

        foundTilesCache.Clear();
        foundTilesCache.Add(position);
        int horizontalLineLength = 1 + CollectTilesInBothDirections(tiles, position, Direction.Right, mainTile.Color, foundTilesCache);
        int verticalLineLength = 1 + CollectTilesInBothDirections(tiles, position, Direction.Up, mainTile.Color, foundTilesCache);

        if (horizontalLineLength < 3 || verticalLineLength < 3)
        {
            return false;
        }

        foreach (TilePosition foundTilePosition in foundTilesCache)
        {
            if (tiles[foundTilePosition].State.Value.movement.HasValue == true)
            {
                return false;
            }
        }

        if (mode == ProcessMatchMode.CheckAndConfirmChanges)
        {
            foreach (TilePosition foundTilePosition in foundTilesCache)
            {
                if (foundTilePosition == position)
                {
                    tiles[position].UpgradeTile(TileType.DestroyerSquare);
                }
                else
                {
                    tiles[foundTilePosition].ProcessSuccessMatch();
                }
            }
        }

        return true;
    }
}
