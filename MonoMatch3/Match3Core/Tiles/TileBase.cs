using System;
using MonoGameLibrary;
using MonoGameLibrary.StatefulEvent;

namespace MonoMatch3.Match3Core.Tiles;

public abstract class TileBase
{
    public abstract TileType TileType { get; }
    public IStatefulEvent<TilePosition> Position => position;
    public IStatefulEvent<TileState> State => state;

    private byte bottomLinePositionY => (byte)(gameSettings.board.height - 1);

    private readonly GameSettings gameSettings;
    private readonly IGameEvents gameEvents;
    private readonly Board board;
    private readonly StatefulEventInt<TilePosition> position = StatefulEventInt.CreateGenericStruct(TilePosition.Zero);
    private readonly StatefulEventInt<TileState> state = StatefulEventInt.CreateGenericStruct(TileState.Default);

    public TileBase(GameSettings gameSettings, IGameEvents gameEvents, Board board, TilePosition position)
    {
        this.gameSettings = gameSettings;
        this.gameEvents = gameEvents;
        this.board = board;
        this.position.Set(position);

        this.gameEvents.CurrentTime.OnValueChanged += OnTimeChanged;
    }

    public void StartFallDownToPosition()
    {
        StartMovementToPosition(Direction.Down, TimeSpan.FromSeconds(gameSettings.board.timings.fallDownDuration));
    }

    public void StartMovementToPosition(Direction direction, TimeSpan duration)
    {
        TileState tileState = state.Value;
        if (tileState.movement.HasValue && gameEvents.CurrentTime.Value <= tileState.movement.Value.finishTime)
        {
            throw new InvalidOperationException("previous movement didn't finished yet");
        }

        TileState.Movement movement;
        movement.direction = direction;
        movement.startTime = gameEvents.CurrentTime.Value;
        movement.finishTime = gameEvents.CurrentTime.Value + duration;
        tileState.movement = movement;
        state.Set(tileState);
    }

    public void Die()
    {
        this.gameEvents.CurrentTime.OnValueChanged -= OnTimeChanged;
    }

    private void OnTimeChanged(TimeSpan time)
    {
        TileState tileState = state.Value;
        if (tileState.movement.HasValue && time >= tileState.movement.Value.finishTime)
        {
            tileState.movement = null;
            state.Set(tileState);
            if (TryFallDown() == false)
            {
                // try merge
            }
        }
    }

    internal bool TryFallDown()
    {
        if (position.Value.Y == bottomLinePositionY)
        {
            return false;
        }

        TilePosition newPosition = position.Value.Shift(Direction.Down);

        if (board.IsCellFree(newPosition) == false)
        {
            return false;
        }

        position.Set(newPosition);
        StartFallDownToPosition();
        return true;
    }
}
