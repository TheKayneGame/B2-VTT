using Godot;
using Godot.Collections;

public class Collision : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    [Signal]
    public delegate void Moved(Vector2 pos);
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

    Dictionary<string, Point> dictPoints = new Dictionary<string, Point>();
    Dictionary<string, Segment> dictSegments = new Dictionary<string, Segment>();

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

    public Dictionary<object, object> toDict()
    {
        return new Dictionary<object, object>() {
            {"Points", pointsToDict()},
            {"Segments", segmentsToDict()}
        };

    }

    private Dictionary<object, object> pointsToDict()
    {
        Array<Point> points = new Array<Point>(nodePoints.GetChildren());
        Dictionary<object, object> pointsDict = new Dictionary<object, object>();
        foreach (Point point in points)
        {
            pointsDict.Add(point.Name, point.ToDict());
        }
        return pointsDict;
    }

    private Dictionary<object, object> segmentsToDict()
    {
        Array<Segment> segments = new Array<Segment>(nodeSegments.GetChildren());
        Dictionary<object, object> pointsDict = new Dictionary<object, object>();
        foreach (Segment segment in segments)
        {
            pointsDict.Add(segment.Name, segment.ToDict());
        }
        return pointsDict;
    }

    public void fromDict(Dictionary<string, object> dict)
    {
        var temp = new Dictionary<string, Dictionary<string, object>>((Dictionary)dict);
        pointsFromDict(temp["Points"]);
        segmentsFromDict(temp["Segments"]);

        foreach (var item in dictPoints)
        {
            nodePoints.AddChild(item.Value);
        }

        foreach (var item in dictSegments)
        {
            nodeSegments.AddChild(item.Value);
        }
        
        dictPoints.Clear();
        dictSegments.Clear();

    }

    private void pointsFromDict(Dictionary<string, object> dict)
    {
        foreach (var item in dict)
        {
            Point point = pointScene.Instance<Point>();
            point.Name = item.Key as string;
            var temp = new Dictionary<string, float>((Dictionary)item.Value);
            point.Position = new Vector2(temp["PosX"], temp["PosY"]);
            dictPoints.Add(item.Key, point);
        }
    }

    private void segmentsFromDict(Dictionary<string, object> dict)
    {
        foreach (var item in dict)
        {
            Segment segment = segmentScene.Instance<Segment>();
            segment.Name = item.Key as string;
            var temp = new Dictionary<string, string>((Dictionary)item.Value);
            segment.PointA = dictPoints[temp["PointA"]];
            segment.PointB = dictPoints[temp["PointB"]];
            dictSegments.Add(item.Key, segment);
            
        }
    }


    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
