using System.Collections.Generic;
using System.Data;

namespace MonoGameLibrary.Graphics;

internal class SpritesPool
{
    private readonly Dictionary<string, Sprite> allSprites = new Dictionary<string, Sprite>();

    internal void Add(string spriteId, Sprite sprite)
    {
        if (allSprites.TryAdd(spriteId, sprite) == false)
        {
            throw new DuplicateNameException($"{nameof(SpritesPool)}: sprite with id '{spriteId}' already exists");
        }
    }

    internal Sprite Get(string spriteId)
    {
        if (allSprites.TryGetValue(spriteId, out Sprite sprite) == false)
        {
            throw new KeyNotFoundException($"{nameof(SpritesPool)}: sprite with id '{spriteId}' not found");
        }

        return sprite;
    }
}
