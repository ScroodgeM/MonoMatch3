namespace MonoMatch3.Match3Core.Tiles;

public class DestroyerSquare(TilePosition position) : TileBase(position)
{
    public override TileType TileType => TileType.DestroyerSquare;
}
