using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public class TextureAtlas(Texture2D texture)
{
    private readonly Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    public IEnumerable<string> AllSpriteNames => sprites.Keys;

    public Sprite GetSprite(string name) => sprites[name];

    public static void Load(ContentManager content, string[] fileNames, SpritesPool spritesPool)
    {
        foreach (string fileName in fileNames)
        {
            spritesPool.Add(FromFile(content, fileName));
        }
    }

    public static TextureAtlas FromFile(ContentManager content, string fileName)
    {
        fileName = Path.ChangeExtension(fileName, "json");
        string filePath = Path.Combine(content.RootDirectory, fileName);
        using Stream stream = TitleContainer.OpenStream(filePath);
        AtlasDefinition definition = JsonSerializer.Deserialize<AtlasDefinition>(stream);

        string texturePath = definition.texturePath;
        Texture2D texture = content.Load<Texture2D>(texturePath);
        TextureAtlas atlas = new TextureAtlas(texture);

        if (definition.regions != null)
        {
            foreach (AtlasDefinition.Region region in definition.regions)
            {
                Rectangle sourceRectangle = new(region.x, region.y, region.w, region.h);

                Vector2 pivot = new(region.pivotX, region.pivotY);

                SpriteEffects effects = SpriteEffects.None;
                if (region.flipVertically == true)
                {
                    effects |= SpriteEffects.FlipVertically;
                }

                if (region.flipHorizontally == true)
                {
                    effects |= SpriteEffects.FlipHorizontally;
                }

                atlas.sprites.Add(region.name, new Sprite(texture, sourceRectangle, pivot, region.scale, effects));
            }
        }

        if (atlas.sprites.Count == 0)
        {
            Rectangle sourceRectangle = new(0, 0, texture.Width, texture.Height);
            Vector2 pivot = new Vector2(texture.Width, texture.Height) * 0.5f;
            atlas.sprites.Add(Path.GetFileNameWithoutExtension(texturePath), new Sprite(texture, sourceRectangle, pivot, 1f, SpriteEffects.None));
        }

        return atlas;
    }
}
