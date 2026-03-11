using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.StatefulEvent;

namespace MonoGameLibrary.Graphics;

internal class TextRenderer
{
    private struct TextData
    {
        public IStatefulEvent<string> text;
        public Transform transform;
    }

    private SpriteBatch spriteBatch;
    private SpriteFont font;
    private byte textIncrementalId = 0;
    private readonly Dictionary<byte, TextData> allTexts = new Dictionary<byte, TextData>();

    internal void Init(SpriteBatch spriteBatch)
    {
        this.spriteBatch = spriteBatch;
    }

    internal void SetFont(SpriteFont font)
    {
        this.font = font;
    }

    internal byte AddText(IStatefulEvent<string> text, Transform transform)
    {
        if (allTexts.Count >= byte.MaxValue)
        {
            throw new NotSupportedException($"Sorry, you reached the maximum number of simultaneous texts: {byte.MaxValue}.");
        }

        TextData newData;
        newData.text = text;
        newData.transform = transform;

        while (allTexts.TryAdd(textIncrementalId, newData) == false)
        {
            unchecked
            {
                textIncrementalId++;
            }
        }

        return textIncrementalId;
    }

    internal void UpdateTransform(byte textId, Transform transform)
    {
        if (allTexts.TryGetValue(textId, out TextData data) == true)
        {
            data.transform = transform;
            allTexts[textId] = data;
        }
    }

    internal void RemoveText(byte spriteId)
    {
        allTexts.Remove(spriteId);
    }

    internal void Draw()
    {
        spriteBatch.Begin(sortMode: SpriteSortMode.BackToFront);

        foreach (TextData textData in allTexts.Values)
        {
            string text = textData.text.Value;
            Transform transform = textData.transform;

            spriteBatch.DrawString(
                font,
                text,
                transform.position,
                transform.color,
                transform.rotation,
                font.MeasureString(text) * 0.5f,
                transform.scale,
                SpriteEffects.None,
                transform.layerDepth
            );
        }

        spriteBatch.End();
    }
}
