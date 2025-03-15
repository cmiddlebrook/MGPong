using CALIMOE;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection.Metadata;

namespace MGPong;

public abstract class Ball
{
    protected const float NORMAL_SPEED = 500f;

    protected SpriteObject _sprite;
    protected Rectangle _playArea;
    protected int _size;
    protected float _speed;
    protected SoundEffect _wallHitFx;
    protected readonly Random _rand = new Random();

    public Rectangle Bounds => _sprite.Bounds;

    public float CenterY => _sprite.Center.Y;


    public Ball(Rectangle playArea, Texture2D texture, SoundEffect wallHit)
    {
        _playArea = playArea;
        _size = texture.Width;
        _sprite = new SpriteObject(texture, GetStartPosition(), GetStartVelocity(), Vector2.One);
        _wallHitFx = wallHit;
    }

    protected abstract Vector2 GetStartPosition();
    protected abstract Vector2 GetStartVelocity();

    public virtual void Update(GameTime gt)
    {
        if (_sprite.Position.Y < _playArea.Top || _sprite.Position.Y > _playArea.Bottom - _size)
        {
            _sprite.ReverseYDirection();
            _wallHitFx.Play();
        }
        _sprite.Update(gt);
    }

    public virtual void NewBall()
    {
        _speed = NORMAL_SPEED;
        _sprite.Reset();
    }


    public virtual void Draw(SpriteBatch sb)
    {
        _sprite.Draw(sb);
    }
}
