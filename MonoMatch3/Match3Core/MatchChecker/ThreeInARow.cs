using System.Collections.Generic;
using MonoMatch3.Match3Core.Tiles;
using MonoMatch3Core.Enums;

namespace MonoMatch3.Match3Core.MatchChecker;

public class ThreeInARow(Settings settings, Direction lineDirection) : CheckerBase(settings)
{
    protected static readonly HashSet<TilePosition> foundTilesCache = new HashSet<TilePosition>();

    internal override bool TryProcessMatch(Dictionary<TilePosition, TileBase> tiles, TilePosition position)
    {
        if (tiles.TryGetValue(position, out TileBase mainTile) == false)
        {
            return false;
        }

        TileType lineType = mainTile.TileType;
        switch (lineType)
        {
            case TileType.Simple1:
            case TileType.Simple2:
            case TileType.Simple3:
            case TileType.Simple4:
            case TileType.Simple5:
                break;
            default:
                return false;
        }

        foundTilesCache.Clear();
        foundTilesCache.Add(position);
        CollectTilesOfTheSameTypeInDirection(tiles, position, lineDirection, lineType);
        CollectTilesOfTheSameTypeInDirection(tiles, position, Helpers.Invert(lineDirection), lineType);

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

    private void CollectTilesOfTheSameTypeInDirection(Dictionary<TilePosition, TileBase> tiles, TilePosition position, Direction direction, TileType tileType)
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

            if (tile.TileType != tileType)
            {
                return;
            }

            foundTilesCache.Add(shiftedPosition);
            position = shiftedPosition;
        }
    }
}
