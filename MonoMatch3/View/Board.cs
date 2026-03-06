using System;
using System.Collections.Generic;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Timers;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.Tiles;

namespace MonoMatch3.View;

public class Board
{
    private readonly SpriteRenderer spriteRenderer;
    private readonly Settings settings;
    private readonly IGameEvents gameEvents;
    private readonly ITimer timer;
    private readonly MonoMatch3Core.Board.Board boardCore;
    private readonly Dictionary<TileBase, Tile> tileViews = new Dictionary<TileBase, Tile>();

    public Board(SpriteRenderer spriteRenderer, Settings settings, IGameEvents gameEvents, ITimer timer, MonoMatch3Core.Board.Board boardCore)
    {
        this.spriteRenderer = spriteRenderer;
        this.settings = settings;
        this.gameEvents = gameEvents;
        this.timer = timer;
        this.boardCore = boardCore;

        this.boardCore.OnTileCreated += OnTileCreated;
        this.boardCore.OnTileRemoved += OnTileRemoved;
    }

    public void Die()
    {
        this.boardCore.OnTileCreated -= OnTileCreated;
        this.boardCore.OnTileRemoved -= OnTileRemoved;

        foreach (Tile tileView in tileViews.Values)
        {
            tileView.Die();
        }

        tileViews.Clear();
    }

    private void OnTileCreated(TileBase tile)
    {
        tileViews.Add(tile, new Tile(spriteRenderer, settings, gameEvents, timer, tile));
    }

    private void OnTileRemoved(TileBase tile, TileRemoveReason removeReason)
    {
        tileViews.Remove(tile, out Tile tileView);
        tileView.Remove(removeReason);
    }
}
