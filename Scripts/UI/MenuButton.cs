using Godot;
using System;

public class ButtonMessage
{
	public string Text { get; private set; }
	public ButtonMessage(string message)
	{
		Text = message;
	}
}

public partial class MenuButton : Button
{
	public ButtonMessage Message;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Pressed += () => {
			Events.onButtonPressed.Invoke(this, null);
		};
		this.FocusEntered += () => {
			Events.onFocusEntered.Invoke(this, null);
		};
	}

}
