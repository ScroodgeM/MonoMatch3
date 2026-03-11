using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.StatefulEvent;

namespace MonoGameLibrary.Graphics;

public class RenderSystem
{
    #region refactor this

#warning REFACTORING-IN-PROGRESS
    public SpriteRenderer SpriteRenderer => spriteRenderer;

    #endregion refactor this

    private readonly ContentManager contentManager;
    private readonly SpriteRenderer spriteRenderer;
    private readonly TilemapRenderer tilemapRenderer;
    private readonly TextRenderer textRenderer;

    private const uint spriteIdMarkerMask = 1 << 24;
    private const uint spriteIdMask = 0xFFFF;

    private const uint textIdMarkerMask = 1 << 25;
    private const uint textIdMask = 0xFF;

    internal RenderSystem(ContentManager contentManager)
    {
        this.contentManager = contentManager;
        this.spriteRenderer = new SpriteRenderer();
        this.tilemapRenderer = new TilemapRenderer(spriteRenderer);
        this.textRenderer = new TextRenderer();
    }

    internal void Init(SpriteBatch spriteBatch)
    {
        spriteRenderer.Init(spriteBatch);
        textRenderer.Init(spriteBatch);
    }

    internal void Draw(GameTime gameTime)
    {
        spriteRenderer.Draw(gameTime);
        textRenderer.Draw();
    }

    public void LoadTilemaps(IEnumerable<string> tilemapIds)
    {
        foreach (string tilemapId in tilemapIds)
        {
            LoadTilemap(tilemapId);
        }
    }

    public void LoadTilemap(string tilemapId) => tilemapRenderer.Load(contentManager, tilemapId);

    public void LoadFont(string fontName) => textRenderer.SetFont(contentManager.Load<SpriteFont>(fontName));

    public uint AddSprite(string spriteId, Transform transform) => spriteIdMarkerMask | spriteRenderer.Add(spriteId, transform);

    public uint AddText(IStatefulEvent<string> text, Transform transform) => textIdMarkerMask | textRenderer.Add(text, transform);

    public void ShowTilemap(string tilemapId, Transform transform) => tilemapRenderer.Show(tilemapId, transform);

    public void HideTilemap(string tilemapId) => tilemapRenderer.Hide(tilemapId);

    public void RemoveGraphic(IEnumerable<uint> graphicIds)
    {
        foreach (uint graphicId in graphicIds)
        {
            RemoveGraphic(graphicId);
        }
    }

    public void RemoveGraphic(uint graphicId)
    {
        if ((graphicId & spriteIdMarkerMask) == spriteIdMarkerMask)
        {
            spriteRenderer.Remove((ushort)(graphicId & spriteIdMask));
        }

        if ((graphicId & textIdMarkerMask) == textIdMarkerMask)
        {
            textRenderer.Remove((byte)(graphicId & textIdMask));
        }
    }

    internal Rectangle GetRectangle(uint graphicId)
    {
        if ((graphicId & spriteIdMarkerMask) == spriteIdMarkerMask)
        {
            return spriteRenderer.GetRectangle((ushort)(graphicId & spriteIdMask));
        }

        throw new NotSupportedException("only sprites currently supports GetRectangle method");
    }
}
