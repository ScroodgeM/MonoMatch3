using System.Collections.Generic;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.Tiles;

namespace MonoMatch3Core.MatchChecker;

internal class ThreeInARow(Settings settings, Direction lineDirection) : CheckerBase(settings)
{
    protected static readonly HashSet<TilePosition> foundTilesCache = new HashSet<TilePosition>();

    internal override bool TryProcessMatch(Dictionary<TilePosition, TileBase> tiles, TilePosition position)
    {
        if (tiles.TryGetValue(position, out TileBase mainTile) == false)
        {
            return false;
        }

        foundTilesCache.Clear();
        foundTilesCache.Add(position);
        CollectTilesOfTheSameColorInDirection(tiles, position, lineDirection, mainTile.Color);
        CollectTilesOfTheSameColorInDirection(tiles, position, lineDirection.Invert(), mainTile.Color);

        foreach (TilePosition foundTilePosition in foundTilesCache)
        {
            if (tiles[foundTilePosition].State.Value.movement.HasValue == true)
            {
                return false;
            }
        }

        return ProcessFoundTiles(tiles, position);
    }

    protected virtual bool ProcessFoundTiles(Dictionary<TilePosition, TileBase> tiles, TilePosition position)
    {
        if (foundTilesCache.Count != 3)
        {
            return false;
        }

        foreach (TilePosition foundTilePosition in foundTilesCache)
        {
            tiles[foundTilePosition].ProcessSuccessMatch();
        }

        return true;
    }

    private void CollectTilesOfTheSameColorInDirection(Dictionary<TilePosition, TileBase> tiles, TilePosition position, Direction direction, TileColor tileColor)
    {
        while (true)
        {
            if (settings.TryShift(position, direction, out TilePosition shiftedPosition) == false)
            {
                return;
            }

            if (tiles.TryGetValue(shiftedPosition, out TileBase tile) == false)
            {
                return;
            }

            if (tile.Color != tileColor)
            {
                return;
            }

            foundTilesCache.Add(shiftedPosition);
            position = shiftedPosition;
        }
    }
}
