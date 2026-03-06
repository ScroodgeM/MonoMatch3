using MonoGameLibrary;

namespace MonoMatch3.Match3Core.Tiles;

public class DestroyerVerticalLine(Settings settings, IGameEvents gameEvents, Board board, TilePosition position)
    : TileBase(settings, gameEvents, board, position)
{
    public override TileType TileType => TileType.DestroyerVerticalLine;
    public override void ProcessSuccessMatch() => throw new System.InvalidOperationException("we should not be here");
    public override void ChangeTypeTo(TileType newTileType) => throw new System.InvalidOperationException("we should not be here");
}
