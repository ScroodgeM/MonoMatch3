using System;
using MonoGameLibrary;
using MonoMatch3.Match3Core.Tiles;
using MonoMatch3Core.Enums;

namespace MonoMatch3.Match3Core;

public class TilesFactory(Settings settings, IGameEvents gameEvents, Board board, Random sessionRandomGenerator)
{
    public TileBase CreateRandom(TilePosition position)
    {
        int poolIndex = sessionRandomGenerator.Next(settings.board.generatorPool.Length);
        return Create(position, settings.board.generatorPool[poolIndex]);
    }

    public TileBase Create(TilePosition position, TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Simple1:
            case TileType.Simple2:
            case TileType.Simple3:
            case TileType.Simple4:
            case TileType.Simple5:
                return new Simple(settings, gameEvents, board, tileType, position);

            case TileType.DestroyerHorizontalLine:
                return new DestroyerHorizontalLine(settings, gameEvents, board, position);

            case TileType.DestroyerVerticalLine:
                return new DestroyerVerticalLine(settings, gameEvents, board, position);

            case TileType.DestroyerSquare:
                return new DestroyerSquare(settings, gameEvents, board, position);

            default:
                throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null);
        }
    }
}
