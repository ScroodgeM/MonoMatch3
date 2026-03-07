using MonoGameLibrary;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;

namespace MonoMatch3Core.Tiles;

internal class Simple(Settings settings, IGameEvents gameEvents, Board.Board board, TileColor tileColor, TilePosition position)
    : TileBase(settings, gameEvents, board, tileColor, position)
{
    public override TileType Type => TileType.Simple;

    internal override void ProcessSuccessMatch()
    {
        board.RemoveTile(position.Value, TileRemoveReason.SuccessMatch);
    }

    internal override void UpgradeTile(TileType newTileType)
    {
        board.ReplaceTile(position.Value, TileRemoveReason.SuccessMatch, newTileType);
    }

    internal override void DestroyBySpecial()
    {
        board.RemoveTile(position.Value, TileRemoveReason.DestroyedBySpecial);
    }
}
