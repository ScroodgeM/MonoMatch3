using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

public class MouseInfo
{
    public MouseState PreviousState { get; private set; }
    public MouseState CurrentState { get; private set; }

    private readonly IGameEvents gameEvents;

    public MouseInfo(IGameEvents gameEvents)
    {
        this.gameEvents = gameEvents;

        PreviousState = new MouseState();
        CurrentState = Mouse.GetState();

        this.gameEvents.OnUpdate += OnUpdate;
    }

    public void Die()
    {
        this.gameEvents.OnUpdate -= OnUpdate;
    }

    private void OnUpdate(GameTime gameTime)
    {
        PreviousState = CurrentState;
        CurrentState = Mouse.GetState();
    }

    public Point Position
    {
        get => CurrentState.Position;
        set => SetPosition(value.X, value.Y);
    }

    public int X
    {
        get => CurrentState.X;
        set => SetPosition(value, CurrentState.Y);
    }

    public int Y
    {
        get => CurrentState.Y;
        set => SetPosition(CurrentState.X, value);
    }

    public Point PositionDelta => CurrentState.Position - PreviousState.Position;

    public int XDelta => CurrentState.X - PreviousState.X;

    public int YDelta => CurrentState.Y - PreviousState.Y;

    public bool WasMoved => PositionDelta != Point.Zero;

    public int ScrollWheel => CurrentState.ScrollWheelValue;

    public int ScrollWheelDelta => CurrentState.ScrollWheelValue - PreviousState.ScrollWheelValue;


    public bool IsButtonDown(MouseButton button)
    {
        switch (button)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton == ButtonState.Pressed;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Pressed;
            case MouseButton.Right:
                return CurrentState.RightButton == ButtonState.Pressed;
            case MouseButton.XButton1:
                return CurrentState.XButton1 == ButtonState.Pressed;
            case MouseButton.XButton2:
                return CurrentState.XButton2 == ButtonState.Pressed;
            default:
                return false;
        }
    }

    public bool IsButtonUp(MouseButton button)
    {
        switch (button)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton == ButtonState.Released;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Released;
            case MouseButton.Right:
                return CurrentState.RightButton == ButtonState.Released;
            case MouseButton.XButton1:
                return CurrentState.XButton1 == ButtonState.Released;
            case MouseButton.XButton2:
                return CurrentState.XButton2 == ButtonState.Released;
            default:
                return false;
        }
    }

    public bool WasButtonJustPressed(MouseButton button)
    {
        switch (button)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton == ButtonState.Pressed && PreviousState.LeftButton == ButtonState.Released;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Pressed && PreviousState.MiddleButton == ButtonState.Released;
            case MouseButton.Right:
                return CurrentState.RightButton == ButtonState.Pressed && PreviousState.RightButton == ButtonState.Released;
            case MouseButton.XButton1:
                return CurrentState.XButton1 == ButtonState.Pressed && PreviousState.XButton1 == ButtonState.Released;
            case MouseButton.XButton2:
                return CurrentState.XButton2 == ButtonState.Pressed && PreviousState.XButton2 == ButtonState.Released;
            default:
                return false;
        }
    }

    public bool WasButtonJustReleased(MouseButton button)
    {
        switch (button)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton == ButtonState.Released && PreviousState.LeftButton == ButtonState.Pressed;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Released && PreviousState.MiddleButton == ButtonState.Pressed;
            case MouseButton.Right:
                return CurrentState.RightButton == ButtonState.Released && PreviousState.RightButton == ButtonState.Pressed;
            case MouseButton.XButton1:
                return CurrentState.XButton1 == ButtonState.Released && PreviousState.XButton1 == ButtonState.Pressed;
            case MouseButton.XButton2:
                return CurrentState.XButton2 == ButtonState.Released && PreviousState.XButton2 == ButtonState.Pressed;
            default:
                return false;
        }
    }

    public void SetPosition(int x, int y)
    {
        Mouse.SetPosition(x, y);
        CurrentState = new MouseState(
            x,
            y,
            CurrentState.ScrollWheelValue,
            CurrentState.LeftButton,
            CurrentState.MiddleButton,
            CurrentState.RightButton,
            CurrentState.XButton1,
            CurrentState.XButton2
        );
    }
}
