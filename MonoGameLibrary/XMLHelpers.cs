using System;
using System.Collections.Generic;
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

    public static T[] GetEnumsValues<T>(IEnumerable<XElement> elements, string attributeName) where T : struct, Enum
    {
        List<T> result = new List<T>();
        foreach (XElement element in elements)
        {
            result.Add(GetEnumOrDefault<T>(element, attributeName));
        }

        return result.ToArray();
    }

    public static T GetEnumOrDefault<T>(XElement container, string attributeName, T defaultValue = default) where T : struct, Enum
    {
        XAttribute xAttribute = container.Attribute(attributeName);
        return xAttribute != null && Enum.TryParse(xAttribute.Value, out T parsedValue) == true ? parsedValue : defaultValue;
    }

    public static T[] GetCustoms<T>(IEnumerable<XElement> elements, Func<XElement, T> constructor)
    {
        List<T> result = new List<T>();
        foreach (XElement element in elements)
        {
            result.Add(constructor(element));
        }

        return result.ToArray();
    }
}
