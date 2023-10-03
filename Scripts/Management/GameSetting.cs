using Godot;
using System;

public enum SettingCategory
{
    SFXON,
    SFXVOLUMEDB,
    MUSICON,
    MUSICVOLUMEDB
}

public class GameSetting<T> : IGameSetting
{
    public SettingCategory Name { get; private set; }
    public T Value;

    public GameSetting(SettingCategory name, T value)
    {
        Name = name;
        Value = value;
    }

    public void Update(IGameSetting newSetting)
    {
        if(newSetting is not GameSetting<T> || newSetting.Name != Name)
            return;
        Value = ((GameSetting<T>) newSetting).Value;
    }
}


