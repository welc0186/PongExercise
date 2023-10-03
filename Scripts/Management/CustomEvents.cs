using System.Collections;
using System.Collections.Generic;
using Godot;
using System;


public static class Events
{
    // ** UI **
    public static readonly CustomEvent onButtonPressed = new CustomEvent();
    public static readonly CustomEvent onFocusEntered  = new CustomEvent();

    // ** Main Game **
    public static readonly CustomEvent onGoalEnter    = new CustomEvent();
    public static readonly CustomEvent onBallSpawned  = new CustomEvent();
    public static readonly CustomEvent onBallCollided = new CustomEvent();
    public static readonly CustomEvent onBallKicked   = new CustomEvent();

    // ** Game State **
    public static readonly CustomEvent onNewGame           = new CustomEvent();
    public static readonly CustomEvent onGameOver          = new CustomEvent();
    public static readonly CustomEvent onMainMenuRequested = new CustomEvent();

    // ** Config **
    public static readonly CustomEvent onSettingsChanged = new CustomEvent();
}

public class CustomEvent
{
    private event Action<Node, object> _event;

    public void Invoke(Node sender, object data)
    {
        _event?.Invoke(sender, data);
    }

    public void Subscribe(Action<Node, object> listener)
    {
        _event += listener;
    }

    public void Unsubscribe(Action<Node, object> listener)
    {
        _event -= listener;
    }
}
