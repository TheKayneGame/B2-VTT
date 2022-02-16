using Godot;
using Godot.Collections;

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
            SetPointPosition(GetParent<Node2D>().GetLocalMousePosition());
    }

    public void SetPointPosition(Vector2 pos)
    {
        Position = pos;
        EmitSignal(nameof(Moved), pos);
    }

    public void Delete()
    {
        EmitSignal(nameof(Deleted));
        QueueFree();
    }



    public void _MouseEntered()
    {
        AddToGroup("PointHovering");

    }

    public void _MouseExited()
    {
        RemoveFromGroup("PointHovering");
    }

    public Dictionary<object, object> ToDict()
    {
        return new Dictionary<object, object>(){
                    {"PosX", Position.x},
                    {"PosY", Position.y}
        };

    }

    public void FromDict(Dictionary<object, object> dict)
    {
        Position = new Vector2((float)dict["PosX"], (float)dict["PosY"]);

    }


}
