using System;
using MonoGameLibrary;
using MonoGameLibrary.StatefulEvent;

namespace MonoMatch3.Match3Core.Tiles;

public abstract class TileBase
{
    public abstract TileType TileType { get; }
    public IStatefulEvent<TilePosition> Position => position;
    public IStatefulEvent<TileState> State => state;

    private readonly GameSettings gameSettings;
    private readonly IGameEvents gameEvents;
    private readonly StatefulEventInt<TilePosition> position = StatefulEventInt.CreateGenericStruct(TilePosition.Zero);
    private readonly StatefulEventInt<TileState> state = StatefulEventInt.CreateGenericStruct(TileState.Default);

    public TileBase(GameSettings gameSettings, IGameEvents gameEvents, TilePosition position)
    {
        this.gameSettings = gameSettings;
        this.gameEvents = gameEvents;
        this.position.Set(position);

        this.gameEvents.CurrentTime.OnValueChanged += OnTimeChanged;
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
        }
    }
}
