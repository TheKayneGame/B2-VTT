using Godot;
using System;
using System.Linq;
public class Token : KinematicBody2D
{
    // Declare member variables here. Examples:
    [Export]
    public string DragGroup = "";
    private bool _dragging = false;
    private bool _rotating = false;
    private bool _freeMove = false;
    private Vector2 clickPos;
    private Vector2 curMousePos;
    public Vector2 TargetPos {get; set;}
    private Sprite sprite;
    private Polygon2D fovCone;
    private Area2D area;
    private CollisionShape2D areaCollision;
    private KinematicBody2D body;
    private CollisionShape2D bodyCollision;
    private Vector2 textureSize;


    // Called when the node enters the scene tree for the first time.
    
    public override void _Ready()
    {
        //Connect("mouse_entered", this,)
        sprite = GetNode<Sprite>("Sprite");
        fovCone = GetNode<Polygon2D>("ViewCone");
        area = GetNode<Area2D>("Area");
        areaCollision = GetNode<CollisionShape2D>("Collision");
        //body = GetNode<KinematicBody2D>("Body");
        bodyCollision = GetNode<CollisionShape2D>("Collision");
        
        textureSize = sprite.Texture.GetSize();

        
        //this.Scale= new Vector2(2,2);
        FitSprite();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        

    }

    public override void _PhysicsProcess(float delta)
    {
        if (_dragging)
        {
            var map = area.GetOverlappingAreas().Cast<Map>().FirstOrDefault(x => x.GetType() == typeof(Map));
            if (map != null && !_freeMove){
                TargetPos = map.GetClosestGridPosition(GetGlobalMousePosition());                
                GD.Print(Position);
                
            }
            else {
                TargetPos = GetGlobalMousePosition() + clickPos;
            }
            
        }
        
        Position = Position.LinearInterpolate(TargetPos, delta*10f);

        if (_rotating)
        {
            float angle = Position.AngleToPoint(GetGlobalMousePosition());
            fovCone.RotationDegrees = (float)Math.Round(((180 / Math.PI) * angle) / 90) * 90;
            GD.Print(angle);
        }
    }

    public void FitSprite()
    {
        Vector2 nodeSize = new Vector2(bodyCollision.Scale*2);
        float imageScaleX = nodeSize.x / textureSize.x;
        float imageScaleY = nodeSize.y / textureSize.y;

        float imageScalar = (imageScaleX < imageScaleY) ? imageScaleX : imageScaleY;
        
        sprite.Scale = new Vector2(imageScalar, imageScalar);
    }

    public void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {

        Vector2 mousePos = GetGlobalMousePosition();
        if (!GetTree().IsInputHandled() && IsOnTop())
        {
            
            if (@event.IsActionPressed("token_interact_primary"))
            {
                clickPos = GlobalPosition - mousePos;
                _dragging = true;
                GD.Print("Dragging Location: ", clickPos);
                GetTree().SetInputAsHandled();
                Raise();
                return;
            }

            if (@event.IsActionPressed("token_interact_secondary"))
            {
                curMousePos = mousePos;
                _rotating = true;
                fovCone.Visible = true;
                GD.Print("Rotating", curMousePos);
                GetTree().SetInputAsHandled();
                Raise();
                return;
            }
        }
    }
    public override void _Input(InputEvent @event)
    {

        if (IsOnTop())
        {
            if (@event.IsActionPressed("debug_2"))
            {
                    QueueFree();
            }

            if(@event.IsActionPressed("token_free_move"))
            {
                _freeMove = true;
            }
        }

        if (@event.IsActionReleased("token_interact_primary"))
        {
            _dragging = false;
        }

        if (@event.IsActionReleased("token_interact_secondary"))
        {
            _rotating = false;
            fovCone.Visible = false;
        }

        if(@event.IsActionReleased("token_free_move"))
        {
            _freeMove = false;
        }
    }

    public void _MouseEnter()
    {
        AddToGroup("hovering");

    }

    public void _MouseExit()
    {
        RemoveFromGroup("hovering");
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
