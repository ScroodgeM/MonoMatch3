using MonoGameLibrary;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.Specials;

namespace MonoMatch3Core.Tiles;

internal class DestroyerSquare(Settings settings, IGameEvents gameEvents, Board.Board board, TileColor tileColor, TilePosition position)
    : TileBase(settings, gameEvents, board, tileColor, position)
{
    public override TileType Type => TileType.DestroyerSquare;

    internal override void ProcessSuccessMatch()
    {
        MakeBoom(TileRemoveReason.SuccessMatch);
    }

    internal override void UpgradeTile(TileType newTileType)
    {
    }

    internal override void DestroyBySpecial()
    {
        MakeBoom(TileRemoveReason.DestroyedBySpecial);
    }

    private void MakeBoom(TileRemoveReason removeReason)
    {
        board.RegisterSpecial(new SquareBomb(settings, gameEvents, position.Value));
        board.RemoveTile(position.Value, removeReason);
    }
}
