using System;
using System.Collections.Generic;
using MonoGameLibrary;
using MonoMatch3.Match3Core.Tiles;

namespace MonoMatch3.Match3Core;

public class Board
{
    public event Action<TileBase> OnTileCreated = tile => { };

    private readonly IGameEvents gameEvents;
    private readonly GameSettings gameSettings;
    private readonly TilesFactory tilesFactory;
    private readonly BoardInput boardInput;

    private readonly Random sessionRandom = new Random(Guid.NewGuid().GetHashCode());
    private readonly Dictionary<TilePosition, TileBase> tiles = new Dictionary<TilePosition, TileBase>();

    public Board(IGameEvents gameEvents, BoardInput boardInput, GameSettings gameSettings)
    {
        this.gameEvents = gameEvents;
        this.gameSettings = gameSettings;
        this.tilesFactory = new TilesFactory(sessionRandom, gameSettings.board.generatorPool);
        this.boardInput = boardInput;

        this.boardInput.OnTileClick += OnTileClick;
    }

    ~Board()
    {
        this.boardInput.OnTileClick -= OnTileClick;
    }

    private void OnTileClick(TilePosition position)
    {
        if (tiles.ContainsKey(position) == false)
        {
            RegisterTile(tilesFactory.CreateRandom(position));
        }
    }

    private void RegisterTile(TileBase tile)
    {
        tiles.Add(tile.Position, tile);
        OnTileCreated(tile);
    }
}
