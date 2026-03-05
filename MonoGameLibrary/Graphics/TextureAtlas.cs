using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public class TextureAtlas(Texture2D texture)
{
    private const string DefaultSpriteName = "default";

    private readonly Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    public IEnumerable<string> AllSpriteNames => sprites.Keys;

    public Sprite GetSprite(string name) => sprites[name];

    public static TextureAtlas FromFile(ContentManager content, string fileName)
    {
        string filePath = Path.Combine(content.RootDirectory, fileName);

        using Stream stream = TitleContainer.OpenStream(filePath);
        using XmlReader reader = XmlReader.Create(stream);

        XDocument doc = XDocument.Load(reader);
        XElement root = doc.Root;

        string texturePath = root.Element("Texture").Value;
        Texture2D texture = content.Load<Texture2D>(texturePath);
        TextureAtlas atlas = new TextureAtlas(texture);

        XElement regionsRoot = root.Element("Regions");
        if (regionsRoot != null)
        {
            foreach (XElement region in regionsRoot.Elements("Region"))
            {
                string name = XMLHelpers.GetStringOrDefault(region, "name");
                if (string.IsNullOrEmpty(name) == false)
                {
                    Rectangle sourceRectangle = new(
                        x: XMLHelpers.GetIntOrDefault(region, "x"),
                        y: XMLHelpers.GetIntOrDefault(region, "y"),
                        width: XMLHelpers.GetIntOrDefault(region, "width"),
                        height: XMLHelpers.GetIntOrDefault(region, "height")
                    );

                    Vector2 pivot = new(
                        x: XMLHelpers.GetFloatOrDefault(region, "pivotX"),
                        y: XMLHelpers.GetFloatOrDefault(region, "pivotY")
                    );

                    SpriteEffects effects = SpriteEffects.None;
                    if (XMLHelpers.GetBooleanOrDefault(region, "flipVertically") == true)
                    {
                        effects |= SpriteEffects.FlipVertically;
                    }

                    if (XMLHelpers.GetBooleanOrDefault(region, "flipHorizontally") == true)
                    {
                        effects |= SpriteEffects.FlipHorizontally;
                    }

                    atlas.sprites.Add(name, new Sprite(texture, sourceRectangle, pivot, effects));
                }
            }
        }

        if (atlas.sprites.Count == 0)
        {
            Rectangle sourceRectangle = new(0, 0, texture.Width, texture.Height);
            Vector2 pivot = new Vector2(texture.Width, texture.Height) * 0.5f;
            atlas.sprites.Add(DefaultSpriteName, new Sprite(texture, sourceRectangle, pivot, SpriteEffects.None));
        }

        return atlas;
    }
}
