using Godot;
using System;

public class Token : Area2D
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
    private CollisionShape2D collisionBox;
    private Vector2 textureSize;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Connect("mouse_entered", this,)
        sprite = GetNode<Sprite>("Sprite");
        fovCone = GetNode<Polygon2D>("ViewCone");
        collisionBox = GetNode<CollisionShape2D>("CollisionBox");
        textureSize = sprite.Texture.GetSize();
        //this.Scale= new Vector2(2,2);
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

    public void FitSprite(){
        Vector2 nodeSize = new Vector2(collisionBox.Scale * 2);
        float imageScaleX = nodeSize.x / textureSize.x;
        float imageScaleY = nodeSize.y / textureSize.y;

        float imageScalar = (imageScaleX < imageScaleY) ? imageScaleX : imageScaleY;
        sprite.Scale = new Vector2(imageScalar,imageScalar);
    }

    public void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {

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

        if (@event.IsActionPressed("debug_2"))
        {
            if (IsOnTop())
            {
                //crashes when called after spawn and mouse has not moved, because _MouseEnter is not called so this node is not added to the "hovering" group.
                //Caus
                GD.Print("debug_2");
                QueueFree();
                GetTree().SetInputAsHandled();
            }

        }
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
        return object.ReferenceEquals(GetTop(), this);
    }

    private object GetTop()
    {
        Godot.Collections.Array hoveringNodes = GetTree().GetNodesInGroup("hovering");
        if (hoveringNodes.Count < 1)
            return null;
        return hoveringNodes[hoveringNodes.Count - 1] as Node2D;
    }


}
