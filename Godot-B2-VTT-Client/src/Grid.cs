using Godot;
using System;

public class Grid : Node2D
{
    [Export]
    public int CellSize { get; set; }
    [Export]
    public int GridSize { get; set; }
}
