using CALIMOE;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGPong;

class ScoreBar
{
    private TextObject _leftScore;
    private TextObject _rightScore;
    private SpriteObject _leftSprite;
    private SpriteObject _rightSprite;

    public int Height => _leftSprite.Bounds.Height;

    public ScoreBar(Texture2D texture, SpriteFont font, int boardWidth)
    {
        _leftScore = new TextObject(font, "", new Vector2(texture.Width + 20, 0));
        _rightScore = new TextObject(font, "", new Vector2((boardWidth / 2) + 30, 0));
        _leftSprite = new SpriteObject(texture, Vector2.Zero, Vector2.Zero, Vector2.One);
        _rightSprite = new SpriteObject(texture, new Vector2(boardWidth - texture.Width, 0), Vector2.Zero, Vector2.One);
    }

    public void Draw(SpriteBatch sb, int leftScore, int rightScore)
    {
        _leftSprite.Draw(sb);
        _rightSprite.DrawFlippedHorizontally(sb);
        _leftScore.DrawText(sb, $"{leftScore}");
        _rightScore.DrawText(sb, $"{rightScore}");
    }
}
