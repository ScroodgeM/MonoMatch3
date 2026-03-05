using System;
using System.Collections.Generic;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoMatch3.Match3Core.Tiles;

namespace MonoMatch3.View;

public class Board
{
    private readonly SpriteRenderer spriteRenderer;
    private readonly GameSettings gameSettings;
    private readonly IGameEvents gameEvents;
    private readonly Match3Core.Board boardCore;
    private readonly Dictionary<TileBase, Tile> tileViews = new Dictionary<TileBase, Tile>();

    public Board(SpriteRenderer spriteRenderer, GameSettings gameSettings, IGameEvents gameEvents, Match3Core.Board boardCore)
    {
        this.spriteRenderer = spriteRenderer;
        this.gameSettings = gameSettings;
        this.gameEvents = gameEvents;
        this.boardCore = boardCore;

        this.boardCore.OnTileCreated += OnTileCreated;
        this.boardCore.OnTileRemoved += OnTileRemoved;
    }

    public void Die()
    {
        this.boardCore.OnTileCreated -= OnTileCreated;
        this.boardCore.OnTileRemoved -= OnTileRemoved;
    }

    private void OnTileCreated(TileBase tile)
    {
        tileViews.Add(tile, new Tile(spriteRenderer, gameSettings, gameEvents, tile));
    }

    private void OnTileRemoved(TileBase tile)
    {
        tileViews.Remove(tile, out Tile tileView);
        tileView.Die();
    }
}
