using Godot;
using Microsoft.VisualBasic;
using System;

public partial class EnemyPaddle : RigidBody2D
{
	
	public const float PADDLE_SPEED      = 50f;
	public const float REACT_SECONDS     = 1f;
	public const float REACT_THRESHOLD_Y = 260f;
	public const float MIN_X_POS         = 20f;
	public const float MAX_X_POS         = 160f;
	public const float COURT_WIDTH       = 180f;

	private Ball _ball;
	private Vector2 _lastBallPos;
	private Vector2 _targetPos;
	private Vector2? _wayPoint;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Events.onBallSpawned.Subscribe(OnBallSpawned);
	}

    public override void _ExitTree()
    {
        Events.onBallSpawned.Unsubscribe(OnBallSpawned);
    }

    private void OnBallSpawned(Node node, object data)
    {
        if(node is Ball)
		{
			_ball = (Ball) node;
			_lastBallPos = _ball.GlobalPosition;
			_wayPoint = null;
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
		ProcessWaypoint();
		MoveToBall();
	}

    private void ProcessWaypoint()
    {
		if(_ball == null)
		{
			_wayPoint = null;
			return;
		}

		if(_wayPoint == null)
		{
			SimpleTimer.Create(REACT_SECONDS, this, true).Timeout += () => {
				_wayPoint = null;
			};
			_wayPoint = CalculateIntercept(_lastBallPos, _ball.GlobalPosition);
		}
		_lastBallPos = _ball.GlobalPosition;
		
    }

    private Vector2? CalculateIntercept(Vector2 lastBallPos, Vector2 currBallPos)
    {
			var interceptVariant = Geometry2D.LineIntersectsLine(
				_lastBallPos, _ball.GlobalPosition - _lastBallPos, this.GlobalPosition, Vector2.Right
				);
			if (interceptVariant.VariantType == Variant.Type.Nil)
			{
				return null;
			}

			var intercept = (Vector2) interceptVariant.AsVector2();

			// Bound within 2 widths of court
			var xBounded = intercept.X % (COURT_WIDTH*2);

			// Bounce left
			if (xBounded < 0)
			{
				return new Vector2(-xBounded, intercept.Y);
			}

			// Bounce right
			if (xBounded > COURT_WIDTH)
			{
				return new Vector2(2*COURT_WIDTH - xBounded, intercept.Y);
			}

			return intercept;
    }

    // should be called every frame to update ball's previous position
    private bool BallApproaching()
    {
		if(_ball == null)
			return false;
		var ret = false;
		var currBallPos = _ball.GlobalPosition;
        if (currBallPos.Y < _lastBallPos.Y)
			ret = true;
		_lastBallPos = _ball.GlobalPosition;
		GD.Print(_lastBallPos, ret);
		return ret;
    }

    private void MoveToBall()
    {
        if (_ball == null || _wayPoint == null)
		{
			LinearVelocity = Vector2.Zero;
			return;
		}
		var waypointDistance = this.GlobalPosition.DistanceTo(_wayPoint.Value);
		if(waypointDistance > 2f)
		{
			int direction = Mathf.Sign(_wayPoint.Value.X - this.GlobalPosition.X);
			LinearVelocity = new Vector2(direction, 0) * PADDLE_SPEED;
			return;
		}
		LinearVelocity = Vector2.Zero;
    }

}
