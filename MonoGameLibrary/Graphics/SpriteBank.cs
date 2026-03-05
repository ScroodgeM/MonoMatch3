using System.Collections.Generic;

namespace MonoGameLibrary.Graphics;

public class SpriteBank
{
    private readonly Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    public void Add(string spriteId, Sprite sprite)
    {
        sprites.Add(spriteId, sprite);
    }

    public bool TryGet(string spriteId, out Sprite sprite) => sprites.TryGetValue(spriteId, out sprite);
}
