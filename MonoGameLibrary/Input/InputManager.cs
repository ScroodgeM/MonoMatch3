using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.Input;

public class InputManager
{
    public MouseInfo Mouse { get; private set; }

    private readonly IGameEvents gameEvents;
    private readonly SpriteRenderer spriteRenderer;
    private readonly TextRenderer textRenderer;

    public InputManager(IGameEvents gameEvents, SpriteRenderer spriteRenderer, TextRenderer textRenderer)
    {
        this.gameEvents = gameEvents;
        this.spriteRenderer = spriteRenderer;
        this.textRenderer = textRenderer;

        Mouse = new MouseInfo(gameEvents);
    }

    public ScreenButton CreateScreenButton(ScreenButton.Transform transform)
    {
        return new ScreenButton(spriteRenderer, textRenderer, gameEvents, Mouse, transform);
    }
}
