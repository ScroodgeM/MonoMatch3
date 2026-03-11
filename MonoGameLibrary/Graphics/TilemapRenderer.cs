using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace MonoGameLibrary.Graphics;

internal class TilemapRenderer(SpriteRenderer spriteRenderer)
{
    private readonly Dictionary<string, Tilemap> allTilemaps = new Dictionary<string, Tilemap>();

    internal void Load(ContentManager content, string fileName)
    {
        fileName = Path.ChangeExtension(fileName, "json");
        string filePath = Path.Combine(content.RootDirectory, fileName);
        using Stream stream = TitleContainer.OpenStream(filePath);
        Load(content, JsonSerializer.Deserialize<TilemapDefinition>(stream));
    }

    private void Load(ContentManager contentManager, TilemapDefinition definition)
    {
        Tilemap tilemap = Tilemap.Load(contentManager, definition, spriteRenderer);
        allTilemaps.Add(definition.tileset.name, tilemap);
    }

    internal void Show(string tilemapId, Transform transform)
    {
        allTilemaps[tilemapId].Show(spriteRenderer, transform);
    }

    internal void Hide(string tilemapId)
    {
        allTilemaps[tilemapId].Hide(spriteRenderer);
    }
}
