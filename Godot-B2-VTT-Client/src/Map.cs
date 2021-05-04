using Godot;
using System;

public class Map : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private Grid grid;

    public Vector2 CentreOffset {get; private set;}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        grid = GetNode<Grid>("Grid");
        CentreOffset = new Vector2( grid.CellSize/2f, grid.CellSize/2f);
    }

    public Vector2 GetClosestGridPosition(Vector2 pos)
    {
        pos -= Position + CentreOffset;
        float closestX  = (float) (Math.Round(pos.x/ grid.CellSize) * grid.CellSize);
        float closestY  = (float) (Math.Round(pos.y/ grid.CellSize) * grid.CellSize);

        return new Vector2(closestX,closestY) + Position + CentreOffset;
    }




    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
