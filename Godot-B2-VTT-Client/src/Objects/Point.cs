using Godot;
using System;

public class Point : Position2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    private bool dragging;

    [Signal]
    public delegate void Moved(Vector2 pos);

    [Signal]
    public delegate void Deleted();
    public override void _Ready()
    {
        AddToGroup("CollisionPoint");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (dragging)
            SetPointPosition(GetGlobalMousePosition());
    }

    public void SetPointPosition(Vector2 pos)
    {
        Position = pos;
        EmitSignal(nameof(Moved), pos);
    }



    public void _MouseEntered()
    {
        AddToGroup("hovering");

    }

    public void _MouseExited()
    {
        RemoveFromGroup("hovering");
    }

    public override void _UnhandledInput(InputEvent @event)
    {

        if (@event.IsActionReleased("token_interact_primary"))
        {
            dragging = false;
        }
    }


}
