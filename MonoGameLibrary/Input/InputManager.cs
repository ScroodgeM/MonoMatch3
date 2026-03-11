using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.Input;

public class InputManager(IGameEvents gameEvents, RenderSystem renderSystem)
{
    public MouseInfo Mouse { get; private set; } = new(gameEvents);

    public ScreenButton CreateScreenButton(ScreenButton.Transform transform)
    {
        return new ScreenButton(renderSystem, gameEvents, Mouse, transform);
    }
}
