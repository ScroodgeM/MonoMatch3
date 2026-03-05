namespace MonoMatch3.Match3Core.Tiles;

public class Simple(TilePosition position, TileType tileType) : TileBase(position)
{
    public override TileType TileType => tileType;
}
