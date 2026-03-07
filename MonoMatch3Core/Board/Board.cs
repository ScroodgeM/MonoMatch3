using System;
using System.Collections.Generic;
using MonoGameLibrary;
using MonoGameLibrary.Input;
using MonoGameLibrary.StatefulEvent;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.MatchChecker;
using MonoMatch3Core.Specials;
using MonoMatch3Core.Tiles;

namespace MonoMatch3Core.Board;

public class Board
{
    public IStatefulEvent<bool, TilePosition> SelectedTile => selectedTile;
    public event Action<TileBase> OnTileCreated = tile => { };
    public event Action<TileBase, TileRemoveReason> OnTileRemoved = (tile, reason) => { };
    public event Action<SpecialBase> OnSpecialCreated = special => { };

    private readonly IGameEvents gameEvents;
    private readonly Settings settings;
    private readonly TilesFactory tilesFactory;
    private readonly BoardInput boardInput;
    private readonly Aggregator matchChecker;

    private readonly StatefulEventInt<bool, TilePosition> selectedTile = StatefulEventInt.Create(false).Add(StatefulEventInt.CreateGenericStruct(TilePosition.Unboarded));
    private readonly Random sessionRandom = new Random(Guid.NewGuid().GetHashCode());
    private readonly Dictionary<TilePosition, TileBase> tiles = new Dictionary<TilePosition, TileBase>();

    public static Board Create(IGameEvents gameEvents, InputManager inputManager, Settings settings)
    {
        return new Board(gameEvents, inputManager, settings);
    }

    public static void Destroy(Board instance)
    {
        instance.Die();
    }

    public void RunGame()
    {
        FillBoard();
    }

    internal bool IsCellFree(TilePosition position)
    {
        return tiles.ContainsKey(position) == false;
    }

    internal bool TryProcessMatch(TilePosition position, ProcessMatchMode mode)
    {
        return matchChecker.TryProcessMatch(tiles, position, mode);
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
        TileColor color = removedTile.Color;
        OnTileRemoved(removedTile, reason);
        removedTile.Die();
        RegisterTile(tilesFactory.Create(newTileType, color, position));
    }

    internal void RegisterSpecial(SpecialBase special)
    {
        OnSpecialCreated(special);
        special.OnTileDestroyAttempt += OnSpecialTileDestroyAttempt;
        special.OnCompleted += OnSpecialCompleted;
    }

    private Board(IGameEvents gameEvents, InputManager inputManager, Settings settings)
    {
        this.gameEvents = gameEvents;
        this.settings = settings;
        this.tilesFactory = new TilesFactory(settings, gameEvents, this, sessionRandom);
        this.boardInput = new BoardInput(gameEvents, inputManager, settings);
        this.matchChecker = new Aggregator(settings);

        this.boardInput.OnTileClick += OnTileClick;
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

    private void FillBoard()
    {
        BoardArea area = settings.GetBoardArea();
        for (byte x = area.LeftLineX; x <= area.RightLineX; x++)
        {
            SpawnNewTileOnTop(x);
        }
    }

    private void SpawnNewTileOnTop(byte positionX)
    {
        TilePosition spawnPosition = new TilePosition(true, positionX, settings.GetBoardArea().TopLineY);

        RegisterTile(tilesFactory.CreateRandom(spawnPosition));
    }

    private void OnTileClick(TilePosition position)
    {
        if (selectedTile.Value1 == true)
        {
            selectedTile.SetValue1(false);

            TileBase.TrySwap(tiles, position, selectedTile.Value2);
        }
        else
        {
            selectedTile.SetValues(true, position);
        }
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
        gameEvents.Timer.Wait(TimeSpan.FromSeconds(settings.board.timings.delayBeforeFallIntoFreeCell)).Done(() => { ProcessFreeCell(position); });
    }

    private void OnSpecialTileDestroyAttempt(TilePosition position)
    {
        if (tiles.TryGetValue(position, out TileBase tile) == true)
        {
            tile.DestroyBySpecial();
        }
    }

    private void OnSpecialCompleted(SpecialBase special)
    {
        special.OnTileDestroyAttempt -= OnSpecialTileDestroyAttempt;
        special.OnCompleted -= OnSpecialCompleted;
    }

    private void ProcessFreeCell(TilePosition position)
    {
        if (IsCellFree(position) == false)
        {
            return;
        }

        if (position.Y == settings.GetBoardArea().TopLineY)
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
