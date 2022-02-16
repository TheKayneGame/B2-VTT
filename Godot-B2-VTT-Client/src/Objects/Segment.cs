using Godot;
using Godot.Collections;

public class Segment : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public enum SegmentType
    {
        Wall,
        Window,
        Door,
        SecretDoor
    }

    [Export]
    NodePath collsionShapePath;

    [Export]
    NodePath lineVisualPath;

    [Export]
    NodePath lightOccluderPath;

    CollisionShape2D collisionShape;

    Line2D lineVisual;
    LightOccluder2D lightOccluder;

    RectangleShape2D shape2D;
    

    public bool Opened;

    public SegmentType Type;

    private Point pointA;

    public float Width = 10;
    public Point PointA
    {
        get => pointA;
        set
        {
            pointA = value;
            if (!IsInsideTree())
                return;
            SetPointA(value);
        }
    }

    private Point pointB;
    public Point PointB
    {
        get => pointB;
        set
        {
            pointB = value;
            if (!IsInsideTree())
                return;
            SetPointB(value);
        }
    }


    public override void _Ready()
    {
        collisionShape = GetNode<CollisionShape2D>(collsionShapePath);
        lineVisual = GetNode<Line2D>(lineVisualPath);
        lightOccluder = GetNode<LightOccluder2D>(lightOccluderPath);

        shape2D = collisionShape.Shape as RectangleShape2D;
        if (pointA != null)
            SetPointA(pointA);

        if (pointB != null)
            SetPointB(pointB);

    }


    public void SetPointA(Point point)
    {
        UpdateCollisionPosition();
        lineVisual.SetPointPosition(0, point.Position);
        lightOccluder.Occluder.Polygon = new Vector2[2] {point.Position, lightOccluder.Occluder.Polygon[1]};

        if (pointA.IsConnected(nameof(Point.Moved), this, nameof(_OnPointAMoved)))
            PointA.Disconnect(nameof(Point.Moved), this, nameof(_OnPointAMoved));

        if (pointA.IsConnected(nameof(Point.Deleted), this, nameof(_OnPointRemoved)))
            PointA.Disconnect(nameof(Point.Deleted), this, nameof(_OnPointRemoved));

        pointA.Connect(nameof(Point.Moved), this, nameof(_OnPointAMoved));
        PointA.Connect(nameof(Point.Deleted), this, nameof(_OnPointRemoved));

    }

    private void SetPointB(Point point)
    {
        UpdateCollisionPosition();
        lineVisual.SetPointPosition(1, pointB.Position);
        lightOccluder.Occluder.Polygon = new Vector2[2] {lightOccluder.Occluder.Polygon[0], point.Position};

        if (pointB.IsConnected(nameof(Point.Moved), this, nameof(_OnPointBMoved)))
            PointB.Disconnect(nameof(Point.Moved), this, nameof(_OnPointBMoved));

        if (pointB.IsConnected(nameof(Point.Deleted), this, nameof(_OnPointRemoved)))
            PointB.Disconnect(nameof(Point.Deleted), this, nameof(_OnPointRemoved));

        pointB.Connect(nameof(Point.Moved), this, nameof(_OnPointBMoved));
        PointB.Connect(nameof(Point.Deleted), this, nameof(_OnPointRemoved));
    }
    private void _OnPointAMoved(Vector2 pos)
    {
        UpdateCollisionPosition();
        lineVisual.SetPointPosition(0, pos);
        lightOccluder.Occluder.Polygon = new Vector2[2] {pos, pointA.Position};
    }

    private void _OnPointBMoved(Vector2 pos)
    {
        UpdateCollisionPosition();
        lineVisual.SetPointPosition(1, pos);
        lightOccluder.Occluder.Polygon = new Vector2[2] {pointA.Position, pos};
    }

    private void UpdateCollisionPosition()
    {
        if (PointA == null || pointB == null)
            return;
        shape2D.Extents = new Vector2(PointA.Position.DistanceTo(PointB.Position) / 2 - Width / 2, Width / 2);
        collisionShape.Position = new Vector2((PointA.Position.x + PointB.Position.x) / 2, (pointA.Position.y + PointB.Position.y) / 2);
        collisionShape.Rotation = PointA.Position.AngleToPoint(PointB.Position);
    }

    private void _OnPointRemoved()
    {

        QueueFree();
    }

    public void _MouseEntered()
    {
        AddToGroup("SegmentHovering");
        lineVisual.DefaultColor = Colors.Aquamarine;

    }

    public void _MouseExited()
    {
        RemoveFromGroup("SegmentHovering");
        lineVisual.DefaultColor = Colors.White;
    }

    public Dictionary<object, object> ToDict()
    {
        return new Dictionary<object, object>(){
                    {"PointA", PointA.Name},
                    {"PointB", PointB.Name}
        };

    }
}
