using System;
using MonoGameLibrary.StatefulEvent;
using MonoMatch3Core.Enums;

namespace MonoMatch3Core.Data;

public struct TileState : IValue<TileState>
{
    public Movement? movement;

    public struct Movement
    {
        public Direction direction;
        public TimeSpan startTime;
        public TimeSpan finishTime;

        public static bool operator ==(Movement a, Movement b) =>
            a.direction == b.direction
            &&
            a.startTime == b.startTime
            &&
            a.finishTime == b.finishTime;

        public static bool operator !=(Movement a, Movement b) => (a == b) == false;
    }

    public static TileState Default
    {
        get
        {
            TileState result;
            result.movement = null;
            return result;
        }
    }

    public bool Equals(TileState other)
    {
        if (movement.HasValue == false && other.movement.HasValue == false)
        {
            return true;
        }

        if (movement.HasValue == true && other.movement.HasValue == true)
        {
            return movement.Value == other.movement.Value;
        }

        return false;
    }
}
