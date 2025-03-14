using CALIMOE;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace MGPong;

public class PowerBall : Ball
{
    public enum BallType
    {
        FastBall,
        BigPaddle,
        FastPaddle,
    }

    private BallType[] _ballPool = (BallType[])Enum.GetValues(typeof(BallType));
    private int _currentBall = 1;
    private TimeSpan _timer;
    private TimeSpan _interval;
    bool _inPlay = false;

    public BallType Type => _ballPool[_currentBall];
    public PowerBall(Rectangle playArea, Texture2D texture, SoundEffect wallHit)
        : base(playArea, texture, wallHit)
    {
        _speed = 250f;
        _interval = TimeSpan.FromSeconds(5);
        _sprite = new SpriteObject(texture, GetStartPosition(), GetStartVelocity(), Vector2.One * 1.5f);
        NewBall();
    }

    protected override Vector2 GetStartPosition()
    {
        return GetRandomPosition();
    }
    private Vector2 GetRandomPosition()
    {
        int startX = _playArea.Right - 100;
        int startY = _rand.Next(_playArea.Top + 100, _playArea.Bottom - 100);
        return new Vector2(startX, startY);
    }
    protected override Vector2 GetStartVelocity()
    {
        return Vector2.Zero;
    }

    private Vector2 GetRandomVelocity()
    {
        Vector2 randomVelocity = new Vector2(-100f, (_rand.Next(10, 50)));
        randomVelocity.Y *= _rand.Next(2) == 0 ? 1 : -1;
        randomVelocity.Normalize();
        randomVelocity *= _speed;

        return randomVelocity;
    }

    private void SetBallColour()
    {
        switch (Type)
        {
            case BallType.FastBall:
                _sprite.Colour = new Color(255, 50, 50) * 0.7f; // red
                break;

            case BallType.BigPaddle:
                _sprite.Colour = new Color(57, 255, 20) * 0.7f; // green
                break;

            case BallType.FastPaddle:
                _sprite.Colour = new Color(4, 118, 208) * 0.8f; // blue
                break;
        }
    }
    public override void Update(GameTime gt)
    {
        _timer += gt.ElapsedGameTime;
        if (_timer >= _interval)
        {
            _inPlay = true;
        }

        if (_inPlay)
        {
            if (_sprite.Position.X < _playArea.Left - _size)
            {
                NewBall();
            }
            base.Update(gt);
        }
    }

    public override void Draw(SpriteBatch sb)
    {
        if (_inPlay)
        {
            base.Draw(sb);
        }
    }

    public void CyclePowerBall()
    {
        _currentBall = (_currentBall + 1) % _ballPool.Length;
    }
    public override void NewBall()
    {
        _inPlay = false;
        _timer = TimeSpan.Zero;
        _sprite.Reset();
        _sprite.Position = GetStartPosition();
        _sprite.Velocity = GetRandomVelocity();
        SetBallColour();
    }


}
