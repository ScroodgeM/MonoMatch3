using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.StatefulEvent;

namespace MonoGameLibrary.Input;

public class ScreenButton
{
    public struct Transform
    {
        public string spriteId;
        public Vector2 position;
        public Vector2 scale;
        public string text;
        public float spriteLayerDepth;
        public float textLayerDepth;
    }

    public event Action OnClick = () => { };

    private readonly SpriteRenderer spriteRenderer;
    private readonly TextRenderer textRenderer;
    private readonly IGameEvents gameEvents;
    private readonly MouseInfo mouseInfo;
    private readonly Transform transform;
    private readonly ushort mySpriteId;
    private readonly byte myTextId;

    internal ScreenButton(SpriteRenderer spriteRenderer, TextRenderer textRenderer, IGameEvents gameEvents, MouseInfo mouseInfo, Transform transform)
    {
        this.spriteRenderer = spriteRenderer;
        this.textRenderer = textRenderer;
        this.gameEvents = gameEvents;
        this.mouseInfo = mouseInfo;
        this.transform = transform;

        Graphics.Transform spriteTransform = Graphics.Transform.Default;
        spriteTransform.position = transform.position;
        spriteTransform.scale = transform.scale;
        spriteTransform.layerDepth = transform.spriteLayerDepth;
        mySpriteId = spriteRenderer.AddSprite(transform.spriteId, spriteTransform);

        Graphics.Transform textTransform = Graphics.Transform.Default;
        textTransform.position = transform.position;
        textTransform.scale = transform.scale;
        textTransform.layerDepth = transform.textLayerDepth;
        myTextId = textRenderer.AddText(StatefulEventInt.Create(transform.text), textTransform);

        gameEvents.OnUpdate += OnUpdate;
    }

    public void Die()
    {
        spriteRenderer.RemoveSprite(mySpriteId);
        textRenderer.RemoveText(myTextId);
        gameEvents.OnUpdate -= OnUpdate;
    }

    private void OnUpdate(GameTime gameTime)
    {
        if (mouseInfo.WasButtonJustPressed(MouseButton.Left) == true
            &&
            spriteRenderer
                .Pool
                .Get(transform.spriteId)
                .GetRectangle(transform.position)
                .Contains(mouseInfo.Position) == true)
        {
            OnClick();
        }
    }
}
