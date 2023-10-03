using Godot;
using System;

public class SettingsCheckboxFactory : IMenuItemFactory
{
	PackedScene _checkboxScene;
	string _text;
	GameSetting<bool> _setting;

	public SettingsCheckboxFactory(string text, GameSetting<bool> setting)
	{
		_checkboxScene = GD.Load<PackedScene>("res://Scenes/settings_checkbox.tscn");
		_text = text;
		_setting = setting;
	}

	public Control MakeMenuItem()
	{
		var checkbox = (SettingsCheckbox) _checkboxScene.Instantiate();
		checkbox.Text = _text;
		checkbox.Setting = _setting;
		return checkbox;
	}
}

public partial class SettingsCheckbox : CheckBox
{

	public GameSetting<bool> Setting;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		Setting.Update(GameConfig.Instance.GetGameSetting(Setting));
		this.ButtonPressed = Setting.Value;
		this.Toggled += (b) => {
		  Setting.Value = b;
		  Events.onSettingsChanged.Invoke(this, Setting);
		};
		this.FocusEntered += () => {
			Events.onFocusEntered.Invoke(this, null);
		};
	}
}
