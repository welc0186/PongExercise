using Godot;
using System;

public class LabelFactory : IMenuItemFactory
{
    
	PackedScene _labelScene;
	string _text;

	public LabelFactory(string text)
	{
		_labelScene = GD.Load<PackedScene>("res://Scenes/menu_label.tscn");
		_text = text;
	}
	
	public Control MakeMenuItem()
    {
        var label = (Label) _labelScene.Instantiate();
		label.Text = _text;
		return label;
    }

}
