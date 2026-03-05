using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public class SpriteRenderer
{
    private struct SpriteData
    {
        public Sprite sprite;
        public Sprite.Transform transform;
    }

    private SpriteBatch spriteBatch;
    private ushort animationsIncrementalId = 0;
    private readonly Dictionary<ushort, SpriteData> allSprites = new Dictionary<ushort, SpriteData>();

    internal void Init(SpriteBatch spriteBatch)
    {
        this.spriteBatch = spriteBatch;
    }

    public ushort Add(Sprite sprite, Sprite.Transform transform)
    {
        if (allSprites.Count >= ushort.MaxValue)
        {
            throw new NotSupportedException($"Sorry, you reached the maximum number of simultaneous sprite: {ushort.MaxValue}.");
        }

        SpriteData newData;
        newData.sprite = sprite;
        newData.transform = transform;

        while (allSprites.TryAdd(animationsIncrementalId, newData) == false)
        {
            unchecked
            {
                animationsIncrementalId++;
            }
        }

        return animationsIncrementalId;
    }

    public void Update(ushort spriteId, Sprite.Transform transform)
    {
        if (allSprites.TryGetValue(spriteId, out SpriteData data) == true)
        {
            data.transform = transform;
            allSprites[spriteId] = data;
        }
    }

    public void Remove(ushort spriteId)
    {
        allSprites.Remove(spriteId);
    }

    public void RemoveAll()
    {
        allSprites.Clear();
    }

    public void Draw(GameTime gameTime)
    {
        spriteBatch.Begin(sortMode: SpriteSortMode.BackToFront);

        foreach (SpriteData spriteData in allSprites.Values)
        {
            spriteData.sprite.Draw(spriteBatch, spriteData.transform, gameTime);
        }

        spriteBatch.End();
    }
}
