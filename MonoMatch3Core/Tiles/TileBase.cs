using System;
using System.Collections.Generic;
using MonoGameLibrary;
using MonoGameLibrary.StatefulEvent;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;

namespace MonoMatch3Core.Tiles;

public abstract class TileBase
{
    public event Action<TilePosition> OnMoveAttemptFailed = targetPosition => { };
    public abstract TileType Type { get; }
    public TileColor Color => color;
    public IStatefulEvent<TilePosition> Position => position;
    public IStatefulEvent<TileState> State => state;

    protected readonly Settings settings;
    protected readonly IGameEvents gameEvents;
    protected readonly Board.Board board;

    protected readonly TileColor color;
    protected readonly StatefulEventInt<TilePosition> position = StatefulEventInt.CreateGenericStruct(TilePosition.Unboarded);
    protected readonly StatefulEventInt<TileState> state = StatefulEventInt.CreateGenericStruct(TileState.Default);

    protected TileBase(Settings settings, IGameEvents gameEvents, Board.Board board, TileColor color, TilePosition position)
    {
        this.settings = settings;
        this.gameEvents = gameEvents;
        this.board = board;

        this.color = color;
        this.position.Set(position);

        this.gameEvents.CurrentTime.OnValueChanged += OnTimeChanged;

        StartFallDownToPosition();
    }

    private void StartFallDownToPosition()
    {
        StartMovementToPosition(Direction.Down, TimeSpan.FromSeconds(settings.board.timings.fallDownDuration));
    }

    private void StartMovementToPosition(Direction direction, TimeSpan duration)
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

    internal abstract void UpgradeTile(TileType newTileType);

    internal abstract void DestroyBySpecial();

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
                TryProcessMatch(ProcessMatchMode.CheckAndConfirmChanges);
            }
        }
    }

    private bool TryProcessMatch(ProcessMatchMode mode) => board.TryProcessMatch(position.Value, mode);

    internal bool TryFallDown()
    {
        if (position.Value.Y == settings.GetBoardArea().BottomLineY)
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

    public static bool TrySwap(Dictionary<TilePosition, TileBase> tiles, TilePosition position1, TilePosition position2)
    {
        if (position1.IsNeighborOf(position2) == false)
        {
            return false;
        }

        if (tiles.TryGetValue(position1, out TileBase tile1) == false || tile1.State.Value.movement.HasValue == true)
        {
            return false;
        }

        if (tiles.TryGetValue(position2, out TileBase tile2) == false || tile2.State.Value.movement.HasValue == true)
        {
            return false;
        }

        tile1.position.Set(TilePosition.Unboarded);
        tile2.position.Set(position1);
        tile1.position.Set(position2);

        if (tile1.TryProcessMatch(ProcessMatchMode.CheckOnly) == true
            ||
            tile2.TryProcessMatch(ProcessMatchMode.CheckOnly) == true)
        {
            Direction tile1Direction = Helpers.OffsetToDirection(position1, position2);
            Direction tile2Direction = Helpers.OffsetToDirection(position2, position1);

            tile1.StartMovementToPosition(tile1Direction, TimeSpan.FromSeconds(tile1.settings.board.timings.swapTilesDuration));
            tile2.StartMovementToPosition(tile2Direction, TimeSpan.FromSeconds(tile2.settings.board.timings.swapTilesDuration));

            return true;
        }

        tile1.position.Set(TilePosition.Unboarded);
        tile2.position.Set(position2);
        tile1.position.Set(position1);

        tile1.OnMoveAttemptFailed(position2);
        tile2.OnMoveAttemptFailed(position1);
        return false;
    }
}
