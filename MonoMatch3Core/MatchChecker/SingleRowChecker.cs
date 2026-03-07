using System.Collections.Generic;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.Tiles;

namespace MonoMatch3Core.MatchChecker;

internal abstract class SingleRowChecker(Settings settings, Direction direction) : CheckerBase(settings)
{
    private static readonly HashSet<TilePosition> foundTilesCache = new HashSet<TilePosition>();

    internal override bool TryProcessMatch(Dictionary<TilePosition, TileBase> tiles, TilePosition position, ProcessMatchMode mode)
    {
        if (tiles.TryGetValue(position, out TileBase mainTile) == false)
        {
            return false;
        }

        foundTilesCache.Clear();
        foundTilesCache.Add(position);
        CollectTilesInBothDirections(tiles, position, direction, mainTile.Color, foundTilesCache);

        foreach (TilePosition foundTilePosition in foundTilesCache)
        {
            if (tiles[foundTilePosition].State.Value.movement.HasValue == true)
            {
                return false;
            }
        }

        if (ValidateTilesFound(foundTilesCache) == false)
        {
            return false;
        }

        if (mode == ProcessMatchMode.CheckAndConfirmChanges)
        {
            foreach (TilePosition tilePosition in foundTilesCache)
            {
                ConfirmMatchEffect(tiles[tilePosition], tilePosition == position);
            }
        }

        return true;
    }

    protected abstract bool ValidateTilesFound(HashSet<TilePosition> tiles);

    protected abstract void ConfirmMatchEffect(TileBase tile, bool isTriggerTile);
}
