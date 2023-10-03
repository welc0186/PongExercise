using Godot;
using System;
using System.Diagnostics;
using System.Collections.Generic;

public class SFXAudioClip
{
	private string _path;
	private float _volumeDB;
	public CustomEvent Trigger;
	public const int MAX_VOLUMEDB = 10;

	public SFXAudioClip(string path, float volumeDB, CustomEvent trigger)
	{
		_path = path;
		_volumeDB = volumeDB;
		Trigger = trigger;
	}

	public void Play(Node parent)
	{
		if(parent == null)
			return;
		AudioStreamPlayer player = new AudioStreamPlayer();
        parent.AddChild(player);
		player.ProcessMode = Node.ProcessModeEnum.Always;
		player.Stream = GD.Load<AudioStream>(_path);
		player.VolumeDb = _volumeDB;
		player.Bus = "SFX1";
		if (player.VolumeDb > MAX_VOLUMEDB)
		{
			GD.Print("Volume exceeded maximum setting");
			player.VolumeDb = MAX_VOLUMEDB;
		}
		player.Play();
		player.Finished += player.QueueFree;
	}
}

public partial class SFXAudioPlayer : Node
{
	
	public const string BUTTON = "res://Sounds/Menu__006.wav";
	public const string GOAL   = "res://Sounds/Explosion2__006.wav";
	public const string BALL   = "res://Sounds/Menu__004.wav";

	// List of audio clips and events that trigger them
	public readonly List<SFXAudioClip> Clips = new List<SFXAudioClip>() {
		new SFXAudioClip(BUTTON,   0, Events.onButtonPressed),
		new SFXAudioClip(BUTTON,  -5, Events.onFocusEntered),
		new SFXAudioClip(BUTTON,  -5, Events.onSettingsChanged),
		new SFXAudioClip(GOAL,   -10, Events.onGoalEnter),
		new SFXAudioClip(BALL,    -5, Events.onBallCollided),
		new SFXAudioClip(BALL,   -10, Events.onBallKicked)
	};

	public override void _Ready()
	{
		foreach(SFXAudioClip Clip in Clips)
		{
			Clip.Trigger.Subscribe((n, o) => Clip.Play(this));
		}
	}

}
