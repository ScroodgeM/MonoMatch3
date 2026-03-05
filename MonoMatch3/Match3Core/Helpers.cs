using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoMatch3.Match3Core.Tiles;

namespace MonoMatch3.Match3Core;

public static class Helpers
{
    public static bool IsPositionValid(int positionX, int positionY, GameSettings.Board boardSettings)
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

    public static bool TryScreenToBoard(this GameSettings gameSettings, Point screenPoint, out TilePosition tilePosition)
    {
        int tilePositionX = (int)MathF.Floor(screenPoint.X / gameSettings.view.cellSize);
        int tilePositionY = (int)MathF.Floor(screenPoint.Y / gameSettings.view.cellSize);

        if (IsPositionValid(tilePositionX, tilePositionY, gameSettings.board) == true)
        {
            tilePosition = new TilePosition((byte)tilePositionX, (byte)tilePositionY);
            return true;
        }

        tilePosition = default;
        return false;
    }

    public static Point BoardToScreen(this GameSettings gameSettings, TilePosition tilePosition)
    {
        return (
                new Vector2(
                    0.5f + tilePosition.X,
                    0.5f + tilePosition.Y
                )
                *
                gameSettings.view.cellSize
            )
            .ToPoint();
    }

    public static Vector2 BoardToScreen(this GameSettings gameSettings, Direction direction)
    {
        float cellSize = gameSettings.view.cellSize;
        return direction switch
        {
            Direction.Left => new Vector2(-cellSize, 0),
            Direction.Right => new Vector2(+cellSize, 0),
            Direction.Down => new Vector2(0, +cellSize),
            Direction.Up => new Vector2(0, -cellSize),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static string GetSpriteId(this GameSettings gameSettings, TileType tileType)
    {
        foreach (GameSettings.View.Tile tile in gameSettings.view.tiles)
        {
            if (tile.type == tileType)
            {
                return tile.spriteId;
            }
        }

        throw new KeyNotFoundException($"Sprite name for {tileType} not found");
    }
}
