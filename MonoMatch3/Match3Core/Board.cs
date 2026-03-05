using System;
using System.Collections.Generic;
using MonoGameLibrary;
using MonoGameLibrary.Timers;
using MonoMatch3.Match3Core.Tiles;

namespace MonoMatch3.Match3Core;

public class Board
{
    public event Action<TileBase> OnTileCreated = tile => { };

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
        this.tilesFactory = new TilesFactory(sessionRandom, gameSettings.board.generatorPool);
        this.boardInput = boardInput;

        this.boardInput.OnTileClick += OnTileClick;
    }

    public void RunGame()
    {
        FillBoard();
    }

    ~Board()
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
        TilePosition position = new TilePosition(positionX, 0);
        TileBase tile = tilesFactory.CreateRandom(position);
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
        tiles.Add(tile.Position, tile);
        OnTileCreated(tile);
    }
}
