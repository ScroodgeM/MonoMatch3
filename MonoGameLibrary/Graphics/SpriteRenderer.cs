using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics.SpriteAnimations;

namespace MonoGameLibrary.Graphics;

public class SpriteRenderer
{
    private struct SpriteData
    {
        public Sprite sprite;
        public Transform transform;
        public Sprite.Animator animator;
    }

    internal SpritesPool Pool => spritesPool;

    private readonly SpritesPool spritesPool = new SpritesPool();
    private SpriteBatch spriteBatch;
    private ushort spriteIncrementalId = 0;
    private readonly Dictionary<ushort, SpriteData> allSprites = new Dictionary<ushort, SpriteData>();

    internal void Init(SpriteBatch spriteBatch)
    {
        this.spriteBatch = spriteBatch;
    }

    public ushort AddSprite(string spriteId, Transform transform)
    {
        if (allSprites.Count >= ushort.MaxValue)
        {
            throw new NotSupportedException($"Sorry, you reached the maximum number of simultaneous sprite: {ushort.MaxValue}.");
        }

        SpriteData newData;
        newData.sprite = spritesPool.Get(spriteId);
        newData.transform = transform;
        newData.animator = null;

        while (allSprites.TryAdd(spriteIncrementalId, newData) == false)
        {
            unchecked
            {
                spriteIncrementalId++;
            }
        }

        return spriteIncrementalId;
    }

    public void UpdateTransform(ushort spriteId, Transform transform)
    {
        if (allSprites.TryGetValue(spriteId, out SpriteData data) == true)
        {
            data.transform = transform;
            allSprites[spriteId] = data;
        }
    }

    public void AddAnimation(ushort spriteId, SpriteAnimationBase animation)
    {
        if (allSprites.TryGetValue(spriteId, out SpriteData data) == true)
        {
            Sprite.Animator animator = data.animator == null ? new Sprite.Animator() : data.animator;
            animator.Add(animation);
            data.animator = animator;
            allSprites[spriteId] = data;
        }
    }

    public void RemoveSprite(ushort spriteId)
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
            spriteData.sprite.Draw(spriteBatch, spriteData.transform, spriteData.animator, gameTime);
        }

        spriteBatch.End();
    }
}
