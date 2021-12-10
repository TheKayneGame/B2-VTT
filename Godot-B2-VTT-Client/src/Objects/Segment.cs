using Godot;
using System;

public class Segment : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    NodePath collsionShapePath;

    [Export]
    NodePath lineVisualPath;

    CollisionShape2D collisionShape;

    Line2D lineVisual;
    private Point pointA;
    public Point PointA
    {
        get => pointA;
        set
        {
            pointA = value;
            SetPointA(pointA);
        }
    }

    private Point pointB;
    public Point PointB
    {
        get => pointB;
        set
        {
            pointB = value;
            SetPointB(pointB);
        }
    }

    public override void _Ready()
    {
        collisionShape = GetNode<CollisionShape2D>(collsionShapePath);
        lineVisual = GetNode<Line2D>(lineVisualPath);
    }

    public void SetPointA(Point point)
    {
        (collisionShape.Shape as SegmentShape2D).A = pointA.Position;
        lineVisual.SetPointPosition(0, pointA.Position);

        if (pointA.IsConnected(nameof(Point.Moved), this, nameof(_OnPointAMoved)))
            PointA.Disconnect(nameof(Point.Moved), this, nameof(_OnPointAMoved));

        pointA.Connect(nameof(Point.Moved), this, nameof(_OnPointAMoved));

    }

    private void SetPointB(Point point)
    {
        (collisionShape.Shape as SegmentShape2D).B = pointB.Position;
        lineVisual.SetPointPosition(1, pointB.Position);

        if (pointB.IsConnected(nameof(Point.Moved), this, nameof(_OnPointBMoved)))
            PointB.Disconnect(nameof(Point.Moved), this, nameof(_OnPointBMoved));

        pointB.Connect(nameof(Point.Moved), this, nameof(_OnPointBMoved));
    }
    private void _OnPointAMoved(Vector2 pos)
    {
        (collisionShape.Shape as SegmentShape2D).A = pos;
        lineVisual.SetPointPosition(0, pos);
    }

    private void _OnPointBMoved(Vector2 pos)
    {
        (collisionShape.Shape as SegmentShape2D).B = pos;
        lineVisual.SetPointPosition(1, pos);
    }

    private void _





    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
