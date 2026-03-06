using MonoGameLibrary;

namespace MonoMatch3.Match3Core.Tiles;

public class Simple(Settings settings, IGameEvents gameEvents, Board board, TileType tileType, TilePosition position)
    : TileBase(settings, gameEvents, board, position)
{
    public override TileType TileType => tileType;
    public override void ProcessSuccessMatch()
    {
        board.RemoveTile(this.Position.Value, TileRemoveReason.SuccessMatch);
    }
}
