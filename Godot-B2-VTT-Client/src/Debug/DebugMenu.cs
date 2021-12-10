using Godot;
using System;

public class DebugMenu : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    private WindowDialog DebugWindow;
    private CanvasLayer UILayer;

    public override void _Ready()
    {
        DebugWindow = GetNode<WindowDialog>("DebugWindow");
        DebugWindow.Visible = true;
        DebugWindow.SetPosition(GetGlobalMousePosition());
        UILayer = GetParent<CanvasLayer>();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }


    public void _onButtonGridFinderPressed()
    {
        GetTree().SetInputAsHandled();
        WindowDialog gridFinderMenu = GD.Load<PackedScene>("res://src/Map/Grid/Finder/GridFinderMenu.tscn").Instance() as WindowDialog;
        UILayer.AddChild(gridFinderMenu);
    }
    public void _onButtonColisionEditorPressed()
    {
        GetTree().SetInputAsHandled();
        WindowDialog gridFinderMenu = GD.Load<PackedScene>("res://src/Objects/CollisionEditorMenu.tscn").Instance() as WindowDialog;
        UILayer.AddChild(gridFinderMenu);
    }
}


