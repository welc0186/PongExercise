using Godot;
using System;

public interface IMenuItemFactory
{
	public Control MakeMenuItem();
}

public interface IGameSetting
{
	public SettingCategory Name { get; }
	public void Update(IGameSetting newSetting);
}
