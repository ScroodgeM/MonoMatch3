using System;
using System.Collections.Generic;
using MonoGameLibrary;
using MonoGameLibrary.Timers;
using MonoMatch3.Match3Core.MatchChecker;
using MonoMatch3.Match3Core.Tiles;
using MonoMatch3Core.Enums;

namespace MonoMatch3.Match3Core;

public class Board
{
    public event Action<TileBase> OnTileCreated = tile => { };
    public event Action<TileBase, TileRemoveReason> OnTileRemoved = (tile, reason) => { };

    private byte topLinePositionY => 0;

    private readonly IGameEvents gameEvents;
    private readonly ITimer timer;
    private readonly Settings settings;
    private readonly TilesFactory tilesFactory;
    private readonly BoardInput boardInput;
    private readonly Aggregator matchChecker;

    private readonly Random sessionRandom = new Random(Guid.NewGuid().GetHashCode());
    private readonly Dictionary<TilePosition, TileBase> tiles = new Dictionary<TilePosition, TileBase>();

    public Board(IGameEvents gameEvents, ITimer timer, BoardInput boardInput, Settings settings)
    {
        this.gameEvents = gameEvents;
        this.timer = timer;
        this.settings = settings;
        this.tilesFactory = new TilesFactory(settings, gameEvents, this, sessionRandom);
        this.boardInput = boardInput;
        this.matchChecker = new Aggregator(settings);

        this.boardInput.OnTileClick += OnTileClick;
    }

    public void RunGame()
    {
        FillBoard();
    }

    public bool IsCellFree(TilePosition position)
    {
        return tiles.ContainsKey(position) == false;
    }

    public void Die()
    {
        this.boardInput.OnTileClick -= OnTileClick;
    }

    public bool TryProcessMatch(TilePosition position)
    {
        return matchChecker.TryProcessMatch(tiles, position);
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
        TileBase tile = tilesFactory.CreateRandom(new TilePosition(positionX, topLinePositionY));
        tile.StartFallDownToPosition();
        RegisterTile(tile);
    }

    private void OnTileClick(TilePosition position)
    {
        if (tiles.TryGetValue(position, out TileBase tile) == true && tile.State.Value.movement.HasValue == false)
        {
            RemoveTile(position, TileRemoveReason.None);
        }
    }

    private void RegisterTile(TileBase tile)
    {
        tiles.Add(tile.Position.Value, tile);
        tile.Position.OnValueChangedFromTo += (oldPosition, newPosition) =>
        {
            tiles.Remove(oldPosition);
            tiles.Add(newPosition, tile);
            WaitAndProcessFreeCell(oldPosition);
        };
        OnTileCreated(tile);
    }

    internal void RemoveTile(TilePosition position, TileRemoveReason reason)
    {
        tiles.Remove(position, out TileBase removedTile);
        OnTileRemoved(removedTile, reason);
        removedTile.Die();
        WaitAndProcessFreeCell(position);
    }

    public void ReplaceTile(TilePosition position, TileRemoveReason reason, TileType newTileType)
    {
        tiles.Remove(position, out TileBase removedTile);
        OnTileRemoved(removedTile, reason);
        removedTile.Die();
        RegisterTile(tilesFactory.Create(position, newTileType));
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
