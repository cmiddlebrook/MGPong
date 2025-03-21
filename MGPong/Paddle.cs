﻿using CALIMOE;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MGPong;

public class Paddle
{
    private SpriteObject _sprite;
    private Rectangle _playArea;
    private int _defaultPaddleLength = 0;
    private int _paddleLength = 0;
    private float _scaleFactor = 1.0f;
    private float _maxScaleFactor = 1.0f;
    private int _numBoosts = 0;
    private Vector2 _startPosition = Vector2.Zero;

    public Rectangle Bounds => _sprite.Bounds;

    public float CenterY => _sprite.Center.Y;

    public Paddle(Rectangle playArea, Texture2D texture, int xPosition)
    {
        _playArea = playArea;
        _defaultPaddleLength = texture.Height;
        int startY = (playArea.Height - (texture.Height / 2)) / 2;
        _startPosition = new Vector2(xPosition, startY);
        _sprite = new SpriteObject(texture, _startPosition, Vector2.Zero, Vector2.One);
    }

    public void Reset()
    {
        _scaleFactor = 1.0f;
        _sprite.Reset();
    }

    public void ResetPositionOnly()
    {        
        Vector2 scale = _sprite.Scale;
        _sprite.Reset();
        _sprite.Scale = scale;
        _sprite.Position = _startPosition;
    }

    public void MoveUp(GameTime gt, int speed)
    {
        float newY = (float)(_sprite.Position.Y - speed * gt.ElapsedGameTime.TotalSeconds);
        _sprite.Position = new Vector2(_sprite.Position.X, Math.Max(newY, _playArea.Top));
    }

    public void MoveDown(GameTime gt, int speed)
    {
        float newY = (float)(_sprite.Position.Y + speed * gt.ElapsedGameTime.TotalSeconds);
        _sprite.Position = new Vector2(_sprite.Position.X, Math.Min(newY, _playArea.Bottom - _sprite.Bounds.Height));
    }

    public void Grow(bool max = false)
    {
        _numBoosts++;
        float scaleFactor = max ? _maxScaleFactor : Math.Min(_numBoosts * 0.15f, _maxScaleFactor);
        _paddleLength = (int)(_defaultPaddleLength * (1 + scaleFactor));
        _startPosition.Y = (_playArea.Height - (_paddleLength / 2)) / 2;
        _sprite.Scale = new Vector2(1, 1 + scaleFactor);
    }

    public void Draw(SpriteBatch sb) 
	{
        _sprite.Draw(sb);
	}
}
