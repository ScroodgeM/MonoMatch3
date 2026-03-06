using MonoGameLibrary;
using MonoMatch3Core.Enums;

namespace MonoMatch3.Match3Core.Tiles;

public class DestroyerSquare(Settings settings, IGameEvents gameEvents, Board board, TilePosition position)
    : TileBase(settings, gameEvents, board, position)
{
    public override TileType TileType => TileType.DestroyerSquare;
    public override void ProcessSuccessMatch() => throw new System.InvalidOperationException("we should not be here");
    public override void ChangeTypeTo(TileType newTileType) => throw new System.InvalidOperationException("we should not be here");
}
