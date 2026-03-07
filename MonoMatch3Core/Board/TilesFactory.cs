using System;
using MonoGameLibrary;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.Tiles;

namespace MonoMatch3Core.Board;

internal class TilesFactory(Settings settings, IGameEvents gameEvents, Board board, Random sessionRandomGenerator)
{
    internal TileBase CreateRandom(TilePosition position)
    {
        int poolIndex = sessionRandomGenerator.Next(settings.board.generatorPool.Length);
        return Create(TileType.Simple, settings.board.generatorPool[poolIndex], position);
    }

    internal TileBase Create(TileType type, TileColor color, TilePosition position)
    {
        switch (type)
        {
            case TileType.Simple:
                return new Simple(settings, gameEvents, board, color, position);

            case TileType.DestroyerHorizontalLine:
                return new DestroyerHorizontalLine(settings, gameEvents, board, color, position);

            case TileType.DestroyerVerticalLine:
                return new DestroyerVerticalLine(settings, gameEvents, board, color,position);

            case TileType.DestroyerSquare:
                return new DestroyerSquare(settings, gameEvents, board, color,position);

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
