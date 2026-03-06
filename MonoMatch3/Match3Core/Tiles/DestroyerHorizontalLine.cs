using MonoGameLibrary;

namespace MonoMatch3.Match3Core.Tiles;

public class DestroyerHorizontalLine(Settings settings, IGameEvents gameEvents, Board board, TilePosition position)
    : TileBase(settings, gameEvents, board, position)
{
    public override TileType TileType => TileType.DestroyerHorizontalLine;
    public override void ProcessSuccessMatch() => throw new System.InvalidOperationException("we should not be here");
}
