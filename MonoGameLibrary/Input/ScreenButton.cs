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

    private readonly RenderSystem renderSystem;
    private readonly IGameEvents gameEvents;
    private readonly MouseInfo mouseInfo;
    private readonly Transform transform;
    private readonly uint mySpriteId;
    private readonly uint myTextId;

    internal ScreenButton(RenderSystem renderSystem, IGameEvents gameEvents, MouseInfo mouseInfo, Transform transform)
    {
        this.renderSystem = renderSystem;
        this.renderSystem = renderSystem;
        this.gameEvents = gameEvents;
        this.mouseInfo = mouseInfo;
        this.transform = transform;

        Graphics.Transform spriteTransform = Graphics.Transform.Default;
        spriteTransform.position = transform.position;
        spriteTransform.scale = transform.scale;
        spriteTransform.layerDepth = transform.spriteLayerDepth;
        mySpriteId = renderSystem.AddSprite(transform.spriteId, spriteTransform);

        Graphics.Transform textTransform = Graphics.Transform.Default;
        textTransform.position = transform.position;
        textTransform.scale = transform.scale;
        textTransform.layerDepth = transform.textLayerDepth;
        myTextId = renderSystem.AddText(StatefulEventInt.Create(transform.text), textTransform);

        gameEvents.OnUpdate += OnUpdate;
    }

    public void Die()
    {
        renderSystem.RemoveGraphic(mySpriteId);
        renderSystem.RemoveGraphic(myTextId);
        gameEvents.OnUpdate -= OnUpdate;
    }

    private void OnUpdate(GameTime gameTime)
    {
        if (mouseInfo.WasButtonJustPressed(MouseButton.Left) == true
            &&
            renderSystem.GetRectangle(mySpriteId).Contains(mouseInfo.Position) == true)
        {
            OnClick();
        }
    }
}
