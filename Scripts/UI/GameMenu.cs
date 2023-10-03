using Godot;
using System;

public partial class GameMenu : CanvasLayer
{
	
	bool _paused;
	Node _menuNode;

	// ** Title Menu **
	IMenuItemFactory[] _titleMenu = new IMenuItemFactory[] {
		new LabelFactory("PONG"),
		new ButtonFactory("Play",     new ButtonMessage("New Game")),
		new ButtonFactory("Settings", new ButtonMessage("Settings Menu")),
		new ButtonFactory("Quit",     new ButtonMessage("Quit Game"))
	};

	// ** Settings Menu **
	IMenuItemFactory[] _settingsMenu = new IMenuItemFactory[] {
		new LabelFactory("Settings"),
		new SettingsCheckboxFactory("Sound", new GameSetting<bool>(SettingCategory.SFXON, true)),
		new SettingsSliderFactory(new GameSetting<int>(SettingCategory.SFXVOLUMEDB, 8)),
		new LabelFactory("Music"),
		new SettingsSliderFactory(new GameSetting<int>(SettingCategory.MUSICVOLUMEDB, 8)),
		new ButtonFactory("Back", new ButtonMessage("Main Menu")),
	};

	// ** Pause Menu **
	IMenuItemFactory[] _pauseMenu = new IMenuItemFactory[] {
		new LabelFactory("Game Paused"),
		new ButtonFactory("New",       new ButtonMessage("New Game")),
		new ButtonFactory("Resume",    new ButtonMessage("Resume Game")),
		new ButtonFactory("Main Menu", new ButtonMessage("Main Menu")),
		new ButtonFactory("Quit",      new ButtonMessage("Quit Game"))
	};

	// ** Game Over Menu **
	IMenuItemFactory[] _gameOverMenu = new IMenuItemFactory[] {
		new LabelFactory("Game Over"),
		new ButtonFactory("New",       new ButtonMessage("New Game")),
		new ButtonFactory("Main Menu", new ButtonMessage("Main Menu")),
		new ButtonFactory("Quit",      new ButtonMessage("Quit Game"))
	};
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// CreateMenu(_pauseMenu);
		ProcessMode = ProcessModeEnum.Always;
		if(HasNode("/root/MainGame"))
		{
			this.Hide();
			_paused = true;
			PressPause();
		}
		if(HasNode("/root/TitleScreen"))
		{
			CreateMenu(_titleMenu);
		}
		Events.onButtonPressed.Subscribe(OnButtonPressed);
		Events.onGameOver.Subscribe(OnGameOver);
	}
    
	public override void _ExitTree()
	{
		Events.onButtonPressed.Unsubscribe(OnButtonPressed);
		Events.onGameOver.Unsubscribe(OnGameOver);
	}

    private void OnGameOver(Node node, object data)
    {
        CreateMenu(_gameOverMenu);
		PressPause();
    }

    private void CreateMenu(IMenuItemFactory[] menuItems)
    {
		if(_menuNode == null)
		{
			_menuNode = FindChild("MenuItems");
		}
		foreach(Node child in _menuNode.GetChildren())
		{
			child.QueueFree();
		}

		bool firstFocus = true;
		foreach(IMenuItemFactory factory in menuItems)
		{
			var menuItem = (Control) factory.MakeMenuItem();
			_menuNode.AddChild(menuItem);
			if(menuItem.FocusMode != Control.FocusModeEnum.None && firstFocus)
			{
				firstFocus = false;
				menuItem.GrabFocus();
			}
		}
    }

    private void OnButtonPressed(Node node, object data)
    {
		if(node is not MenuButton)
		{
			return;
		}
		var button = (MenuButton) node;
		switch(button.Message.Text)
		{
			case "New Game":
				Events.onNewGame.Invoke(this, null);
				if(_paused)
					PressPause();
				break;
			case "Resume Game":
				PressPause();
				break;
			case "Main Menu":
				Events.onMainMenuRequested.Invoke(this, null);
				CreateMenu(_titleMenu);
				_paused = true;
				break;
			case "Settings Menu":
				CreateMenu(_settingsMenu);
				break;
			case "Quit Game":
				GetTree().Quit();
				break;
			default:
				break;
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{	
		if (Input.IsActionJustPressed("ui_cancel") && !HasNode("/root/TitleScreen"))
		{
			CreateMenu(_pauseMenu);
			PressPause();
		}
	}

    private void PressPause()
    {
		_paused = !_paused;
		GetTree().Paused = _paused;
		this.Visible = _paused;
    }

}
