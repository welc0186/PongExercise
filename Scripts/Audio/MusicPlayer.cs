using Godot;
using System;

public partial class MusicPlayer : AudioStreamPlayer
{
	public const string MUSIC01_PATH = "res://Music/spooky_professorlamp.wav";
	public const string MUSIC_BUS = "Music";
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.ProcessMode = Node.ProcessModeEnum.Always;
		this.Bus = MUSIC_BUS;
		this.Stream = GD.Load<AudioStream>(MUSIC01_PATH);
		this.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
