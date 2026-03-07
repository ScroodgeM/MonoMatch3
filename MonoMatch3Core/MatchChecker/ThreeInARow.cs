using System.Collections.Generic;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.Tiles;

namespace MonoMatch3Core.MatchChecker;

internal class ThreeInARow(Settings settings, Direction direction) : SingleRowChecker(settings, direction)
{
    protected override bool ValidateTilesFound(HashSet<TilePosition> tiles) => tiles.Count == 3;

    protected override void ConfirmMatchEffect(TileBase tile, bool isTriggerTile) => tile.ProcessSuccessMatch();
}
