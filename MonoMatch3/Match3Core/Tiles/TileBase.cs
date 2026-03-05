using System;

namespace MonoMatch3.Match3Core.Tiles;

public abstract class TileBase
{
    public event Action OnRemoved = () => { };

    public abstract TileType TileType { get; }
    public TilePosition Position => position;

    private TilePosition position;

    public TileBase(TilePosition position)
    {
        this.position = position;
    }

    public void Die()
    {
        OnRemoved();
    }
}
