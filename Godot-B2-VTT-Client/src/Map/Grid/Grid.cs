using Godot;
using System;

public class Grid
{
    public Grid(Vector2 size, Vector2 offset){
        Size = size;
        Offset = offset;
        CellCentre = size/2f;
        
    }
    public Vector2 Size { get; set; }

    public Vector2 Offset { get; set; }

    public Vector2 CellCentre { get; set; }



}
