using System;
using MonoGameLibrary;
using MonoMatch3.Match3Core.Tiles;

namespace MonoMatch3.Match3Core;

public class TilesFactory(GameSettings gameSettings, IGameEvents gameEvents, Random sessionRandomGenerator, TileType[] generatorPool)
{
    public TileBase CreateRandom(TilePosition position)
    {
        int poolIndex = sessionRandomGenerator.Next(generatorPool.Length);
        return Create(position, generatorPool[poolIndex]);
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
                return new Simple(gameSettings, gameEvents, tileType, position);

            case TileType.DestroyerHorizontalLine:
                return new DestroyerHorizontalLine(gameSettings, gameEvents, position);

            case TileType.DestroyerVerticalLine:
                return new DestroyerVerticalLine(gameSettings, gameEvents, position);

            case TileType.DestroyerSquare:
                return new DestroyerSquare(gameSettings, gameEvents, position);

            default:
                throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null);
        }
    }
}
