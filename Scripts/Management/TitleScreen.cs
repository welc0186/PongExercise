using Godot;
using System;

public partial class TitleScreen : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Events.onNewGame.Subscribe(OnNewGame);
	}

	public override void _ExitTree()
	{
		Events.onNewGame.Unsubscribe(OnNewGame);
	}

    private void OnNewGame(Node node, object data)
    {
        GetTree().ChangeSceneToFile("res://Scenes/main_scene.tscn");
    }

}
