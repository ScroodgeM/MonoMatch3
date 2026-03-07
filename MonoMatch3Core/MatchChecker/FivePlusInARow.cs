using System.Collections.Generic;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.Tiles;

namespace MonoMatch3Core.MatchChecker;

internal class FivePlusInARow(Settings settings, Direction lineDirection, TileType specialBonus) : SingleRowChecker(settings, lineDirection)
{
    protected override bool ValidateTilesFound(HashSet<TilePosition> tiles) => tiles.Count >= 5;

    protected override void ConfirmMatchEffect(TileBase tile, bool isTriggerTile)
    {
        if (isTriggerTile == true)
        {
            tile.UpgradeTile(specialBonus);
        }
        else
        {
            tile.ProcessSuccessMatch();
        }
    }
}
