using System;
using System.Collections.Generic;
using MonoGameLibrary;
using MonoGameLibrary.Input;
using MonoGameLibrary.StatefulEvent;
using MonoGameLibrary.Timers;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.MatchChecker;
using MonoMatch3Core.Tiles;

namespace MonoMatch3Core.Board;

public class Board
{
    public IStatefulEvent<bool, TilePosition> SelectedTile => selectedTile;
    public event Action<TileBase> OnTileCreated = tile => { };
    public event Action<TileBase, TileRemoveReason> OnTileRemoved = (tile, reason) => { };

    private byte topLinePositionY => 0;

    private readonly IGameEvents gameEvents;
    private readonly ITimer timer;
    private readonly Settings settings;
    private readonly TilesFactory tilesFactory;
    private readonly BoardInput boardInput;
    private readonly Aggregator matchChecker;

    private StatefulEventInt<bool, TilePosition> selectedTile = StatefulEventInt.Create(false).Add(StatefulEventInt.CreateGenericStruct(TilePosition.Unboarded));
    private readonly Random sessionRandom = new Random(Guid.NewGuid().GetHashCode());
    private readonly Dictionary<TilePosition, TileBase> tiles = new Dictionary<TilePosition, TileBase>();

    public static Board Create(IGameEvents gameEvents, ITimer timer, InputManager inputManager, Settings settings)
    {
        return new Board(gameEvents, timer, inputManager, settings);
    }

    public static void Destroy(Board instance)
    {
        instance.Die();
    }

    private Board(IGameEvents gameEvents, ITimer timer, InputManager inputManager, Settings settings)
    {
        this.gameEvents = gameEvents;
        this.timer = timer;
        this.settings = settings;
        this.tilesFactory = new TilesFactory(settings, gameEvents, this, sessionRandom);
        this.boardInput = new BoardInput(gameEvents, inputManager, settings);
        this.matchChecker = new Aggregator(settings);

        this.boardInput.OnTileClick += OnTileClick;
    }

    public void RunGame()
    {
        FillBoard();
    }

    internal bool IsCellFree(TilePosition position)
    {
        return tiles.ContainsKey(position) == false;
    }

    private void Die()
    {
        this.boardInput.OnTileClick -= OnTileClick;

        foreach (TileBase tile in tiles.Values)
        {
            tile.Die();
        }

        tiles.Clear();
    }

    internal bool TryProcessMatch(TilePosition position)
    {
        return matchChecker.TryProcessMatch(tiles, position);
    }

    internal void RemoveTile(TilePosition position, TileRemoveReason reason)
    {
        tiles.Remove(position, out TileBase removedTile);
        OnTileRemoved(removedTile, reason);
        removedTile.Die();
        WaitAndProcessFreeCell(position);
    }

    internal void ReplaceTile(TilePosition position, TileRemoveReason reason, TileType newTileType)
    {
        tiles.Remove(position, out TileBase removedTile);
        OnTileRemoved(removedTile, reason);
        removedTile.Die();
        RegisterTile(tilesFactory.Create(position, newTileType));
    }

    private void FillBoard()
    {
        for (byte x = 0; x <= settings.board.width; x++)
        {
            SpawnNewTileOnTop(x);
        }
    }

    private void SpawnNewTileOnTop(byte positionX)
    {
        TileBase tile = tilesFactory.CreateRandom(new TilePosition(true, positionX, topLinePositionY));
        tile.StartFallDownToPosition();
        RegisterTile(tile);
    }

    private void OnTileClick(TilePosition position)
    {
        if (selectedTile.Value1 == true && TrySwap(position, selectedTile.Value2) == true)
        {
            selectedTile.SetValue1(false);
        }
        else
        {
            selectedTile.SetValues(true, position);
        }
    }

    private bool TrySwap(TilePosition position1, TilePosition position2)
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

        TileBase.SwapTiles(tile1, tile2);
        return true;
    }

    private void RegisterTile(TileBase tile)
    {
        tiles.Add(tile.Position.Value, tile);
        tile.Position.OnValueChangedFromTo += (oldPosition, newPosition) =>
        {
            if (oldPosition.Boarded == true)
            {
                tiles.Remove(oldPosition);
                WaitAndProcessFreeCell(oldPosition);
            }

            if (newPosition.Boarded == true)
            {
                tiles.Add(newPosition, tile);
            }
        };
        OnTileCreated(tile);
    }

    private void WaitAndProcessFreeCell(TilePosition position)
    {
        timer.Wait(TimeSpan.FromSeconds(settings.board.timings.delayBeforeFallIntoFreeCell)).Done(() => { ProcessFreeCell(position); });
    }

    private void ProcessFreeCell(TilePosition position)
    {
        if (IsCellFree(position) == false)
        {
            return;
        }

        if (position.Y == topLinePositionY)
        {
            SpawnNewTileOnTop(position.X);
            return;
        }

        TilePosition positionJustAbove = position.Shift(Direction.Up);

        if (IsCellFree(positionJustAbove) == true)
        {
            ProcessFreeCell(positionJustAbove);
            return;
        }

        tiles[positionJustAbove].TryFallDown();
    }
}
