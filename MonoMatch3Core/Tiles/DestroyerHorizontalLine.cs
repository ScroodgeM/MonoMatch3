using MonoGameLibrary;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.Specials;

namespace MonoMatch3Core.Tiles;

internal class DestroyerHorizontalLine(Settings settings, IGameEvents gameEvents, Board.Board board, TileColor tileColor, TilePosition position)
    : TileBase(settings, gameEvents, board, tileColor, position)
{
    public override TileType Type => TileType.DestroyerHorizontalLine;

    internal override void ProcessSuccessMatch()
    {
        MakeBoom(TileRemoveReason.DestroyedBySpecial);
    }

    internal override void UpgradeTile(TileType newTileType)
    {
        ProcessSuccessMatch();
    }

    internal override void DestroyBySpecial()
    {
        MakeBoom(TileRemoveReason.DestroyedBySpecial);
    }

    private void MakeBoom(TileRemoveReason removeReason)
    {
        board.RegisterSpecial(new LineDestroyer(settings, gameEvents, position.Value, Direction.Left));
        board.RegisterSpecial(new LineDestroyer(settings, gameEvents, position.Value, Direction.Right));
        board.RemoveTile(position.Value, removeReason);
    }
}
