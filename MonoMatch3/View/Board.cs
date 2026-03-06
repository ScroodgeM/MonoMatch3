using System;
using System.Collections.Generic;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Timers;
using MonoMatch3.Match3Core;
using MonoMatch3.Match3Core.Tiles;
using MonoMatch3Core.Enums;

namespace MonoMatch3.View;

public class Board
{
    private readonly SpriteRenderer spriteRenderer;
    private readonly Settings settings;
    private readonly IGameEvents gameEvents;
    private readonly ITimer timer;
    private readonly Match3Core.Board boardCore;
    private readonly Dictionary<TileBase, Tile> tileViews = new Dictionary<TileBase, Tile>();

    public Board(SpriteRenderer spriteRenderer, Settings settings, IGameEvents gameEvents, ITimer timer, Match3Core.Board boardCore)
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
    }

    private void OnTileCreated(TileBase tile)
    {
        tileViews.Add(tile, new Tile(spriteRenderer, settings, gameEvents, timer, tile));
    }

    private void OnTileRemoved(TileBase tile, TileRemoveReason removeReason)
    {
        tileViews.Remove(tile, out Tile tileView);
        tileView.Die(removeReason);
    }
}
