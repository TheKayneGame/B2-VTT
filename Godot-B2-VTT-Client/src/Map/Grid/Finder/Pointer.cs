using Godot;
using System;

public class Pointer : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public bool Dragging = false;
    public bool Active = false;

    public bool IsSet = false;


	[Signal]
    public delegate void Moved(Vector2 position);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void Activate()
    {
        if (IsSet)
        {
            Visible = true;
            Active = true;
        }
    }

    public void Deactivate()
    {
        if (IsSet)
        {
            Visible = false;
            Active = false;
        }
    }

    public void Spawn()
    {
        Position = GetGlobalMousePosition();
        Visible = true;
        Active = true;
		IsSet = true;
        EmitSignal(nameof(Moved), Position);

    }
    public void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        Vector2 mousePos = GetGlobalMousePosition();
        if (!GetTree().IsInputHandled() && Active)
        {
            if (@event.IsActionPressed("token_interact_primary"))
            {
                GetTree().SetInputAsHandled();
                Dragging = true;
                Raise();
                return;
            }
        }
    }

    public override void _Process(float delta)
    {
        if (Dragging)
        {
            Position = GetGlobalMousePosition().Snapped(Vector2.One/2);
			EmitSignal(nameof(Moved), Position);
        }
    }

    private bool IsOnTop()
    {
        return object.ReferenceEquals(GetTop(), this);
    }

    private object GetTop()
    {
        Godot.Collections.Array hoveringNodes = GetTree().GetNodesInGroup("hovering");
        if (hoveringNodes.Count < 1)
            return null;
        return hoveringNodes[hoveringNodes.Count - 1] as Node2D;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionReleased("token_interact_primary"))
        {
            Dragging = false;
        }

    }
}

