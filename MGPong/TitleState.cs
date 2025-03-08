using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using CALIMOE;

namespace MGPong;

public class TitleState : GameState
{
    private TextObject _titleText;
    private TextObject _instructions;
    private Song _titleMusic;

    public TitleState(StateManager sm, AssetManager am, InputHelper ih) :base(sm, am, ih)
	{
        _name = "Title";
	}

    public override void LoadContent()
    {
        _titleMusic = _am.LoadMusic("NaturalLife");
        _titleText = new TextObject(_am.LoadFont("Title"));
        _instructions = new TextObject(_am.LoadFont("Instructions"));
    }

    public override void Enter()
    {
        MediaPlayer.Volume = 0.3f;
        MediaPlayer.Play(_titleMusic);
        base.Enter();
    }

    public override void HandleInput(GameTime gt)
    {
        base.HandleInput(gt);

        if (_ih.KeyPressed(Keys.Space))
        {
            _sm.SwitchState("Play");
        }
    }

    public override void Update(GameTime gt)
    {
        HandleInput(gt);
        base.Update(gt);
    }

    public override void Draw(SpriteBatch sb)
    {
        _titleText.DrawText(sb, "MG Pong", TextObject.CenterText.Horizontal, 50);
        _instructions.DrawText(sb, "W & S to control the paddle", TextObject.CenterText.Horizontal, 150);
        _instructions.DrawText(sb, "P to pause the game", TextObject.CenterText.Horizontal, 200);
        _instructions.DrawText(sb, "Escape to quit", TextObject.CenterText.Horizontal, 250);
        _instructions.DrawText(sb, "AI paddle gets better with each point scored!", TextObject.CenterText.Horizontal, 300);
        _instructions.DrawText(sb, "Press [SPACE] to play!", TextObject.CenterText.Horizontal, 400);

        base.Draw(sb);
    }
}
