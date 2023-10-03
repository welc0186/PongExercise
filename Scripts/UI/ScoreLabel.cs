using Godot;
using System;

public partial class ScoreLabel : Label
{
	
	public const string ENEMY_GOAL = "EnemyGoal";
	private int _score;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Events.onGoalEnter.Subscribe(OnGoalEnter);
		Events.onNewGame.Subscribe(OnNewGame);
		UpdateScore(0);
	}

    public override void _ExitTree()
	{
		Events.onGoalEnter.Unsubscribe(OnGoalEnter);
		Events.onNewGame.Unsubscribe(OnNewGame);
	}

    private void OnNewGame(Node node, object data)
    {
        UpdateScore(0);
    }

	void UpdateScore(int score)
	{
		_score = score;
		this.Text = _score.ToString();
	}

	void OnGoalEnter(Node goal, object data)
	{
		if(goal.Name == ENEMY_GOAL)
		{
			_score++;
			UpdateScore(_score);
		}
	}

}
