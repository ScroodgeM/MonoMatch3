using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;

namespace MonoMatch3.View;

public static class Helpers
{
    public static void GetSpriteView(this Settings settings, TileType tileType, TileColor tileColor, out string spriteId, out Color tintColor)
    {
        foreach (Settings.View.TypedTile typedTile in settings.view.tiles)
        {
            if (typedTile.type != tileType)
            {
                continue;
            }

            foreach (Settings.View.TypedTile.ColoredTile coloredTile in typedTile.perColor)
            {
                if (coloredTile.color != tileColor)
                {
                    continue;
                }

                spriteId = coloredTile.spriteId;
                tintColor = coloredTile.tintColor;
                return;
            }
        }

        throw new KeyNotFoundException($"Sprite name for {tileType} not found");
    }

    public static float ToLayerDepth(this RenderLayer renderLayer)
    {
        return (float)renderLayer / (float)RenderLayer.MaxValue;
    }
}
