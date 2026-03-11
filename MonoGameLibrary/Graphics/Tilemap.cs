using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public class Tilemap
{
    private readonly TilemapDefinition definition;
    private readonly string[] tiles;
    private readonly List<ushort> activeSprites = new List<ushort>();

    private Tilemap(TilemapDefinition definition, string[] tileset)
    {
        this.definition = definition;

        int tilesCount = definition.mapH * definition.mapW;

        this.tiles = new string[tilesCount];
        for (int i = 0; i < tilesCount; i++)
        {
            tiles[i] = tileset[definition.map[i]];
        }
    }

    public void Show(SpriteRenderer spriteRenderer, Transform transform)
    {
        if (activeSprites.Count > 0)
        {
            throw new InvalidOperationException($"tilemap {definition.tileset.name} already shown");
        }

        Vector2 tilemapPosition = transform.position;
        Vector2 tileSizeScaled = new Vector2(definition.tileset.tileW, definition.tileset.tileH) * transform.scale * definition.tileset.scale;

        for (int x = 0; x < definition.mapW; x++)
        {
            for (int y = 0; y < definition.mapH; y++)
            {
                string spriteId = tiles[y * definition.mapW + x];
                transform.position = tilemapPosition + new Vector2(x, y) * tileSizeScaled;
                activeSprites.Add(spriteRenderer.Add(spriteId, transform));
            }
        }
    }

    public void Hide(SpriteRenderer spriteRenderer)
    {
        foreach (ushort activeSprite in activeSprites)
        {
            spriteRenderer.Remove(activeSprite);
        }

        activeSprites.Clear();
    }

    internal static Tilemap Load(ContentManager content, TilemapDefinition definition, SpriteRenderer spriteRenderer)
    {
        return new Tilemap(definition, CreateTileset(content, definition.tileset, spriteRenderer));
    }

    private static string[] CreateTileset(ContentManager content, TilemapDefinition.TilesetDefinition definition, SpriteRenderer spriteRenderer)
    {
        int columns = definition.w / definition.tileW;
        int rows = definition.h / definition.tileH;

        int count = rows * columns;

        string[] tileset = new string[count];
        Texture2D texture = content.Load<Texture2D>(definition.texturePath);

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Rectangle rectangle = new Rectangle
                (
                    definition.x + x * definition.tileW,
                    definition.y + y * definition.tileH,
                    definition.tileW,
                    definition.tileH
                );

                string spriteId = $"{definition.name}_tile_x{x}_y{y}";
                Sprite sprite = new Sprite(texture, rectangle, Vector2.Zero, definition.scale, SpriteEffects.None);
                spriteRenderer.Pool.Add(spriteId, sprite);

                tileset[y * columns + x] = spriteId;
            }
        }

        return tileset;
    }
}
