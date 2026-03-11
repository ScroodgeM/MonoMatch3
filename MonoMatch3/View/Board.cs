using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoMatch3Core;
using MonoMatch3Core.Data;
using MonoMatch3Core.Enums;
using MonoMatch3Core.Tiles;

namespace MonoMatch3.View;

public class Board
{
    private readonly RenderSystem renderSystem;
    private readonly Settings settings;
    private readonly IGameEvents gameEvents;
    private readonly MonoMatch3Core.Board.Board boardCore;
    private readonly Dictionary<TileBase, Tile> tileViews = new Dictionary<TileBase, Tile>();

    public Board(RenderSystem renderSystem, Settings settings, IGameEvents gameEvents, MonoMatch3Core.Board.Board boardCore)
    {
        this.renderSystem = renderSystem;
        this.settings = settings;
        this.gameEvents = gameEvents;
        this.boardCore = boardCore;

        this.boardCore.OnTileCreated += OnTileCreated;
        this.boardCore.OnTileRemoved += OnTileRemoved;
        this.boardCore.SelectedTile.OnValueChanged += OnSelectedValueChanged;

        Transform transform = Transform.Default;
        transform.position = settings.GetBoardTopLeftCornerOffset();
        transform.layerDepth = RenderLayer.Background.ToLayerDepth();
        transform.color = Color.White * 0.5f;
        this.renderSystem.ShowTilemap(settings.view.boardTilemapId, transform);
    }

    public void Die()
    {
        renderSystem.HideTilemap(settings.view.boardTilemapId);

        boardCore.OnTileCreated -= OnTileCreated;
        boardCore.OnTileRemoved -= OnTileRemoved;
        boardCore.SelectedTile.OnValueChanged -= OnSelectedValueChanged;

        foreach (Tile tileView in tileViews.Values)
        {
            tileView.Die();
        }

        tileViews.Clear();
    }

    private void OnTileCreated(TileBase tile)
    {
        tileViews.Add(tile, new Tile(renderSystem, settings, gameEvents, tile));
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
