using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGPong;

class ScoreBar
{
    private Texture2D _texture = null;
    private SpriteFont _font = null;
    private Vector2 _leftScorePos;
    private Vector2 _rightScorePos;
    private int _boardWidth = 0;

    public ScoreBar(Texture2D tx, SpriteFont font, int boardWidth)
    {        
        _texture = tx;
        _font = font;

        _boardWidth = boardWidth;
        _leftScorePos = new Vector2(_texture.Width + 20, 0);
        _rightScorePos = new Vector2(_boardWidth - _texture.Width - 32, 0);

    }

    public int getHeight()
    {
        return _texture.Height;
    }
    public void Draw(SpriteBatch sb, int leftScore, int rightScore)
    {
        sb.Draw(_texture, new Vector2(0, 0), Color.White);
        sb.Draw(_texture, new Vector2(_boardWidth - _texture.Width, 0), null, Color.White,
            0f, new Vector2(0, 0), 1f, SpriteEffects.FlipHorizontally, 0f);

        sb.DrawString(_font, $"{leftScore}", _leftScorePos, Color.White);
        sb.DrawString(_font, $"{rightScore}", _rightScorePos, Color.White);

    }
}
