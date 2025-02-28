using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection.Metadata;

namespace MGPong;

public class Paddle
{
    private Rectangle _playArea;
    private Vector2 _position;
    private Rectangle _boundingRect;

    private Texture2D _texture = null;

    public Paddle(Rectangle playArea, int xPosition)
    {
        _playArea = playArea;
        _position.X = xPosition;
    }

    public void SetTexture(Texture2D tx)
    {
        _texture = tx;
        Reset();
    }

    public void Reset()
    {
        _position.Y = _playArea.Top + (_playArea.Height / 2) - (_texture.Height / 2);
        UpdateBoundingRectangle();
    }

    public float getX()
    {
        return _position.X;
    }

    public float getY()
    {
        return _position.Y;
    }

    public Rectangle getBoundingRect()
    {
        return _boundingRect;
    }

    public void MoveUp(GameTime gt, int speed = 300)
    {
        float newY = (float)(_position.Y - speed * gt.ElapsedGameTime.TotalSeconds);
        _position.Y = Math.Max(newY, _playArea.Top);
        UpdateBoundingRectangle();
    }

    public void MoveDown(GameTime gt, int speed = 300)
    {
        float newY = (float)(_position.Y + speed * gt.ElapsedGameTime.TotalSeconds);
        _position.Y = Math.Min(newY, _playArea.Bottom - _texture.Height);

        UpdateBoundingRectangle();
    }

    private void UpdateBoundingRectangle()
    {
        _boundingRect = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
    }

    public void Draw(SpriteBatch sb) 
	{
		sb.Draw(_texture, _position, Color.White);
	}
}
