using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public static class AtlasLoader
{
    public static void Load(ContentManager content, string[] fileNames, SpriteRenderer spriteRenderer)
    {
        foreach (string fileName in fileNames)
        {
            Load(content, fileName, spriteRenderer);
        }
    }

    private static void Load(ContentManager content, string fileName, SpriteRenderer spriteRenderer)
    {
        fileName = Path.ChangeExtension(fileName, "json");
        string filePath = Path.Combine(content.RootDirectory, fileName);
        using Stream stream = TitleContainer.OpenStream(filePath);
        Load(content, JsonSerializer.Deserialize<AtlasDefinition>(stream), spriteRenderer);
    }

    private static void Load(ContentManager content, AtlasDefinition definition, SpriteRenderer spriteRenderer)
    {
        string texturePath = definition.texturePath;
        Texture2D texture = content.Load<Texture2D>(texturePath);

        int spritesFound = 0;

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

                Sprite sprite = new Sprite(texture, sourceRectangle, pivot, region.scale, effects);
                spriteRenderer.Pool.Add(region.name, sprite);

                spritesFound++;
            }
        }

        if (spritesFound == 0)
        {
            Rectangle sourceRectangle = new(0, 0, texture.Width, texture.Height);
            Vector2 pivot = new Vector2(texture.Width, texture.Height) * 0.5f;
            string spriteId = Path.GetFileNameWithoutExtension(texturePath);
            Sprite sprite = new Sprite(texture, sourceRectangle, pivot, 1f, SpriteEffects.None);
            spriteRenderer.Pool.Add(spriteId, sprite);
        }
    }
}
