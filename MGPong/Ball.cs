using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MGPong;

class Ball
{
    private Rectangle _playArea;
    private Rectangle _boundingRect;
    private Vector2 _position;
    private Vector2 _velocity;
    private Texture2D _texture;
    private int _size;
    private readonly Random _rand = new Random();

    private const int MOVE_SPEED = 300;

    public Ball(Rectangle playArea)
    {
        _playArea = playArea;
    }

    public void SetTexture(Texture2D tx)
    {
        _texture = tx;
        _size = tx.Width;
        Reset();
    }

    public void Reset()
    {
        _position.X = (_playArea.Width - (_size / 2)) / 2;
        _position.Y = (_playArea.Height - (_size / 2)) / 2;

        _boundingRect = new Rectangle((int)_position.X, (int)_position.Y, _size, _size);

        // For random horizontal direction (left or right)
        _velocity.X = _rand.Next(2) == 0 ? -100f : 100f;

        // For random vertical velocity (between -50 and 50)
        _velocity.Y = (float)(_rand.Next(-50, 51));
        _velocity.Normalize();
        _velocity *= MOVE_SPEED;

    }
    public void Serve()
    {
        Reset();
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
    public void Update(GameTime gt)
    {
        _position += _velocity * (float)gt.ElapsedGameTime.TotalSeconds;
        _boundingRect.X = (int)_position.X;
        _boundingRect.Y = (int)_position.Y;

        // bounce off vertical edges
        if (_position.Y < _playArea.Top || _position.Y > _playArea.Bottom - _size)
        {
            _velocity.Y *= -1;
        }
    }

    public void BounceOffPaddle()
    {
        _velocity.X *= -1;
    }

    public void Draw(SpriteBatch sb)
    {        
        sb.Draw(_texture, _position, Color.White);
    }
}
