using Godot;
using System;

public class icon : Area2D
{
    // Declare member variables here. Examples:
    [Export]
    public string DragGroup = "";
    private bool _dragging = false;
    private bool _rotating = false;
    private Vector2 clickPos;
    private Vector2 curMousePos;
    private Sprite sprite;
    private Polygon2D fovCone;
    private Vector2 textureSize;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Connect("mouse_entered", this,)
        sprite = GetNode<Sprite>("Sprite");
        fovCone = GetNode<Polygon2D>("ViewCone");

        textureSize = sprite.Texture.GetSize();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (_dragging)
            Position = GetViewport().GetMousePosition() + clickPos;

        if (_rotating)
        {
            float angle = Position.AngleToPoint(GetViewport().GetMousePosition());
            fovCone.RotationDegrees = (float)Math.Round(((180 / Math.PI) * angle) / 90) * 90;
            GD.Print(angle);
        }

    }

    public void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        //GD.Print("event?");
        Vector2 mousePos = (@event as InputEventMouse).Position;
        if (!GetTree().IsInputHandled() && IsOnTop())
        {
            

            if (@event.IsActionPressed("token_interact_primary"))
            {
                clickPos = Position - mousePos;
                _dragging = true;
                GD.Print("Dragging Location: ", clickPos);
                GetTree().SetInputAsHandled();
                Raise();
            }

            if (@event.IsActionPressed("token_interact_secondary"))
            {
                curMousePos = mousePos;
                _rotating = true;
                fovCone.Visible = true;
                GD.Print("Rotating", curMousePos);
                GetTree().SetInputAsHandled();
                Raise();
            }

        }


    }
    public override void _Input(InputEvent @event)
    {

        if (@event.IsActionReleased("token_interact_primary"))
        {
            GD.Print("Dropped");
            _dragging = false;
        }

        if (@event.IsActionReleased("token_interact_secondary"))
        {
            _rotating = false;
            fovCone.Visible = false;
        }
        //GetTree().SetInputAsHandled();
    }

    public void _MouseEnter()
    {
        AddToGroup("hovering");
        GD.Print("Enter!");

    }

    public void _MouseExit()
    {

        RemoveFromGroup("hovering");
        GD.Print("Exited!");

    }


    private bool IsOnTop()
    {
        Godot.Collections.Array hoveringNodes  = GetTree().GetNodesInGroup("hovering");
        Node2D topNode = hoveringNodes[hoveringNodes.Count - 1] as Node2D;
        return object.ReferenceEquals(topNode, this);
    }


}
