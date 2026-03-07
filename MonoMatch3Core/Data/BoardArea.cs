namespace MonoMatch3Core.Data;

public readonly struct BoardArea(byte width, byte height)
{
    public readonly byte TopLineY = 0;
    public readonly byte BottomLineY = (byte)(height - 1);

    public readonly byte LeftLineX = 0;
    public readonly byte RightLineX = (byte)(width - 1);

    public bool Contains(TilePosition position) => Contains(position.X, position.Y);

    public bool Contains(int x, int y)
    {
        return x >= 0
               &&
               x < width
               &&
               y >= 0
               &&
               y < height;
    }
}
