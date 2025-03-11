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
    private string _instructionsText;
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

        _instructionsText += "\nYou control the left paddle, try to score points against the AI paddle,";
        _instructionsText += "\n                 which gets better with each point scored!";
        _instructionsText += "\n\n                           W - Move paddle up";
        _instructionsText += "\n                           S - Move paddle down";
        _instructionsText += "\n                           P - Pause the game";
        _instructionsText += "\n                           ESCAPE - Return to title screen";
        _instructionsText += "\n\n\n                       Press [SPACEBAR] to play!";
    }

    public override void Enter()
    {
        MediaPlayer.Volume = 0.3f;
        //MediaPlayer.Play(_titleMusic);
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
        _instructions.DrawText(sb, _instructionsText, TextObject.CenterText.Horizontal, 150);
        base.Draw(sb);
    }
}
