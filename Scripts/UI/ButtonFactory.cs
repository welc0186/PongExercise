using Godot;
using System;

public class ButtonFactory : IMenuItemFactory
{
    
	PackedScene _buttonScene;
	string _text;
	ButtonMessage _message;

	public ButtonFactory(string text, ButtonMessage m)
	{
		_buttonScene = GD.Load<PackedScene>("res://Scenes/menu_button.tscn");
		_text = text;
		_message = m;
	}
	
	public Control MakeMenuItem()
    {
        var button = (MenuButton) _buttonScene.Instantiate();
		button.Text = _text;
		button.Message = _message;
		return button;
    }

}
