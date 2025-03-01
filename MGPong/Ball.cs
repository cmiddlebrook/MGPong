using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
    private SoundEffect _paddleHitFx;
    private SoundEffect _wallHitFx;
    private int _size;
    private int _speed = 300;
    private readonly Random _rand = new Random();

    public Ball(Rectangle playArea, SoundEffect paddleHit, SoundEffect wallHit)
    {
        _playArea = playArea;
        _paddleHitFx = paddleHit;
        _wallHitFx = wallHit;
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

        _velocity.X = _rand.Next(2) == 0 ? -100f : 100f;

        _velocity.Y = (float)(_rand.Next(10, 50));
        _velocity.Y *= _rand.Next(2) == 0 ? 1 : -1;

        _velocity.Normalize();
        _velocity *= _speed;

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
            _wallHitFx.Play(0.4f, 0, 0);
        }
    }

    public void BounceOffPaddle()
    {
        _velocity.X *= -1; // Reverse X direction
        _velocity *= 1.1f; // Increase speed while maintaining direction
        _paddleHitFx.Play();
    }

    public void Draw(SpriteBatch sb)
    {        
        sb.Draw(_texture, _position, Color.White);
    }
}
