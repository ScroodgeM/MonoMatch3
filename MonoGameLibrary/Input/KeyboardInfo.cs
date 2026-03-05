using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

public class KeyboardInfo
{
    public KeyboardState PreviousState { get; private set; }
    public KeyboardState CurrentState { get; private set; }

    private readonly IGameEvents gameEvents;

    public KeyboardInfo(IGameEvents gameEvents)
    {
        this.gameEvents = gameEvents;

        PreviousState = new KeyboardState();
        CurrentState = Keyboard.GetState();

        this.gameEvents.OnUpdate += OnUpdate;
    }

    public void Die()
    {
        this.gameEvents.OnUpdate -= OnUpdate;
    }

    private void OnUpdate(GameTime gameTime)
    {
        PreviousState = CurrentState;
        CurrentState = Keyboard.GetState();
    }

    public bool IsKeyDown(Keys key) => CurrentState.IsKeyDown(key);

    public bool IsKeyUp(Keys key) => CurrentState.IsKeyUp(key);

    public bool WasKeyJustPressed(Keys key) => CurrentState.IsKeyDown(key) && PreviousState.IsKeyUp(key);

    public bool WasKeyJustReleased(Keys key) => CurrentState.IsKeyUp(key) && PreviousState.IsKeyDown(key);
}
