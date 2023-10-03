using Godot;
using System;
using System.Collections.Generic;

public partial class BusManager : Node
{
	
	public const string SFX_BUS = "SFX1";
	public const string MUSIC_BUS = "Music";
	public const int MAX_VOLUMEDB = 20;

	List<IGameSetting> gameSettings = new List<IGameSetting>()
	{
		new GameSetting<bool>(SettingCategory.SFXON, true),
		new GameSetting<int>(SettingCategory.SFXVOLUMEDB, 0),
		new GameSetting<int>(SettingCategory.MUSICVOLUMEDB, -6)
	};
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Events.onSettingsChanged.Subscribe(OnSettingsChanged);
	}

	public override void _ExitTree()
	{
		Events.onSettingsChanged.Unsubscribe(OnSettingsChanged);
	}

    private void OnSettingsChanged(Node node, object obj)
    {
        if(obj is not IGameSetting)
			return;
		foreach(IGameSetting setting in gameSettings)
		{
			setting.Update((IGameSetting) obj);
			SyncSetting(setting);
		}
    }

    private void SyncSetting(IGameSetting setting)
    {
        switch(setting.Name)
		{
			case SettingCategory.SFXON:
				UpdateSFXON(setting);
				break;
			case SettingCategory.SFXVOLUMEDB:
				UpdateSFXVolumeDB(setting);
				break;
			case SettingCategory.MUSICVOLUMEDB:
				UpdateMusicDB(setting);
				break;
			default:
				break;
		}
    }

    private void UpdateSFXVolumeDB(IGameSetting setting)
    {
        if(setting is not GameSetting<int> || setting.Name != SettingCategory.SFXVOLUMEDB)
			return;
		var busID = AudioServer.GetBusIndex(SFX_BUS);
		var settingDB = ((GameSetting<int>) setting).Value;

		// Volume slider level 8 is 0dB, each level is +/- 10dB
		var volumeDB = 10 * (settingDB - 8);
		AudioServer.SetBusVolumeDb(busID, volumeDB);
    }

	// SFX sound is on or off
    private void UpdateSFXON(IGameSetting setting)
    {
        if(setting is not GameSetting<bool> || setting.Name != SettingCategory.SFXON)
			return;
		var busID = AudioServer.GetBusIndex(SFX_BUS);
		AudioServer.SetBusMute(busID, !((GameSetting<bool>) setting).Value);
    }

	private void UpdateMusicDB(IGameSetting setting)
    {
        if(setting is not GameSetting<int> || setting.Name != SettingCategory.MUSICVOLUMEDB)
			return;
		var busID = AudioServer.GetBusIndex(MUSIC_BUS);
		var settingDB = ((GameSetting<int>) setting).Value;

		// Volume slider level 8 is -6dB, each level is +/- 5dB
		var volumeDB = -6 + 5 * (settingDB - 8);
		AudioServer.SetBusVolumeDb(busID, volumeDB);
    }

}
