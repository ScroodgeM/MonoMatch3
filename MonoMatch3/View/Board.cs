using System.Collections.Generic;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.Tiles;

namespace MonoMatch3.View;

public class Board
{
    private readonly SpriteRenderer spriteRenderer;
    private readonly Settings settings;
    private readonly IGameEvents gameEvents;
    private readonly MonoMatch3Core.Board.Board boardCore;
    private readonly Dictionary<TileBase, Tile> tileViews = new Dictionary<TileBase, Tile>();

    public Board(SpriteRenderer spriteRenderer, Settings settings, IGameEvents gameEvents, MonoMatch3Core.Board.Board boardCore)
    {
        this.spriteRenderer = spriteRenderer;
        this.settings = settings;
        this.gameEvents = gameEvents;
        this.boardCore = boardCore;

        this.boardCore.OnTileCreated += OnTileCreated;
        this.boardCore.OnTileRemoved += OnTileRemoved;
        this.boardCore.SelectedTile.OnValueChanged += OnSelectedValueChanged;
    }

    public void Die()
    {
        this.boardCore.OnTileCreated -= OnTileCreated;
        this.boardCore.OnTileRemoved -= OnTileRemoved;
        this.boardCore.SelectedTile.OnValueChanged -= OnSelectedValueChanged;

        foreach (Tile tileView in tileViews.Values)
        {
            tileView.Die();
        }

        tileViews.Clear();
    }

    private void OnTileCreated(TileBase tile)
    {
        tileViews.Add(tile, new Tile(spriteRenderer, settings, gameEvents, tile));
    }

    private void OnTileRemoved(TileBase tile, TileRemoveReason removeReason)
    {
        tileViews.Remove(tile, out Tile tileView);
        tileView.Remove(removeReason);
    }

    private void OnSelectedValueChanged(bool isSelected, TilePosition position)
    {
        foreach (KeyValuePair<TileBase, Tile> tile in tileViews)
        {
            bool tileIsSelected = isSelected == true && tile.Key.Position.Value == position;
            tile.Value.SetSelected(tileIsSelected);
        }
    }
}
