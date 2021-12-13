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

    [Export]
    NodePath MouseColliderPath;

    public Collision currentCollision;

    Point selectedPoint;

    Point movingPoint;

    Segment selectedSegment;

    Segment movingSegment;

    bool alternativeHold;

    bool creatingWall;

    Vector2 curMousePos;

    public override void _Ready()
    {

    }


    public override void _Process(float delta)
    {
        if (movingPoint != null)
        {
            Vector2 temppos = currentCollision.GetLocalMousePosition().Snapped(Vector2.One * 8);
            movingPoint.SetPointPosition(temppos);
        }

    }

    public void SetCurrentCollision(Node input)
    {
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

        //todo Add Existing connection check

        curMousePos = currentCollision.GetLocalMousePosition();

        Array pointHovering = GetTree().GetNodesInGroup("PointHovering");
        Array segmentHovering = GetTree().GetNodesInGroup("SegmentHovering");

        if (currentCollision == null)
            return;


        if (@event.IsActionPressed("token_interact_primary"))
        {


            if (pointHovering.Count > 1)
            {
                if (movingPoint != null)
                    setToPoint(pointHovering[0] as Point);
            }

            
            if (pointHovering.Count > 0)
            {
                pickupPoint(pointHovering[0] as Point);
                return;
            }

            if (selectedPoint == null)
            {
                pickupPoint(createPoint(curMousePos));
                return;
            }
            return;
        }

        if (@event.IsActionPressed("token_interact_secondary"))
        {
            if (creatingWall)
            {
                cancelWall();
                creatingWall = false;
                return;
            }
            if (pointHovering.Count > 0)
            {
                removePoint(pointHovering[0] as Point);
                movingPoint = null;
            }
            return;
        }

        if (@event.IsActionReleased("token_interact_primary"))
        {
            if (segmentHovering.Count > 0)
            {
                splitSegment(segmentHovering[0] as Segment, curMousePos, movingPoint);
            }

            if (alternativeHold)
            {
                if (movingPoint != null)
                {
                    creatingWall = true;
                    selectedPoint = movingPoint;
                    createWall(curMousePos);
                }
                return;
            }
            if (movingPoint != null)
            {
                movingPoint = null;
            }

            return;
        }

        if (@event.IsActionPressed("token_alternative_interact"))
        {
            alternativeHold = true;
        }

        if (@event.IsActionReleased("token_alternative_interact"))
        {
            alternativeHold = false;
        }


    }

    private Point splitSegment(Segment seg, Vector2 pos, Point point = null)
    {
        Point tempA = seg.PointA;
        Point tempB = seg.PointB;

        Point tempNew = point;
        if (tempNew == null)
            currentCollision.AddPoint(pos);

        currentCollision.AddSegment(tempA, tempNew);
        currentCollision.AddSegment(tempB, tempNew);
        seg.QueueFree();
        return tempNew;
    }

    private void cancelWall()
    {
        if (movingPoint == null || movingSegment == null)
            return;
        selectedPoint = null;
        movingPoint.QueueFree();
        movingSegment.QueueFree();
        movingPoint = null;
        movingSegment = null;
    }

    private void placeWall()
    {
        movingPoint = null;
        movingSegment = null;

    }
    private Point createPoint(Vector2 pos)
    {
        return currentCollision.AddPoint(curMousePos);
    }

    private void removePoint(Point point)
    {
        point.Delete();
    }

    private void setToPoint(Point point)
    {
        movingSegment.PointB = point;
        movingPoint.QueueFree();
        movingPoint = null;
        movingSegment = null;
    }

    private void createWall(Vector2 pos)
    {
        movingPoint = currentCollision.AddPoint(pos);
        movingSegment = currentCollision.AddSegment(selectedPoint, movingPoint);
    }

    private void pickupPoint(Point point)
    {
        movingPoint = point;
        movingPoint.Raise();
    }
    private void placePointOnSegment()
    {

    }


}
