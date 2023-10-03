using Godot;
using System;

public partial class Goal : Area2D
{
	
	// Called when the node enters the scene tree for the first time.
	// TO DO: Should I store this as delegate so I can unsubscribe to the signal?
	public override void _Ready()
	{
		this.BodyEntered += (Node2D body) => {
			if(body is Ball)
				Events.onGoalEnter.Invoke(this, null);
		};
	}

}
