using CALIMOE;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MGPong;

class Ball
{
    private SpriteObject _sprite;
    private Rectangle _playArea;
    private SoundEffect _paddleHitFx;
    private SoundEffect _wallHitFx;
    private int _size;
    private int _speed = 600;
    private float _speedIncrease = 60f;
    private readonly Random _rand = new Random();

    public Rectangle Bounds => _sprite.Bounds;

    public Ball(Rectangle playArea, Texture2D texture, SoundEffect paddleHit, SoundEffect wallHit)
    {
        _playArea = playArea;        
        _paddleHitFx = paddleHit;
        _wallHitFx = wallHit;
        _size = texture.Width;

        int startX = (playArea.Width - (_size / 2)) / 2;
        int startY = (playArea.Height - (_size / 2)) / 2;
        _sprite = new SpriteObject(texture, new Vector2(startX, startY), getRandomVelocity(), Vector2.One);
    }

    private Vector2 getRandomVelocity()
    {
        Vector2 randomVelocity = new Vector2(_rand.Next(2) == 0 ? -100f : 100f, (_rand.Next(10, 50)));
        randomVelocity.Y *= _rand.Next(2) == 0 ? 1 : -1;
        randomVelocity.Normalize();
        randomVelocity *= _speed;

        return randomVelocity;
    }


    public void Update(GameTime gt)
    {
        if (_sprite.Position.Y < _playArea.Top || _sprite.Position.Y > _playArea.Bottom - _size)
        {
            _sprite.ReverseYDirection();
            _wallHitFx.Play();
        }
        _sprite.Update(gt);
    }

    public void BounceOffPaddle()
    {
        _sprite.ReverseXDirection();
        _sprite.AdjustSpeed(_speedIncrease);
        _paddleHitFx.Play();
    }

    public void Reset()
    {
        _sprite.Reset();
        _sprite.Velocity = getRandomVelocity();
    }

    public void Draw(SpriteBatch sb)
    {
        _sprite.Draw(sb);
    }
}
