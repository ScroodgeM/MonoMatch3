using MonoGameLibrary;

namespace MonoMatch3.Match3Core.Tiles;

public class Simple(GameSettings gameSettings, IGameEvents gameEvents, TileType tileType, TilePosition position)
    : TileBase(gameSettings, gameEvents, position)
{
    public override TileType TileType => tileType;
}
