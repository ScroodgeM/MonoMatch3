using System;
using MonoGameLibrary.StatefulEvent;
using MonoMatch3Core.Enums;

namespace MonoMatch3Core.Data;

public readonly struct TilePosition(bool boarded, byte x, byte y) : IEquatable<TilePosition>, IValue<TilePosition>
{
    public static TilePosition Unboarded => new TilePosition(false, 0, 0);

    public bool Boarded => boarded;
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
                    return new TilePosition(boarded, --newX, newY);
                break;

            case Direction.Right:
                if (newX < byte.MaxValue)
                    return new TilePosition(boarded, ++newX, newY);
                break;

            case Direction.Down:
                if (newY < byte.MaxValue)
                    return new TilePosition(boarded, newX, ++newY);
                break;

            case Direction.Up:
                if (newY > byte.MinValue)
                    return new TilePosition(boarded, newX, --newY);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        throw new OverflowException($"can't shift {ToString()} to {direction}");
    }

    public bool IsNeighborOf(TilePosition other)
    {
        if (Boarded != other.Boarded)
        {
            return false;
        }

        if (X == other.X)
        {
            return Math.Abs(Y - other.Y) == 1;
        }

        if (Y == other.Y)
        {
            return Math.Abs(X - other.X) == 1;
        }

        return false;
    }

    public bool Equals(TilePosition other) => this == other;

    public override bool Equals(object obj) => obj is TilePosition tilePosition && Equals(tilePosition);

    public override int GetHashCode() => ((int)x << 9) | ((int)y << 1) | (boarded ? 1 : 0);

    public override string ToString() => $"{nameof(TilePosition)} (boarded={boarded}, x={x} ,y={y})";

    public static bool operator ==(TilePosition a, TilePosition b) => a.Boarded == b.Boarded && a.X == b.X && a.Y == b.Y;

    public static bool operator !=(TilePosition a, TilePosition b) => (a == b) == false;
}
