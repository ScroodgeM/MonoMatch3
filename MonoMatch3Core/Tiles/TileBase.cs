using System;
using MonoGameLibrary;
using MonoGameLibrary.StatefulEvent;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;

namespace MonoMatch3Core.Tiles;

public abstract class TileBase
{
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

    public static void SwapTiles(TileBase tile1, TileBase tile2)
    {
        TilePosition tile1NewPosition = tile2.position.Value;
        TilePosition tile2NewPosition = tile1.position.Value;

        Direction tile1Direction = Helpers.OffsetToDirection(tile1.position.Value, tile1NewPosition);
        Direction tile2Direction = Helpers.OffsetToDirection(tile2.position.Value, tile2NewPosition);

        tile1.position.Set(TilePosition.Unboarded);
        tile2.position.Set(tile2NewPosition);
        tile1.position.Set(tile1NewPosition);

        tile1.StartMovementToPosition(tile1Direction, TimeSpan.FromSeconds(tile1.settings.board.timings.swapTilesDuration));
        tile2.StartMovementToPosition(tile2Direction, TimeSpan.FromSeconds(tile2.settings.board.timings.swapTilesDuration));
    }
}
