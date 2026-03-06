using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoMatch3.Match3Core.Tiles;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;

namespace MonoMatch3.Match3Core;

public static class Helpers
{
    public static Direction Invert(Direction origin)
    {
        return origin switch
        {
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            Direction.Down => Direction.Up,
            Direction.Up => Direction.Down,
            _ => throw new ArgumentOutOfRangeException(nameof(origin), origin, null)
        };
    }

    public static bool TryShift(this Settings settings, TilePosition position, Direction direction, out TilePosition shiftedPosition)
    {
        bool canShift =
            (direction == Direction.Left && position.X > 0)
            ||
            (direction == Direction.Right && position.X < settings.board.width - 1)
            ||
            (direction == Direction.Up && position.Y > 0)
            ||
            (direction == Direction.Down && position.Y < settings.board.height - 1);

        shiftedPosition = canShift ? position.Shift(direction) : default;
        return canShift;
    }

    public static bool IsPositionValid(int positionX, int positionY, Settings.Board boardSettings)
    {
        return
            positionX >= 0
            &&
            positionX < boardSettings.width
            &&
            positionY >= 0
            &&
            positionY < boardSettings.height;
    }

    public static bool TryScreenToBoard(this Settings settings, Point screenPoint, out TilePosition tilePosition)
    {
        int tilePositionX = (int)MathF.Floor(screenPoint.X / settings.view.cellSize);
        int tilePositionY = (int)MathF.Floor(screenPoint.Y / settings.view.cellSize);

        if (IsPositionValid(tilePositionX, tilePositionY, settings.board) == true)
        {
            tilePosition = new TilePosition((byte)tilePositionX, (byte)tilePositionY);
            return true;
        }

        tilePosition = default;
        return false;
    }

    public static Point BoardToScreen(this Settings settings, TilePosition tilePosition)
    {
        return (
                new Vector2(
                    0.5f + tilePosition.X,
                    0.5f + tilePosition.Y
                )
                *
                settings.view.cellSize
            )
            .ToPoint();
    }

    public static Vector2 BoardToScreen(this Settings settings, Direction direction)
    {
        float cellSize = settings.view.cellSize;
        return direction switch
        {
            Direction.Left => new Vector2(-cellSize, 0),
            Direction.Right => new Vector2(+cellSize, 0),
            Direction.Down => new Vector2(0, +cellSize),
            Direction.Up => new Vector2(0, -cellSize),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static string GetSpriteId(this Settings settings, TileType tileType)
    {
        foreach (Settings.View.Tile tile in settings.view.tiles)
        {
            if (tile.type == tileType)
            {
                return tile.spriteId;
            }
        }

        throw new KeyNotFoundException($"Sprite name for {tileType} not found");
    }
}
