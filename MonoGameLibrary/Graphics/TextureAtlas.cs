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
                string name = GetStringOrDefault(region, "name");
                if (string.IsNullOrEmpty(name) == false)
                {
                    Rectangle sourceRectangle = new(
                        x: GetIntOrDefault(region, "x"),
                        y: GetIntOrDefault(region, "y"),
                        width: GetIntOrDefault(region, "width"),
                        height: GetIntOrDefault(region, "height")
                    );

                    Vector2 pivot = new(
                        x: GetFloatOrDefault(region, "pivotX"),
                        y: GetFloatOrDefault(region, "pivotY")
                    );

                    SpriteEffects effects = SpriteEffects.None;
                    if (GetBooleanOrDefault(region, "flipVertically") == true)
                    {
                        effects |= SpriteEffects.FlipVertically;
                    }

                    if (GetBooleanOrDefault(region, "flipHorizontally") == true)
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

    private static string GetStringOrDefault(XElement container, string attributeName, string defaultValue = null)
    {
        XAttribute xAttribute = container.Attribute(attributeName);
        return xAttribute == null ? defaultValue : xAttribute.Value;
    }

    private static int GetIntOrDefault(XElement container, string attributeName, int defaultValue = 0)
    {
        XAttribute xAttribute = container.Attribute(attributeName);
        return xAttribute == null ? defaultValue : int.Parse(xAttribute.Value);
    }

    private static float GetFloatOrDefault(XElement container, string attributeName, float defaultValue = 0f)
    {
        XAttribute xAttribute = container.Attribute(attributeName);
        return xAttribute == null ? defaultValue : float.Parse(xAttribute.Value);
    }

    private static bool GetBooleanOrDefault(XElement container, string attributeName, bool defaultValue = false)
    {
        XAttribute xAttribute = container.Attribute(attributeName);
        return xAttribute == null ? defaultValue : bool.Parse(xAttribute.Value);
    }
}
