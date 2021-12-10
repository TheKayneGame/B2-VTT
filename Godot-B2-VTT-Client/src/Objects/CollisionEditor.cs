using Godot;
//using System.Collections;
using Godot.Collections;
using System.Linq;

public class CollisionEditor : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    [Export]
    PackedScene collisionScene;

    Collision currentCollision;

    Point selectedPoint;

    Point movingPoint;

    Segment selectedSegment;

    Segment movingSegment;

    bool draggingPoint;

    Vector2 curMousePos;

    public override void _Ready()
    {
        
    }


    public override void _Process(float delta)
    {
        if (movingPoint != null)
        {
            movingPoint.SetPointPosition(currentCollision.GetLocalMousePosition());
        }
    }

    public void SetCurrentCollision(Node input){
        currentCollision = input.GetNodeOrNull<Collision>("Collision");
        if (currentCollision == null)
        {
            currentCollision = collisionScene.Instance() as Collision;
            input.AddChild(currentCollision);
        }

    }

    public override void _UnhandledInput(InputEvent @event)
    {
        //Er gaan losse punten en segmenten bestaan, elk van deze segmenten zullen twee nodes refereren om zo hun dimensies te krijgen. zij verbinden ook de node's "onMoved" signaal met hun eigen update signaal.
        //Een segment bepaald voor zich zelf wat hij is en wat hij doet (is hij deur is hij raam, open of dicht) makkelijk te serialiseren.
        //tl;dr wolk van nodes met segmenten die ze verbinden. Tot morgen :)

        if (currentCollision == null)
            return;


        if (@event.IsActionPressed("token_interact_primary"))
        {

            // Physics2DDirectSpaceState space = GetWorld2d().DirectSpaceState;
            // Dictionary temp = space.IntersectRay(new Vector2(), GetGlobalMousePosition());
            // if (temp.Count > 0)
            // {
            //     Node nodeUnderMouse = (temp["collider"] as Node);
            //     if (nodeUnderMouse.IsInGroup("CollisionPoint"))
            //     {
            //         if (movingPoint == null){
            //             movingPoint = nodeUnderMouse as Point;
            //             draggingPoint = true;
            //             return;
            //         }

            //     }
            // }

            curMousePos = currentCollision.GetLocalMousePosition();

            Array hovering = GetTree().GetNodesInGroup("hovering");
            if (hovering.Count > 0)
            {
                if (movingPoint == null)
                {
                    movingPoint = hovering[hovering.Count - 1] as Point;
                    movingPoint.Raise();
                    draggingPoint = true;
                    return;
                }

            }

            if (selectedPoint == null)
            {

                selectedPoint = currentCollision.AddPoint(curMousePos);
            }
            else
            {
                if (hovering.Count > 1)
                {
                    Point newPoint = hovering[hovering.Count - 2] as Point;
                    if (newPoint == selectedPoint){
                        dropMoving();
                        return;
                    }
                        
                    movingSegment.PointB = newPoint;
                    movingPoint.QueueFree();
                    movingPoint = null;
                    movingSegment = null;
                    selectedPoint = null;
                    return;

                }
                selectedPoint = movingPoint;
            }

            movingPoint = currentCollision.AddPoint(curMousePos);
            movingSegment = currentCollision.AddSegment(selectedPoint, movingPoint);



        }

        if (@event.IsActionPressed("token_interact_secondary"))
        {
            if (selectedPoint != null)
            {
                dropMoving();
            }


        }

        if (@event.IsActionReleased("token_interact_primary"))
        {
            if (draggingPoint)
            {
                if ((curMousePos - curMousePos).Length() < 1)
                {
                    selectedPoint = movingPoint;
                    movingPoint = currentCollision.AddPoint(curMousePos);
                    movingSegment = currentCollision.AddSegment(selectedPoint, movingPoint);
                }
                else
                {
                    movingPoint = null;
                }
                draggingPoint = false;

            }

        }



    }

    private void dropMoving()
    {
        selectedPoint = null;
        movingPoint.QueueFree();
        movingSegment.QueueFree();
        movingPoint = null;
        movingSegment = null;
    }




}
