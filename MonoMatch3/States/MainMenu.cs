using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Graphics.SpriteAnimations;
using MonoGameLibrary.Input;
using MonoMatch3.View;
using MonoMatch3Core.Data;

namespace MonoMatch3.States;

internal class MainMenu(
    SpriteRenderer spriteRenderer,
    TextRenderer textRenderer,
    InputManager inputManager,
    Settings settings,
    PresentationParameters presentationParameters)
    : BaseState(spriteRenderer, textRenderer, settings)
{
    private ScreenButton screenButton;

    internal override void Start()
    {
        Transform transform = Transform.Default;
        transform.position = new Vector2(presentationParameters.BackBufferWidth, presentationParameters.BackBufferHeight) * 0.5f;
        transform.layerDepth = RenderLayer.Background.ToLayerDepth();

        ushort spriteId = spriteRenderer.AddSprite(settings.system.mainMenuLogoSpriteId, transform);
        spriteRenderer.AddAnimation(spriteId, new PingPongColorChannels(0.5f, 1f, 0.20f, 0.25f, 0.33f));
        spriteRenderer.AddAnimation(spriteId, new PingPongScale(1.0f, 1.1f, 0.16f));
        RegisterSpriteToRemoveOnDeath(spriteId);

        ScreenButton.Transform screenButtonTransform;
        screenButtonTransform.position = new Vector2(settings.view.startButtonPositionX, settings.view.startButtonPositionY);
        screenButtonTransform.scale = Vector2.One;
        screenButtonTransform.spriteId = settings.system.buttonBackgroundSpriteId;
        screenButtonTransform.text = "Start";
        screenButtonTransform.spriteLayerDepth = RenderLayer.UIEements.ToLayerDepth();
        screenButtonTransform.textLayerDepth = RenderLayer.Text.ToLayerDepth();
        screenButton = inputManager.CreateScreenButton(screenButtonTransform);
        screenButton.OnClick += () => SwitchToState(State.Gameplay);
    }

    internal override void Die()
    {
        screenButton.Die();
        base.Die();
    }
}
