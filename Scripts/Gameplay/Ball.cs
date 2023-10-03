using Godot;
using System;

public static class BallSpawner
{
	public static Ball Spawn()
	{
		var ballScene = GD.Load<PackedScene>("res://Scenes/ball.tscn");
		Ball ball = ballScene.Instantiate<Ball>();
		return ball;
	}
}

public partial class Ball : RigidBody2D
{
	public const float BALL_SPEED = 200f;
	public const float KICK_DELAY = 0.5f;

	// Max should not exceed Pi/2 radians (mirrored)
	public const float KICK_RADS_MIN = 0.7f;  //~40 degrees
	public const float KICK_RADS_MAX = 1.22f; //~70 degrees
	public const float PI = 3.14f;

	public override void _Ready()
	{
		SimpleTimer.Create(KICK_DELAY, this).Timeout += Kick;
		Events.onBallSpawned.Invoke(this, null);
		this.ContactMonitor = true;
		this.MaxContactsReported = 1;
		this.BodyEntered += (Node body) => {
			if(body is not Goal)
			{
				Events.onBallCollided.Invoke(this, null);
			}
		};
	}

	void Kick()
	{
		var random = new RandomNumberGenerator();

		// Random initial angle
		random.Randomize();
		var kickRads = random.RandfRange(KICK_RADS_MIN, KICK_RADS_MAX);

		// Random left/right direction
		random.Randomize();
		kickRads = random.RandfRange(0,1) > 0.5f? kickRads : PI - kickRads;

		var kickVector = Vector2.Right.Rotated(kickRads);
		LinearVelocity = kickVector * BALL_SPEED;
		Events.onBallKicked.Invoke(this, null);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
	}

}
