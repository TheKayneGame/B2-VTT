using Godot;
using System;

public class FOVCone : Polygon2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Polygon = DrawCircleArcPoly(Position, 200, -135, -45);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        
    }

    public Vector2[] DrawCircleArcPoly(Vector2 center, float radius, float angleFrom, float angleTo)
    {
        int nbPoints = 32;
        var pointsArc = new Vector2[nbPoints + 1];
        pointsArc[0] = center;

        for (int i = 0; i < nbPoints; ++i)
        {
            float anglePoint = Mathf.Deg2Rad(angleFrom + i * (angleTo - angleFrom) / nbPoints - 90);
            pointsArc[i + 1] = center + new Vector2(Mathf.Cos(anglePoint), Mathf.Sin(anglePoint)) * radius;
        }

        return pointsArc;

    }
}
