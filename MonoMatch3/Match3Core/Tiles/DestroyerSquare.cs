using MonoGameLibrary;

namespace MonoMatch3.Match3Core.Tiles;

public class DestroyerSquare(GameSettings gameSettings, IGameEvents gameEvents, Board board, TilePosition position)
    : TileBase(gameSettings, gameEvents, board, position)
{
    public override TileType TileType => TileType.DestroyerSquare;
}
