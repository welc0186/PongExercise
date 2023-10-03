using Godot;
using System;

public static class BallDeathParticleSpawner
{
	public const int SCREEN_CENTER_X = 90;
	public const int SCREEN_CENTER_Y = 160;
	public const string BALL_PARTICLES_PATH = "res://Scenes/ball_death_particles.tscn";
	
	public static void Spawn(Vector2 location, Node parent)
	{
		var ballParticleScene = GD.Load<PackedScene>(BALL_PARTICLES_PATH);
		var particlesNode = ballParticleScene.Instantiate<Node2D>();
		parent.AddChild(particlesNode);
		particlesNode.GlobalPosition = location;
		Vector2 screenCenter = new Vector2(SCREEN_CENTER_X, SCREEN_CENTER_Y);
		particlesNode.LookAt(screenCenter);
	}
}

public partial class BallDeathParticles : GpuParticles2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.ProcessMode = ProcessModeEnum.Always;
		
		// Set up Timer
		this.Emitting = true;
		var timer = new Timer();
		timer.WaitTime = this.Lifetime;
		timer.Autostart = true;
		timer.Timeout += this.QueueFree;
		timer.ProcessMode = ProcessModeEnum.Always;
		AddChild(timer);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
