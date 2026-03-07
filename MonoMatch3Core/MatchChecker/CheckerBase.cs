using System.Collections.Generic;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.Tiles;

namespace MonoMatch3Core.MatchChecker;

internal abstract class CheckerBase(Settings settings)
{
    internal abstract bool TryProcessMatch(Dictionary<TilePosition, TileBase> tiles, TilePosition position, ProcessMatchMode mode);

    protected int CollectTilesInBothDirections(Dictionary<TilePosition, TileBase> tiles, TilePosition position, Direction direction, TileColor color, HashSet<TilePosition> output)
    {
        int tilesFound = 0;

        foreach (TilePosition foundTile in CollectTilesOfTheSameColorInDirection(tiles, position, direction, color))
        {
            if (output.Add(foundTile) == true)
            {
                tilesFound++;
            }
        }

        foreach (TilePosition foundTile in CollectTilesOfTheSameColorInDirection(tiles, position, direction.Invert(), color))
        {
            if (output.Add(foundTile) == true)
            {
                tilesFound++;
            }
        }

        return tilesFound;
    }

    private IEnumerable<TilePosition> CollectTilesOfTheSameColorInDirection(Dictionary<TilePosition, TileBase> tiles, TilePosition position, Direction direction, TileColor color)
    {
        while (true)
        {
            if (settings.TryShift(position, direction, out TilePosition shiftedPosition) == false)
            {
                yield break;
            }

            if (tiles.TryGetValue(shiftedPosition, out TileBase tile) == false)
            {
                yield break;
            }

            if (tile.Color != color)
            {
                yield break;
            }

            yield return shiftedPosition;
            position = shiftedPosition;
        }
    }
}
