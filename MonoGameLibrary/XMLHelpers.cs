using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MonoGameLibrary;

public static class XMLHelpers
{
    public static string GetString(XElement container, string attributeName) =>
        container.Attribute(attributeName).Value;

    public static byte GetByte(XElement container, string attributeName) =>
        byte.Parse(container.Attribute(attributeName).Value);

    public static int GetInt(XElement container, string attributeName) =>
        int.Parse(container.Attribute(attributeName).Value);

    public static float GetFloat(XElement container, string attributeName) =>
        float.Parse(container.Attribute(attributeName).Value);

    public static bool GetBoolean(XElement container, string attributeName) =>
        bool.Parse(container.Attribute(attributeName).Value);

    public static T GetEnum<T>(XElement container, string attributeName) where T : struct, Enum =>
        Enum.Parse<T>(container.Attribute(attributeName).Value);

    public static T[] GetCustoms<T>(IEnumerable<XElement> elements, Func<XElement, T> constructor) =>
        new List<XElement>(elements).ConvertAll(x => constructor(x)).ToArray();

    public static T[] GetArray<T>(IEnumerable<XElement> elements, string attributeName, Func<XElement, string, T> valueExtractor) =>
        new List<XElement>(elements).ConvertAll(x => valueExtractor(x, attributeName)).ToArray();
}
