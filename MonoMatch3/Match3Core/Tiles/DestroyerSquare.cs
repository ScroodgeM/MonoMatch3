using MonoGameLibrary;

namespace MonoMatch3.Match3Core.Tiles;

public class DestroyerSquare(Settings settings, IGameEvents gameEvents, Board board, TilePosition position)
    : TileBase(settings, gameEvents, board, position)
{
    public override TileType TileType => TileType.DestroyerSquare;
}
