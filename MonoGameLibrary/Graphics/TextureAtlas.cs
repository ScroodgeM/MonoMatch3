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
    private readonly Texture2D texture = texture;
    private readonly Dictionary<string, TextureRegion> regions = new Dictionary<string, TextureRegion>();

    public TextureRegion GetRegion(string name) => regions[name];

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
        if (regionsRoot == null)
        {
            return atlas;
        }

        foreach (XElement region in regionsRoot.Elements("Region"))
        {
            string name = GetStringOrDefault(region, "name");
            if (string.IsNullOrEmpty(name) == false)
            {
                TextureRegion textureRegion = new TextureRegion(
                    texture: texture,
                    x: GetIntOrDefault(region, "x"),
                    y: GetIntOrDefault(region, "y"),
                    width: GetIntOrDefault(region, "width"),
                    height: GetIntOrDefault(region, "height"));
                atlas.regions.Add(name, textureRegion);
            }
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
}
