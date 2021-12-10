using Godot;
using System;

public class Collision : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    [Export]
    PackedScene pointScene;

    [Export]
    PackedScene segmentScene;

    [Export]
    NodePath pointsPath;

    [Export]
    NodePath segmentsPath;

    Node2D nodePoints;

    Node2D nodeSegments;
    public override void _Ready()
    {
        nodePoints = GetNode<Node2D>(pointsPath);
        nodeSegments = GetNode<Node2D>(segmentsPath);
    }

    public Point AddPoint(Vector2 position)
    {
        Point point = pointScene.Instance() as Point;
        point.Position = position;
        nodePoints.AddChild(point);
        return point;
    }

    public Segment AddSegment(Point pointA, Point pointB)
    {
        Segment segment = segmentScene.Instance() as Segment;
        nodeSegments.AddChild(segment);
        segment.PointA = pointA;
        segment.PointB = pointB;
        return segment;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
