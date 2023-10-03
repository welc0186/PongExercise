using Godot;
using System;

public partial class MainGame : Node2D
{
	
	public readonly Vector2 BALL_SPAWN_LOC = new Vector2(90, 160);

	private Ball _ball;
	private PackedScene ballParticleScene;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Events.onGoalEnter.Subscribe(OnGoalEnter);
		Events.onNewGame.Subscribe(SpawnBallEvent);
		Events.onMainMenuRequested.Subscribe(ToMainMenu);
		SpawnBall();
	}

    public override void _ExitTree()
	{
		Events.onGoalEnter.Unsubscribe(OnGoalEnter);
		Events.onNewGame.Unsubscribe(SpawnBallEvent);
		Events.onMainMenuRequested.Unsubscribe(ToMainMenu);
	}

    private void ToMainMenu(Node node, object data)
    {
        GetTree().ChangeSceneToFile("res://Scenes/title_screen.tscn");
    }


    private void OnGoalEnter(Node node, object data)
    {
		BallDeathParticleSpawner.Spawn(_ball.GlobalPosition, this);
		if(node.Name == ScoreLabel.ENEMY_GOAL)
		{
			SpawnBall();
			return;
		}
		Events.onGameOver.Invoke(null, null);
    }

    void SpawnBall()
	{
		if (_ball != null)
		{
			_ball.QueueFree();
		}
		_ball = BallSpawner.Spawn();
		var scene = GetTree().CurrentScene;
		scene.AddChild(_ball);
		_ball.GlobalPosition = BALL_SPAWN_LOC;
	}

	void SpawnBallEvent(Node node, object data)
	{
		SpawnBall();
	}

}
