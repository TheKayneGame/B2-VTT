using Godot;
using System;

public class icon : Area2D
{
    // Declare member variables here. Examples:
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
            fovCone.RotationDegrees = (float) Math.Round(((180 / Math.PI) * angle)/90) * 90;
            GD.Print(angle);
        }
            
    }

    public void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx){
        //GD.Print("event?");
        Vector2 mousePos = (@event as InputEventMouse).Position;

        if (@event.IsActionPressed("token_interact_primary"))
        {
            clickPos = Position - mousePos;
            _dragging = true;
            GD.Print("Dragging Location: ", clickPos);
        }

        if (@event.IsActionPressed("token_interact_secondary"))
        {
            curMousePos = mousePos;
                _rotating = true;
                GD.Print("Rotating", curMousePos);
        }
    }

    public void _MouseEnter(){
        GD.Print("Enter!");

    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionReleased("token_interact_secondary"))
        {
            _rotating = false;
        }
        if (@event.IsActionReleased("token_interact_primary"))
        {
            GD.Print("Dropped");
            _dragging = false;
        }
    }


}
