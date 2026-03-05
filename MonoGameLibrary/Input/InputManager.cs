namespace MonoGameLibrary.Input;

public class InputManager
{
    public KeyboardInfo Keyboard { get; private set; }
    public MouseInfo Mouse { get; private set; }

    public InputManager(IGameEvents gameEvents)
    {
        Keyboard = new KeyboardInfo(gameEvents);
        Mouse = new MouseInfo(gameEvents);
    }
}
