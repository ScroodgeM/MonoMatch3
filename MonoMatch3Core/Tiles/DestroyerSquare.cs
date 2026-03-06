using MonoGameLibrary;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;

namespace MonoMatch3Core.Tiles;

internal class DestroyerSquare(Settings settings, IGameEvents gameEvents, Board.Board board, TilePosition position)
    : TileBase(settings, gameEvents, board, position)
{
    public override TileType TileType => TileType.DestroyerSquare;

    internal override void ProcessSuccessMatch() => throw new System.InvalidOperationException("we should not be here");

    internal override void ChangeTypeTo(TileType newTileType) => throw new System.InvalidOperationException("we should not be here");
}
