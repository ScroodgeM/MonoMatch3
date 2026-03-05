using MonoGameLibrary;

namespace MonoMatch3.Match3Core.Tiles;

public class DestroyerVerticalLine(GameSettings gameSettings, IGameEvents gameEvents, TilePosition position)
    : TileBase(gameSettings, gameEvents, position)
{
    public override TileType TileType => TileType.DestroyerVerticalLine;
}
