using MonoGameLibrary.Graphics;
using MonoMatch3.Match3Core;
using MonoMatch3.Match3Core.Tiles;

namespace MonoMatch3.View;

public class Tile
{
    private TileBase tileCore;
    private readonly SpriteRenderer spriteRenderer;
    private readonly ushort mySpriteId;
    private Sprite.Transform mySpriteTransform;

    public Tile(SpriteRenderer spriteRenderer, GameSettings gameSettings, TileBase tileCore)
    {
        this.tileCore = tileCore;
        this.spriteRenderer = spriteRenderer;

        this.mySpriteTransform = Sprite.Transform.Default;
        this.mySpriteTransform.position = gameSettings.BoardToScreen(tileCore.Position).ToVector2();

        string spriteId = gameSettings.GetSpriteId(tileCore.TileType);
        this.mySpriteId = this.spriteRenderer.AddSprite(spriteId, mySpriteTransform);

        tileCore.OnRemoved += OnRemoved;
    }

    ~Tile()
    {
        tileCore.OnRemoved -= OnRemoved;
    }

    private void OnRemoved()
    {
        this.spriteRenderer.RemoveSprite(mySpriteId);
    }
}
