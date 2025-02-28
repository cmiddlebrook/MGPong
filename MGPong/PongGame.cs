using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace MGPong;

public class PongGame : Game
{
    private const float YREACT = 20.0f;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Random _rand = new Random();
    private Rectangle _playArea;

    private Texture2D _board;
    private Texture2D _scoreBar;
    private SpriteFont _scoreFont;
    private Paddle _playerPaddle;
    private Paddle _aiPaddle;
    private int _aiSpeed = 300;
    private Ball _ball;
    private int _playerScore = 0;
    private int _aiPlayerScore = 0;
    private int _winScore = 3;
    private Vector2 _playerScorePos;
    private Vector2 _aiPlayerScorePos;

    //public enum GameState { Idle, Start, Play, CheckEnd }
    //private GameState _gameState;

    public PongGame()
    {
        //this.TargetElapsedTime = new TimeSpan(333333); // 30 FPS

        // full screen mode
        //_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        //_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        //_graphics.IsFullScreen = true;
        //_graphics.ApplyChanges();

        _graphics = new GraphicsDeviceManager(this);

        Content.RootDirectory = "Content";
        Window.AllowUserResizing = false;
        Window.Title = "MG 01 - Pong";
        IsMouseVisible = true;

        //_gameState = GameState.Idle;
    }

    protected override void Initialize()
    {        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        // this was the hack to create a 1 pixel texture for drawing simple shapes
        //Globals.pixel = new Texture2D(GraphicsDevice, 1, 1);
        //Globals.pixel.SetData<Color>([Color.White]);

        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _scoreFont = Content.Load<SpriteFont>("Score");
        LoadBackgroundElements();
        LoadPaddles();
        LoadBall();
    }

    private void LoadBackgroundElements()
    {
        _board = Content.Load<Texture2D>("Board");
        _scoreBar = Content.Load<Texture2D>("ScoreBar");
        _playArea = new Rectangle(0, _scoreBar.Height, _board.Width, _board.Height);
        _playerScorePos = new Vector2(_scoreBar.Width + 20, 0);
        _aiPlayerScorePos = new Vector2(_board.Width - _scoreBar.Width - 32, 0);

        _graphics.PreferredBackBufferWidth = _board.Width;
        _graphics.PreferredBackBufferHeight = _scoreBar.Height + _board.Height;
        _graphics.ApplyChanges();
    }

    private void LoadPaddles()
    {
        Texture2D _playerPaddleTX = Content.Load<Texture2D>("LeftPaddle");
        _playerPaddle = new Paddle(_playArea, 10);
        _playerPaddle.SetTexture(Content.Load<Texture2D>("LeftPaddle"));

        Texture2D aiPaddleTX = Content.Load<Texture2D>("RightPaddle");
        _aiPaddle = new Paddle(_playArea, _playArea.Width - aiPaddleTX.Width - 10);
        _aiPaddle.SetTexture(aiPaddleTX);
    }


    private void LoadBall()
    {
        _ball = new Ball(_playArea);
        _ball.SetTexture(Content.Load<Texture2D>("WhiteBall"));
    }
    protected override void Update(GameTime gt)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || IsKeyDown(Keys.Escape))
            Exit();

        _ball.Update(gt);
        HandlePlayerMovement(gt);
        MoveAIPlayer(gt);
        CheckPaddleCollision();
        CheckPointScored();

        base.Update(gt);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);     

        _spriteBatch.Begin();
        DrawBackgroundElements();
        DrawScores();
        DrawPaddlesAndBall();
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawBackgroundElements()
    {
        _spriteBatch.Draw(_scoreBar, new Vector2(0, 0), Color.White);
        _spriteBatch.Draw(_scoreBar, new Vector2(_playArea.Width - _scoreBar.Width, 0), null, Color.White,
            0f, new Vector2(0, 0), 1f, SpriteEffects.FlipHorizontally, 0f);
        _spriteBatch.Draw(_board, _playArea, Color.White);
    }

    private void DrawScores()
    {
        _spriteBatch.DrawString(_scoreFont, $"{_playerScore}", _playerScorePos, Color.White);
        _spriteBatch.DrawString(_scoreFont, $"{_aiPlayerScore}", _aiPlayerScorePos, Color.White);
    }

    private void DrawPaddlesAndBall()
    {
        _playerPaddle.Draw(_spriteBatch);
        _aiPaddle.Draw(_spriteBatch);
        _ball.Draw(_spriteBatch);
    }

    protected bool IsKeyDown(Keys key)
    {
        return Keyboard.GetState().IsKeyDown(key);
    }

    protected void CheckPaddleCollision()
    {
        Rectangle ballRect = _ball.getBoundingRect();
        if (ballRect.Intersects(_playerPaddle.getBoundingRect()) || ballRect.Intersects(_aiPaddle.getBoundingRect()))
        {
            _ball.BounceOffPaddle();
        }
    }

    protected void CheckPointScored()
    {
        Rectangle ballRect = _ball.getBoundingRect();
        if (ballRect.X < 0)
        {
            AIPlayerScored();
        }
        else if (ballRect.X + ballRect.Width > _playArea.Width)
        {
            PlayerScored();
        }
    }

    protected void AIPlayerScored()
    {
        _aiPlayerScore++;
        ResetGame();
    }

    protected void PlayerScored()
    {
        _playerScore++;
        ResetGame();
    }

    protected void ResetGame()
    {
        _ball.Reset();
        _aiPaddle.Reset();
        _playerPaddle.Reset();
    }
    protected void HandlePlayerMovement(GameTime gt)
    {
        if (IsKeyDown(Keys.W))
        {
            _playerPaddle.MoveUp(gt);
        }

        if (IsKeyDown(Keys.S))
        {
            _playerPaddle.MoveDown(gt);
        }
    }

    protected void MoveAIPlayer(GameTime gt)
    {
        float ballY = _ball.getY();
        float paddleY = _aiPaddle.getY();

        if (Math.Abs(ballY - paddleY) > YREACT)
        {
            if (ballY < paddleY)
            {
                _aiPaddle.MoveUp(gt, _aiSpeed);
            }
            else if (ballY > paddleY)
            {
                _aiPaddle.MoveDown(gt, _aiSpeed);
            }
        }
    }
}
