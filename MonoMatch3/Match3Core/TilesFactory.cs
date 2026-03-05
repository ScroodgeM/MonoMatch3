using System;
using MonoMatch3.Match3Core.Tiles;

namespace MonoMatch3.Match3Core;

public class TilesFactory(Random sessionRandomGenerator, TileType[] generatorPool)
{
    public TileBase CreateRandom(TilePosition position)
    {
        int poolIndex = sessionRandomGenerator.Next(generatorPool.Length);
        return Create(position, generatorPool[poolIndex]);
    }

    public static TileBase Create(TilePosition position, TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Simple1:
            case TileType.Simple2:
            case TileType.Simple3:
            case TileType.Simple4:
            case TileType.Simple5:
                return new Simple(position, tileType);

            case TileType.DestroyerHorizontalLine:
                return new DestroyerHorizontalLine(position);

            case TileType.DestroyerVerticalLine:
                return new DestroyerVerticalLine(position);

            case TileType.DestroyerSquare:
                return new DestroyerSquare(position);

            default:
                throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null);
        }
    }
}
