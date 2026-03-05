using MonoGameLibrary;

namespace MonoMatch3.Match3Core.Tiles;

public class Simple(GameSettings gameSettings, IGameEvents gameEvents, Board board, TileType tileType, TilePosition position)
    : TileBase(gameSettings, gameEvents, board, position)
{
    public override TileType TileType => tileType;
}
