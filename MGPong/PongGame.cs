using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using CALIMOE;

namespace MGPong;

public class PongGame : Calimoe
{
    private const float YREACT = 5.0f;

    private TextObject _titleObj = null;
    private Random _rand = new Random();
    private Rectangle _playArea;
    private Texture2D _board;
    private ScoreBar _scoreBar;
    private Paddle _playerPaddle;
    private Paddle _aiPaddle;
    private SoundEffect _scoreFx;
    private int _aiSpeed = 100;
    private Ball _ball;
    private int _playerScore = 0;
    private int _aiPlayerScore = 0;
    private bool _paused = true;


    public PongGame()
    {
        //this.TargetElapsedTime = new TimeSpan(333333); // 30 FPS

        // full screen mode
        //_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        //_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        //_graphics.IsFullScreen = true;
        //_graphics.ApplyChanges();


        Window.AllowUserResizing = false;
        Window.Title = "Game #1 - MG Pong";

    }

    protected override void Initialize()
    {        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        base.LoadContent();

        LoadSoundFx();
        LoadFonts();
        LoadBackgroundElements();
        LoadPaddles();
        LoadBall();
    }

    private void LoadFonts()
    {
        _titleObj = new TextObject(Content.Load<SpriteFont>("Fonts/Title"));
    }

    private void LoadBackgroundElements()
    {
        _board = Content.Load<Texture2D>("Textures/Board");
        _scoreBar = new ScoreBar(Content.Load<Texture2D>("Textures/ScoreBar"), Content.Load<SpriteFont>("Fonts/Score"), _board.Width);
        _playArea = new Rectangle(0, _scoreBar.getHeight(), _board.Width, _board.Height);

        _graphics.PreferredBackBufferWidth = _board.Width;
        _graphics.PreferredBackBufferHeight = _scoreBar.getHeight() + _board.Height;
        _graphics.ApplyChanges();
    }

    private void LoadPaddles()
    {
        Texture2D _playerPaddleTX = Content.Load<Texture2D>("Textures/LeftPaddle");
        _playerPaddle = new Paddle(_playArea, 10);
        _playerPaddle.SetTexture(_playerPaddleTX);

        Texture2D aiPaddleTX = Content.Load<Texture2D>("Textures/RightPaddle");
        _aiPaddle = new Paddle(_playArea, _playArea.Width - aiPaddleTX.Width - 10);
        _aiPaddle.SetTexture(aiPaddleTX);
    }


    private void LoadBall()
    {
        _ball = new Ball(   _playArea, 
                            Content.Load<Texture2D>("Textures/WhiteBall"), 
                            Content.Load<SoundEffect>("SoundFx/PaddleHit"), 
                            Content.Load<SoundEffect>("SoundFx/WallHit"));
    }

    private void LoadSoundFx()
    {
        _scoreFx = Content.Load<SoundEffect>("SoundFx/Score");
    }

    protected override void Update(GameTime gt)
    {
        if (_paused)
        {
            WaitForStart();
        }
        else
        {
            _ball.Update(gt);
            HandlePlayerMovement(gt);
            MoveAIPlayer(gt);
            CheckPaddleCollision();
            CheckPointScored();
        }

        base.Update(gt);
    }



    protected override void Draw(GameTime gt)
    {
        GraphicsDevice.Clear(Color.Black);     

        _spriteBatch.Begin();

        if (_paused)
        {
            _titleObj.DrawText(_spriteBatch, "MG Pong", TextObject.CenterText.Horizontal, 50);
            _titleObj.DrawText(_spriteBatch, "Press [SPACE] to start!", TextObject.CenterText.Both);
        }
        else
        {
            DrawScoreBar();
            DrawBoard();
            DrawPaddlesAndBall();
        }

        _spriteBatch.End();

        base.Draw(gt);
    }

    private void DrawScoreBar()
    {
        _scoreBar.Draw(_spriteBatch, _playerScore, _aiPlayerScore);
    }

    private void DrawBoard()
    {
        _spriteBatch.Draw(_board, _playArea, Color.White);
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
        Rectangle ballRect = _ball.Bounds;
        if (ballRect.Intersects(_playerPaddle.getBoundingRect()) || ballRect.Intersects(_aiPaddle.getBoundingRect()))
        {
            _ball.BounceOffPaddle();
        }
    }

    protected void CheckPointScored()
    {
        Rectangle ballRect = _ball.Bounds;
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
        _scoreFx.Play();
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

    protected void WaitForStart()
    {
        if (IsKeyDown(Keys.Space))
        {
            _paused = false;
        }
    }

    protected void MoveAIPlayer(GameTime gt)
    {
        float ballY = _ball.Bounds.Y;
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
