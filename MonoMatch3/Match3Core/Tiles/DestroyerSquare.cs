using MonoGameLibrary;

namespace MonoMatch3.Match3Core.Tiles;

public class DestroyerSquare(GameSettings gameSettings, IGameEvents gameEvents, TilePosition position)
    : TileBase(gameSettings, gameEvents, position)
{
    public override TileType TileType => TileType.DestroyerSquare;
}
