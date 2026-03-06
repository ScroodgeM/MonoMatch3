using System;
using MonoGameLibrary.StatefulEvent;
using MonoMatch3Core.Enums;

namespace MonoMatch3Core.Data;

public readonly struct TilePosition(byte x, byte y) : IEquatable<TilePosition>, IValue<TilePosition>
{
    public byte X => x;
    public byte Y => y;

    public TilePosition Shift(Direction direction)
    {
        byte newX = x;
        byte newY = y;

        switch (direction)
        {
            case Direction.Left:
                if (newX > byte.MinValue)
                    return new TilePosition(--newX, newY);
                break;

            case Direction.Right:
                if (newX < byte.MaxValue)
                    return new TilePosition(++newX, newY);
                break;

            case Direction.Down:
                if (newY < byte.MaxValue)
                    return new TilePosition(newX, ++newY);
                break;

            case Direction.Up:
                if (newY > byte.MinValue)
                    return new TilePosition(newX, --newY);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        throw new OverflowException($"can't shift {ToString()} to {direction}");
    }

    public static TilePosition Zero => new TilePosition(0, 0);

    public bool Equals(TilePosition other) => x == other.X && y == other.Y;

    public override bool Equals(object obj) => obj is TilePosition tilePosition && Equals(tilePosition);

    public override int GetHashCode() => (int)x << 8 | (int)y;

    public override string ToString() => $"{nameof(TilePosition)} (x={x} ,y={y})";
}
