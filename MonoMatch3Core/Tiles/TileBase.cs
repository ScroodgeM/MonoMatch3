using System;
using MonoGameLibrary;
using MonoGameLibrary.StatefulEvent;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;

namespace MonoMatch3Core.Tiles;

public abstract class TileBase
{
    public abstract TileType TileType { get; }
    public IStatefulEvent<TilePosition> Position => position;
    public IStatefulEvent<TileState> State => state;

    private byte bottomLinePositionY => (byte)(settings.board.height - 1);

    protected readonly Settings settings;
    protected readonly IGameEvents gameEvents;
    protected readonly Board.Board board;
    protected readonly StatefulEventInt<TilePosition> position = StatefulEventInt.CreateGenericStruct(TilePosition.Zero);
    protected readonly StatefulEventInt<TileState> state = StatefulEventInt.CreateGenericStruct(TileState.Default);

    protected TileBase(Settings settings, IGameEvents gameEvents, Board.Board board, TilePosition position)
    {
        this.settings = settings;
        this.gameEvents = gameEvents;
        this.board = board;
        this.position.Set(position);

        this.gameEvents.CurrentTime.OnValueChanged += OnTimeChanged;
    }

    internal void StartFallDownToPosition()
    {
        StartMovementToPosition(Direction.Down, TimeSpan.FromSeconds(settings.board.timings.fallDownDuration));
    }

    internal void StartMovementToPosition(Direction direction, TimeSpan duration)
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

    internal abstract void ProcessSuccessMatch();

    internal abstract void ChangeTypeTo(TileType newTileType);

    internal void Die()
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
                board.TryProcessMatch(position.Value);
            }
        }
    }

    internal bool TryFallDown()
    {
        if (position.Value.Y == bottomLinePositionY)
        {
            return false;
        }

        if (state.Value.movement != null)
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
