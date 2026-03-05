using System.Collections.Generic;
using System.Data;

namespace MonoGameLibrary.Graphics;

public class SpritesPool
{
    private readonly Dictionary<string, Sprite> allSprites = new Dictionary<string, Sprite>();

    public void Add(TextureAtlas atlas)
    {
        foreach (string spriteId in atlas.AllSpriteNames)
        {
            if (allSprites.TryAdd(spriteId, atlas.GetSprite(spriteId)) == false)
            {
                throw new DuplicateNameException($"{nameof(SpritesPool)}: sprite with id '{spriteId}' already exists");
            }
        }
    }

    public Sprite Get(string spriteId)
    {
        if (allSprites.TryGetValue(spriteId, out Sprite sprite) == false)
        {
            throw new KeyNotFoundException($"{nameof(SpritesPool)}: sprite with id '{spriteId}' not found");
        }

        return sprite;
    }
}
