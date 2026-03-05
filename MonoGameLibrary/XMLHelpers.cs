using System.Xml.Linq;

namespace MonoGameLibrary;

public static class XMLHelpers
{
    public static string GetStringOrDefault(XElement container, string attributeName, string defaultValue = null)
    {
        XAttribute xAttribute = container.Attribute(attributeName);
        return xAttribute != null ? xAttribute.Value : defaultValue;
    }

    public static byte GetByteOrDefault(XElement container, string attributeName, byte defaultValue = 0)
    {
        XAttribute xAttribute = container.Attribute(attributeName);
        return xAttribute != null && byte.TryParse(xAttribute.Value, out byte parsedValue) == true ? parsedValue : defaultValue;
    }

    public static int GetIntOrDefault(XElement container, string attributeName, int defaultValue = 0)
    {
        XAttribute xAttribute = container.Attribute(attributeName);
        return xAttribute != null && int.TryParse(xAttribute.Value, out int parsedValue) == true ? parsedValue : defaultValue;
    }

    public static float GetFloatOrDefault(XElement container, string attributeName, float defaultValue = 0f)
    {
        XAttribute xAttribute = container.Attribute(attributeName);
        return xAttribute != null && float.TryParse(xAttribute.Value, out float parsedValue) == true ? parsedValue : defaultValue;
    }

    public static bool GetBooleanOrDefault(XElement container, string attributeName, bool defaultValue = false)
    {
        XAttribute xAttribute = container.Attribute(attributeName);
        return xAttribute != null && bool.TryParse(xAttribute.Value, out bool parsedValue) == true ? parsedValue : defaultValue;
    }
}
