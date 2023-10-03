using Godot;
using System;

public class SettingsSliderFactory : IMenuItemFactory
{
	PackedScene _sliderScene;
	GameSetting<int> _setting;

	public SettingsSliderFactory(GameSetting<int> setting)
	{
		_sliderScene = GD.Load<PackedScene>("res://Scenes/settings_slider.tscn");
		_setting = setting;
	}

	public Control MakeMenuItem()
	{
		var slider = (SettingsSlider) _sliderScene.Instantiate();
		slider.Setting = _setting;
		return slider;
	}
}

public partial class SettingsSlider : HSlider
{
	public GameSetting<int> Setting;
	private double _value;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		Setting.Update(GameConfig.Instance.GetGameSetting(Setting));
		this.Value = Setting.Value;
		_value = this.Value;
		this.DragEnded += (b) => {UpdateValue();};
		this.ValueChanged += (d) => {
			if(d != _value)
			{
				UpdateValue();
			}
		};
		this.FocusEntered += () => {Events.onFocusEntered.Invoke(this, null);};
	}

    private void UpdateValue()
    {
        _value = this.Value;
		Setting.Value = Mathf.RoundToInt(_value);
		Events.onSettingsChanged.Invoke(this, Setting);
    }

    public override void _Process(double delta)
    {
        _value = this.Value;
    }
}
