using System;
using System.Collections.Generic;
using MonoGameLibrary;
using MonoGameLibrary.Timers;
using MonoMatch3.Match3Core.Tiles;

namespace MonoMatch3.Match3Core;

public class Board
{
    public event Action<TileBase> OnTileCreated = tile => { };
    public event Action<TileBase> OnTileRemoved = tile => { };

    private byte topLinePositionY => 0;

    private readonly IGameEvents gameEvents;
    private readonly ITimer timer;
    private readonly GameSettings gameSettings;
    private readonly TilesFactory tilesFactory;
    private readonly BoardInput boardInput;

    private readonly Random sessionRandom = new Random(Guid.NewGuid().GetHashCode());
    private readonly Dictionary<TilePosition, TileBase> tiles = new Dictionary<TilePosition, TileBase>();

    public Board(IGameEvents gameEvents, ITimer timer, BoardInput boardInput, GameSettings gameSettings)
    {
        this.gameEvents = gameEvents;
        this.timer = timer;
        this.gameSettings = gameSettings;
        this.tilesFactory = new TilesFactory(gameSettings, gameEvents, this, sessionRandom, gameSettings.board.generatorPool);
        this.boardInput = boardInput;

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

    private void FillBoard()
    {
        for (byte x = 0; x <= gameSettings.board.width; x++)
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
        timer.Wait(TimeSpan.FromSeconds(0.3)).Done(() =>
        {
            if (tiles.ContainsKey(position) == false)
            {
                RegisterTile(tilesFactory.CreateRandom(position));
            }
        });
    }

    private void RegisterTile(TileBase tile)
    {
        tiles.Add(tile.Position.Value, tile);
        tile.Position.OnValueChangedFromTo += (oldPosition, newPosition) =>
        {
            tiles.Remove(oldPosition);
            tiles.Add(newPosition, tile);

            timer.Wait(TimeSpan.FromSeconds(gameSettings.board.timings.delayBeforeFallIntoFreeCell)).Done(() => { ProcessFreeCell(oldPosition); }
            );
        };
        OnTileCreated(tile);
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

        if (tiles[positionJustAbove].TryFallDown() == false)
        {
            throw new InvalidOperationException("we should not get here");
        }
    }
}
