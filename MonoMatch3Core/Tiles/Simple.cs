using MonoGameLibrary;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;

namespace MonoMatch3Core.Tiles;

internal class Simple(Settings settings, IGameEvents gameEvents, Board.Board board, TileType tileType, TilePosition position)
    : TileBase(settings, gameEvents, board, position)
{
    public override TileType TileType => tileType;

    internal override void ProcessSuccessMatch()
    {
        board.RemoveTile(position.Value, TileRemoveReason.SuccessMatch);
    }

    internal override void ChangeTypeTo(TileType newTileType)
    {
        board.ReplaceTile(position.Value, TileRemoveReason.SuccessMatch, newTileType);
    }
}
