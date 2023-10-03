using Godot;
using System;

public static class SimpleTimer
{

	public static Timer Create(float seconds = 1, Node parent = null, bool pausable = false)
	{
		var timer = new Timer();
		timer.WaitTime = seconds;
		timer.Autostart = true;
		timer.Timeout += timer.QueueFree;
		if(!pausable)
			timer.ProcessMode = Node.ProcessModeEnum.Always;
		if(parent != null)
			parent.AddChild(timer);
		return timer;
	}

}
