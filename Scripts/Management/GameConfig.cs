using Godot;
using System;
using System.Collections.Generic;

public partial class GameConfig : Node
{
	
	public static GameConfig Instance;

	private List<IGameSetting> _settings = new List<IGameSetting>()
	{
		new GameSetting<bool>(SettingCategory.SFXON, true),
		new GameSetting<int>(SettingCategory.SFXVOLUMEDB, 8)
	};
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		Events.onSettingsChanged.Subscribe(OnSettingsChanged);
	}

    public override void _ExitTree()
    {
        Events.onSettingsChanged.Unsubscribe(OnSettingsChanged);
    }

	public IGameSetting GetGameSetting(IGameSetting settingReq)
	{
		foreach(IGameSetting setting in _settings)
		{
			if(setting.Name.Equals(settingReq.Name))
				return setting;
		}
		return null;
	}

	public IGameSetting GetGameSetting(SettingCategory name)
	{
		foreach(IGameSetting setting in _settings)
		{
			if(setting.Name.Equals(name))
				return setting;
		}
		return null;
	}

    private void OnSettingsChanged(Node node, object obj)
    {
        if(obj is not IGameSetting)
			return;
		foreach(IGameSetting setting in _settings)
		{
			setting.Update((IGameSetting) obj);
		}
    }

}
