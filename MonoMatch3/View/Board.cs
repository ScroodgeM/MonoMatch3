using MonoGameLibrary.Graphics;
using MonoMatch3.Match3Core.Tiles;

namespace MonoMatch3.View;

public class Board
{
    private readonly SpriteRenderer spriteRenderer;
    private readonly GameSettings gameSettings;
    private readonly Match3Core.Board boardCore;

    public Board(SpriteRenderer spriteRenderer, GameSettings gameSettings, Match3Core.Board boardCore)
    {
        this.spriteRenderer = spriteRenderer;
        this.gameSettings = gameSettings;
        this.boardCore = boardCore;

        this.boardCore.OnTileCreated += OnTileCreated;
    }

    ~Board()
    {
        this.boardCore.OnTileCreated -= OnTileCreated;
    }


    private void OnTileCreated(TileBase tile)
    {
        Tile tileView = new Tile(spriteRenderer, gameSettings, tile);
    }
}
